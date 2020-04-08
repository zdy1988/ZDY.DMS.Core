using System;
using ZDY.DMS.AspNetCore.Bootstrapper.Module;
using ZDY.DMS.Commands;
using ZDY.DMS.Repositories;

namespace ZDY.DMS.AspNetCore.Messaging
{
    public abstract class CommandHandlerBase<TServiceModule, TCommand> : MessageHandlerBase<TServiceModule, TCommand>
        where TServiceModule : IServiceModule
        where TCommand : ICommand
    {
        public CommandHandlerBase(Func<Type, IRepositoryContext> repositoryContextFactory)
            : base(repositoryContextFactory)
        {

        }
    }
}
