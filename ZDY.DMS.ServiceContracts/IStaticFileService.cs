using System;
using System.Collections.Generic;
using System.Text;
using ZDY.DMS.Models;

namespace ZDY.DMS.ServiceContracts
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
