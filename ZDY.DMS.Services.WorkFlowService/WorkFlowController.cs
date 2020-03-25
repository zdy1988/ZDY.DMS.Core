using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZDY.DMS.AspNetCore.Mvc;
using ZDY.DMS.Domain.Enums;
using ZDY.DMS.KeyGeneration;
using ZDY.DMS.Models;
using ZDY.DMS.Repositories;

namespace ZDY.DMS.Services.WorkFlowService
{
    public class WorkFlowController : ApiDataServiceController<Guid, WorkFlow>
    {
        public WorkFlowController(IRepositoryContext repositoryContext)
            : base(repositoryContext, new GuidKeyGenerator())
        {

        }

        protected override void BeforeAdd(WorkFlow entity)
        {
            if (this.Repository.Exists(t => t.CompanyId == this.UserIdentity.CompanyId && t.Name == entity.Name))
            {
                throw new InvalidOperationException("这个流程名称已存在");
            }

            entity.CompanyId = this.UserIdentity.CompanyId;
            entity.CreaterId = this.UserIdentity.Id;
            entity.State = (int)WorkFlowState.Designing;
        }

        protected override void BeforeUpdate(WorkFlow original, WorkFlow entity)
        {
            original.FormId = entity.FormId;
            original.Name = entity.Name;
            original.Type = entity.Type;
            original.ManagerId = entity.ManagerId;
            original.InstanceManagerId = entity.InstanceManagerId;
            original.IsRemoveCompletedInstance = entity.IsRemoveCompletedInstance;
            original.Note = entity.Note;
        }

        public async override Task Delete(Guid id)
        {
            var original = await this.Repository.FindByKeyAsync(id);

            original.IsDisabled = true;
            original.State = (int)WorkFlowState.Deleted;

            this.Repository.Update(original);
            this.Repository.Context.Commit();
        }

        [HttpPost]
        public async Task Save(Guid id, string designJson)
        {
            var entity = await this.FindByKey(id);

            if (entity.State != (int)WorkFlowState.Designing)
            {
                throw new InvalidOperationException("只有设计中的流程才可以保存！");
            }

            entity.DesignJson = designJson;
            entity.State = (int)WorkFlowState.Designing;

            await this.Repository.UpdateAsync(entity);
            await this.Repository.Context.CommitAsync();
        }

        [HttpPost]
        public async Task<WorkFlow> SaveAs(Guid id, string name, string designJson)
        {
            var entity = await this.FindByKey(id);
            var newEntity = JsonConvert.DeserializeObject<WorkFlow>(JsonConvert.SerializeObject(entity));
            newEntity.Id = this.KeyGenerator.Generate(newEntity);
            newEntity.Name = name;
            newEntity.DesignJson = designJson;
            return await this.Add(newEntity);
        }

        [HttpPost]
        public async Task<IActionResult> Installed(Guid id, string designJson, string runJson)
        {
            var entity = await this.FindByKey(id);

            if (entity.State != (int)WorkFlowState.Designing)
            {
                throw new InvalidOperationException("只有设计中的流程才可以安装！");
            }

            var workFlowInstalled = WorkFlowAnalysis.AnalyticWorkFlowInstalledData(runJson);

            var messages = WorkFlowAnalysis.CheckFlow(workFlowInstalled);

            if (messages.Count() > 0)
            {
                return Ok(new { IsInstallSuccess = false, Messages = messages });
            }
            else
            {
                entity.DesignJson = designJson;
                entity.RuntimeJson = runJson;
                entity.State = (int)WorkFlowState.Installed;
                entity.InstallerId = this.UserIdentity.Id;
                entity.InstallTime = DateTime.Now;

                await this.Repository.UpdateAsync(entity);
                await this.Repository.Context.CommitAsync();

                //安装流程
                await entity.Install();

                return Ok(new { IsInstallSuccess = true, Messages = messages });
            }
        }

        [HttpPost]
        public async Task UnInstalled(Guid id)
        {
            var entity = await this.FindByKey(id);

            entity.RuntimeJson = null;
            entity.State = (int)WorkFlowState.Designing;
            entity.InstallerId = default(Guid);
            entity.InstallTime = null;

            await this.Repository.UpdateAsync(entity);
            await this.Repository.Context.CommitAsync();

            //卸载流程
            await entity.UnInstall();
        }
    }
}
