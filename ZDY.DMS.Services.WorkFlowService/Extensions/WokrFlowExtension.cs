using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZDY.DMS;
using ZDY.DMS.Extensions.DependencyInjection.Autofac;
using ZDY.DMS.Models;
using ZDY.DMS.Repositories;
using ZDY.DMS.ServiceContracts;

namespace ZDY.DMS.Services.WorkFlowService
{
    public static class WorkFlowExtension
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
                    Src = $"WorkflowManage/WorkflowStartUp?id={workFlow.Id}",
                    IsDisabled = false
                };

                await pageRepository.AddAsync(flowPage);
            }
        }

        public static async Task UnInstall(this WorkFlow workFlow)
        {
            var pageRepository = ServiceLocator.GetService<IRepositoryContext>().GetRepository<Guid, Page>();

            await pageRepository.RemoveAsync(t => t.PageCode == workFlow.Id.ToString());
        }
    }
}
