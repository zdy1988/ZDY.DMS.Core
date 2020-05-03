using System.ComponentModel.DataAnnotations;

namespace ZDY.DMS.Services.Shared.Models.Validations
{
    public class InternetUrlAttribute : RegularExpressionAttribute
    {
        public InternetUrlAttribute() : base(@"^http://([\w-]+\.)+[\w-]+(/[\w-./?%&=]*)?$")
        {
            ErrorMessage = "网址不正确";
        }
    }
}
