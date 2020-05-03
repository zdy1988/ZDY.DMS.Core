using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ZDY.DMS.Services.WorkFlowService.Core.Enums
{
    public enum WorkFlowFormKinds
    {
        [Description("普通类型表单")]
        [Category("1")]
        Common = 0,

        [Description("办公类型表单")]
        [Category("1")]
        Office = 1,

        [Description("业务类型表单")]
        [Category("1")]
        Business = 2
    }
}
