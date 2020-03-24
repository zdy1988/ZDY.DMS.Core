using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ZDY.DMS.Models.Validations
{
    /// <summary>
    /// 非零的正整数
    /// </summary>
    public class NotZeroPositiveInteger : RegularExpressionAttribute
    {
        public NotZeroPositiveInteger() : base(@"^\+?[1-9][0-9]*$")
        {
            ErrorMessage = "请输入非零的正整数";
        }
    }
}
