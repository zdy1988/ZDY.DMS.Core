using System;
using System.Collections.Generic;
using System.Text;

namespace ZDY.DMS.Services.WorkFlowService
{
    public static class WorkFlowConstant
    {
        public static Guid StartStepId
        {
            get
            {
                return Guid.Parse("00000000-0000-0000-0000-000000000001");
            }
        }

        public static Guid EndStepId
        {
            get
            {
                return Guid.Parse("00000000-0000-0000-0000-000000000002");
            }
        }
    }
}
