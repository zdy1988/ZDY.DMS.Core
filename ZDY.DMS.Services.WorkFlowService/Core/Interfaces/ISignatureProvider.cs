using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ZDY.DMS.Services.WorkFlowService.Core.Interfaces
{
    public interface ISignatureProvider
    {
        Task<bool> TrySignatureAsync(Guid userId, string password);
    }
}
