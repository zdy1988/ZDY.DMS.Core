using System;
using ZDY.DMS.AspNetCore.Dictionary;
using ZDY.DMS.AspNetCore.Bootstrapper.Module;

namespace ZDY.DMS.Services.PermissionService
{
    public class PermissionServiceModule : ServiceModule
    {
        public PermissionServiceModule(IDictionaryRegister dictionaryRegister)
            : base(dictionaryRegister)
        {

        }
    }
}
