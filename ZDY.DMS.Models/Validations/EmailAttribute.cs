using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;

namespace ZDY.DMS.Models.Validations
{
    public class EmailAttribute : RegularExpressionAttribute
    {
        public EmailAttribute() : base(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}")
        {
            ErrorMessage = "邮箱格式不正确";
        }
    }
}
