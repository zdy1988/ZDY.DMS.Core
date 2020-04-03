using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.Middlewares
{
    public class CookiesAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public CookiesAuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Headers.Keys.Count(t => t == "Authorization") == 0)
            {
                string token = "";

                if (context.Request.Cookies.TryGetValue("zdy_token", out token))
                {
                    if (!String.IsNullOrEmpty(token))
                    {
                        context.Request.Headers.Add("Authorization", $"Bearer {token}");
                    }
                }
            }

            await _next.Invoke(context);
        }
    }
}
