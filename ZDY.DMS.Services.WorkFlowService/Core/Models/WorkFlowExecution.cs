using System;
using System.Collections.Generic;
using ZDY.DMS.Services.WorkFlowService.Enums;
using ZDY.DMS.Services.WorkFlowService.Models;

namespace ZDY.DMS.Services.WorkFlowService.Core.Models
{
    public class WorkFlowExecution
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
        /// 到达的步骤Id和人员
        /// </summary>
        public Dictionary<Guid, List<WorkFlowUser>> ToStepCollection { get; set; }

        /// <summary>
        /// 执行中的实例任务所在的集合Id
        /// 表示在不同的流程实例中，当前实例任务和子流程任务同属一个分组
        /// </summary>
        public Guid GroupId { get; set; }

        /// <summary>
        /// 处理意见
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// 获取当前任务名称
        /// </summary>
        /// <returns></returns>
        public string GetTaskTitle()
        {
            return Task.Title;
        }
    }
}
