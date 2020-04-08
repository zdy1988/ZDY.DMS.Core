using ZDY.DMS.AspNetCore.Bootstrapper.Module;
using ZDY.DMS.Commands;

namespace ZDY.DMS.AspNetCore.Messaging
{
    public abstract class CommandHandlerBase<TServiceModule, TCommand> : MessageHandlerBase<TServiceModule, TCommand>
        where TServiceModule : IServiceModule
        where TCommand : ICommand
    {

    }
}
