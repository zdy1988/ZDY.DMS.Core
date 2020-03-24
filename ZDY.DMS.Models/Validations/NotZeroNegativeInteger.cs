using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ZDY.DMS.Models.Validations
{
    /// <summary>
    /// 非零的负整数
    /// </summary>
    public class NotZeroNegativeInteger : RegularExpressionAttribute
    {
        public NotZeroNegativeInteger() : base(@"^\-[1-9][0-9]*$")
        {
            ErrorMessage = "请输入非零的负整数";
        }
    }
}
