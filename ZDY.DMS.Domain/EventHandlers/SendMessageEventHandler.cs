using System;
using Zdy.Events;
using ZDY.DMS.Domain.Enums;
using ZDY.DMS.Domain.Events;

namespace ZDY.DMS.Domain.EventHandlers
{
    [HandlesAsynchronously]
    public class SendMessageEventHandler : IEventHandler<SendMessageEvent>
    {
        public void Handle(SendMessageEvent args)
        {
            if (args.Message.Type == 0)
            {
                if (args.To.Length == 1)
                {
                    args.Message.Type = (int)MessageKinds.Private;
                }
                else if (args.To.Length > 1 && args.To.Length <= 100)
                {
                    args.Message.Type = (int)MessageKinds.Group;
                }
                else if (args.To.Length > 100)
                {
                    args.Message.Type = (int)MessageKinds.Public;
                }
                else
                {
                    args.Message.Type = (int)MessageKinds.Global;
                }
            }

            //ServiceLocator.GetService<IMessageService>().Push(args.Message, args.To);
        }
    }
}
