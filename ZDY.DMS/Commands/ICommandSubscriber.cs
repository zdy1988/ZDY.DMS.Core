using ZDY.DMS.Messaging;

namespace ZDY.DMS.Commands
{
    /// <summary>
    /// Represents that the implemented classes are command subscribers which
    /// subscribes to the command buses.
    /// </summary>
    /// <seealso cref="ZDY.DMS.Messaging.IMessageSubscriber" />
    public interface ICommandSubscriber : IMessageSubscriber
    {
    }
}
