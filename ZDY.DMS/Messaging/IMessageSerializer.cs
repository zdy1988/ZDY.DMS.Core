using System.Threading;
using System.Threading.Tasks;

namespace ZDY.DMS.Messaging
{
    /// <summary>
    /// Represents that the implemented classes are message serializers.
    /// </summary>
    public interface IMessageSerializer
    {
        /// <summary>
        /// Serializes the specified message into a byte array.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message to be serialized.</typeparam>
        /// <param name="message">The message that is going to be serialized.</param>
        /// <returns>A byte array that contains the serialized data.</returns>
        byte[] Serialize<TMessage>(TMessage message)
            where TMessage : IMessage;

        /// <summary>
        /// Serializes the specified message into a byte array asynchronously.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message to be serialized.</typeparam>
        /// <param name="message">The message that is going to be serialized.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> instance that propagates notification that operations should be canceled.</param>
        /// <returns>The <see cref="Task"/> that when finished, returns the serialized data.</returns>
        Task<byte[]> SerializeAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default(CancellationToken))
            where TMessage : IMessage;

        /// <summary>
        /// Deserializes the message from the specified <see cref="byte"/> array.
        /// </summary>
        /// <param name="value">The <see cref="byte"/> array which contains the message data.</param>
        /// <returns>The deserialized message.</returns>
        IMessage Deserialize(byte[] value);

        /// <summary>
        /// Deserializes a message from the specified <see cref="byte"/> array asynchronously.
        /// </summary>
        /// <param name="value">The byte array that contains the serialized data.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> instance that propagates notification that operations should be canceled.</param>
        /// <returns>The deserialized message.</returns>
        Task<IMessage> DeserializeAsync(byte[] value, CancellationToken cancellationToken = default(CancellationToken));
    }
}
