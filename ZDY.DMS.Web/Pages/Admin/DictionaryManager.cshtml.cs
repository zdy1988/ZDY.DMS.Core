using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZDY.DMS.Services.Common.DataTransferObjects;
using ZDY.DMS.Services.Common.ServiceContracts;

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

        public Dictionary<string, IEnumerable<DictionaryItemDTO>> Dictionary { get; set; }

        public IEnumerable<KeyValuePair<string, string>> DictionaryKinds { get; set; }

        public void OnGet()
        {
             Dictionary = this.dictionaryService.GetDictionary("DictionaryKinds");

             DictionaryKinds = this.selectOptionsFactory.GetSelectOptionsByDictionary("DictionaryKinds");
        }
    }
}