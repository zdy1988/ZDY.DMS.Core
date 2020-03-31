using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using ZDY.DMS.Caching;
using ZDY.DMS.Querying.DataTableGateway;
using ZDY.DMS.Repositories;
using ZDY.DMS.Services.AdminService.Enums;
using ZDY.DMS.Services.Common.Models;
using ZDY.DMS.Services.Common.DataTransferObjects;
using ZDY.DMS.Services.Common.ServiceContracts;
using ZDY.DMS.Tools;
using ZDY.DMS.AspNetCore.Dictionary;
using ZDY.DMS.AspNetCore;

namespace ZDY.DMS.Services.AdminService.Implementation
{
    public class DictionaryService : ServiceBase<AdminServiceModule>, IDictionaryService
    {
        private readonly IRepository<Guid, DictionaryKey> dictionaryKeyRepository;
        private readonly IRepository<Guid, DictionaryValue> dictionaryValueRepository;
        private readonly IDictionaryProvider dictionaryProvider;

        public DictionaryService(Func<Type, IRepositoryContext> repositoryContextFactory,
            IDictionaryProvider dictionaryProvider) : base(repositoryContextFactory)
        {
            this.dictionaryProvider = dictionaryProvider;
            this.dictionaryKeyRepository = this.GetRepository<Guid, DictionaryKey>();
            this.dictionaryValueRepository = this.GetRepository<Guid, DictionaryValue>();
        }

        public Dictionary<string, IEnumerable<KeyValuePaired>> GetDictionary(string keys)
        {
            if (string.IsNullOrEmpty(keys))
            {
                throw new ArgumentNullException("keys");
            }

            Dictionary<string, IEnumerable<KeyValuePaired>> dictionary = new Dictionary<string, IEnumerable<KeyValuePaired>>();

            string[] keyArray = keys.Split(',');

            foreach (string key in keyArray)
            {
                if (this.dictionaryProvider.TryGetKeyValueCache(key, out IEnumerable<KeyValuePaired> cachedKeyValues))
                {
                    dictionary.Add(key, cachedKeyValues);

                    continue;
                }

                if (this.dictionaryProvider.TryGetKeyValuePairs(key, out IEnumerable<KeyValuePaired> keyValues))
                {
                    dictionary.Add(key, keyValues);

                    continue;
                }

                DictionaryKey dictionaryKey = dictionaryKeyRepository.Find(t => t.Code == key);

                if (dictionaryKey != null)
                {
                    IEnumerable<DictionaryValue> dictionaryValues = dictionaryValueRepository.FindAll(t => t.DictionaryKey == dictionaryKey.Code, query => query.Asc(t => t.Order));

                    if (dictionaryValues.Count() > 0)
                    {
                        List<KeyValuePaired> keyValuePairs = new List<KeyValuePaired>();

                        switch (dictionaryKey.Type)
                        {
                            case (int)DictionaryKinds.KeyVlaue:
                                HandleKeyVlaue(keyValuePairs, dictionaryValues);
                                break;
                            case (int)DictionaryKinds.SqlQuery:
                                string sqlQuery = keyValuePairs.FirstOrDefault().Value;
                                this.dictionaryProvider.HandleSqlQuery(keyValuePairs, sqlQuery);
                                break;
                            case (int)DictionaryKinds.Enum:
                                string enumReflection = keyValuePairs.FirstOrDefault().Value;
                                this.dictionaryProvider.HandleEnum(keyValuePairs, enumReflection);
                                break;
                            case (int)DictionaryKinds.Method:
                                string methodReflection = keyValuePairs.FirstOrDefault().Value;
                                this.dictionaryProvider.HandleMethod(keyValuePairs, methodReflection);
                                break;
                            default:
                                throw new InvalidOperationException("数据类型错误");
                        }

                        dictionary.Add(key, keyValuePairs);

                        //存入缓存
                        this.dictionaryProvider.CachedKeyValuePairs(key, keyValuePairs);
                    }
                    else
                    {
                        throw new InvalidOperationException("数据不存在");
                    }
                }
                else
                {
                    throw new InvalidOperationException("数据不存在");
                }
            }

            return dictionary;
        }

        public string GetDictionaryName(string key, string values, string defaultValue = "")
        {
            var dictionary = GetDictionary(key)[key];
            var valueArray = values.Split(',');
            string result = "";
            foreach (var item in dictionary)
            {
                foreach (var val in valueArray)
                {
                    if (item.Value == val)
                    {
                        result += item.Name + ",";
                    }
                }
            }
            return result.Length > 0 ? result.Substring(0, result.Length - 1) : defaultValue;
        }

        private void HandleKeyVlaue(List<KeyValuePaired> dictionary, IEnumerable<DictionaryValue> dictionaryValues)
        {
            foreach (var item in dictionaryValues)
            {
                dictionary.Add(new KeyValuePaired(item.Value, item.Name, item.ParentValue));
            }
        }
    }
}
