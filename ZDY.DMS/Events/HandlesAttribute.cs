using System;

namespace ZDY.DMS.Events
{
    /// <summary>
    /// Represents that the decorated classes or methods are event handlers
    /// that can handle an event with specified type.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class HandlesAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HandlesAttribute"/> class.
        /// </summary>
        /// <param name="eventType">Type of the event that current decorated class or method can handle.</param>
        public HandlesAttribute(Type eventType)
        {
            this.EventType = eventType;
        }

        /// <summary>
        /// Gets the type of the event that can be handled by current decorated class or method.
        /// </summary>
        /// <value>
        /// The type of the event.
        /// </value>
        public Type EventType { get; }
    }
}
