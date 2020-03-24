using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ZDY.DMS.Models.Validations
{
    public class EnglishAttribute : RegularExpressionAttribute
    {
        public EnglishAttribute() : base(@"^[A-Za-z]+$")
        {
            ErrorMessage = "只能输入英文字母";
        }
    }
}
