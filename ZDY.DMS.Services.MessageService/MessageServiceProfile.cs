using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using ZDY.DMS.Services.MessageService.DataTransferObjects;
using ZDY.DMS.Services.MessageService.Models;

namespace ZDY.DMS.Services.MessageService
{
    public class MessageServiceProfile: Profile
    {
        public MessageServiceProfile()
        {
            CreateMap<Message, MessageInboxDTO>()
                .ReverseMap();

            CreateMap<MessageInbox, MessageInboxDTO>()
                .ForMember(d => d.IsReaded, a => a.MapFrom(s => s.IsReaded));

            CreateMap<MessageInboxDTO, (Message, MessageInbox)>()
               .ForMember(x => x.Item1, opts => opts.MapFrom(x => x))
               .ForMember(x => x.Item2, opts => opts.MapFrom(x => x.IsReaded))
               .ReverseMap();
        }
    }
}
