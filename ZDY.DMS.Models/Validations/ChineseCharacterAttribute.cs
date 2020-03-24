using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ZDY.DMS.Models.Validations
{
    public class ChineseCharacterAttribute : RegularExpressionAttribute
    {
        public ChineseCharacterAttribute() : base(@"^[\u4e00-\u9fa5]{0,}$")
        {
            ErrorMessage = "只能输入汉字";
        }
    }
}
