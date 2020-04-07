using System;
using System.Collections.Generic;
using System.Text;

namespace ZDY.DMS.Services.WorkFlowService.CutomSimple
{
    public class Condition
    {
        public bool Pass(object data)
        {
            return true;
        }

        public bool Fail(object data)
        {
            return false;
        }
    }
}
