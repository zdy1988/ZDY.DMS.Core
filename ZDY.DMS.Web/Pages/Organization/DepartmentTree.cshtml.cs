using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZDY.DMS.AspNetCore.Auth;
using ZDY.DMS.Services.OrganizationService.Models;
using ZDY.DMS.Services.OrganizationService.ServiceContracts;

namespace ZDY.DMS.Web.Pages.Organization
{
    public class DepartmentTreeModel : PageModel
    {
        private readonly IDepartmentService departmentService;

        public DepartmentTreeModel(IDepartmentService departmentService)
        {
            this.departmentService = departmentService;
        }

        public Guid CompanyId { get; set; }

        public IEnumerable<Department> DepartmentData { get; set; }

        //public IEnumerable<TreeTableItem> TreeTableItems { get; set; }

        //public IEnumerable<TreeTableHead> TreeTableHeads { get; set; }

        public async Task OnGetAsync(Guid companyId)
        {
            if (companyId == default)
            {
                var userIdentity = this.HttpContext.GetUserIdentity();
                companyId = userIdentity.CompanyId;
            }

            CompanyId = companyId;

            DepartmentData = await this.departmentService.GetAllDepartmentAsync(CompanyId);

            //TreeTableItems = DepartmentData.Select(t => new TreeTableItem
            //{

            //    Id = t.Id,
            //    ParentId = t.ParentId,
            //    Order = t.Order,
            //    Data = t
            //});

            //TreeTableHeads = new List<TreeTableHead>
            //{
            //    new TreeTableHead{ Name="部门名称",Field="DepartmentName" },
            //    new TreeTableHead{ Name="电话号码",Field="Phone" },
            //    new TreeTableHead{ Name="传真号码",Field="Fax" }
            //};
        }
    }
}