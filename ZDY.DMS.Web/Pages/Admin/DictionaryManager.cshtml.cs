using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZDY.DMS.AspNetCore.Dictionary;
using ZDY.DMS.Services.Shared.ServiceContracts;
using ZDY.Metronic.UI;

namespace ZDY.DMS.Web.Pages.Admin
{
    public class DictionaryManagerModel : PageModel
    {
        private readonly IDictionaryService dictionaryService;
        private readonly SelectOptionsFactory selectOptionsFactory;

        public DictionaryManagerModel(IDictionaryService dictionaryService,
            SelectOptionsFactory selectOptionsFactory)
        {
            this.dictionaryService = dictionaryService;
            this.selectOptionsFactory = selectOptionsFactory;
        }

        public Dictionary<string, IEnumerable<KeyValuePaired>> Dictionary { get; set; }

        public List<SelectOption> DictionaryKinds { get; set; }

        public void OnGet()
        {
             Dictionary = this.dictionaryService.GetDictionary("DictionaryKinds");

             DictionaryKinds = this.selectOptionsFactory.GetSelectOptionsByDictionary("DictionaryKinds");
        }
    }
}