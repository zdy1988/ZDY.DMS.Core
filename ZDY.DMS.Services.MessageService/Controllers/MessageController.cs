using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZDY.DMS.AspNetCore.Mvc;
using ZDY.DMS.DataPermission;
using ZDY.DMS.Repositories;
using ZDY.DMS.Services.MessageService.Models;

namespace ZDY.DMS.Services.MessageService.Controllers
{
    public class MessageController : ApiDataServiceController<Guid, Message, MessageServiceModule>
    {
        private readonly IRepository<Guid, Message> messageRepository;

        public MessageController(Func<Type, IRepositoryContext> repositoryContextFactory)
            : base(repositoryContextFactory)
        {
            this.messageRepository = this.RepositoryContext.GetRepository<Guid, Message>();
        }

        public override Task<Message> Add(Message entity)
        {
            throw new NotSupportedException();
        }

        public override Task<Message> Update(Message entity)
        {
            throw new NotSupportedException();
        }

        public override Task Delete(Guid id)
        {
            throw new NotSupportedException();
        }

        public override Task<Message> Find(SearchModel searchModel)
        {
            throw new NotSupportedException();
        }

        public override Task<Tuple<IEnumerable<Message>, int>> Search(SearchModel searchModel)
        {
            throw new NotSupportedException();
        }
    }
}
