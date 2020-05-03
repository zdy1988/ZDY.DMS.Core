using System.ComponentModel.DataAnnotations;

namespace ZDY.DMS.Services.Shared.Models.Validations
{
    public class MoneyAttribute : RegularExpressionAttribute
    {
        public MoneyAttribute() : base("^[0-9]+(.[0-9]{1,3})?$")
        {
            ErrorMessage = "金额不正确";
        }
    }
}
