using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZDY.DMS.AspNetCore.Auth;
using ZDY.DMS.Services.Shared.ServiceContracts;
using ZDY.Metronic.UI;

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

        public List<SelectOption> Genders { get; set; }

        public List<SelectOption> DepartmentOptions { get; set; }

        public async Task OnGetAsync()
        {
            UserId = this.HttpContext.GetUserIdentity().Id;

            Genders = new List<SelectOption> {
                new SelectOption{Value= "男",Name= "男" },
                new SelectOption{Value="女",Name= "女" }
            };

            DepartmentOptions = await this.selectOptionsFactory.GetSelectDepartmentOptions();
        }
    }
}