using System;
using System.Collections.Generic;
using System.Text;

namespace ZDY.DMS.Services.WorkFlowService.Core.Aspects
{
    public class WorkFlowInterceptorResults
    {
        /// <summary>
        /// 是否成功通过
        /// </summary>
        public bool IsPassed { get; set; } = true;

        /// <summary>
        /// 返回信息
        /// </summary>
        public string Message { get; set; }
    }
}
