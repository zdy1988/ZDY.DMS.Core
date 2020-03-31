using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZDY.DMS.AspNetCore.Mvc;
using ZDY.DMS.KeyGeneration;
using ZDY.DMS.Repositories;
using ZDY.DMS.Services.WorkFlowService.Enums;
using ZDY.DMS.Services.WorkFlowService.Models;

namespace ZDY.DMS.Services.WorkFlowService
{
    public class WorkFlowController : ApiDataServiceController<Guid, WorkFlow, WorkFlowServiceModule>
    {
        public WorkFlowController(Func<Type, IRepositoryContext> repositoryContextFactory)
            : base(repositoryContextFactory, new GuidKeyGenerator())
        {

        }

        protected override void BeforeAdd(Models.WorkFlow entity)
        {
            if (this.Repository.Exists(t => t.CompanyId == this.UserIdentity.CompanyId && t.Name == entity.Name))
            {
                throw new InvalidOperationException("这个流程名称已存在");
            }

            entity.CompanyId = this.UserIdentity.CompanyId;
            entity.CreaterId = this.UserIdentity.Id;
            entity.State = (int)WorkFlowState.Designing;
        }

        protected override void BeforeUpdate(Models.WorkFlow original, Models.WorkFlow entity)
        {
            if (original.State != (int)WorkFlowState.Designing)
            {
                throw new InvalidOperationException("只有设计中的流程才可以修改！");
            }

            original.FormId = entity.FormId;
            original.Name = entity.Name;
            original.Type = entity.Type;
            original.Managers = entity.Managers;
            original.InstanceManagers = entity.InstanceManagers;
            original.IsRemoveCompletedInstance = entity.IsRemoveCompletedInstance;
            original.Note = entity.Note;
            original.LastModifyTime = DateTime.Now;
        }

        protected override void BeforeDelete(Models.WorkFlow original)
        {
            if (original.State != (int)WorkFlowState.Designing)
            {
                throw new InvalidOperationException("只有设计中的流程才可以删除！");
            }

            original.State = (int)WorkFlowState.Deleted;
            original.LastModifyTime = DateTime.Now;
        }

        [HttpPost]
        public async Task Save(Guid id, string designJson)
        {
            var original = await this.FindByKey(id);

            if (original.State != (int)WorkFlowState.Designing)
            {
                throw new InvalidOperationException("只有设计中的流程才可以保存！");
            }

            original.DesignJson = designJson;
            original.LastModifyTime = DateTime.Now;

            await this.Repository.UpdateAsync(original);
            await this.Repository.Context.CommitAsync();
        }

        [HttpPost]
        public async Task<Models.WorkFlow> SaveAs(Guid id, string name, string designJson)
        {
            var original = await this.FindByKey(id);
            var entity = JsonConvert.DeserializeObject<Models.WorkFlow>(JsonConvert.SerializeObject(original));

            entity.Id = this.KeyGenerator.Generate(entity);
            entity.Name = name;
            entity.DesignJson = designJson;
            entity.State = (int)WorkFlowState.Designing;
            entity.LastModifyTime = DateTime.Now;

            await this.Repository.AddAsync(entity);
            await this.RepositoryContext.CommitAsync();

            return new Models.WorkFlow { Id = entity.Id };
        }

        [HttpPost]
        public async Task<IActionResult> Installed(Guid id, string designJson, string runtimeJson)
        {
            var original = await this.FindByKey(id);

            if (original.State != (int)WorkFlowState.Designing)
            {
                throw new InvalidOperationException("只有设计中的流程才可以安装！");
            }

            var workFlowInstalled = WorkFlowAnalyzing.WorkFlowInstalledDeserialize(runtimeJson);

            var messages = WorkFlowAnalyzing.CheckFlow(workFlowInstalled);

            if (messages.Count() > 0)
            {
                return Ok(new { IsInstallSuccess = false, Messages = messages });
            }
            else
            {
                original.DesignJson = designJson;
                original.RuntimeJson = runtimeJson;
                original.State = (int)WorkFlowState.Installed;
                original.InstallerId = this.UserIdentity.Id;
                original.InstallTime = DateTime.Now;
                original.LastModifyTime = DateTime.Now;

                await this.Repository.UpdateAsync(original);
                await this.Repository.Context.CommitAsync();

                //安装流程
                //await original.Install();

                return Ok(new { IsInstallSuccess = true, Messages = messages });
            }
        }

        [HttpPost]
        public async Task UnInstalled(Guid id)
        {
            var original = await this.FindByKey(id);

            if (original.State != (int)WorkFlowState.Installed)
            {
                throw new InvalidOperationException("只有已安装的流程才可以卸载！");
            }

            original.RuntimeJson = null;
            original.State = (int)WorkFlowState.Designing;
            original.InstallerId = default;
            original.InstallTime = null;
            original.LastModifyTime = DateTime.Now;

            await this.Repository.UpdateAsync(original);
            await this.Repository.Context.CommitAsync();

            //卸载流程
            //await original.UnInstall();
        }
    }
}
