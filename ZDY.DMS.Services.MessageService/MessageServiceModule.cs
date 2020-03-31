using System;
using System.Collections.Generic;
using System.Text;
using ZDY.DMS.AspNetCore.Dictionary;
using ZDY.DMS.AspNetCore.Module;

namespace ZDY.DMS.Services.MessageService
{
    public class MessageServiceModule  : ServiceModule
    {
        public MessageServiceModule(IDictionaryRegister dictionaryRegister)
            : base(dictionaryRegister)
        {

        }
    }
}
