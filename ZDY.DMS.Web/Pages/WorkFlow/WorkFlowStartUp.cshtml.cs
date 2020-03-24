using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZDY.DMS.AspNetCore.Auth;
using ZDY.DMS.Domain.Enums;
using ZDY.DMS.Models;
using ZDY.DMS.Repositories;
using ZDY.DMS.Services.WorkFlowService.ServiceContracts;

namespace ZDY.DMS.Web.Pages.WorkFlow
{
    public class WorkFlowStartUpModel : PageModel
    {
        private readonly IRepositoryContext repositoryContext;
        private readonly IRepository<Guid, WorkFlowInstance> workFlowInstanceRepository;
        private readonly IRepository<Guid, WorkFlowForm> workFlowFormRepository;
        private readonly IRepository<Guid, Models.WorkFlow> workFlowRepository;

        public WorkFlowStartUpModel(IRepositoryContext repositoryContext,
            IWorkFlowFormService workFlowFormService,
            IWorkFlowService workFlowService)
        {
            this.repositoryContext = repositoryContext;
            this.workFlowInstanceRepository = repositoryContext.GetRepository<Guid, WorkFlowInstance>();
            this.workFlowFormRepository = repositoryContext.GetRepository<Guid, WorkFlowForm>();
            this.workFlowRepository = repositoryContext.GetRepository<Guid, Models.WorkFlow>();
        }

        public Models.WorkFlow Flow { get; set; }

        public WorkFlowForm Form { get; set; }

        public async void OnGet(Guid id)
        {
            var workflow = await this.workFlowRepository.FindAsync(t => t.Id == id && t.State == (int)WorkFlowState.Installed);
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
            var form = await this.workFlowFormRepository.FindAsync(t => t.Id == workflow.FormId && t.State == (int)WorkFlowFormState.Published);
            if (form == null)
            {
                ViewData["ErrorMessage"] = "表单数据丢失或未发布";
                return;
            }

            Flow = workflow;
            Form = form;

            ViewData["Title"] = $"{this.HttpContext.GetUserIdentity().Name}的{workflow.Name}";
        }
    }
}