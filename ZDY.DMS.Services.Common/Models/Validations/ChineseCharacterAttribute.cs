using System.ComponentModel.DataAnnotations;

namespace ZDY.DMS.Services.Common.Models.Validations
{
    public class ChineseCharacterAttribute : RegularExpressionAttribute
    {
        public ChineseCharacterAttribute() : base(@"^[\u4e00-\u9fa5]{0,}$")
        {
            ErrorMessage = "只能输入汉字";
        }
    }
}
