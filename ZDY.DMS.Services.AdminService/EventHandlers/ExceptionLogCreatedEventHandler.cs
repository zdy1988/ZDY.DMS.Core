using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZDY.DMS.AspNetCore.Events;
using ZDY.DMS.AspNetCore.Messaging;
using ZDY.DMS.Repositories;
using ZDY.DMS.Services.AdminService.Enums;
using ZDY.DMS.Services.AdminService.Models;
using ZDY.DMS.Services.Common.Events;

namespace ZDY.DMS.Services.AdminService.EventHandlers
{
    public class ExceptionLogCreatedEventHandler : EventHandlerBase<AdminServiceModule, ExceptionLogCreatedEvent>
    {
        public ExceptionLogCreatedEventHandler(Func<Type, IRepositoryContext> repositoryContextFactory)
            : base(repositoryContextFactory)
        {

        }

        public async override Task<bool> HandleAsync(ExceptionLogCreatedEvent message, CancellationToken cancellationToken = default)
        {
            try
            {
                string content = $@"操作用户：{message.OperatorId}
                                <br>消息类型：{message.Exception.GetType().Name}
                                <br>消息内容：{message.Exception.Message}
                                <br>引发异常的方法：{message.Exception.TargetSite}
                                <br>引发异常源：{message.Exception.Source}";

                await this.GetRepository<Guid, Log>().AddAsync(new Log()
                {
                    TimeStamp = DateTime.Now,
                    Message = content,
                    Type = (int)LogKinds.System
                });

                await this.RepositoryContext.CommitAsync();

                return true;
            }
            catch
            {

                return false;
            }
        }
    }
}
