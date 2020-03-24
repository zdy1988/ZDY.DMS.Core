using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZDY.DMS.AspNetCore.Mvc;
using ZDY.DMS.Domain.Enums;
using ZDY.DMS.KeyGeneration;
using ZDY.DMS.Models;
using ZDY.DMS.Repositories;

namespace ZDY.DMS.Services.WorkFlowService
{
    public class WorkFlowFormController : ApiDataServiceController<Guid, WorkFlowForm>
    {
        public WorkFlowFormController(IRepositoryContext repositoryContext)
            : base(repositoryContext, new GuidKeyGenerator())
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
            original.Name = entity.Name;
            original.Type = entity.Type;
            original.Note = entity.Note;
        }

        protected override void BeforeDelete(WorkFlowForm workFlow)
        {
            workFlow.State = (int)WorkFlowFormState.Deleted;
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
            await this.Repository.UpdateAsync(original);
            await this.RepositoryContext.CommitAsync();
        }

        [HttpPost]
        public async Task SaveAs(Guid id, string name, string designJson, string designFieldJson)
        {
            var original = await this.FindByKey(id);

            var entity = JsonConvert.DeserializeObject<WorkFlowForm>(JsonConvert.SerializeObject(original));
            entity.Id = this.KeyGenerator.Generate(entity);
            entity.Name = name;
            entity.DesignJson = designJson;
            entity.DesignFieldJson = designFieldJson;
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
            await this.Repository.UpdateAsync(original);
            await this.RepositoryContext.CommitAsync();
        }

        [HttpPost]
        public async Task UnPublished(Guid id)
        {
            var entity = await this.FindByKey(id);

            if (entity.State != (int)WorkFlowFormState.Published)
            {
                throw new InvalidOperationException("只有已发布的表单才可以取消发布！");
            }

            entity.State = (int)WorkFlowFormState.Designing;

            await this.Repository.UpdateAsync(entity);
            await this.RepositoryContext.CommitAsync();
        }
    }
}
