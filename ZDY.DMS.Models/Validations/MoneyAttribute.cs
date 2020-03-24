using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ZDY.DMS.Models.Validations
{
    public class MoneyAttribute : RegularExpressionAttribute
    {
        public MoneyAttribute() : base("^[0-9]+(.[0-9]{1,3})?$")
        {
            ErrorMessage = "金额不正确";
        }
    }
}
