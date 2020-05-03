using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZDY.DMS.AspNetCore.Dictionary;
using ZDY.DMS.Services.Shared.ServiceContracts;
using ZDY.Metronic.UI;

namespace ZDY.DMS.Web.Pages.WorkFlow
{
    public class WorkFlowDesignModel : PageModel
    {
        private readonly IDictionaryService dictionaryService;
        private readonly SelectOptionsFactory selectOptionsFactory;

        public WorkFlowDesignModel(IDictionaryService dictionaryService,
            SelectOptionsFactory selectOptionsFactory)
        {
            this.dictionaryService = dictionaryService;
            this.selectOptionsFactory = selectOptionsFactory;
        }

        public Dictionary<string, IEnumerable<KeyValuePaired>> Dictionary { get; set; }

        public List<SelectOption> WorkFlowKinds { get; set; }
        public List<SelectOption> WorkFlowState { get; set; }
        public List<SelectOption> WorkFlowSignatureKinds { get; set; }
        public List<SelectOption> WorkFlowControlKinds { get; set; }
        public List<SelectOption> WorkFlowHandlerKinds { get; set; }
        public List<SelectOption> WorkFlowBackTactic { get; set; }
        public List<SelectOption> WorkFlowHandleTactic { get; set; }
        public List<SelectOption> WorkFlowBackKinds { get; set; }
        public List<SelectOption> WorkFlowCountersignatureTactic { get; set; }
        public List<SelectOption> WorkFlowSubFlowTactic { get; set; }
        public List<SelectOption> WorkFlowTransitConditionKinds { get; set; }

        public List<SelectOption> UserOptions { get; set; }
        public List<SelectOption> DepartmentOptions { get; set; }
        public List<SelectOption> UserGroupOptions { get; set; }
        public List<SelectOption> WorkFlowInstalledFlowOptions { get; set; }
        public List<SelectOption> WorkFlowPublishedFormOptions { get; set; }

        public async void OnGet()
        {
            Dictionary = this.dictionaryService.GetDictionary("WorkFlowKinds,WorkFlowState,WorkFlowSignatureKinds,WorkFlowControlKinds,WorkFlowHandlerKinds,WorkFlowBackTactic,WorkFlowHandleTactic,WorkFlowBackKinds,WorkFlowCountersignatureTactic,WorkFlowSubFlowTactic,WorkFlowTransitConditionKinds");

            WorkFlowKinds = this.selectOptionsFactory.GetSelectOptionsByDictionary("WorkFlowKinds");
            WorkFlowState = this.selectOptionsFactory.GetSelectOptionsByDictionary("WorkFlowState");
            WorkFlowSignatureKinds = this.selectOptionsFactory.GetSelectOptionsByDictionary("WorkFlowSignatureKinds");
            WorkFlowControlKinds = this.selectOptionsFactory.GetSelectOptionsByDictionary("WorkFlowControlKinds");
            WorkFlowHandlerKinds = this.selectOptionsFactory.GetSelectOptionsByDictionary("WorkFlowHandlerKinds");
            WorkFlowBackTactic = this.selectOptionsFactory.GetSelectOptionsByDictionary("WorkFlowBackTactic");
            WorkFlowHandleTactic = this.selectOptionsFactory.GetSelectOptionsByDictionary("WorkFlowHandleTactic");
            WorkFlowBackKinds = this.selectOptionsFactory.GetSelectOptionsByDictionary("WorkFlowBackKinds");
            WorkFlowCountersignatureTactic = this.selectOptionsFactory.GetSelectOptionsByDictionary("WorkFlowCountersignatureTactic");
            WorkFlowSubFlowTactic = this.selectOptionsFactory.GetSelectOptionsByDictionary("WorkFlowSubFlowTactic");
            WorkFlowTransitConditionKinds = this.selectOptionsFactory.GetSelectOptionsByDictionary("WorkFlowTransitConditionKinds");

            UserOptions = await selectOptionsFactory.GetSelectUserOptions();
            DepartmentOptions = await selectOptionsFactory.GetSelectDepartmentOptions();
            UserGroupOptions = await selectOptionsFactory.GetSelectUserGroupOptions();
            WorkFlowInstalledFlowOptions = await selectOptionsFactory.GetInstalledWorkFlowOptions();
            WorkFlowPublishedFormOptions = await selectOptionsFactory.GetPublishedWorkFlowFormOptions();
        }
    }
}