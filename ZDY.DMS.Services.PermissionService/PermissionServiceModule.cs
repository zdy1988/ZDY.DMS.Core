using System;
using System.Collections.Generic;
using System.Text;
using ZDY.DMS.AspNetCore.Dictionary;
using ZDY.DMS.AspNetCore.Module;

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
