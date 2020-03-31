using System;
using System.Collections.Generic;
using System.Text;
using ZDY.DMS.AspNetCore.Dictionary;
using ZDY.DMS.AspNetCore.Module;
using ZDY.DMS.Services.AdminService.Enums;

namespace ZDY.DMS.Services.AdminService
{
    public class AdminServiceModule : ServiceModule
    {
        public AdminServiceModule(IDictionaryRegister dictionaryRegister)
            : base(dictionaryRegister)
        {

        }

        protected override void DictionaryInitializer()
        {
            this.DictionaryRegister.RegisterEnum<DictionaryKinds>();
            this.DictionaryRegister.RegisterEnum<LogKinds>();
        }
    }
}
