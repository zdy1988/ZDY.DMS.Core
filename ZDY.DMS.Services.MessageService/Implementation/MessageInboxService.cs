using System;
using System.Linq;
using System.Collections.Generic;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ZDY.DMS.AspNetCore.Bootstrapper.Service;
using ZDY.DMS.Repositories;
using ZDY.DMS.Services.MessageService.Models;
using ZDY.DMS.Services.MessageService.ServiceContracts;
using ZDY.DMS.Services.MessageService.DataTransferObjects;
using System.Threading.Tasks;
using ZDY.DMS.KeyGeneration;
using ZDY.DMS.Querying;
using ZDY.DMS.Querying.SearchModel.Model;

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

        public Tuple<IEnumerable<MessageInboxDTO>, int> GetAllMessage(Guid receiverId, QueryModel queryModel, int pageIndex, int pageSize, string orderField, bool isAsc)
        {
            if (pageIndex <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageIndex), "The page index should be greater than 0.");
            }

            if (pageSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize), "The page size should be greater than 0.");
            }

            var context = (DbContext)this.RepositoryContext.Session;

            var query = from m in context.Set<Message>()
                        join mi in context.Set<MessageInbox>()
                        on new { messageId = m.Id, receiverId } equals new { messageId = mi.MessageId, receiverId = mi.ReceiverId }
                        select ValueTuple.Create(m, mi);

            var dtoQuery = query.ProjectTo<MessageInboxDTO>(this.mapper.ConfigurationProvider);

            var total = dtoQuery.Count();

            var list = dtoQuery.Sort<MessageInboxDTO>(orderField, isAsc).Skip((pageIndex - 1) * pageSize).Take(pageSize);

            return new Tuple<IEnumerable<MessageInboxDTO>, int>(list, total);
        }
    }
}
