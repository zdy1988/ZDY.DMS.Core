using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZDY.DMS.Services.Common.DataTransferObjects;

namespace ZDY.DMS.Services.Common.ServiceContracts
{
    public interface IDictionaryService
    {
        /// <summary>
        /// 获取字典，支持多个Key，用逗号隔开
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        Dictionary<string, IEnumerable<DictionaryItemDTO>> GetDictionary(string keys);

        /// <summary>
        /// 获取某个Key下字典中Value的名称，支持多个Value，用逗号隔开
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        string GetDictionaryName(string key, string values, string defaultValue = "");
    }
}
