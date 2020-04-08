using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ZDY.DMS.AspNetCore.Auth;
using ZDY.DMS.AspNetCore.Mvc;
using ZDY.DMS.DataPermission;
using ZDY.DMS.Services.MessageService.DataTransferObjects;
using ZDY.DMS.Services.MessageService.ServiceContracts;

namespace ZDY.DMS.Services.MessageService.Controllers
{
    public class MessageInboxController : ApiController<MessageServiceModule>
    {
        private readonly IMessageInboxService messageInboxService;

        public MessageInboxController(IMessageInboxService messageInboxService)
        {
            this.messageInboxService = messageInboxService;
        }

        [HttpPost]
        public Tuple<IEnumerable<MessageInboxDTO>, int> Search(SearchModel searchModel)
        {
            if (searchModel.PageIndex <= 0)
            {
                throw new ArgumentOutOfRangeException("The page index should be greater than 0.");
            }

            if (searchModel.PageSize <= 0)
            {
                throw new ArgumentOutOfRangeException("The page size should be greater than 0.");
            }

            if (string.IsNullOrEmpty(searchModel.OrderField))
            {
                throw new ArgumentOutOfRangeException("The orderField has not been specified.");
            }

            if (this.HttpContext.TryGetUserId(out Guid userId))
            {
                return this.messageInboxService.GetAllMessage(userId, searchModel.GetQueryModel(), searchModel.PageIndex, searchModel.PageSize, searchModel.OrderField, searchModel.IsAsc);
            }
            else
            {
                throw new ArgumentOutOfRangeException("The receiver has not been specified.");
            }
        }
    }
}
