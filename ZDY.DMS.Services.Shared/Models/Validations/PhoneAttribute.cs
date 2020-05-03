using System.ComponentModel.DataAnnotations;

namespace ZDY.DMS.Services.Shared.Models.Validations
{
    public class PhoneAttribute : RegularExpressionAttribute
    {
        /// <summary>
        /// 正确格式为："XXX-XXXXXXX"、"XXXX- XXXXXXXX"、"XXX-XXXXXXX"、"XXX-XXXXXXXX"、"XXXXXXX"和"XXXXXXXX"。
        /// </summary>
        public PhoneAttribute() : base(@"^(0\\d{2}-\\d{8}(-\\d{1,4})?)|(0\\d{3}-\\d{7,8}(-\\d{1,4})?)$")
        {
            ErrorMessage = "电话号码不正确";
        }
    }
}
