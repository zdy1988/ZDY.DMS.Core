using System.ComponentModel.DataAnnotations;

namespace ZDY.DMS.Services.Common.Models.Validations
{
    public class MobileAttribute : RegularExpressionAttribute
    {
        public MobileAttribute() : base("^1[0-9]{10}$")
        {
            ErrorMessage = "手机号码不正确";
        }
    }
}
