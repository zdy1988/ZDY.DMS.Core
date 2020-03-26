using ZDY.DMS.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZDY.DMS.Commands
{
    /// <summary>
    /// Represents the base class for command handlers.
    /// </summary>
    /// <typeparam name="TCommand">The type of the command.</typeparam>
    /// <seealso cref="ZDY.DMS.Messaging.MessageHandler{TCommand}" />
    /// <seealso cref="ZDY.DMS.Commands.ICommandHandler{TCommand}" />
    public abstract class CommandHandler<TCommand> : MessageHandler<TCommand>, ICommandHandler<TCommand>
        where TCommand : ICommand
    {
    }
}
