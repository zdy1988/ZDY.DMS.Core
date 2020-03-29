using System.ComponentModel.DataAnnotations;

namespace ZDY.DMS.Services.Common.Models.Validations
{
    public class EnglishUppercaseAttribute : RegularExpressionAttribute
    {
        public EnglishUppercaseAttribute() : base(@"^[A-Z]+$")
        {
            ErrorMessage = "只能输入大写英文字母";
        }
    }
}
