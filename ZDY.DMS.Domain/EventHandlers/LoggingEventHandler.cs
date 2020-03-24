using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zdy.Events;
using ZDY.DMS;
using ZDY.DMS.Domain.Events;
using ZDY.DMS.Models;
using ZDY.DMS.Repositories;
using ZDY.DMS.ServiceContracts;

namespace ZDY.DMS.Domain.EventHandlers
{
    [HandlesAsynchronously]
    public class LoggingEventHandler : IEventHandler<LoggingEvent>
    {
        private readonly IRepositoryContext repositoryContext;
        private readonly IRepository<Guid, Log> logRepository;

        public LoggingEventHandler(IRepositoryContext repositoryContext)
        {
            this.repositoryContext = repositoryContext;
            this.logRepository = repositoryContext.GetRepository<Guid, Log>();
        }

        public void Handle(LoggingEvent evt)
        {
            logRepository.AddAsync(new Log()
            {
                TimeStamp = evt.TimeStamp,
                Message = evt.Info,
                Type = (int)evt.LogType
            });
        }
    }
}
