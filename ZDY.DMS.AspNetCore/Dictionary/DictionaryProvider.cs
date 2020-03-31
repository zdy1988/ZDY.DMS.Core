using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using ZDY.DMS.Caching;
using ZDY.DMS.Querying.DataTableGateway;
using ZDY.DMS.Tools;

namespace ZDY.DMS.AspNetCore.Dictionary
{
    public class DictionaryProvider : DisposableObject, IDictionaryProvider
    {
        private readonly IDictionaryRegister dictionaryRegister;
        private readonly ICacheManager cacheManager;
        private readonly IDataTableGateway dataTableGateway;

        public DictionaryProvider(IDictionaryRegister dictionaryRegister,
            ICacheManager cacheManager,
            IDataTableGateway dataTableGateway)
        {
            this.dictionaryRegister = dictionaryRegister;
            this.cacheManager = cacheManager;
            this.dataTableGateway = dataTableGateway;
        }

        public bool TryGetKeyValuePairs(string key, out IEnumerable<KeyValuePaired> keyValues)
        {
            try
            {
                GetDictionary(key, out Dictionary<string, IEnumerable<KeyValuePaired>> dictionary);

                if (dictionary.ContainsKey(key))
                {
                    keyValues = dictionary[key];

                    return true;
                }
            }
            catch
            {

            }

            keyValues = null;

            return false;
        }

        public bool TryGetKeyValueCache(string key, out IEnumerable<KeyValuePaired> keyValues)
        {
            string cacheCode = $"$cached_dictionary_{key}";

            try
            {
                if (this.cacheManager.IsSet(cacheCode))
                {
                    keyValues = this.cacheManager.Get<IEnumerable<KeyValuePaired>>(cacheCode);

                    return true;
                }
            }
            catch
            {

            }

            keyValues = null;

            return false;
        }

        public void CachedKeyValuePairs(string key, IEnumerable<KeyValuePaired> keyValues)
        {
            this.cacheManager.Set($"$cached_dictionary_{key}", keyValues, 120);
        }

        public void GetDictionary(string keys, out Dictionary<string, IEnumerable<KeyValuePaired>> dictionary)
        {
            if (string.IsNullOrEmpty(keys))
            {
                throw new ArgumentNullException("keys");
            }

            dictionary = new Dictionary<string, IEnumerable<KeyValuePaired>>();

            string[] keyArray = keys.Split(',');

            foreach (string key in keyArray)
            {
                if (TryGetKeyValueCache(key, out IEnumerable<KeyValuePaired> cachedKeyValues))
                {
                    dictionary.Add(key, cachedKeyValues);

                    continue;
                }

                if (this.dictionaryRegister.GetItems().TryGetValue(key, out Tuple<DictionaryItemKinds, string> item))
                {
                    List<KeyValuePaired> keyValues = new List<KeyValuePaired>();

                    switch (item.Item1)
                    {
                        case DictionaryItemKinds.KeyVlaue:
                            HandleKeyVlaue(keyValues, item.Item2);
                            break;
                        case DictionaryItemKinds.SqlQuery:
                            HandleSqlQuery(keyValues, item.Item2);
                            break;
                        case DictionaryItemKinds.Enum:
                            HandleEnum(keyValues, item.Item2);
                            break;
                        case DictionaryItemKinds.Method:
                            HandleMethod(keyValues, item.Item2);
                            break;
                        default:
                            throw new InvalidOperationException("数据类型错误");
                    }

                    dictionary.Add(key, keyValues);

                    //装入缓存
                    CachedKeyValuePairs(key, keyValues);
                }
                else
                {
                    throw new InvalidOperationException("数据不存在");
                }
            }
        }

        public void HandleKeyVlaue(List<KeyValuePaired> dictionary, string value)
        {
            try
            {
                var list = JsonConvert.DeserializeObject<List<KeyValuePaired>>(value);
                dictionary.AddRange(list);
            }
            catch
            {
                throw new InvalidOperationException("流程数据解析异常");
            }
        }

        public void HandleSqlQuery(List<KeyValuePaired> dictionary, string value)
        {
            var dt = this.dataTableGateway.ExecuteDataTable(value);

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
                dictionary.Add(new KeyValuePaired(row["Value"].ToString(), row["Name"].ToString(), row["Parent"].ToString()));
            }
        }

        public void HandleEnum(List<KeyValuePaired> dictionary, string value)
        {
            var reflection = value.Split(',');
            var dllName = reflection[0];
            var typeName = reflection[1];
            Type type = Assembly.Load(dllName).GetType(typeName, true);
            var enums = EnumHelper.GetEnums(type);
            foreach (var e in enums)
            {
                dictionary.Add(new KeyValuePaired(e.Value, e.Desription, e.Category));
            }
        }

        public void HandleMethod(List<KeyValuePaired> dictionary, string value)
        {
            var reflection = value.Split(',');
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

                if (result is List<KeyValuePaired>)
                {
                    dictionary.AddRange((List<KeyValuePaired>)result);
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
