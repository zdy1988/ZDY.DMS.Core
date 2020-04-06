using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZDY.DMS.Services.WorkFlowService.Core.Interfaces;

namespace ZDY.DMS.Services.WorkFlowService.Core.Services
{
    public class SignatureProvider : ISignatureProvider
    {
        public Task<bool> TrySignatureAsync()
        {
            return Task.FromResult(true);
        }
    }
}
