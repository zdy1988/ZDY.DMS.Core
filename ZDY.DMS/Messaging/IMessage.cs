using System;

namespace ZDY.DMS.Messaging
{
    public interface IMessage
    {
        /// <summary>
        /// Gets or sets the identifier of the message.
        /// </summary>
        /// <value>
        /// The identifier of the message.
        /// </value>
        Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the timestamp which describes when the current message occurs.
        /// </summary>
        /// <value>
        /// The timestamp which describes when the current message occurs.
        /// </value>
        DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets an instance of <see cref="MessageMetadata"/> class which contains the
        /// metadata information of current message.
        /// </summary>
        MessageMetadata Metadata { get; }

        /// <summary>
        /// Gets the .NET CLR assembly qualified name of the current message.
        /// </summary>
        /// <returns>The assembly qualified name of the current message.</returns>
        string GetMessageClrTypeName();
    }
}
