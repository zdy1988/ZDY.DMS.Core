using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZDY.DMS.AspNetCore.Mvc;
using ZDY.DMS.DataPermission;
using ZDY.DMS.KeyGeneration;
using ZDY.DMS.Repositories;
using ZDY.DMS.Services.Common.Models;
using ZDY.DMS.Services.Common.ServiceContracts;

namespace ZDY.DMS.Services.AdminService.Controllers
{
    public class FileController : ApiDataServiceController<Guid, File>
    {
        private readonly IAppSettingService appSettingService;

        public FileController(IRepositoryContext repositoryContex,
            IAppSettingService appSettingService)
            : base(repositoryContex, new GuidKeyGenerator())
        {
            this.appSettingService = appSettingService;
        }

        public override Task<Tuple<IEnumerable<File>, int>> Search(SearchModel searchModel)
        {
            return base.Search(searchModel);
        }

        public override Task<File> Find(SearchModel searchModel)
        {
            throw new NotImplementedException();
        }

        public override Task<File> FindByKey(Guid id)
        {
            throw new NotImplementedException();
        }

        public override Task<File> Add(File entity)
        {
            throw new NotImplementedException();
        }

        public override Task<File> Update(File entity)
        {
            throw new NotImplementedException();
        }

        protected override void BeforeDelete(Guid id)
        {
            var file = this.Repository.FindByKey(id);

            string staticFileServerPath = this.appSettingService.GetAppSetting("StaticFileServerPath");
            string path = staticFileServerPath + file.Path;
            System.IO.File.Delete(path);
        }
    }
}
