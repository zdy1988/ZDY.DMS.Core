using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ZDY.DMS.AspNetCore.Mvc;
using ZDY.DMS.KeyGeneration;
using ZDY.DMS.Repositories;
using ZDY.DMS.Services.WorkFlowService.Enums;
using ZDY.DMS.Services.WorkFlowService.Models;

namespace ZDY.DMS.Services.WorkFlowService
{
    public class WorkFlowFormController : ApiDataServiceController<Guid, WorkFlowForm, WorkFlowServiceModule>
    {
        public WorkFlowFormController(Func<Type, IRepositoryContext> repositoryContextFactory)
            : base(repositoryContextFactory, new GuidKeyGenerator())
        {

        }

        protected override void BeforeAdd(WorkFlowForm entity)
        {
            if (Repository.Exists(t => t.CompanyId == this.UserIdentity.CompanyId && t.Name == entity.Name))
            {
                throw new InvalidOperationException("这个表单名称已存在");
            }

            entity.CompanyId = this.UserIdentity.CompanyId;
            entity.CreaterId = this.UserIdentity.Id;
            entity.State = (int)WorkFlowFormState.Designing;
        }

        protected override void BeforeUpdate(WorkFlowForm original, WorkFlowForm entity)
        {
            if (original.State != (int)WorkFlowFormState.Designing)
            {
                throw new InvalidOperationException("只有设计中的表单才可以修改！");
            }

            original.Name = entity.Name;
            original.Type = entity.Type;
            original.Note = entity.Note;
            original.LastModifyTime = DateTime.Now;
        }

        protected override void BeforeDelete(WorkFlowForm original)
        {
            if (original.State != (int)WorkFlowFormState.Designing)
            {
                throw new InvalidOperationException("只有设计中的表单才可以删除！");
            }

            original.State = (int)WorkFlowFormState.Deleted;
            original.LastModifyTime = DateTime.Now;
        }

        [HttpPost]
        public async Task Save(Guid id, string designJson, string designFieldJson)
        {
            var original = await this.FindByKey(id);

            if (original.State != (int)WorkFlowFormState.Designing)
            {
                throw new InvalidOperationException("只有设计中的表单才可以保存！");
            }

            original.DesignJson = designJson;
            original.DesignFieldJson = designFieldJson;
            original.LastModifyTime = DateTime.Now;

            await this.Repository.UpdateAsync(original);
            await this.RepositoryContext.CommitAsync();
        }

        [HttpPost]
        public async Task<WorkFlowForm> SaveAs(Guid id, string name, string designJson, string designFieldJson)
        {
            var original = await this.FindByKey(id);

            var entity = JsonConvert.DeserializeObject<WorkFlowForm>(JsonConvert.SerializeObject(original));
            entity.Id = this.KeyGenerator.Generate(entity);
            entity.Name = name;
            entity.DesignJson = designJson;
            entity.DesignFieldJson = designFieldJson;
            entity.State = (int)WorkFlowFormState.Designing;
            entity.LastModifyTime = DateTime.Now;

            await this.Repository.AddAsync(entity);
            await this.RepositoryContext.CommitAsync();

            return new WorkFlowForm { Id = entity.Id };
        }

        [HttpPost]
        public async Task Published(Guid id, string designJson, string designFieldJson)
        {
            var original = await this.FindByKey(id);

            if (original.State != (int)WorkFlowFormState.Designing)
            {
                throw new InvalidOperationException("只有设计中的表单才可以发布！");
            }

            original.DesignJson = designJson;
            original.DesignFieldJson = designFieldJson;
            original.State = (int)WorkFlowFormState.Published;
            original.LastModifyTime = DateTime.Now;

            await this.Repository.UpdateAsync(original);
            await this.RepositoryContext.CommitAsync();
        }

        [HttpPost]
        public async Task UnPublished(Guid id)
        {
            var original = await this.FindByKey(id);

            if (original.State != (int)WorkFlowFormState.Published)
            {
                throw new InvalidOperationException("只有已发布的表单才可以回退发布！");
            }

            original.State = (int)WorkFlowFormState.Designing;
            original.LastModifyTime = DateTime.Now;

            await this.Repository.UpdateAsync(original);
            await this.RepositoryContext.CommitAsync();
        }
    }
}
