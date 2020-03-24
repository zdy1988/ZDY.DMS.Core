using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ZDY.DMS.Models.Validations
{
    public class InternetUrlAttribute : RegularExpressionAttribute
    {
        public InternetUrlAttribute() : base(@"^http://([\w-]+\.)+[\w-]+(/[\w-./?%&=]*)?$")
        {
            ErrorMessage = "网址不正确";
        }
    }
}
