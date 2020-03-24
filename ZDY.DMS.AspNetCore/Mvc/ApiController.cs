using System;
using Microsoft.AspNetCore.Mvc;
using ZDY.DMS.AspNetCore.Auth;

namespace ZDY.DMS.AspNetCore.Mvc
{
    //[Authorize]
    //[ApiController]
    [ApiRoute(ApiVersions.v1)]
    public class ApiController : ControllerBase
    {
        protected UserIdentity UserIdentity
        {
            get
            {
                return this.HttpContext.GetUserIdentity();
            }
        }
    }
}
