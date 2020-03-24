using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using ZDY.DMS.Extensions.DependencyInjection.Autofac;

namespace ZDY.DMS.AspNetCore
{
    public class ConfigurationManager
    {
        public static string DbContextName { get; } = "JxcDbContext";
        public static string GetAppSetting(string key)
        {
            return ServiceLocator.GetService<IConfiguration>().GetValue<string>($"AppSettings:{key}", "");
        }

        public static T GetAppSetting<T>(string key)
        {
            return ServiceLocator.GetService<IConfiguration>().GetValue<T>($"AppSettings:{key}");
        }
    }
}
