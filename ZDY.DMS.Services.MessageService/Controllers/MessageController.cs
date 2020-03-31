using System;
using ZDY.DMS.AspNetCore.Mvc;
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
    }
}
