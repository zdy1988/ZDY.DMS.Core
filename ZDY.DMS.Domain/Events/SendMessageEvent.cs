using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zdy.Events;
using ZDY.DMS.Models;

namespace ZDY.DMS.Domain.Events
{
    public class SendMessageEvent : IEvent
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

        public Message Message { get; set; }

        public Guid[] To { get; set; }
    }
}
