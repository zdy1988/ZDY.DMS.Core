using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZDY.DMS.AspNetCore.Auth;
using ZDY.DMS.Services.OrganizationService.ServiceContracts;
using ZDY.Metronic.UI;

namespace ZDY.DMS.Web.Pages.Organization
{
    public class DepartmentTreeModel : PageModel
    {
        private readonly IDepartmentService departmentService;

        public DepartmentTreeModel(IDepartmentService departmentService)
        {
            this.departmentService = departmentService;
        }

        public List<TreeTableField> Fields { get; set; }

        public List<TreeTableItem> Departments { get; set; }

        public async Task OnGetAsync(Guid companyId)
        {
            if (companyId == default)
            {
                var identity = this.HttpContext.GetUserIdentity();
                companyId = identity.CompanyId;
            }

            Fields = new List<TreeTableField> {
                new TreeTableField{ DisplayName = "部门名称", FieldName ="DepartmentName" },
                new TreeTableField{ DisplayName = "联系电话", FieldName ="Phone" },
                new TreeTableField{ DisplayName = "传真号码", FieldName ="Fax" },
                new TreeTableField{ DisplayName = "信息备注", FieldName ="Note" }
            };

            Departments = (await this.departmentService.GetAllDepartmentAsync(companyId)).Select(t => new TreeTableItem
            {
                Id = t.Id,
                ParentId = t.ParentId,
                Data = t,
                Order = t.Order
            }).ToList();
        }
    }
}