using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ZDY.DMS.Repositories;
using ZDY.DMS.DataPermission;
using ZDY.DMS.AspNetCore.Auth;
using ZDY.DMS.Services.WorkFlowService.ServiceContracts;
using ZDY.DMS.Services.Common.ServiceContracts;
using ZDY.DMS.Services.Common.Models;
using ZDY.DMS.Services.PermissionService.Models;
using ZDY.DMS.Services.OrganizationService.Models;
using ZDY.Metronic.UI;

namespace ZDY.DMS.Web
{
    public class SelectOptionsFactory
    {
        private readonly UserIdentity UserIdentity;
        private readonly IRepositoryContext repositoryContext;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IDictionaryService dictionaryService;
        private readonly IWorkFlowService workFlowService;
        private readonly IWorkFlowFormService  workFlowFormService;

        public SelectOptionsFactory(IRepositoryContext repositoryContext,
            IHttpContextAccessor httpContextAccessor,
            IDictionaryService dictionaryService,
            IWorkFlowService workFlowService,
            IWorkFlowFormService workFlowFormService)
        {
            this.UserIdentity = httpContextAccessor.HttpContext.GetUserIdentity();
            this.repositoryContext = repositoryContext;
            this.httpContextAccessor = httpContextAccessor;
            this.dictionaryService = dictionaryService;
            this.workFlowService = workFlowService;
            this.workFlowFormService = workFlowFormService;
        }

        private async Task<List<SelectOption>> GetOptionsUnderCompanyPermission<TEntity>(string keyProperty, string valueProperty)
            where TEntity : class, ICompanyEntity<Guid>
        {
            var data = await repositoryContext.GetRepository<Guid, TEntity>().FindAllAsync(t => t.CompanyId == UserIdentity.CompanyId);

            return data.Select(t => new SelectOption
            {
                Value = t.GetType().GetProperty(keyProperty).GetValue(t, null).ToString(),
                Name = t.GetType().GetProperty(valueProperty).GetValue(t, null).ToString()
            }).ToList();
        }

        public List<SelectOption> GetSelectOptionsByDictionary(string dictionaryKey)
        {
            var dictionary = this.dictionaryService.GetDictionary(dictionaryKey);

            if (dictionary.ContainsKey(dictionaryKey))
            {
                return dictionary[dictionaryKey].Select(t => new SelectOption { Value = t.Value, Name = t.Name }).ToList();
            }

            return null;
        }

        public async Task<List<SelectOption>> GetInstalledWorkFlowOptions()
        {
            var flowKinds = dictionaryService.GetDictionary("WorkFlowKinds")["WorkFlowKinds"];

            var flows = await workFlowService.GetInstalledWorkFlowCollectionAsync(UserIdentity.CompanyId);

            var options = new List<SelectOption>();

            foreach (var type in flowKinds)
            {
                options.AddRange(flows.Where(t => t.Type == int.Parse(type.Value)).Select(t => new SelectOption { Value = t.Id.ToString(), Name = t.Name, Section = type.Name }).ToList());
            }

            return options;
        }

        public async Task<List<SelectOption>> GetPublishedWorkFlowFormOptions()
        {
            var formKinds = dictionaryService.GetDictionary("WorkFlowFormKinds")["WorkFlowFormKinds"];

            var forms = await workFlowFormService.GetPublishedWorkFlowFormCollectionAsync(UserIdentity.CompanyId);

            var options = new List<SelectOption>();

            foreach (var type in formKinds)
            {
                options.AddRange(forms.Where(t => t.Type == int.Parse(type.Value)).Select(t => new SelectOption { Value = t.Id.ToString(), Name = t.Name, Section = type.Name }).ToList());
            }

            return options;
        }

        public async Task<List<SelectOption>> GetSelectUserGroupOptions()
        {
            var data = await repositoryContext.GetRepository<Guid,UserGroup>().FindAllAsync(t => t.CompanyId == UserIdentity.CompanyId);

            return data.Select(t => new SelectOption { Value = t.Id.ToString(), Name = t.GroupName }).ToList();
        }

        public async Task<List<SelectOption>> GetSelectDepartmentOptions()
        {
            var data = await repositoryContext.GetRepository<Guid, Department>().FindAllAsync(t => t.CompanyId == UserIdentity.CompanyId);

            return data.Select(t => new SelectOption { Value = t.Id.ToString(), Name = t.DepartmentName }).ToList();
        }

        public async Task<List<SelectOption>> GetSelectUserOptions()
        {
            var users = await repositoryContext.GetRepository<Guid, User>().FindAllAsync(t => t.CompanyId == UserIdentity.CompanyId && t.IsDisabled == false);

            var departments = await GetSelectDepartmentOptions();

            var options = new List<SelectOption>();

            if (departments.Count() > 0)
            {
                foreach (var department in departments)
                {
                    options.AddRange(users.Where(t => t.DepartmentId == Guid.Parse(department.Value)).Select(t => new SelectOption { Value = t.Id.ToString(), Name = t.NickName, Section = department.Name }).ToList());
                }
            }

            return options;
        }

        //public async Task<List<SelectOption>> GetSelectCustomerOptions()
        //{
        //    var customers = await repositoryContext.GetRepository<Guid, Customer>().FindAllAsync(t => t.IsDisabled == false && t.CompanyId == UserIdentity.CompanyId, t => t.Desc(a => a.TimeStamp).Desc(b => b.Order));

        //    return customers.Select(t => new SelectOption(t.Id.ToString(), t.CustomerName)).ToList();
        //}

        //public async Task<List<SelectOption>> GetSelectSupplierOptions()
        //{
        //    var suppliers = await repositoryContext.GetRepository<Guid, Supplier>().FindAllAsync(t => t.IsDisabled == false && t.CompanyId == UserIdentity.CompanyId, t => t.Desc(a => a.TimeStamp).Desc(b => b.Order));

        //    return suppliers.Select(t => new SelectOption(t.Id.ToString(), t.SupplierName)).ToList();
        //}

        //public async Task<List<SelectOption>> GetSelectWarehouseOptions()
        //{
        //    var warehouses = await repositoryContext.GetRepository<Guid, Warehouse>().FindAllAsync(t => t.IsDisabled == false && t.CompanyId == UserIdentity.CompanyId, t => t.Desc(a => a.TimeStamp).Desc(b => b.Order));

        //    return warehouses.Select(t => new SelectOption(t.Id.ToString(), t.WarehouseName)).ToList();
        //}
    }
}
