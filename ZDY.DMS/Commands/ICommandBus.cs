using ZDY.DMS.Messaging;

namespace ZDY.DMS.Commands
{
    /// <summary>
    /// Represents that the implemented classes are the message buses that transfers
    /// the command messages.
    /// </summary>
    /// <seealso cref="ZDY.DMS.Messaging.IMessageBus" />
    /// <seealso cref="ZDY.DMS.Commands.ICommandSender" />
    /// <seealso cref="ZDY.DMS.Commands.ICommandSubscriber" />
    public interface ICommandBus : IMessageBus, ICommandSender, ICommandSubscriber
    {
    }
}
