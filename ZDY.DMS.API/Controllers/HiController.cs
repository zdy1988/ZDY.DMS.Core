using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZDY.DMS.AspNetCore.Mvc;

namespace ZDY.DMS.API.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [ApiRoute(ApiVersions.v1)]
    public class HiController : Controller
    {
        [HttpGet]
        public object SystemInformation()
        {
            return Ok(new
            {
                Service = "Hi,DMS!",
                Environment.MachineName,
                Environment.Is64BitOperatingSystem,
                Environment.Is64BitProcess,
                Environment.OSVersion.VersionString
            });
        }
    }
}