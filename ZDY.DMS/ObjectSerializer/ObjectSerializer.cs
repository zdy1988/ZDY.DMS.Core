using System;
using System.Threading;
using System.Threading.Tasks;

namespace ZDY.DMS
{
    /// <summary>
    /// Represents the base class for the object serializers.
    /// </summary>
    /// <seealso cref="ZDY.DMS.IObjectSerializer" />
    public abstract class ObjectSerializer : IObjectSerializer
    {
        /// <summary>
        /// Deserializes the object from a <see cref="byte" /> array.
        /// </summary>
        /// <typeparam name="TObject">The type of the object to be deserialized.</typeparam>
        /// <param name="data">The <see cref="byte" /> array which contains the object data.</param>
        /// <returns>
        /// The deserialized object
        /// </returns>
        public virtual TObject Deserialize<TObject>(byte[] data) => (TObject)Deserialize(data, typeof(TObject));

        /// <summary>
        /// Deserializes the object from a <see cref="byte" /> array.
        /// </summary>
        /// <param name="data">The <see cref="byte" /> array which contains the object data.</param>
        /// <param name="objType">The type of the object to be deserialized.</param>
        /// <returns>
        /// The deserialized object.
        /// </returns>
        public abstract object Deserialize(byte[] data, Type objType);

        /// <summary>
        /// Deserializes the object from a <see cref="byte" /> array asynchronously.
        /// </summary>
        /// <typeparam name="TObject">The type of the object to be deserialized.</typeparam>
        /// <param name="data">The <see cref="byte" /> array which contains the object data.</param>
        /// <param name="cancellationToken">The cancellation token which propagates notification that operations should be canceled.</param>
        /// <returns>
        /// The deserialized object.
        /// </returns>
        public virtual Task<TObject> DeserializeAsync<TObject>(byte[] data, CancellationToken cancellationToken = default(CancellationToken)) => Task.FromResult(Deserialize<TObject>(data));

        /// <summary>
        /// the object from a <see cref="byte" /> array asynchronously.
        /// </summary>
        /// <param name="data">The <see cref="byte" /> array which contains the object data.</param>
        /// <param name="objType">The type of the object to be deserialized.</param>
        /// <param name="cancellationToken">The cancellation token which propagates notification that operations should be canceled.</param>
        /// <returns>
        /// The deserialized object.
        /// </returns>
        public virtual Task<object> DeserializeAsync(byte[] data, Type objType, CancellationToken cancellationToken = default(CancellationToken)) => Task.FromResult(Deserialize(data, objType));

        /// <summary>
        /// Serializes the specified object into a <see cref="byte" /> array.
        /// </summary>
        /// <typeparam name="TObject">The type of the object to be serialized.</typeparam>
        /// <param name="obj">The object to be serialized.</param>
        /// <returns>
        /// The byte array which contains the serialized data.
        /// </returns>
        public virtual byte[] Serialize<TObject>(TObject obj) => Serialize(typeof(TObject), obj);

        /// <summary>
        /// Serializes the specified object into a <see cref="byte" /> array.
        /// </summary>
        /// <param name="objType">The type of the object to be serialized.</param>
        /// <param name="obj">The object to be serialized.</param>
        /// <returns>
        /// The byte array which contains the serialized data.
        /// </returns>
        public abstract byte[] Serialize(Type objType, object obj);

        /// <summary>
        /// Serializes the specified object into a <see cref="byte" /> array asynchronously.
        /// </summary>
        /// <typeparam name="TObject">The type of the object to be serialized.</typeparam>
        /// <param name="obj">The object to be serialized.</param>
        /// <param name="cancellationToken">The cancellation token which propagates notification that operations should be canceled.</param>
        /// <returns>
        /// The byte array which contains the serialized data.
        /// </returns>
        public virtual Task<byte[]> SerializeAsync<TObject>(TObject obj, CancellationToken cancellationToken = default(CancellationToken)) => Task.FromResult(Serialize(obj));

        /// <summary>
        /// Serializes the specified object into a <see cref="byte" /> array
        /// </summary>
        /// <param name="objType">The type of the object to be serialized.</param>
        /// <param name="obj">The object to be serialized.</param>
        /// <param name="cancellationToken">The cancellation token which propagates notification that operations should be canceled.</param>
        /// <returns>
        /// The byte array which contains the serialized data.
        /// </returns>
        public virtual Task<byte[]> SerializeAsync(Type objType, object obj, CancellationToken cancellationToken = default(CancellationToken)) => Task.FromResult(Serialize(objType, obj));
    }
}
