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
    public class WorkFlowInstanceManagerModel : PageModel
    {
        private readonly IDictionaryService dictionaryService;

        public WorkFlowInstanceManagerModel(IDictionaryService dictionaryService)
        {
            this.dictionaryService = dictionaryService;
        }

        public Dictionary<string, IEnumerable<DictionaryItemDTO>> Dictionary { get; set; }

        public IEnumerable<KeyValuePair<string, string>> WorkFlowInstanceState { get; set; }

        public void OnGet()
        {
            Dictionary = dictionaryService.GetDictionary("WorkFlowInstanceState");

            WorkFlowInstanceState = Dictionary["WorkFlowInstanceState"].Select(t => new KeyValuePair<string, string>(t.Value, t.Name));
        }
    }
}