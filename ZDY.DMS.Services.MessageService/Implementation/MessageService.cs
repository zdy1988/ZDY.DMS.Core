using System;
using ZDY.DMS.AspNetCore.Dictionary;
using ZDY.DMS.AspNetCore.Bootstrapper.Service;
using ZDY.DMS.Repositories;
using ZDY.DMS.Services.MessageService.Models;
using ZDY.DMS.Services.MessageService.ServiceContracts;

namespace ZDY.DMS.Services.MessageService.Implementation
{
    public class MessageService : ServiceBase<MessageServiceModule>, IMessageService
    {
        private readonly IRepository<Guid, Message> messageRepository;
        private readonly IDictionaryProvider dictionaryProvider;

        public MessageService(Func<Type, IRepositoryContext> repositoryContextFactory,
            IDictionaryProvider dictionaryProvider) : base(repositoryContextFactory)
        {
            this.dictionaryProvider = dictionaryProvider;
            this.messageRepository = this.GetRepository<Guid, Message>();
        }
    }
}
