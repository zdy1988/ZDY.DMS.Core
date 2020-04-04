using System;
using System.Collections.Generic;
using System.Text;
using ZDY.DMS.Services.WorkFlowService.Models;

namespace ZDY.DMS.Services.WorkFlowService.DataObjects
{
    public class WorkFlowExecutionContext
    {
        /// <summary>
        /// 流程定义数据
        /// </summary>
        public WorkFlowDefinition Definition { get; set; }

        /// <summary>
        /// 执行中的流程实例
        /// </summary>
        public WorkFlowInstance Instance { get; set; }

        /// <summary>
        /// 执行中的流程实例所在的实例集合Id
        /// </summary>
        public Guid GroupId { get; set; }

        /// <summary>
        /// 执行中的流程实例产生的所有任务
        /// </summary>
        public List<WorkFlowTask> Tasks { get; set; }

        /// <summary>
        /// 执行中的实例任务
        /// </summary>
        public WorkFlowTask Task { get; set; }

        /// <summary>
        /// 执行中任务所处的步骤
        /// </summary>
        public WorkFlowStep Step { get; set; }

        /// <summary>
        /// 发送人员
        /// </summary>
        public WorkFlowUser Sender { get; set; }

        /// <summary>
        /// 接收的步骤Id和人员
        /// </summary>
        public Dictionary<Guid, List<WorkFlowUser>> Steps { get; set; }
    }
}
