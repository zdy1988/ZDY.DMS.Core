using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZDY.DMS.AspNetCore.Dictionary;
using ZDY.DMS.Repositories;
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
        private readonly IWorkFlowTaskService workFlowTaskService;
        private readonly IWorkFlowInstanceService workFlowInstanceService;
        private readonly IWorkFlowHostService workFlowHostService;

        public WorkFlowTaskExecuteModel(
            IDictionaryService dictionaryService,
            SelectOptionsFactory selectOptionsFactory,
            IWorkFlowTaskService workFlowTaskService,
            IWorkFlowInstanceService workFlowInstanceService,
            IWorkFlowHostService workFlowHostService)
        {
            this.dictionaryService = dictionaryService;
            this.selectOptionsFactory = selectOptionsFactory;
            this.workFlowTaskService = workFlowTaskService;
            this.workFlowInstanceService = workFlowInstanceService;
            this.workFlowHostService = workFlowHostService;
        }

        public Dictionary<string, IEnumerable<KeyValuePaired>> Dictionary { get; set; }

        public IEnumerable<KeyValuePair<string, string>> UserOptions { get; set; }

        public WorkFlowInstance Instance { get; set; }
        public WorkFlowTask CurrentTask { get; set; }
        public WorkFlowStep CurrentStep { get; set; }
        public List<WorkFlowStep> NextSteps { get; set; } = new List<WorkFlowStep>();
        public List<WorkFlowTask> HasCommentTasks { get; set; } = new List<WorkFlowTask>();

        public bool CanExecute { get; set; }

        public async Task OnGetAsync(Guid id)
        {
            var task = await this.workFlowTaskService.GetWorkFlowTaskByKeyAsync(id);
            if (task == null)
            {
                ViewData["ErrorMessage"] = "任务数据未找到或丢失";
                return;
            }

            var instance = await this.workFlowInstanceService.GetWorkFlowInstanceByKeyAsync(task.InstanceId);
            if (instance == null)
            {
                ViewData["ErrorMessage"] = "流程实例未找到或丢失";
                return;
            }

            //获得当前步骤信息
            Instance = instance;
            CurrentTask = task;
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
                HasCommentTasks = await workFlowHostService.GetWorkFlowCommentsAsync(instance);
            }

            //更新打开时间
            if (task.Is(WorkFlowTaskState.Pending))
            {
                await this.workFlowTaskService.WorkFlowTaskOpenedAsync(task.Id);
            }

            Dictionary = dictionaryService.GetDictionary("WorkFlowTaskState"); 

            UserOptions = await this.selectOptionsFactory.GetSelectUserOptions();

            CanExecute = CurrentTask.Is(WorkFlowTaskState.Pending, WorkFlowTaskState.Opened);
        }
    }
}