using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZDY.DMS.AspNetCore.Auth;
using ZDY.DMS.Repositories;
using ZDY.DMS.Services.WorkFlowService.Core.Enums;
using ZDY.DMS.Services.WorkFlowService.Models;
using ZDY.DMS.Services.WorkFlowService.ServiceContracts;

namespace ZDY.DMS.Web.Pages.WorkFlow
{
    public class WorkFlowStartUpModel : PageModel
    {
        private readonly IWorkFlowService workFlowService;
        private readonly IWorkFlowFormService workFlowFormService;

        public WorkFlowStartUpModel(IWorkFlowService workFlowService, IWorkFlowFormService workFlowFormService)
        {
            this.workFlowService = workFlowService;
            this.workFlowFormService = workFlowFormService;
        }

        public string Title { get; set; }

        public Services.WorkFlowService.Models.WorkFlow Flow { get; set; }

        public WorkFlowForm Form { get; set; }

        public async Task OnGetAsync(Guid id)
        {
            var workflow = await this.workFlowService.GetInstalledWorkFlowByKeyAsync(id);

            if (workflow == null)
            {
                ViewData["ErrorMessage"] = "流程数据丢失或未发布";
                return;
            }

            if (workflow.FormId == default)
            {
                ViewData["ErrorMessage"] = "流程没有绑定任何表单";
                return;
            }

            var form = await this.workFlowFormService.GetPublishedWorkFlowFormByKeyAsync(workflow.FormId);

            if (form == null)
            {
                ViewData["ErrorMessage"] = "表单数据丢失或未发布";
                return;
            }

            Flow = workflow;

            Form = form;

            Title = $"{this.HttpContext.GetUserIdentity().Name}的{workflow.Name}";
        }
    }
}