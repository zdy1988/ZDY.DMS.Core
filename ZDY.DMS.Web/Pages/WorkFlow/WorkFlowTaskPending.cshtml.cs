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
    public class WorkFlowTaskPendingModel : PageModel
    {
        private readonly IDictionaryService dictionaryService;

        public WorkFlowTaskPendingModel(IDictionaryService dictionaryService)
        {
            this.dictionaryService = dictionaryService;
        }

        public Dictionary<string, IEnumerable<DictionaryItemDTO>> Dictionary { get; set; }

        public IEnumerable<KeyValuePair<string, string>> WorkFlowTaskState { get; set; }

        public void OnGet()
        {
            Dictionary = this.dictionaryService.GetDictionary("WorkFlowTaskState");
            WorkFlowTaskState = Dictionary["WorkFlowTaskState"].Select(t => new KeyValuePair<string, string>(t.Value, t.Name));
        }
    }
}