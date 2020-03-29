namespace ZDY.DMS.Services.Common.ServiceContracts
{
    public interface IAppSettingService
    {
        string GetAppSetting(string key);

        T GetAppSetting<T>(string key);
    }
}
