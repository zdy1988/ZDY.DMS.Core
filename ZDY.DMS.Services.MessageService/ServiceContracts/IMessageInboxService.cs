using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZDY.DMS.Services.MessageService.DataTransferObjects;
using ZDY.DMS.Services.MessageService.Models;

namespace ZDY.DMS.Services.MessageService.ServiceContracts
{
    public interface IMessageInboxService
    {
        Task AddMessageAsync(Message message, params Guid[] receiver);

        Task<List<MessageInboxDTO>> GetAllMessageAsync(Guid receiverId);
    }
}
