using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;

namespace ZDY.DMS.Models.Validations
{
    public class MobileAttribute : RegularExpressionAttribute
    {
        public MobileAttribute() : base("^1[0-9]{10}$")
        {
            ErrorMessage = "手机号码不正确";
        }
    }
}
