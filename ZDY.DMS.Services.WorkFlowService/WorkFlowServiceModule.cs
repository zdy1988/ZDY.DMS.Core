using System;
using System.Collections.Generic;
using System.Text;
using ZDY.DMS.AspNetCore.Dictionary;
using ZDY.DMS.AspNetCore.Bootstrapper.Module;
using ZDY.DMS.Events;
using ZDY.DMS.Services.Common.Events;
using ZDY.DMS.Services.WorkFlowService.Enums;
using ZDY.DMS.Services.WorkFlowService.EventHandlers;

namespace ZDY.DMS.Services.WorkFlowService
{
    public class WorkFlowServiceModule: ServiceModule
    {
        public WorkFlowServiceModule(IDictionaryRegister dictionaryRegister, IEventSubscriber eventSubscriber)
            : base(dictionaryRegister, eventSubscriber)
        {

        }

        protected override void DictionaryInitializer()
        {
            this.DictionaryRegister.RegisterEnum<WorkFlowBackKinds>();
            this.DictionaryRegister.RegisterEnum<WorkFlowBackTacticKinds>("WorkFlowBackTactic");
            this.DictionaryRegister.RegisterEnum<WorkFlowControlKinds>();
            this.DictionaryRegister.RegisterEnum<WorkFlowCountersignatureTacticKinds>("WorkFlowCountersignatureTactic");
            this.DictionaryRegister.RegisterEnum<WorkFlowExecuteKinds>();
            this.DictionaryRegister.RegisterEnum<WorkFlowFormKinds>();
            this.DictionaryRegister.RegisterEnum<WorkFlowFormState>();
            this.DictionaryRegister.RegisterEnum<WorkFlowHandlerKinds>();
            this.DictionaryRegister.RegisterEnum<WorkFlowHandleTacticKinds>("WorkFlowHandleTactic");
            this.DictionaryRegister.RegisterEnum<WorkFlowInstanceState>();
            this.DictionaryRegister.RegisterEnum<WorkFlowKinds>();
            this.DictionaryRegister.RegisterEnum<WorkFlowSignatureKinds>();
            this.DictionaryRegister.RegisterEnum<WorkFlowState>();
            this.DictionaryRegister.RegisterEnum<WorkFlowStepKinds>();
            this.DictionaryRegister.RegisterEnum<WorkFlowSubFlowTacticKinds>("WorkFlowSubFlowTactic");
            this.DictionaryRegister.RegisterEnum<WorkFlowTaskKinds>();
            this.DictionaryRegister.RegisterEnum<WorkFlowTaskState>();
            this.DictionaryRegister.RegisterEnum<WorkFlowTransitConditionKinds>();
        }

        protected override void EventHandlersInitializer()
        {
            EventSubscriber.Subscribe<UserCreatedEvent, UserCreatedEventHandler>();
        }
    }
}
