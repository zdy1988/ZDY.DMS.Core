using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZDY.DMS.AspNetCore.Auth;
using ZDY.DMS.Services.Common.ServiceContracts;

namespace ZDY.DMS.Web.Pages.Home
{
    public class UserProfileModel : PageModel
    {
        private readonly IDictionaryService dictionaryService;
        private readonly SelectOptionsFactory selectOptionsFactory;

        public UserProfileModel(IDictionaryService dictionaryService,
            SelectOptionsFactory selectOptionsFactory)
        {
            this.dictionaryService = dictionaryService;
            this.selectOptionsFactory = selectOptionsFactory;
        }

        public Guid UserId { get; set; }

        public IEnumerable<KeyValuePair<string, string>> Genders { get; set; }

        public IEnumerable<KeyValuePair<string, string>> DepartmentOptions { get; set; }

        public async Task OnGetAsync()
        {
            UserId = this.HttpContext.GetUserIdentity().Id;

            Genders = new List<KeyValuePair<string, string>> {
                new KeyValuePair<string, string>("男", "男"),
                new KeyValuePair<string, string>("女", "女")
            };

            DepartmentOptions = await this.selectOptionsFactory.GetSelectDepartmentOptions();
        }
    }
}