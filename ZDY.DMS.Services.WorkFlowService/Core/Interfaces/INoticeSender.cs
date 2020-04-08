using System;
using System.Collections.Generic;
using System.Text;

namespace ZDY.DMS.Services.WorkFlowService.Core.Interfaces
{
    public interface INoticeSender
    {
        void Push(string title, string content, params Guid[] receiver);
    }
}
