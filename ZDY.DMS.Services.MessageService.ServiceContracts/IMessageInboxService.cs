using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZDY.DMS.Querying.SearchModel.Model;
using ZDY.DMS.Services.MessageService.DataTransferObjects;
using ZDY.DMS.Services.MessageService.Models;

namespace ZDY.DMS.Services.MessageService.ServiceContracts
{
    public interface IMessageInboxService
    {
        Task AddMessageAsync(Message message, params Guid[] receiver);

        Tuple<IEnumerable<MessageInboxDTO>, int> GetAllMessage(Guid receiverId, QueryModel queryModel, int pageIndex, int pageSize, string orderField, bool isAsc);
    }
}
