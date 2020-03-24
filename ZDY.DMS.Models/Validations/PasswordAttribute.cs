using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ZDY.DMS.Models.Validations
{
    public class PasswordAttribute : RegularExpressionAttribute
    {
        public PasswordAttribute() : base(@"^[a-zA-Z]\w{5,17}$")
        {
            ErrorMessage = "正确格式为：以字母开头，长度在6~18之间，只能包含字符、数字和下划线";
        }
    }
}
