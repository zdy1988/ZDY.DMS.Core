using System;
using ZDY.DMS.AspNetCore.Mvc;
using ZDY.DMS.KeyGeneration;
using ZDY.DMS.Repositories;
using ZDY.DMS.Services.AdminService.Models;

namespace ZDY.DMS.Services.AdminService.Controllers
{
    //[Authorize(Roles = "Administrator")]
    public class DictionaryKeyController : ApiDataServiceController<Guid, DictionaryKey, AdminServiceModule>
    {
        private readonly IRepository<Guid, DictionaryValue> dictionaryValueRepository;

        public DictionaryKeyController() 
            : base(new GuidKeyGenerator())
        {
            dictionaryValueRepository = this.RepositoryContext.GetRepository<Guid, DictionaryValue>();
        }

        protected override void BeforeAdd(DictionaryKey entity)
        {
            if (this.Repository.Exists(t => t.Code == entity.Code))
            {
                throw new InvalidOperationException("此字典标识已存在！");
            }
        }

        protected override void BeforeUpdate(DictionaryKey original, DictionaryKey entity)
        {
            if (this.Repository.Exists(t => t.Code == entity.Code && t.Id != entity.Id))
            {
                throw new InvalidOperationException("此字典标识已存在！");
            }

            var originalCode = original.Code;
            var newCode = entity.Code;

            original.Code = entity.Code;
            original.Name = entity.Name;
            original.Order = entity.Order;
            original.Type = entity.Type;

            dictionaryValueRepository.Update(t => t.DictionaryKey == originalCode, query => new DictionaryValue { DictionaryKey = newCode });
        }
    }
}
