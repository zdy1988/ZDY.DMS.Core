using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ZDY.DMS.Services.WorkFlowService.Enums
{
    public enum WorkFlowSignatureKinds
    {
        /// <summary>
        /// 无需发表意见
        /// </summary>
        [Description("无需发表意见")]
        [Category("1")]
        NoComment = 0,

        /// <summary>
        /// 可发表意见，无需签名
        /// </summary>
        [Description("可发表意见，无需签名")]
        [Category("1")]
        CommentAndNoSignature = 1,

        /// <summary>
        /// 可发表意见，必须签名
        /// </summary>
        [Description("可发表意见，必须签名")]
        [Category("1")]
        CommentAndSignature = 2
    }
}
