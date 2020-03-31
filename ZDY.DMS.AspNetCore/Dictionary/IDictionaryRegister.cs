using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace ZDY.DMS.AspNetCore.Dictionary
{
    public interface IDictionaryRegister: IDisposable
    {
        void Register(string value, string key, DictionaryItemKinds kind);

        void RegisterEnum(Type enumType, string key = default);

        void RegisterEnum<TEnum>(string key = default);

        void RegisterSqlQuery(string sql, string key);

        Tuple<DictionaryItemKinds, string> GetValue(string key);

        ConcurrentDictionary<string, Tuple<DictionaryItemKinds, string>> GetItems();
    }
}
