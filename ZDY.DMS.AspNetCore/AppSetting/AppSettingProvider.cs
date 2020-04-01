using Microsoft.Extensions.Configuration;

namespace ZDY.DMS.AspNetCore
{
    public class AppSettingProvider : IAppSettingProvider
    {
        private readonly IConfiguration configuration;

        public AppSettingProvider(IConfiguration configuration)
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
