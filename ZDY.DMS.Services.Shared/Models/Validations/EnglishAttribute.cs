using System.ComponentModel.DataAnnotations;

namespace ZDY.DMS.Services.Shared.Models.Validations
{
    public class EnglishAttribute : RegularExpressionAttribute
    {
        public EnglishAttribute() : base(@"^[A-Za-z]+$")
        {
            ErrorMessage = "只能输入英文字母";
        }
    }
}
