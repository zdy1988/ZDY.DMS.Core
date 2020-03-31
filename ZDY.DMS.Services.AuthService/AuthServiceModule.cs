using System;
using System.Collections.Generic;
using System.Text;
using ZDY.DMS.AspNetCore.Dictionary;
using ZDY.DMS.AspNetCore.Module;

namespace ZDY.DMS.Services.AuthService
{
    public class AuthServiceModule : ServiceModule
    {
        public AuthServiceModule(IDictionaryRegister dictionaryRegister)
            : base(dictionaryRegister)
        {

        }
    }
}
