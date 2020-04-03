using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZDY.DMS.AspNetCore.Messaging;
using ZDY.DMS.Repositories;
using ZDY.DMS.Services.AdminService.Models;
using ZDY.DMS.Services.Common.Events;

namespace ZDY.DMS.Services.AdminService.EventHandlers
{
    public class WorkFlowUnInstalledEventHandler : EventHandlerBase<AdminServiceModule, WorkFlowUnInstalledEvent>
    {
        public WorkFlowUnInstalledEventHandler(Func<Type, IRepositoryContext> repositoryContextFactory)
            : base(repositoryContextFactory)
        {

        }

        public async override Task<bool> HandleAsync(WorkFlowUnInstalledEvent message, CancellationToken cancellationToken = default)
        {
            var pageRepository = this.GetRepository<Guid, Page>();

            var flowPage = await pageRepository.FindAsync(t => t.PageCode == message.FlowId.ToString());

            if (flowPage != null)
            {
                await pageRepository.RemoveAsync(flowPage);
                await pageRepository.Context.CommitAsync();
            }

            return true;
        }
    }
}
