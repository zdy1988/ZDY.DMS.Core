using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZDY.DMS.AspNetCore.Mvc;
using ZDY.DMS.Services.WorkFlowService.Core.Models;
using ZDY.DMS.Services.WorkFlowService.Core.Enums;
using ZDY.DMS.Services.WorkFlowService.Models;
using ZDY.DMS.Services.WorkFlowService.ServiceContracts;

namespace ZDY.DMS.Services.WorkFlowService
{
    public class WorkFlowHostController : ApiController<WorkFlowServiceModule>
    {
        private readonly IWorkFlowHostService workFlowHostService;

        public WorkFlowHostController(IWorkFlowHostService workFlowHostService)
        {
            this.workFlowHostService = workFlowHostService;
        }

        [HttpPost]
        public async Task StartUp(WorkFlowInstance instance)
        {
            var userIdentity = this.UserIdentity;

            instance.CompanyId = userIdentity.CompanyId;
            instance.CreaterId = userIdentity.Id;
            instance.CreaterName = userIdentity.Name;

            await workFlowHostService.StartUp(instance);
        }

        private async Task Excute(WorkFlowExecute execute)
        {
            if (execute.TaskId == default || execute.TaskId == null)
            {
                throw new InvalidOperationException("流程参数有误");
            }

            //获取当前用户信息
            var userIdentity = this.UserIdentity;

            execute.Sender = new WorkFlowUser
            {
                Id = userIdentity.Id,
                Name = userIdentity.Name,
                CompanyId = userIdentity.CompanyId
            };

            //处理步骤
            await workFlowHostService.Execute(execute);
        }

        [HttpPost]
        public async Task ExcuteSubmit(WorkFlowExecute execute)
        {
            execute.ExecuteType = WorkFlowExecuteKinds.Submit;

            await Excute(execute);
        }

        [HttpPost]
        public async Task ExcuteBack(WorkFlowExecute execute)
        {
            execute.ExecuteType = WorkFlowExecuteKinds.Back;

            await Excute(execute);
        }

        [HttpPost]
        public async Task ExcuteRedirect(WorkFlowExecute execute)
        {
            execute.ExecuteType = WorkFlowExecuteKinds.Redirect;

            await Excute(execute);
        }
    }
}
