using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZDY.DMS.Services.Common.DataTransferObjects;
using ZDY.DMS.Services.Common.ServiceContracts;

namespace ZDY.DMS.Web.Pages.WorkFlow
{
    public class WorkFlowFormDesign : PageModel
    {
        private readonly IDictionaryService dictionaryService;

        public WorkFlowFormDesign(IDictionaryService dictionaryService)
        {
            this.dictionaryService = dictionaryService;
        }

        public Dictionary<string, IEnumerable<DictionaryItemDTO>> Dictionary { get; set; }

        public IEnumerable<KeyValuePair<string, string>> WorkFlowFormKinds { get; set; }
        public IEnumerable<KeyValuePair<string, string>> WorkFlowFormState { get; set; }

        public void OnGet()
        {
            Dictionary = dictionaryService.GetDictionary("WorkFlowFormKinds,WorkFlowFormState");

            WorkFlowFormKinds = Dictionary["WorkFlowFormKinds"].Select(t => new KeyValuePair<string, string>(t.Value, t.Name));
            WorkFlowFormState = Dictionary["WorkFlowFormState"].Select(t => new KeyValuePair<string, string>(t.Value, t.Name));
        }
    }
}