﻿using System;
using System.Collections.Generic;
using System.Text;
using ZDY.DMS.Events;

namespace ZDY.DMS.Services.Common.Events
{
    public class WorkFlowInstallEvent : Event
    {
        /// <summary>
        /// 流程所在的公司
        /// </summary>
        public Guid CompanyId { get; set; }

        /// <summary>
        /// 需要安装的流程Id
        /// </summary>
        public Guid FlowId { get; set; }

        /// <summary>
        /// 需要安装的流程名称
        /// </summary>
        public string FlowName { get; set; }

        public WorkFlowInstallEvent(Guid companyId, Guid flowId, string flowName)
        {
            this.CompanyId = companyId;
            this.FlowId = flowId;
            this.FlowName = flowName;
        }
    }
}
