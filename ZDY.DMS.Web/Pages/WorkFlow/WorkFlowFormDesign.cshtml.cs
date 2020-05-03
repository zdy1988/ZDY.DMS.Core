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
    public class WorkFlowFormDesign : PageModel
    {
        private readonly IDictionaryService dictionaryService;
        private readonly SelectOptionsFactory selectOptionsFactory;

        public WorkFlowFormDesign(IDictionaryService dictionaryService,
             SelectOptionsFactory selectOptionsFactory)
        {
            this.dictionaryService = dictionaryService;
            this.selectOptionsFactory = selectOptionsFactory;
        }

        public Dictionary<string, IEnumerable<KeyValuePaired>> Dictionary { get; set; }

        public List<SelectOption> WorkFlowFormKinds { get; set; }
        public List<SelectOption> WorkFlowFormState { get; set; }

        public void OnGet()
        {
            Dictionary = dictionaryService.GetDictionary("WorkFlowFormKinds,WorkFlowFormState");

            WorkFlowFormKinds = this.selectOptionsFactory.GetSelectOptionsByDictionary("WorkFlowFormKinds");
            WorkFlowFormState = this.selectOptionsFactory.GetSelectOptionsByDictionary("WorkFlowFormState");
        }
    }
}