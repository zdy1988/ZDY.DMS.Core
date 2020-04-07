using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZDY.DMS.Repositories;
using ZDY.DMS.Services.WorkFlowService.Core.Interfaces;
using ZDY.DMS.Services.WorkFlowService.Models;
using ZDY.DMS.StringEncryption;

namespace ZDY.DMS.Services.WorkFlowService.Core.Services
{
    public class SignatureProvider : ISignatureProvider
    {
        private readonly IRepositoryContext repositoryContext;
        private readonly IStringEncryption stringEncryption;

        private readonly IRepository<Guid, WorkFlowSignature> workFlowSignatureRepository;

        public SignatureProvider(Func<Type, IRepositoryContext> repositoryContextFactory,
            IStringEncryption stringEncryption)
        {
            this.repositoryContext = repositoryContextFactory.Invoke(typeof(WorkFlowServiceModule));

            this.stringEncryption = stringEncryption;

            this.workFlowSignatureRepository = this.repositoryContext.GetRepository<Guid, WorkFlowSignature>();
        }

        public async Task<bool> TrySignatureAsync(Guid userId, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            password = stringEncryption.Encrypt(password);

            return await workFlowSignatureRepository.ExistsAsync(t => t.UserId == userId && t.Password == password);
        }
    }
}
