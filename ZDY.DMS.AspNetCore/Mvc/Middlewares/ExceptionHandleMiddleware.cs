using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using ZDY.DMS.AspNetCore.Mvc;

namespace Microsoft.AspNetCore.Builder
{
    public class ExceptionHandleMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandleMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var message = "";
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                if (e is InvalidOperationException)
                {
                    context.Response.StatusCode = 510;
                    message = e.Message;
                }
                else
                {
                    context.Response.StatusCode = 400;
                    message = e.Message;
                }
            }
            finally
            {
                switch (context.Response.StatusCode)
                {
                    case 510:
                        break;
                    case 401:
                        message = "操作未被授权...";
                        break;
                    case 404:
                        message = "服务未找到...";
                        break;
                    case 502:
                        message = "请求时发生错误...";
                        break;
                    default:

#if !DEBUG
                    message = "未知错误";
#endif

                        break;
                }

                if (!string.IsNullOrWhiteSpace(message))
                {
                    await ConstructResponseResultAsync(context, message, context.Response.StatusCode);
                }
            }
        }

        private static async Task ConstructResponseResultAsync(HttpContext context, string message, int ststusCode)
        {
            context.Response.ContentType = "application/json;charset=utf-8";

            var result = new ApiResult
            {
                StatusCode = ststusCode,
                IsSuccess = ststusCode == 200,
                Message = message
            };

            await context.Response.WriteAsync(JsonConvert.SerializeObject(result));
        }
    }

    public static class ExceptionHandleBuilderExtensions
    {
        public static IApplicationBuilder UseErrorHandle(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionHandleMiddleware>();
        }
    }
}
