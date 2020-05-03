using System.ComponentModel.DataAnnotations;

namespace ZDY.DMS.Services.Shared.Models.Validations
{
    public class IDCardAttribute : RegularExpressionAttribute
    {
        public IDCardAttribute() : base(@"^\d{15}|\d{18}$")
        {
            ErrorMessage = "身份证号码不正确";
        }
    }
}
