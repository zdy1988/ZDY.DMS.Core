using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ZDY.DMS.Models.Validations
{
    public class IDCardAttribute : RegularExpressionAttribute
    {
        public IDCardAttribute() : base(@"^\d{15}|\d{18}$")
        {
            ErrorMessage = "身份证号码不正确";
        }
    }
}
