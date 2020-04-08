using System;
using ZDY.DMS.AspNetCore.Dictionary;
using ZDY.DMS.AspNetCore.Bootstrapper.Module;

namespace ZDY.DMS.Services.OrganizationService
{
    public class OrganizationServiceModule : ServiceModule
    {
        public OrganizationServiceModule(IDictionaryRegister dictionaryRegister)
            : base(dictionaryRegister)
        {

        }
    }
}
