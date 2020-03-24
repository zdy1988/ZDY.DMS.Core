using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZDY.DMS.DataTransferObjects;
using ZDY.DMS.ServiceContracts;

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

        public Dictionary<string, IEnumerable<DictionaryItemDTO>> Dictionary { get; set; }

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
        public IEnumerable<KeyValuePair<string, string>> WorkFlowOptions { get; set; }
        public IEnumerable<KeyValuePair<string, string>> WorkFlowFormOptions { get; set; }

        public async void OnGet()
        {
            Dictionary = this.dictionaryService.GetDictionary("WorkFlowKinds,WorkFlowState,WorkFlowSignatureKinds,WorkFlowControlKinds,WorkFlowHandlerKinds,WorkFlowBackTactic,WorkFlowHandleTactic,WorkFlowBackKinds,WorkFlowCountersignatureTactic,WorkFlowSubFlowTactic,WorkFlowTransitConditionKinds");

            WorkFlowKinds = Dictionary["WorkFlowKinds"].Select(t => new KeyValuePair<string, string>(t.Value, t.Name));
            WorkFlowState = Dictionary["WorkFlowState"].Select(t => new KeyValuePair<string, string>(t.Value, t.Name));
            WorkFlowSignatureKinds = Dictionary["WorkFlowSignatureKinds"].Select(t => new KeyValuePair<string, string>(t.Value, t.Name));
            WorkFlowControlKinds = Dictionary["WorkFlowControlKinds"].Select(t => new KeyValuePair<string, string>(t.Value, t.Name));
            WorkFlowHandlerKinds = Dictionary["WorkFlowHandlerKinds"].Select(t => new KeyValuePair<string, string>(t.Value, t.Name));
            WorkFlowBackTactic = Dictionary["WorkFlowBackTactic"].Select(t => new KeyValuePair<string, string>(t.Value, t.Name));
            WorkFlowHandleTactic = Dictionary["WorkFlowHandleTactic"].Select(t => new KeyValuePair<string, string>(t.Value, t.Name));
            WorkFlowBackKinds = Dictionary["WorkFlowBackKinds"].Select(t => new KeyValuePair<string, string>(t.Value, t.Name));
            WorkFlowCountersignatureTactic = Dictionary["WorkFlowCountersignatureTactic"].Select(t => new KeyValuePair<string, string>(t.Value, t.Name));
            WorkFlowSubFlowTactic = Dictionary["WorkFlowSubFlowTactic"].Select(t => new KeyValuePair<string, string>(t.Value, t.Name));
            WorkFlowTransitConditionKinds = Dictionary["WorkFlowTransitConditionKinds"].Select(t => new KeyValuePair<string, string>(t.Value, t.Name));

            UserOptions = await selectOptionsFactory.GetSelectUserOptions();
            DepartmentOptions = await selectOptionsFactory.GetSelectDepartmentOptions();
            UserGroupOptions = await selectOptionsFactory.GetSelectUserGroupOptions();
            WorkFlowOptions = await selectOptionsFactory.GetInstalledWorkFlowOptions();
            WorkFlowFormOptions = await selectOptionsFactory.GetPublishedWorkFlowFormOptions();
        }
    }
}