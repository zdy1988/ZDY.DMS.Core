using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ZDY.DMS.Services.WorkFlowService.Enums
{
    public enum WorkFlowKinds
    {
        [Description("其他类型流程")]
        [Category("1")]
        Other = 0,

        [Description("办公类型流程")]
        [Category("1")]
        Office = 1,

        [Description("业务类型流程")]
        [Category("1")]
        Business = 2
    }
}
