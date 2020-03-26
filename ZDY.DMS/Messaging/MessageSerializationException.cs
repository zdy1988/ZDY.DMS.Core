﻿using System;

namespace ZDY.DMS.Messaging
{
    /// <summary>
    /// Represents the error that occurs during the message serialization processes.
    /// </summary>
    /// <seealso cref="ZDY.DMS.ApworksException" />
    public sealed class MessageSerializationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageSerializationException"/> class.
        /// </summary>
        public MessageSerializationException()
            : base()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageSerializationException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public MessageSerializationException(string message)
            : base(message)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageSerializationException"/> class.
        /// </summary>
        /// <param name="format">The format of the error message.</param>
        /// <param name="args">The arguments to be used for constructing the error message.</param>
        public MessageSerializationException(string format, params object[] args)
            : base(string.Format(format, args))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageSerializationException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public MessageSerializationException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
