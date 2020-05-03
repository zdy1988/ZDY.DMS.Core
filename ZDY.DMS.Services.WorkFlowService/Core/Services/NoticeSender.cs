using System;
using System.Collections.Generic;
using System.Text;
using ZDY.DMS.Events;
using ZDY.DMS.Services.Shared.Events;
using ZDY.DMS.Services.WorkFlowService.Core.Interfaces;

namespace ZDY.DMS.Services.WorkFlowService.Core.Services
{
    public class NoticeSender: INoticeSender
    {
        private readonly IEventPublisher eventPublisher;

        public NoticeSender(IEventPublisher eventPublisher)
        {
            this.eventPublisher = eventPublisher;
        }

        public void Push(string title, string content, params Guid[] receiver)
        {
            title = $"【审批】{title}";
            content = $"【审批】{content}";

            var message = new PublicMessageCreatedEvent(title, content, 0, receiver);

            eventPublisher.Publish<PublicMessageCreatedEvent>(message);
        }
    }
}
