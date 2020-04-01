namespace ZDY.DMS.AspNetCore
{
    public interface IAppSettingProvider
    {
        string GetAppSetting(string key);

        T GetAppSetting<T>(string key);
    }
}
