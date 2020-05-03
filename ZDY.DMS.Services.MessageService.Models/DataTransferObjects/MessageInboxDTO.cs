using System;
using System.Collections.Generic;
using System.Text;
using ZDY.DMS.Services.MessageService.Models;

namespace ZDY.DMS.Services.MessageService.DataTransferObjects
{
    public class MessageInboxDTO : Message
    {
        public bool IsReaded { get; set; } = false;
    }
}
