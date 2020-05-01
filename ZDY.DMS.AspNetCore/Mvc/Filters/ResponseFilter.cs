using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace ZDY.DMS.AspNetCore.Mvc.Filters
{
    public class ResponseFilter : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result is FileContentResult)
            {
                return;
            }

            var apiResult = new ApiResult();
            apiResult.StatusCode = context.HttpContext.Response.StatusCode;
            apiResult.IsSuccess = apiResult.StatusCode == 200;

            if (context.Result is ObjectResult)
            {
                var objectResult = context.Result as ObjectResult;
                if (objectResult is null)
                {
                    throw new InvalidOperationException("数据不存在");
                }
                apiResult.Data = objectResult.Value;
                context.Result = new JsonResult(apiResult);
            }
            else if (context.Result is JsonResult)
            {
                var jsonResult = context.Result as JsonResult;
                apiResult.Data = jsonResult.Value;
                context.Result = new JsonResult(apiResult);
            }
            else if (context.Result is ContentResult)
            {
                var contentResult = context.Result as ContentResult;
                apiResult.Message = contentResult.Content;
                context.Result = new JsonResult(apiResult);
            }
            else if (context.Result is EmptyResult)
            {
                context.Result = new JsonResult(apiResult);
            }
        }
    }
}
