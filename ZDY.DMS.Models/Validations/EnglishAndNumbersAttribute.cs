using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ZDY.DMS.Models.Validations
{
    public class EnglishAndNumbersAttribute : RegularExpressionAttribute
    {
        public EnglishAndNumbersAttribute() : base(@"^[A-Za-z0-9]+$")
        {
            ErrorMessage = "只能输入数字和26个英文字母";
        }
    }
}
