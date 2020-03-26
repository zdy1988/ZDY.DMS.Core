﻿using ZDY.DMS.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZDY.DMS.Events
{
    /// <summary>
    /// Represents that the implemented classes are event publishers.
    /// </summary>
    public interface IEventPublisher : IMessagePublisher
    {
    }
}
