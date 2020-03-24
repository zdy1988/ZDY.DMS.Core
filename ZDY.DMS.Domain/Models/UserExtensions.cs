using System;
using System.Collections.Generic;
using System.Text;
using ZDY.DMS.Extensions.DependencyInjection.Autofac;
using ZDY.DMS.ServiceContracts;

namespace ZDY.DMS.Domain.Models
{
    public static class UserExtensions
    {
        public static string GetUserAvatarUrl(this ZDY.DMS.Models.User user)
        {
            return string.IsNullOrEmpty(user.AvatarUrl) ? ServiceLocator.GetService<IStaticFileService>().GetFileUrl(user.Avatar) : user.AvatarUrl;
        }
    }
}
