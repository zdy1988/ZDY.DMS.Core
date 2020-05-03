using System;
using System.Threading;
using System.Threading.Tasks;
using ZDY.DMS.AspNetCore.Messaging;
using ZDY.DMS.Services.AdminService.Models;
using ZDY.DMS.Services.Shared.Events;

namespace ZDY.DMS.Services.AdminService.EventHandlers
{
    public class WorkFlowInstalledEventHandler : EventHandlerBase<AdminServiceModule, WorkFlowInstalledEvent>
    {
        public WorkFlowInstalledEventHandler()
        { 
            
        }

        public async override Task<bool> HandleAsync(WorkFlowInstalledEvent message, CancellationToken cancellationToken = default)
        {
            var pageRepository = this.GetRepository<Guid, Page>();

            if (await pageRepository.ExistsAsync(t => t.PageCode == message.FlowId.ToString()))
            {
                return false;
            }

            var parent = await pageRepository.FindAsync(t => t.PageCode == "N_WorkflowStartUp");

            if (parent != null)
            {
                var flowPage = new Page
                {
                    CompanyId = message.CompanyId,
                    PageName = message.FlowName,
                    PageCode = message.FlowId.ToString(),
                    ParentId = parent.Id,
                    MenuName = message.FlowName,
                    Icon = "icon-share",
                    IsInMenu = true,
                    IsPermissionRequired = false,
                    Order = 0,
                    Src = $"WorkFlow/WorkFlowStartUp?id={message.FlowId}",
                    IsDisabled = false
                };

                await pageRepository.AddAsync(flowPage);
                await pageRepository.Context.CommitAsync();

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
