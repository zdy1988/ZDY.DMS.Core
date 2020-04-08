using System;
using ZDY.DMS.AspNetCore.Mvc;
using ZDY.DMS.KeyGeneration;
using ZDY.DMS.Repositories;
using ZDY.DMS.Services.AdminService.Models;

namespace ZDY.DMS.Services.AdminService.Controllers
{
    //[Authorize(Roles = "Administrator")]
    public class DictionaryValueController : ApiDataServiceController<Guid, DictionaryValue, AdminServiceModule>
    {
        private readonly IRepository<Guid, DictionaryKey> dictionaryKeyRepository;

        public DictionaryValueController()
            : base(new GuidKeyGenerator())
        {
            dictionaryKeyRepository = this.RepositoryContext.GetRepository<Guid, DictionaryKey>();
        }

        protected override void BeforeAdd(DictionaryValue entity)
        {
            if (this.Repository.Exists(t => t.DictionaryKey == entity.DictionaryKey && t.Value == entity.Value))
            {
                throw new InvalidOperationException("此字典数据已存在！");
            }
        }

        protected override void BeforeUpdate(DictionaryValue original, DictionaryValue entity)
        {
            if (this.Repository.Exists(t => t.DictionaryKey == entity.DictionaryKey && t.Value == entity.Value && t.Id != entity.Id))
            {
                throw new InvalidOperationException("此字典数据已存在！");
            }

            original.Name = entity.Name;
            original.Note = entity.Note;
            original.Order = entity.Order;
            original.ParentValue = entity.ParentValue;
            original.Value = entity.Value;
        }
    }
}
