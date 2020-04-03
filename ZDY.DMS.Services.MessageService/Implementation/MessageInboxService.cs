using System;
using System.Linq;
using System.Collections.Generic;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ZDY.DMS.AspNetCore.Dictionary;
using ZDY.DMS.AspNetCore.Service;
using ZDY.DMS.Repositories;
using ZDY.DMS.Services.MessageService.Models;
using ZDY.DMS.Services.MessageService.ServiceContracts;
using ZDY.DMS.Services.MessageService.DataTransferObjects;
using System.Threading.Tasks;
using ZDY.DMS.KeyGeneration;

namespace ZDY.DMS.Services.MessageService.Implementation
{
    public class MessageInboxService : ServiceBase<Guid,Message,MessageServiceModule>, IMessageInboxService
    {
        private readonly IMapper mapper;

        public MessageInboxService(Func<Type, IRepositoryContext> repositoryContextFactory, IMapper mapper) 
            : base(repositoryContextFactory, new GuidKeyGenerator())
        {
            this.mapper = mapper;
        }

        public async Task AddMessageAsync(Message message, params Guid[] receiver)
        {
            message.Id = this.KeyGenerator.Generate(message);

            await this.GetRepository<Guid, Message>().AddAsync(message);

            foreach (var receiverId in receiver)
            {
                await this.GetRepository<Guid, MessageInbox>().AddAsync(new MessageInbox
                {
                    MessageId = message.Id,
                    ReceiverId = receiverId,
                });
            }

            await this.RepositoryContext.CommitAsync();
        }

        public async Task<List<MessageInboxDTO>> GetAllMessageAsync(Guid receiverId)
        {
            var context = (DbContext)this.RepositoryContext.Session;

            var query = from m in context.Set<Message>()
                        join mi in context.Set<MessageInbox>()
                        on new { messageId = m.Id, receiverId } equals new { messageId = mi.MessageId, receiverId = mi.ReceiverId }
                        select ValueTuple.Create(m, mi);

            return await query.ProjectTo<MessageInboxDTO>(this.mapper.ConfigurationProvider).ToListAsync();
        }
    }
}
