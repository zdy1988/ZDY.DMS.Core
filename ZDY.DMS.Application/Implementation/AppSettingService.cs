using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using ZDY.DMS.ServiceContracts;

namespace ZDY.DMS.Application.Implementation
{
    public class AppSettingService : IAppSettingService
    {
        private readonly IConfiguration configuration;

        public AppSettingService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string GetAppSetting(string key)
        {
            return this.configuration.GetValue<string>($"AppSettings:{key}", "");
        }

        public T GetAppSetting<T>(string key)
        {
            return this.configuration.GetValue<T>($"AppSettings:{key}");
        }
    }
}
