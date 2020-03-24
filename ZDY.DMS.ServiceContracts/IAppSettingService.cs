using System;
using System.Collections.Generic;
using System.Text;

namespace ZDY.DMS.ServiceContracts
{
    public interface IAppSettingService
    {
        string GetAppSetting(string key);

        T GetAppSetting<T>(string key);
    }
}
