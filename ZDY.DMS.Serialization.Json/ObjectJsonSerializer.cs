using Newtonsoft.Json;
using System;
using System.Text;

namespace ZDY.DMS.Serialization.Json
{
    /// <summary>
    /// Represents the object serializer that serializes an object into 
    /// </summary>
    /// <seealso cref="ZDY.DMS.ObjectSerializer" />
    public sealed class ObjectJsonSerializer : ObjectSerializer
    {
        private readonly Encoding encoding;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectJsonSerializer"/> class.
        /// </summary>
        /// <param name="encoding">The encoding to be used for serialize/deserialize the object.</param>
        public ObjectJsonSerializer(Encoding encoding = null)
            => this.encoding = encoding ?? Encoding.UTF8;

        /// <summary>
        /// Serializes the specified object into a <see cref="T:System.Byte" /> array.
        /// </summary>
        /// <param name="objType">The type of the object to be serialized.</param>
        /// <param name="obj">The object to be serialized.</param>
        /// <returns>
        /// The byte array which contains the serialized data.
        /// </returns>
        public override byte[] Serialize(Type objType, object obj)
            => this.encoding.GetBytes(JsonConvert.SerializeObject(obj, objType, Formatting.Indented, null));

        /// <summary>
        /// Deserializes the object from a <see cref="T:System.Byte" /> array.
        /// </summary>
        /// <param name="data">The <see cref="T:System.Byte" /> array which contains the object data.</param>
        /// <param name="objType">The type of the object to be deserialized.</param>
        /// <returns>
        /// The deserialized object.
        /// </returns>
        public override object Deserialize(byte[] data, Type objType)
            => JsonConvert.DeserializeObject(this.encoding.GetString(data), objType);

        /// <summary>
        /// Deserializes the object from a <see cref="T:System.Byte" /> array.
        /// </summary>
        /// <typeparam name="TObject">The type of the object to be deserialized.</typeparam>
        /// <param name="data">The <see cref="T:System.Byte" /> array which contains the object data.</param>
        /// <returns>
        /// The deserialized object
        /// </returns>
        public override TObject Deserialize<TObject>(byte[] data)
            => JsonConvert.DeserializeObject<TObject>(this.encoding.GetString(data));
    }
}
