using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZDY.DMS.Services.Common.ServiceContracts;

namespace ZDY.DMS.Web.Pages.User
{
    public class UserDetailModel : PageModel
    {
        private readonly IDictionaryService dictionaryService;
        private readonly SelectOptionsFactory selectOptionsFactory;

        public UserDetailModel(IDictionaryService dictionaryService,
            SelectOptionsFactory selectOptionsFactory)
        {
            this.dictionaryService = dictionaryService;
            this.selectOptionsFactory = selectOptionsFactory;
        }

        public IEnumerable<KeyValuePair<string, string>> Genders { get; set; }

        public IEnumerable<KeyValuePair<string, string>> DepartmentOptions { get; set; }

        public async Task OnGetAsync()
        {
            Genders = new List<KeyValuePair<string, string>> {
                new KeyValuePair<string, string>("男", "男"),
                new KeyValuePair<string, string>("女", "女")
            };

            DepartmentOptions = await this.selectOptionsFactory.GetSelectDepartmentOptions();
        }
    }
}