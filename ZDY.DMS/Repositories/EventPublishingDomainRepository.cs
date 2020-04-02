using ZDY.DMS.Events;
using ZDY.DMS.Messaging;

namespace ZDY.DMS.Repositories
{
    /// <summary>
    /// Represents the domain repository that can publish the uncommitted domain events in an aggregate
    /// to the message bus.
    /// </summary>
    public abstract class EventPublishingDomainRepository : DomainRepository
    {
        #region Private Fields
        private readonly IEventPublisher publisher;
        private bool disposed = false;
        #endregion

        #region Protected Properties        
        /// <summary>
        /// Gets the instance of the event publisher.
        /// </summary>
        /// <value>
        /// The event publisher which publishes the events.
        /// </value>
        protected IEventPublisher Publisher => this.publisher;
        #endregion

        #region Ctor        
        /// <summary>
        /// Initializes a new instance of the <see cref="EventPublishingDomainRepository"/> class.
        /// </summary>
        /// <param name="publisher">The event publisher which publishes the events.</param>
        protected EventPublishingDomainRepository(IEventPublisher publisher)
        {
            this.publisher = publisher;
            this.publisher.MessagePublished += OnMessagePublished;
        }
        #endregion

        #region Protected Methods        
        /// <summary>
        /// Invokes when the event has been published.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MessagePublishedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnMessagePublished(object sender, MessagePublishedEventArgs e) { }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    this.publisher.Dispose();
                }

                disposed = true;
                base.Dispose(disposing);
            }
        }
        #endregion
    }
}
