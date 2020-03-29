using System;
using ZDY.DMS.AspNetCore.Mvc;
using ZDY.DMS.Repositories;
using ZDY.DMS.Services.MessageService.Models;

namespace ZDY.DMS.Services.MessageService
{
    public class MessageController: ApiController
    {
        private readonly IRepositoryContext repositoryContext;
        private readonly IRepository<Guid, Message> messageRepository;

        public MessageController(IRepositoryContext repositoryContext)
        {
            this.repositoryContext = repositoryContext;
            this.messageRepository = repositoryContext.GetRepository<Guid, Message>();
        }
    }
}
