using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zdy.Events;

namespace ZDY.DMS.Domain.Events
{
    public class SendEmailEvent : IEvent
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
        public string From { get; set; }
        public string To { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string[] AttachmentPaths { get; set; }
    }
}
