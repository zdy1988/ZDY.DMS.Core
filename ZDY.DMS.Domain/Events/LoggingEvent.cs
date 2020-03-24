using System;
using Zdy.Events;
using ZDY.DMS.Domain.Enums;

namespace ZDY.DMS.Domain.Events
{
    public class LoggingEvent : IEvent
    {
        private Guid id = Guid.NewGuid();
        private DateTime timeStamp = DateTime.UtcNow;

        public Guid ID
        {
            get { return id; }
            set { id = value; }
        }
        /// <summary>
        /// 获取产生领域事件的时间戳。
        /// </summary>
        public DateTime TimeStamp
        {
            get { return timeStamp; }
            set { timeStamp = value; }
        }

        public string Info { get; set; }
        public LogKinds LogType { get; set; }
    }
}
