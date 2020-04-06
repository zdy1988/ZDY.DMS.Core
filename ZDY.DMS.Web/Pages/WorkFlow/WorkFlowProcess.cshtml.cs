using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZDY.DMS.Repositories;
using ZDY.DMS.Services.WorkFlowService.Core.Models;
using ZDY.DMS.Services.WorkFlowService.Models;
using ZDY.DMS.Services.WorkFlowService.ServiceContracts;

namespace ZDY.DMS.Web.Pages.WorkFlow
{
    public class WorkFlowProcessModel : PageModel
    {
        private readonly IRepositoryContext  repositoryContext;
        private readonly IRepository<Guid, WorkFlowInstance> workFlowInstanceRepository;
        private readonly IWorkFlowHostService  workFlowHostService;

        public WorkFlowProcessModel(IRepositoryContext repositoryContext,
            IWorkFlowHostService workFlowHostService)
        {
            this.repositoryContext = repositoryContext;
            this.workFlowInstanceRepository = repositoryContext.GetRepository<Guid, WorkFlowInstance>();
            this.workFlowHostService = workFlowHostService;
        }

        public WorkFlowInstance Instance { get; set; }
        public List<WorkFlowTask> Process { get; set; } = new List<WorkFlowTask>();
        public Dictionary<int, List<WorkFlowStep>> States { get; set; } = new Dictionary<int, List<WorkFlowStep>>();

        public async Task OnGetAsync(Guid id)
        {
            var workFlowInstanceEntity = await workFlowInstanceRepository.FindAsync(t => t.Id == id);
            if (workFlowInstanceEntity == null)
            {
                ViewData["ErrorMessage"] = "流程实例未找到或丢失";
                return;
            }

            Instance = workFlowInstanceEntity;

            Process = await workFlowHostService.GetWorkFlowProcessAsync(Instance.Id);

            States = await workFlowHostService.GetWorkFlowProcessStatesAsync(Instance);
        }
    }
}