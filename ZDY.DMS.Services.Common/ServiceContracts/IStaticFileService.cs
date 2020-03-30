using System;
using ZDY.DMS.Services.Common.Models;

namespace ZDY.DMS.Services.Common.ServiceContracts
{
    public interface IStaticFileService
    {
        string GetFileCodeByKey(Guid fileKey);

        Guid GetFileKeyByCode(string fileCode);

        string GetFileUrl(Guid fileKey);

        string GetFileUrl(File file);

        string[] GetFileUrls(Guid bussinessKey, string type);
    }
}
