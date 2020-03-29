using System.ComponentModel.DataAnnotations;

namespace ZDY.DMS.Services.Common.Models.Validations
{
    public class InternetUrlAttribute : RegularExpressionAttribute
    {
        public InternetUrlAttribute() : base(@"^http://([\w-]+\.)+[\w-]+(/[\w-./?%&=]*)?$")
        {
            ErrorMessage = "网址不正确";
        }
    }
}
