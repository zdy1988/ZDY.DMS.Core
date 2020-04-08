using System;
using System.Collections.Generic;
using System.Text;
using ZDY.DMS.AspNetCore.Dictionary;
using ZDY.DMS.AspNetCore.Bootstrapper.Module;

namespace ZDY.DMS.Services.UserService
{
    public class UserServiceModule : ServiceModule
    {
        public UserServiceModule(IDictionaryRegister dictionaryRegister)
            : base(dictionaryRegister)
        {

        }
    }
}
