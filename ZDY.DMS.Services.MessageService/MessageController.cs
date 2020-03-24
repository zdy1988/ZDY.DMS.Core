using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZDY.DMS.AspNetCore.Mvc;
using ZDY.DMS.Models;
using ZDY.DMS.Repositories;

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
