using System;
using System.Threading;
using System.Threading.Tasks;

namespace ZDY.DMS
{
    /// <summary>
    /// Represents that the implemented classes are object serializers.
    /// </summary>
    public interface IObjectSerializer
    {
        /// <summary>
        /// Serializes the specified object into a <see cref="byte"/> array.
        /// </summary>
        /// <typeparam name="TObject">The type of the object to be serialized.</typeparam>
        /// <param name="obj">The object to be serialized.</param>
        /// <returns>The byte array which contains the serialized data.</returns>
        byte[] Serialize<TObject>(TObject obj);

        /// <summary>
        /// Serializes the specified object into a <see cref="byte"/> array asynchronously.
        /// </summary>
        /// <typeparam name="TObject">The type of the object to be serialized.</typeparam>
        /// <param name="obj">The object to be serialized.</param>
        /// <param name="cancellationToken">The cancellation token which propagates notification that operations should be canceled.</param>
        /// <returns>The byte array which contains the serialized data.</returns>
        Task<byte[]> SerializeAsync<TObject>(TObject obj, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Serializes the specified object into a <see cref="byte"/> array.
        /// </summary>
        /// <param name="objType">The type of the object to be serialized.</param>
        /// <param name="obj">The object to be serialized.</param>
        /// <returns>The byte array which contains the serialized data.</returns>
        byte[] Serialize(Type objType, object obj);

        /// <summary>
        /// Serializes the specified object into a <see cref="byte"/> array
        /// </summary>
        /// <param name="objType">The type of the object to be serialized.</param>
        /// <param name="obj">The object to be serialized.</param>
        /// <param name="cancellationToken">The cancellation token which propagates notification that operations should be canceled.</param>
        /// <returns>The byte array which contains the serialized data.</returns>
        Task<byte[]> SerializeAsync(Type objType, object obj, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Deserializes the object from a <see cref="byte"/> array.
        /// </summary>
        /// <typeparam name="TObject">The type of the object to be deserialized.</typeparam>
        /// <param name="data">The <see cref="byte"/> array which contains the object data.</param>
        /// <returns>The deserialized object</returns>
        TObject Deserialize<TObject>(byte[] data);

        /// <summary>
        /// Deserializes the object from a <see cref="byte"/> array asynchronously.
        /// </summary>
        /// <typeparam name="TObject">The type of the object to be deserialized.</typeparam>
        /// <param name="data">The <see cref="byte"/> array which contains the object data.</param>
        /// <param name="cancellationToken">The cancellation token which propagates notification that operations should be canceled.</param>
        /// <returns>The deserialized object.</returns>
        Task<TObject> DeserializeAsync<TObject>(byte[] data, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Deserializes the object from a <see cref="byte"/> array.
        /// </summary>
        /// <param name="data">The <see cref="byte"/> array which contains the object data.</param>
        /// <param name="objType">The type of the object to be deserialized.</param>
        /// <returns>The deserialized object.</returns>
        object Deserialize(byte[] data, Type objType);

        /// <summary>
        /// the object from a <see cref="byte"/> array asynchronously.
        /// </summary>
        /// <param name="data">The <see cref="byte"/> array which contains the object data.</param>
        /// <param name="objType">The type of the object to be deserialized.</param>
        /// <param name="cancellationToken">The cancellation token which propagates notification that operations should be canceled.</param>
        /// <returns>The deserialized object.</returns>
        Task<object> DeserializeAsync(byte[] data, Type objType, CancellationToken cancellationToken = default(CancellationToken));
    }
}
