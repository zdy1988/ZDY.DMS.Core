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
    public class WorkFlowTaskPendingModel : PageModel
    {
        private readonly IDictionaryService dictionaryService;
        private readonly SelectOptionsFactory selectOptionsFactory;

        public WorkFlowTaskPendingModel(IDictionaryService dictionaryService,
            SelectOptionsFactory selectOptionsFactory)
        {
            this.dictionaryService = dictionaryService;
            this.selectOptionsFactory = selectOptionsFactory;
        }

        public Dictionary<string, IEnumerable<KeyValuePaired>> Dictionary { get; set; }

        public IEnumerable<KeyValuePair<string, string>> WorkFlowTaskState { get; set; }

        public void OnGet()
        {
            Dictionary = this.dictionaryService.GetDictionary("WorkFlowTaskState");

            WorkFlowTaskState = this.selectOptionsFactory.GetSelectOptionsByDictionary("WorkFlowTaskState");
        }
    }
}