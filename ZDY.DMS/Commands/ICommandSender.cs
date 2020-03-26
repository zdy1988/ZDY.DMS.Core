using ZDY.DMS.Messaging;

namespace ZDY.DMS.Commands
{
    /// <summary>
    /// Represents that the implemented classes are command senders which can send commands
    /// to the specified message bus.
    /// </summary>
    /// <seealso cref="ZDY.DMS.Messaging.IMessagePublisher" />
    public interface ICommandSender : IMessagePublisher
    {
    }
}
