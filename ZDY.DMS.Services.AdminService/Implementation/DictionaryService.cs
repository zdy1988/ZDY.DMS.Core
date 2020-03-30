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

namespace ZDY.DMS.Services.AdminService.Implementation
{
    public class DictionaryService : IDictionaryService
    {
        private readonly IRepositoryContext repositoryContext;
        private readonly IRepository<Guid, DictionaryKey> dictionaryKeyRepository;
        private readonly IRepository<Guid, DictionaryValue> dictionaryValueRepository;
        private readonly IDataTableGateway dataTableGateway;

        public DictionaryService(IRepositoryContext repositoryContext,
            IDataTableGateway dataTableGateway)
        {
            this.repositoryContext = repositoryContext;
            this.dictionaryKeyRepository = repositoryContext.GetRepository<Guid, DictionaryKey>();
            this.dictionaryValueRepository = repositoryContext.GetRepository<Guid, DictionaryValue>();
            this.dataTableGateway = dataTableGateway;
        }

        public Dictionary<string, IEnumerable<DictionaryItemDTO>> GetDictionary(string keys)
        {
            if (string.IsNullOrEmpty(keys))
            {
                throw new ArgumentNullException("keys");
            }

            Dictionary<string, IEnumerable<DictionaryItemDTO>> dictionary = new Dictionary<string, IEnumerable<DictionaryItemDTO>>();
            string[] keyArray = keys.Split(',');

            foreach (string key in keyArray)
            {
                string cacheCode = "cache_dictionary_" + key;

                if (ServiceLocator.GetService<ICacheManager>().IsSet(cacheCode))
                {
                    var list = ServiceLocator.GetService<ICacheManager>().Get<IEnumerable<DictionaryItemDTO>>(cacheCode);
                    dictionary.Add(key, list);
                }
                else
                {
                    DictionaryKey dictionaryKey = dictionaryKeyRepository.Find(t => t.Code == key);
                    if (dictionaryKey != null)
                    {
                        IEnumerable<DictionaryValue> dictionaryValueList = dictionaryValueRepository.FindAll(t => t.DictionaryKey == dictionaryKey.Code, query => query.Asc(t => t.Order));
                        if (dictionaryValueList.Count() > 0)
                        {
                            List<DictionaryItemDTO> dictionaryModels = new List<DictionaryItemDTO>();
                            switch (dictionaryKey.Type)
                            {
                                case (int)DictionaryKinds.KeyVlaue:
                                    HandleKeyVlaue(dictionaryModels, dictionaryValueList);
                                    break;
                                case (int)DictionaryKinds.SqlQuery:
                                    HandleSqlQuery(dictionaryModels, dictionaryValueList);
                                    break;
                                case (int)DictionaryKinds.Enum:
                                    HandleEnum(dictionaryModels, dictionaryValueList);
                                    break;
                                case (int)DictionaryKinds.Method:
                                    HandleMethod(dictionaryModels, dictionaryValueList);
                                    break;
                                default:
                                    throw new InvalidOperationException("数据类型错误");
                            }
                            dictionary.Add(key, dictionaryModels);
                            //存入缓存
                            ServiceLocator.GetService<ICacheManager>().Set(cacheCode, dictionaryModels, 120);
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

        private void HandleKeyVlaue(List<DictionaryItemDTO> dictionary, IEnumerable<DictionaryValue> dictionaryValues)
        {
            foreach (var item in dictionaryValues)
            {
                dictionary.Add(new DictionaryItemDTO
                {
                    Name = item.Name,
                    Value = item.Value,
                    Parent = item.ParentValue
                });
            }
        }

        private void HandleSqlQuery(List<DictionaryItemDTO> dictionary, IEnumerable<DictionaryValue> dictionaryValues)
        {
            string sqlQuery = dictionaryValues.FirstOrDefault().Value;

            var dt = dataTableGateway.ExecuteDataTable(sqlQuery);

            if (!dt.Columns.Contains("Name"))
            {
                throw new ArgumentNullException("Name");
            }
            if (!dt.Columns.Contains("value"))
            {
                throw new ArgumentNullException("value");
            }
            if (!dt.Columns.Contains("parent"))
            {
                throw new ArgumentNullException("parent");
            }
            foreach (DataRow row in dt.Rows)
            {
                dictionary.Add(new DictionaryItemDTO
                {
                    Name = row["Name"].ToString(),
                    Value = row["Value"].ToString(),
                    Parent = row["Parent"].ToString()
                });
            }
        }

        private void HandleEnum(List<DictionaryItemDTO> dictionary, IEnumerable<DictionaryValue> dictionaryValues)
        {
            var reflection = dictionaryValues.FirstOrDefault().Value.Split(',');
            var dllName = reflection[0];
            var typeName = reflection[1];
            Type type = Assembly.Load(dllName).GetType(typeName, true);
            var enums = EnumHelper.GetEnums(type);
            foreach (var e in enums)
            {
                dictionary.Add(new DictionaryItemDTO
                {
                    Name = e.Desription,
                    Value = e.Value,
                    Parent = e.Category
                });
            }
        }

        private void HandleMethod(List<DictionaryItemDTO> dictionary, IEnumerable<DictionaryValue> dictionaryValues)
        {
            var reflection = dictionaryValues.FirstOrDefault().Value.Split(',');
            var dllName = reflection[0];
            var methodName = reflection[1];
            var typeName = System.IO.Path.GetFileNameWithoutExtension(methodName);
            Type type = Assembly.Load(dllName).GetType(typeName, true);
            var instance = System.Activator.CreateInstance(type, false);
            var method = type.GetMethod(methodName);

            if (method != null)
            {
                var methodParams = reflection[2] == null ? "" : reflection[2].ToString();

                var result = method.Invoke(instance, new object[] { methodParams });

                if (result is List<DictionaryItemDTO>)
                {
                    dictionary.AddRange((List<DictionaryItemDTO>)result);
                }
                else
                {
                    throw new TypeUnloadedException($"{reflection} 提供的返回值类型不符合规定");
                }
            }
            else
            {
                throw new MissingMethodException(typeName, methodName);
            }
        }
    }
}
