using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZDY.DMS.AspNetCore.Dictionary;
using ZDY.DMS.Services.Common.DataTransferObjects;
using ZDY.DMS.Services.Common.ServiceContracts;

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

        public IEnumerable<KeyValuePair<string, string>> WorkFlowKinds { get; set; }
        public IEnumerable<KeyValuePair<string, string>> WorkFlowState { get; set; }
        public IEnumerable<KeyValuePair<string, string>> WorkFlowSignatureKinds { get; set; }
        public IEnumerable<KeyValuePair<string, string>> WorkFlowControlKinds { get; set; }
        public IEnumerable<KeyValuePair<string, string>> WorkFlowHandlerKinds { get; set; }
        public IEnumerable<KeyValuePair<string, string>> WorkFlowBackTactic { get; set; }
        public IEnumerable<KeyValuePair<string, string>> WorkFlowHandleTactic { get; set; }
        public IEnumerable<KeyValuePair<string, string>> WorkFlowBackKinds { get; set; }
        public IEnumerable<KeyValuePair<string, string>> WorkFlowCountersignatureTactic { get; set; }
        public IEnumerable<KeyValuePair<string, string>> WorkFlowSubFlowTactic { get; set; }
        public IEnumerable<KeyValuePair<string, string>> WorkFlowTransitConditionKinds { get; set; }

        public IEnumerable<KeyValuePair<string, string>> UserOptions { get; set; }
        public IEnumerable<KeyValuePair<string, string>> DepartmentOptions { get; set; }
        public IEnumerable<KeyValuePair<string, string>> UserGroupOptions { get; set; }
        public IEnumerable<KeyValuePair<string, string>> WorkFlowInstalledFlowOptions { get; set; }
        public IEnumerable<KeyValuePair<string, string>> WorkFlowPublishedFormOptions { get; set; }

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