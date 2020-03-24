using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zdy.Events
{
    public interface IEventBus : IUnitOfWork, IDisposable
    {
        Guid ID { get; }
        /// <summary>
        /// 将消息实体发布到EventBus
        /// </summary>
        /// <param name="message">消息实体</param>
        void Publish<TMessage>(TMessage message)
            where TMessage : class, IEvent;
        /// <summary>
        /// 将消息实体批量发布到EventBus
        /// </summary>
        /// <param name="messages">消息实体</param>
        void Publish<TMessage>(IEnumerable<TMessage> messages)
            where TMessage : class, IEvent;
        /// <summary>
        ///  清除发布后等待提交的消息。
        /// </summary>
        void Clear();
    }
}
