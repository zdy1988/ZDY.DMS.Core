using System;
using System.Collections.Generic;
using System.Text;

namespace ZDY.DMS.Services.WorkFlowService.Core.Models
{
    public partial class WorkFlowStep
    {
        /// <summary>
        /// 流转控制 0系统控制 1单选一个分支流转 2多选几个分支流转
        /// </summary>
        public int FlowControl { get; set; } = 0;
        /// <summary>
        /// 处理者类型 0任意人员 1指定人员 2部门 3工作组 15发起者 16发起者主管 17发起者分管领导 18当前处理者主管 19当前处理者分管领导
        /// </summary>
        public int HandlerType { get; set; }
        /// <summary>
        /// 处理者(多个将以分隔符隔开)
        /// </summary>
        public string Handlers { get; set; }
        /// <summary>
        /// 退回策略 0不能退回 1可以退回 2依据策略退回
        /// </summary>
        public int BackTactic { get; set; }
        /// <summary>
        /// 处理策略 0所有人必须处理 1一人同意即可 2依据人数比例 3独立处理
        /// </summary>
        public int HandleTactic { get; set; }
        /// <summary>
        /// 退回类型 0退回前一步 1退回第一步 2退回某一步
        /// </summary>
        public int BackType { get; set; }
        /// <summary>
        /// 策略百分比
        /// </summary>
        public decimal Percentage { get; set; }
        /// <summary>
        /// 会签策略 0 不会签 1 所有步骤同意 2 一个步骤同意即可 3 依据比例
        /// </summary>
        public int CountersignatureTactic { get; set; }
        /// <summary>
        /// 会签策略是依据比例时设置的百分比
        /// </summary>
        public decimal CountersignaturePercentage { get; set; }
        /// <summary>
        /// 子流程处理策略 0 子流程完成后才能提交 1 子流程发起即可提交
        /// </summary>
        public int SubFlowTactic { get; set; }
        /// <summary>
        /// 抄送人员(多个将以分隔符隔开)
        /// </summary>
        public string CopyToUsers { get; set; }
    }
}
