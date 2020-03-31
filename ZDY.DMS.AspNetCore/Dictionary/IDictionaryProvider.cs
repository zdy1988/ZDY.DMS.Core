using System;
using System.Collections.Generic;
using System.Text;

namespace ZDY.DMS.AspNetCore.Dictionary
{
    public interface IDictionaryProvider : IDisposable
    {
        bool TryGetKeyValuePairs(string key, out IEnumerable<KeyValuePaired> keyValues);

        bool TryGetKeyValueCache(string key, out IEnumerable<KeyValuePaired> keyValues);

        void CachedKeyValuePairs(string key, IEnumerable<KeyValuePaired> keyValues);

        void GetDictionary(string keys, out Dictionary<string, IEnumerable<KeyValuePaired>> dictionary);

        void HandleKeyVlaue(List<KeyValuePaired> dictionary, string value);

        void HandleSqlQuery(List<KeyValuePaired> dictionary, string value);

        void HandleEnum(List<KeyValuePaired> dictionary, string value);

        void HandleMethod(List<KeyValuePaired> dictionary, string value);
    }
}
