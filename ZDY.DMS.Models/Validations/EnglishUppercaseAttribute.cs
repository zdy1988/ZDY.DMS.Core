using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ZDY.DMS.Models.Validations
{
    public class EnglishUppercaseAttribute : RegularExpressionAttribute
    {
        public EnglishUppercaseAttribute() : base(@"^[A-Z]+$")
        {
            ErrorMessage = "只能输入大写英文字母";
        }
    }
}
