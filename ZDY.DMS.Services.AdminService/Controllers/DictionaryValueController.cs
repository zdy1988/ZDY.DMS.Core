using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using ZDY.DMS.AspNetCore.Mvc;
using ZDY.DMS.DataPermission;
using ZDY.DMS.KeyGeneration;
using ZDY.DMS.Repositories;
using ZDY.DMS.Services.Common.Models;

namespace ZDY.DMS.Services.AdminService.Controllers
{
    //[Authorize(Roles = "Administrator")]
    public class DictionaryValueController : ApiDataServiceController<Guid, DictionaryValue, AdminServiceModule>
    {
        IRepository<Guid, DictionaryKey> dictionaryKeyRepository;

        public DictionaryValueController(Func<Type, IRepositoryContext> repositoryContextFactory)
            : base(repositoryContextFactory, new GuidKeyGenerator())
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

        public override Task<Tuple<IEnumerable<DictionaryValue>, int>> Search(SearchModel searchModel)
        {
            return base.Search(searchModel);
        }
    }
}
