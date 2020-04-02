using System;
using System.Collections.Generic;
using System.Text;
using ZDY.DMS.AspNetCore.Dictionary;
using ZDY.DMS.AspNetCore.EntityMapper;
using ZDY.DMS.AspNetCore.Module;
using ZDY.DMS.Events;
using ZDY.DMS.Services.AdminService.DataTransferObjects;
using ZDY.DMS.Services.AdminService.Enums;
using ZDY.DMS.Services.AdminService.EventHandlers;
using ZDY.DMS.Services.AdminService.Models;
using ZDY.DMS.Services.Common.Events;

namespace ZDY.DMS.Services.AdminService
{
    public class AdminServiceModule : ServiceModule
    {
        public AdminServiceModule(IDictionaryRegister dictionaryRegister, IEntityMapperRegister entityMapperRegister, IEventSubscriber eventSubscriber)
            : base(dictionaryRegister, entityMapperRegister, eventSubscriber)
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

        protected override void EventHandlersInitializer()
        {
            this.EventSubscriber.Subscribe<WorkFlowInstallEvent, WorkFlowInstallEventHandler>();
            this.EventSubscriber.Subscribe<WorkFlowUnInstallEvent, WorkFlowUnInstallEventHandler>();
        }
    }
}
