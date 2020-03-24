using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ZDY.DMS.Models.Validations
{
    public class EnglishLowercaseAttribute : RegularExpressionAttribute
    {
        public EnglishLowercaseAttribute() : base(@"^[a-z]+$")
        {
            ErrorMessage = "只能输入小写英文字母";
        }
    }
}
