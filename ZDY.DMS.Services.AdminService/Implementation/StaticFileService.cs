using System;
using System.Linq;
using ZDY.DMS.AspNetCore;
using ZDY.DMS.Repositories;
using ZDY.DMS.Services.Common.Models;
using ZDY.DMS.Services.Common.ServiceContracts;
using ZDY.DMS.Tools;

namespace ZDY.DMS.Services.AdminService.Implementation
{
    public class StaticFileService : ServiceBase<AdminServiceModule>, IStaticFileService
    {
        private IRepository<Guid, File> fileRepository;
        private IAppSettingService appSettingService;

        public StaticFileService(Func<Type, IRepositoryContext> repositoryContextFactory,
            IAppSettingService appSettingService) : base(repositoryContextFactory)
        {
            this.fileRepository = this.GetRepository<Guid, File>();
            this.appSettingService = appSettingService;
        }

        private string EncryptKey => appSettingService.GetAppSetting("StaticFileEncryptKey");
        private string FileServerUrl => appSettingService.GetAppSetting("StaticFileServerUrl");
        private string ParseFileUrl(File file) => $"{FileServerUrl}{file.Path}";

        public string GetFileCodeByKey(Guid fileKey)
        {
            return AESHelper.AESEncrypt(EncryptKey + fileKey.ToString(), EncryptKey);
        }

        public Guid GetFileKeyByCode(string fileCode)
        {
            return Guid.Parse(AESHelper.AESDecrypt(fileCode, EncryptKey).Replace(EncryptKey, ""));
        }

        public string GetFileUrl(Guid fileKey)
        {
            var file = fileRepository.FindByKey(fileKey);
            return file == null ? "" : ParseFileUrl(file);
        }

        public string GetFileUrl(File file)
        {
            return file == null ? "" : ParseFileUrl(file);
        }

        public string[] GetFileUrls(Guid bussinessKey, string type)
        {
            var files = fileRepository.FindAll(t => t.BusinessID == bussinessKey);
            return files.Count() <= 0 ? null : files.Select(file => ParseFileUrl(file)).ToArray();
        }
    }
}
