namespace ZDY.DMS.Messaging
{
    /// <summary>
    /// Represents that the implemented classes are message buses.
    /// </summary>
    /// <seealso cref="ZDY.DMS.Messaging.IMessagePublisher" />
    /// <seealso cref="ZDY.DMS.Messaging.IMessageSubscriber" />
    public interface IMessageBus : IMessagePublisher, IMessageSubscriber
    {
    }
}
