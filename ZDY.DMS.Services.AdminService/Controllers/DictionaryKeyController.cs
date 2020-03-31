using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZDY.DMS.AspNetCore.Mvc;
using ZDY.DMS.DataPermission;
using ZDY.DMS.KeyGeneration;
using ZDY.DMS.Repositories;
using ZDY.DMS.Services.Common.Models;

namespace ZDY.DMS.Services.AdminService.Controllers
{
    //[Authorize(Roles = "Administrator")]
    public class DictionaryKeyController : ApiDataServiceController<Guid, DictionaryKey>
    {
        IRepository<Guid, DictionaryValue> dictionaryValueRepository;

        public DictionaryKeyController(IRepositoryContext repositoryContext)
            : base(repositoryContext, new GuidKeyGenerator())
        {
            dictionaryValueRepository = repositoryContext.GetRepository<Guid, DictionaryValue>();
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

        public override Task<Tuple<IEnumerable<DictionaryKey>, int>> Search(SearchModel searchModel)
        {
            return base.Search(searchModel);
        }
    }
}
