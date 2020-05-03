using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ZDY.DMS.Services.WorkFlowService.Core.Enums
{
    public enum WorkFlowHandlerKinds
    {
        /// <summary>
        /// 任意人员
        /// </summary>
        [Description("任意人员")]
        [Category("1")]
        AnyUser = 0,

        /// <summary>
        /// 指定人员
        /// </summary>
        [Description("指定人员")]
        [Category("1")]
        User = 1,

        /// <summary>
        /// 部门
        /// </summary>
        [Description("部门")]
        [Category("1")]
        Department = 2,

        /// <summary>
        /// 角色
        /// </summary>
        [Description("角色")]
        [Category("1")]
        UserGroup = 3,

        /// <summary>
        /// 工作组
        /// </summary>
        [Description("工作组")]
        [Category("1")]
        WorkGroup = 4,

        /// <summary>
        /// 发起人
        /// </summary>
        [Description("发起人")]
        [Category("1")]
        Initiator = 15,

        /// <summary>
        /// 发起人主管
        /// </summary>
        [Description("发起人主管")]
        [Category("1")]
        DirectorForInitiator = 16,

        /// <summary>
        /// 发起人分管领导
        /// </summary>
        [Description("发起人分管领导")]
        [Category("1")]
        LeaderForInitiator = 17,

        /// <summary>
        /// 当前处理人主管
        /// </summary>
        [Description("当前处理人主管")]
        [Category("1")]
        DirectorForHandler = 18,

        /// <summary>
        /// 当前处理人分管领导
        /// </summary>
        [Description("当前处理人分管领导")]
        [Category("1")]
        LeaderForHandler = 19
    }
}
