using System;
using System.Threading.Tasks;
using ZDY.DMS.Repositories;
using ZDY.DMS.Services.Common.Models;
using ZDY.DMS.Services.WorkFlowService.Models;

namespace ZDY.DMS.Services.WorkFlowService
{
    public static class WorkFlowExtensions
    {
        public static async Task Install(this WorkFlow workFlow)
        {
            var pageRepository = ServiceLocator.GetService<IRepositoryContext>().GetRepository<Guid, Page>();

            var parent = await pageRepository.FindAsync(t => t.PageCode == "N_WorkflowStartUp");

            if (parent != null)
            {
                var flowPage = new Page
                {
                    CompanyId = workFlow.CompanyId,
                    PageName = workFlow.Name,
                    PageCode = workFlow.Id.ToString(),
                    ParentId = parent.Id,
                    Icon = "icon-share",
                    IsInMenu = true,
                    IsPassed = true,
                    Level = 0,
                    MenuName = workFlow.Name,
                    Order = 0,
                    Type = "P",
                    Src = $"WorkFlow/WorkFlowStartUp?id={workFlow.Id}",
                    IsDisabled = false
                };

                await pageRepository.AddAsync(flowPage);
                await pageRepository.Context.CommitAsync();
            }
        }

        public static async Task UnInstall(this WorkFlow workFlow)
        {
            var pageRepository = ServiceLocator.GetService<IRepositoryContext>().GetRepository<Guid, Page>();

            var flowPage = await pageRepository.FindAsync(t => t.PageCode == workFlow.Id.ToString());

            if (flowPage != null)
            {
                await pageRepository.RemoveAsync(flowPage);
                await pageRepository.Context.CommitAsync();
            }
        }
    }
}
