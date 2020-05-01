using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ZDY.DMS.Web.Pages
{
    [AllowAnonymous]
    public class ErrorModel : PageModel
    {
        public string Code { get; set; } = "404";

        public string Message { get; set; } = "您访问的页面未找到！";

        public void OnGet(string code)
        {
            if (!string.IsNullOrWhiteSpace(code))
            {
                Code = code;

                Message = Code == "404" ? Message : "您的访问出现错误！";
            }
        }
    }
}