using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZDY.DMS.Repositories;
using ZDY.DMS.Services.WorkFlowService.Core.Extensions;
using ZDY.DMS.Services.WorkFlowService.Core.Interfaces;
using ZDY.DMS.Services.WorkFlowService.Core.Enums;
using ZDY.DMS.Services.WorkFlowService.Models;

namespace ZDY.DMS.Services.WorkFlowService.Core.Services
{
    public class PersistenceProvider : IPersistenceProvider
    {
        private readonly IRepositoryContext repositoryContext;
        private readonly IRepository<Guid, WorkFlow> workFlowRepository;
        private readonly IRepository<Guid, WorkFlowForm> workFlowFormRepository;
        private readonly IRepository<Guid, WorkFlowTask> workFlowTaskRepository;
        private readonly IRepository<Guid, WorkFlowInstance> workFlowInstanceRepository;

        public PersistenceProvider(Func<Type, IRepositoryContext> repositoryContextFactory)
        {
            this.repositoryContext = repositoryContextFactory.Invoke(typeof(WorkFlowServiceModule));

            this.workFlowRepository = this.repositoryContext.GetRepository<Guid, WorkFlow>();
            this.workFlowFormRepository = this.repositoryContext.GetRepository<Guid, WorkFlowForm>();
            this.workFlowTaskRepository = this.repositoryContext.GetRepository<Guid, WorkFlowTask>();
            this.workFlowInstanceRepository = this.repositoryContext.GetRepository<Guid, WorkFlowInstance>();
        }

        /// <summary>
        /// 获取一个流程数据
        /// </summary>
        public async Task<WorkFlow> GetWorkFlowAsync(Guid flowId)
        {
            return await workFlowRepository.FindAsync(t => t.Id == flowId && t.IsDisabled == false);
        }

        /// <summary>
        /// 获取一个表单数据
        /// </summary>
        public async Task<WorkFlowForm> GetWorkFlowFormAsync(Guid formId)
        {
            return await workFlowFormRepository.FindAsync(t => t.Id == formId && t.IsDisabled == false);
        }

        /// <summary>
        /// 获取一个流程实例
        /// </summary>
        public async Task<WorkFlowInstance> GetWorkFlowInstanceAsync(Guid instanceId)
        {
            return await workFlowInstanceRepository.FindAsync(t => t.Id == instanceId && t.IsDisabled == false);
        }

        /// <summary>
        /// 创建一个流程实例
        /// </summary>
        public async Task CreateInstanceAsync(WorkFlowInstance instance)
        {
            await workFlowInstanceRepository.AddAsync(instance);

            await repositoryContext.CommitAsync();
        }

        /// <summary>
        /// 删除一个流程实例
        /// </summary>
        public async Task RemoveInstanceAsync(Guid instanceId)
        {
            //删除实例
            var instance = await workFlowInstanceRepository.FindByKeyAsync(instanceId);

            if (instance != null)
            {
                instance.IsDisabled = true;

                await workFlowInstanceRepository.UpdateAsync(instance);

                //删除任务
                var tasks = await workFlowTaskRepository.FindAllAsync(t => t.InstanceId == instanceId);

                if (tasks.Count() > 0)
                {
                    foreach (var task in tasks)
                    {
                        task.IsDisabled = true;

                        await workFlowTaskRepository.UpdateAsync(task);
                    }
                }
            }

            await repositoryContext.CommitAsync();
        }

        /// <summary>
        /// 更新一个流程实例
        /// </summary>
        public async Task UpdateInstanceAsync(WorkFlowInstance instance)
        {
            await workFlowInstanceRepository.UpdateAsync(instance);

            await repositoryContext.CommitAsync();
        }

        /// <summary>
        /// 获取一个流程任务
        /// </summary>
        public async Task<WorkFlowTask> GetWorkFlowTaskAsync(Guid taskId)
        {
            return await workFlowTaskRepository.FindAsync(t => t.Id == taskId && t.IsDisabled == false);
        }

        /// <summary>
        /// 获取发布子流程的根流程任务
        /// </summary>
        public async Task<WorkFlowTask> GetRootInstanceTaskAsync(Guid instanceId, Guid groupId)
        {
            var parentTask = await workFlowTaskRepository.FindAsync(t => t.GroupId == groupId
                                                                      && t.SubFlowInstanceId == instanceId
                                                                      && t.Type != (int)WorkFlowTaskKinds.Copy
                                                                      && t.IsDisabled == false);

            return parentTask;
        }

        /// <summary>
        /// 新建一个任务
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public async Task CreateTaskAsync(WorkFlowTask task)
        {
            await workFlowTaskRepository.AddAsync(task);

            await repositoryContext.CommitAsync();
        }

        /// <summary>
        /// 删除一个任务
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public async Task RemoveTaskAsync(WorkFlowTask task)
        {
            await workFlowTaskRepository.RemoveAsync(task);

            await repositoryContext.CommitAsync();
        }

        /// <summary>
        /// 更新一个任务
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public async Task UpdateTaskAsync(WorkFlowTask task)
        {
            await workFlowTaskRepository.UpdateAsync(task);

            await repositoryContext.CommitAsync();
        }

        /// <summary>
        /// 获取一个实例中的所有任务
        /// </summary>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        public async Task<List<WorkFlowTask>> GetAllTaskAsync(Guid instanceId)
        {
            var list = await this.workFlowTaskRepository.FindAllAsync(t => t.InstanceId == instanceId && t.IsDisabled == false);

            return list.AsEnumerable().Where(t => t.IsNotCopy()).ToList();
        }
    }
}
