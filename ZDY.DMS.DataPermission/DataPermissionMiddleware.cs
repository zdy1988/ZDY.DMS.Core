using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using ZDY.DMS.Querying.SearchModel.Model;

namespace ZDY.DMS.DataPermission
{
    public class DataPermissionMiddleware
    {
        private readonly RequestDelegate _next;

        public DataPermissionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IDataPermissionContext dataPermissionContext)
        {
            var request = context.Request;

            var permissionKey = request.Path.Value.Replace("/api/v1/", "");

            var permissions = dataPermissionContext.GetSearchSpecifics(permissionKey);

            if (request.Method.ToLower().Equals("post") && request.HasFormContentType && permissions?.Count > 0)
            {
                var form = await ReadRequestFormAsync(request);

                // 适时加入数据限制字段
                if (CanReconstructionForm(form))
                {
                    request.Form = ReconstructionForm(context, form, permissions);
                }
            }

            await _next.Invoke(context);
        }

        public async Task<IFormCollection> ReadRequestFormAsync(HttpRequest request)
        {
            if (!request.Body.CanSeek)
            {
                request.EnableBuffering();
                await request.Body.DrainAsync(default);
                request.Body.Seek(0, SeekOrigin.Begin);
            }

            var form = await request.ReadFormAsync();

            request.Body.Seek(0, SeekOrigin.Begin);

            return form;
        }

        public bool CanReconstructionForm(IFormCollection form)
        {
            return form.ContainsKey("PageIndex")
                && form.ContainsKey("PageSize")
                && form.ContainsKey("OrderField")
                && form.ContainsKey("IsAsc");
        }

        public IFormCollection ReconstructionForm(HttpContext context, IFormCollection form, List<string> permissions)
        {
            var list = form.ToDictionary(t => t.Key, t => t.Value);

            var i = 0;

            while (true)
            {
                if (form.ContainsKey($"Fields[{i}][Field]"))
                {
                    i++;
                }
                else
                {
                    break;
                }
            }

            foreach (var permission in permissions)
            {
                list.Add($"Fields[{i}][Field]", permission);
                list.Add($"Fields[{i}][Method]", ((int)QueryMethod.Equal).ToString());

                var value = GetClaimsIdentityValue(context, permission);
                if (!string.IsNullOrEmpty(value))
                {
                    list.Add($"Fields[{i}][Value]", value);
                }

                i++;
            }

            return new FormCollection(list);
        }

        public string GetClaimsIdentityValue(HttpContext context, string type)
        {
            if (type.Equals("IsDisabled"))
            {
                return "False";
            }

            var claimsIdentity = context.User.Identity as ClaimsIdentity;

            if (claimsIdentity.Claims.Any(c => c.Type == type))
            {
                return claimsIdentity.Claims.FirstOrDefault(c => c.Type == type).Value;
            }

            return null;
        }
    }
}
