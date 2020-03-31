using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace ZDY.DMS.AspNetCore.Dictionary
{
    public class DictionaryRegister : DisposableObject, IDictionaryRegister
    {
        private readonly ConcurrentDictionary<string, Tuple<DictionaryItemKinds, string>> dictionaryItems;

        public DictionaryRegister()
        {
            dictionaryItems = new ConcurrentDictionary<string, Tuple<DictionaryItemKinds, string>>();
        }

        public void Register(string value, string key, DictionaryItemKinds kind)
        {
            if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
            {
                if (!dictionaryItems.ContainsKey(key))
                {
                    dictionaryItems.TryAdd(key, new Tuple<DictionaryItemKinds, string>(kind, value));
                }
            }
        }

        public void RegisterEnum(Type enumType, string key = default)
        {
            if (string.IsNullOrEmpty(key))
            {
                key = enumType.Name;
            }

            var assembly = enumType.AssemblyQualifiedName.Split(',');

            var dllName = assembly[1].Trim();

            var typeName = assembly[0].Trim();

            var value = $"{dllName},{typeName}";

            Register(value, key, DictionaryItemKinds.Enum);
        }

        public void RegisterEnum<TEnum>(string key = default)
        {
            RegisterEnum(typeof(TEnum), key);
        }

        public void RegisterSqlQuery(string sql, string key)
        {
            Register(sql, key, DictionaryItemKinds.SqlQuery);
        }

        public Tuple<DictionaryItemKinds, string> GetValue(string key)
        {
            if (dictionaryItems.TryGetValue(key, out Tuple<DictionaryItemKinds, string> registryItem))
            {
                return registryItem;
            }

            return null;
        }

        public ConcurrentDictionary<string, Tuple<DictionaryItemKinds, string>> GetItems()
        {
            return dictionaryItems;
        }
    }
}
