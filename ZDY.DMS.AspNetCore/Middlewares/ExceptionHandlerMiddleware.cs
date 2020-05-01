using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using ZDY.DMS.AspNetCore.Auth;
using ZDY.DMS.AspNetCore.Events;
using ZDY.DMS.AspNetCore.Mvc;
using ZDY.DMS.Events;

namespace Microsoft.AspNetCore.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IEventPublisher eventPublisher)
        {
            var message = "";
            try
            {
                await _next.Invoke(context);
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

                    if (context.TryGetUserId(out Guid userId))
                    {
                        eventPublisher.Publish<ExceptionLogCreatedEvent>(new ExceptionLogCreatedEvent(e, userId));
                    }
                    else
                    {
                        eventPublisher.Publish<ExceptionLogCreatedEvent>(new ExceptionLogCreatedEvent(e));
                    }
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
            if (context.Request.Path.ToUriComponent().ToLower().StartsWith("/api"))
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
            else
            {
                context.Response.Redirect($"/Error?code={ststusCode}");
            }
        }
    }
}
