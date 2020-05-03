using System.ComponentModel.DataAnnotations;

namespace ZDY.DMS.Services.Shared.Models.Validations
{
    public class EnglishLowercaseAttribute : RegularExpressionAttribute
    {
        public EnglishLowercaseAttribute() : base(@"^[a-z]+$")
        {
            ErrorMessage = "只能输入小写英文字母";
        }
    }
}
