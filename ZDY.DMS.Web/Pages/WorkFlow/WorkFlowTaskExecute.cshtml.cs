using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZDY.DMS.Repositories;
using ZDY.DMS.Services.Common.DataTransferObjects;
using ZDY.DMS.Services.Common.ServiceContracts;
using ZDY.DMS.Services.WorkFlowService;
using ZDY.DMS.Services.WorkFlowService.DataObjects;
using ZDY.DMS.Services.WorkFlowService.Enums;
using ZDY.DMS.Services.WorkFlowService.Models;
using ZDY.DMS.Services.WorkFlowService.ServiceContracts;

namespace ZDY.DMS.Web.Pages.WorkFlow
{
    public class WorkFlowTaskExecuteModel : PageModel
    {
        private readonly IDictionaryService dictionaryService;
        private readonly SelectOptionsFactory selectOptionsFactory;
        private readonly IRepositoryContext repositoryContext;
        private readonly IRepository<Guid, WorkFlowTask> workFlowTaskServiceRepository;
        private readonly IRepository<Guid, WorkFlowInstance> workFlowInstanceRepository;
        private readonly IWorkFlowHostService workFlowHostService;

        public WorkFlowTaskExecuteModel(IRepositoryContext repositoryContext,
            IDictionaryService dictionaryService,
             SelectOptionsFactory selectOptionsFactory,
            IWorkFlowHostService workFlowHostService)
        {
            this.repositoryContext = repositoryContext;
            this.dictionaryService = dictionaryService;
            this.selectOptionsFactory = selectOptionsFactory;
            this.workFlowTaskServiceRepository = repositoryContext.GetRepository<Guid, WorkFlowTask>();
            this.workFlowInstanceRepository = repositoryContext.GetRepository<Guid, WorkFlowInstance>();
            this.workFlowHostService = workFlowHostService;
        }

        public Dictionary<string, IEnumerable<DictionaryItemDTO>> Dictionary { get; set; }

        public IEnumerable<KeyValuePair<string, string>> UserOptions { get; set; }

        public WorkFlowInstance Instance { get; set; }
        public WorkFlowTask CurrentTask { get; set; }
        public WorkFlowStep CurrentStep { get; set; }
        public List<WorkFlowStep> NextSteps { get; set; } = new List<WorkFlowStep>();
        public List<WorkFlowTask> HasCommentTasks { get; set; } = new List<WorkFlowTask>();

        public bool CanExecute { get; set; }

        public async Task OnGetAsync(Guid id)
        {
            var taskEntity = await workFlowTaskServiceRepository.FindByKeyAsync(id);
            if (taskEntity == null)
            {
                ViewData["ErrorMessage"] = "任务数据未找到或丢失";
                return;
            }

            var workFlowInstanceEntity = await workFlowInstanceRepository.FindAsync(t => t.Id == taskEntity.InstanceId);
            if (workFlowInstanceEntity == null)
            {
                ViewData["ErrorMessage"] = "流程实例未找到或丢失";
                return;
            }

            //获得当前步骤信息
            Instance = workFlowInstanceEntity;
            CurrentTask = taskEntity;
            var workFlowInstalled = Services.WorkFlowService.WorkFlowAnalyzing.WorkFlowInstalledDeserialize(Instance.FlowRuntimeJson);
            CurrentStep = workFlowInstalled.GetStep(CurrentTask.StepId);


            //在当前步骤不是最后一个和流传控制为系统控制的时候，不需要加载下一步信息
            //如果下一步有可选择任意人员，则加载下一步
            var nextSteps = workFlowInstalled.GetNextSteps(CurrentTask.StepId);
            var isHandleByAnyUser = nextSteps.Any(t => t.IsHandleBy(WorkFlowHandlerKinds.AnyUser));
            if ((!CurrentStep.IsControlBy(WorkFlowControlKinds.System) && !CurrentStep.IsEnd()) || isHandleByAnyUser)
            {
                NextSteps = nextSteps;
            }

            //加载审批意见
            if (CurrentStep.IsShowComment)
            {
                HasCommentTasks = await workFlowHostService.GetWorkFlowCommentsAsync(workFlowInstanceEntity);
            }

            //更新打开时间
            if (taskEntity.Is(WorkFlowTaskState.Pending))
            {
                taskEntity.State = (int)WorkFlowTaskState.Opened;
                taskEntity.OpenedTime = DateTime.Now;
                workFlowTaskServiceRepository.Update(taskEntity);
            }


            Dictionary = dictionaryService.GetDictionary("WorkFlowTaskState");

            UserOptions = await this.selectOptionsFactory.GetSelectUserOptions();

            CanExecute = CurrentTask.Is(WorkFlowTaskState.Pending, WorkFlowTaskState.Opened);
        }
    }
}