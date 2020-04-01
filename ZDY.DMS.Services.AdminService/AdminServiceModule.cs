using System;
using System.Collections.Generic;
using System.Text;
using ZDY.DMS.AspNetCore.Dictionary;
using ZDY.DMS.AspNetCore.EntityMapper;
using ZDY.DMS.AspNetCore.Module;
using ZDY.DMS.Services.AdminService.DataTransferObjects;
using ZDY.DMS.Services.AdminService.Enums;
using ZDY.DMS.Services.AdminService.Models;

namespace ZDY.DMS.Services.AdminService
{
    public class AdminServiceModule : ServiceModule
    {
        public AdminServiceModule(IDictionaryRegister dictionaryRegister, IEntityMapperRegister entityMapperRegister)
            : base(dictionaryRegister, entityMapperRegister)
        {

        }

        protected override void DictionaryInitializer()
        {
            this.DictionaryRegister.RegisterEnum<DictionaryKinds>();
            this.DictionaryRegister.RegisterEnum<LogKinds>();
        }

        protected override void EntityMapperInitializer()
        {
            this.EntityMapperRegister.CreateMap<MultiLevelPageDTO, Page>();
            this.EntityMapperRegister.CreateMap<Page, MultiLevelPageDTO>();
        }
    }
}
