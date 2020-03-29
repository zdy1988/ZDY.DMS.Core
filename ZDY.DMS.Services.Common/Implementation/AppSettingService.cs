using Microsoft.Extensions.Configuration;
using ZDY.DMS.Services.Common.ServiceContracts;

namespace ZDY.DMS.Services.Common.Implementation
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
