using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System;

namespace ZDY.DMS.AspNetCore.Mvc
{
    /// <summary>
    /// Web Api /api/{version=v1}/[controler]/[action]
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class ApiRouteAttribute : RouteAttribute, IApiDescriptionGroupNameProvider
    {
        /// <summary>
        /// 自定义路由构造函数
        /// </summary>
        /// <param name="actionName"></param>
        public ApiRouteAttribute(string actionName = "[action]") : base("/api/{Version}/[controller]/" + actionName)
        {
        }

        /// <summary>
        /// 自定义路由构造函数
        /// </summary>
        /// <param name="actionName"></param>
        /// <param name="version"></param>
        public ApiRouteAttribute(ApiVersions version, string actionName = "[action]") : base($"/api/{version.ToString()}/[controller]/{actionName}")
        {
            GroupName = version.ToString();
        }

        /// <summary>
        /// 分组名称
        /// </summary>
        public string GroupName { get; set; }
    }

    /// <summary>
    /// Api接口版本 每次新版本增加一个
    /// </summary>
    public enum ApiVersions
    {
        v1 = 1,
        v2 = 2
    }
}
