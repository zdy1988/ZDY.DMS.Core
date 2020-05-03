using System;
using ZDY.DMS.Services.AdminService.Models;

namespace ZDY.DMS.Services.AdminService.ServiceContracts
{
    public interface IFileService
    {
        string GetFileCodeByKey(Guid fileKey);

        Guid GetFileKeyByCode(string fileCode);

        string GetFileUrl(Guid fileKey);

        string GetFileUrl(File file);

        string[] GetFileUrls(Guid bussinessKey, string type);
    }
}
