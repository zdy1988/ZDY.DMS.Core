using System;
using System.Collections.Generic;
using System.Text;
using ZDY.DMS.Services.Common.Models;
using ZDY.DMS.Services.Common.ServiceContracts;

namespace ZDY.DMS.Services.Common.Extensions
{
    public static class UserExtensions
    {
        public static string GetUserAvatarUrl(this User user)
        {
            return string.IsNullOrEmpty(user.AvatarUrl) ? ServiceLocator.GetService<IStaticFileService>().GetFileUrl(user.Avatar) : user.AvatarUrl;
        }
    }
}
