using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZDY.DMS.Models;
using ZDY.DMS.Repositories;
using ZDY.DMS.Services.WorkFlowService.DataObjects;
using ZDY.DMS.Services.WorkFlowService.ServiceContracts;

namespace ZDY.DMS.Web.Pages.WorkFlow
{
    public class WorkFlowProcessModel : PageModel
    {
        private readonly IRepositoryContext  repositoryContext;
        private readonly IRepository<Guid, WorkFlowInstance> workFlowInstanceRepository;
        private readonly IWorkFlowWorkingService  workFlowWorkingService;

        public WorkFlowProcessModel(IRepositoryContext repositoryContext,
            IWorkFlowWorkingService workFlowWorkingService)
        {
            this.repositoryContext = repositoryContext;
            this.workFlowInstanceRepository = repositoryContext.GetRepository<Guid, WorkFlowInstance>();
            this.workFlowWorkingService = workFlowWorkingService;
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

            Process = await workFlowWorkingService.GetWorkFlowProcessAsync(Instance);

            States = await workFlowWorkingService.GetWorkFlowProcessStatesAsync(Instance);
        }
    }
}