using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZDY.DMS.AspNetCore;
using ZDY.DMS.AspNetCore.Mvc;
using ZDY.DMS.DataPermission;
using ZDY.DMS.KeyGeneration;
using ZDY.DMS.Repositories;
using ZDY.DMS.Services.AdminService.Models;

namespace ZDY.DMS.Services.AdminService.Controllers
{
    public class FileController : ApiDataServiceController<Guid, File, AdminServiceModule>
    {
        private readonly IAppSettingProvider appSettingProvider;

        public FileController(Func<Type, IRepositoryContext> repositoryContextFactory,
            IAppSettingProvider appSettingProvider)
            : base(repositoryContextFactory, new GuidKeyGenerator())
        {
            this.appSettingProvider = appSettingProvider;
        }

        public override Task<File> Find(SearchModel searchModel)
        {
            throw new NotSupportedException();
        }

        public override Task<File> FindByKey(Guid id)
        {
            throw new NotSupportedException();
        }

        public override Task<File> Add(File entity)
        {
            throw new NotSupportedException();
        }

        public override Task<File> Update(File entity)
        {
            throw new NotSupportedException();
        }

        protected override void BeforeDelete(Guid id)
        {
            var file = this.Repository.FindByKey(id);

            string staticFileServerPath = this.appSettingProvider.GetAppSetting("StaticFileServerPath");
            string path = staticFileServerPath + file.Path;
            System.IO.File.Delete(path);
        }
    }
}
