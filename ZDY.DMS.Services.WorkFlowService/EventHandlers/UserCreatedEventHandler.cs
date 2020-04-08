using System;
using System.Threading;
using System.Threading.Tasks;
using ZDY.DMS.AspNetCore.Messaging;
using ZDY.DMS.Repositories;
using ZDY.DMS.Services.Common.Events;
using ZDY.DMS.Services.WorkFlowService.Models;

namespace ZDY.DMS.Services.WorkFlowService.EventHandlers
{
    public class UserCreatedEventHandler : EventHandlerBase<WorkFlowServiceModule, UserCreatedEvent>
    {
        private readonly IRepository<Guid, WorkFlowSignature> workFlowSignatureRepository;

        public UserCreatedEventHandler()
        {
            this.workFlowSignatureRepository = this.GetRepository<Guid, WorkFlowSignature>();
        }

        public async override Task<bool> HandleAsync(UserCreatedEvent message, CancellationToken cancellationToken = default)
        {
            var isExist = await this.workFlowSignatureRepository.ExistsAsync(t => t.UserId == message.UserId && t.Password == message.EncryptedPassword);

            if (!isExist)
            {
                await this.workFlowSignatureRepository.AddAsync(new WorkFlowSignature
                {
                    UserId = message.UserId,
                    Password = message.EncryptedPassword,
                    Name = message.Name,
                    CompanyId = message.ConpanyId
                });

                await this.RepositoryContext.CommitAsync();
            }

            return true;
        }
    }
}
