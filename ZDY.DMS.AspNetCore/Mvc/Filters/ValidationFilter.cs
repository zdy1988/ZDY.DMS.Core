using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZDY.DMS.AspNetCore.Mvc.Filters
{
    public class ValidationFilter: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var action = ((ControllerActionDescriptor)context.ActionDescriptor).ActionName.ToLower();

            if (action == "add" || action == "update")
            {
                //只有在提交Model的时候才进行验证
                if (context.ActionArguments.Count == 1
                    && context.ActionArguments.First().Value != null
                    && context.ActionArguments.First().Value.GetType().GetInterfaces().Contains((typeof(IEntity<Guid>))))
                {
                    if (!context.ModelState.IsValid)
                    {
                        string error = string.Empty;

                        foreach (var key in context.ModelState.Keys)
                        {
                            var state = context.ModelState[key];
                            if (state.Errors.Any())
                            {
                                error = state.Errors.First(t => !string.IsNullOrEmpty(t.ErrorMessage)).ErrorMessage;
                                break;
                            }
                        }

                        throw new InvalidOperationException(error);
                    }
                }
            }
        }
    }
}