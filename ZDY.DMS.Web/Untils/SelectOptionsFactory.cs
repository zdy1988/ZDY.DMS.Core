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

        private async Task<List<KeyValuePair<string, string>>> GetOptionsUnderCompanyPermission<TEntity>(string keyProperty, string valueProperty)
            where TEntity : class, ICompanyEntity<Guid>
        {
            var data = await repositoryContext.GetRepository<Guid, TEntity>().FindAllAsync(t => t.CompanyId == UserIdentity.CompanyId);

            return data.Select(t => new KeyValuePair<string, string>(t.GetType().GetProperty(keyProperty).GetValue(t, null).ToString(), t.GetType().GetProperty(valueProperty).GetValue(t, null).ToString())).ToList();
        }

        public IEnumerable<KeyValuePair<string, string>> GetSelectOptionsByDictionary(string dictionaryKey)
        {
            var dictionary = this.dictionaryService.GetDictionary(dictionaryKey);

            if (dictionary.ContainsKey(dictionaryKey))
            {
                return dictionary[dictionaryKey].Select(t => new KeyValuePair<string, string>(t.Value, t.Name));
            }

            return null;
        }

        public async Task<List<KeyValuePair<string, string>>> GetInstalledWorkFlowOptions()
        {
            var flowKinds = dictionaryService.GetDictionary("WorkFlowKinds")["WorkFlowKinds"];

            var flows = await workFlowService.GetInstalledWorkFlows(UserIdentity.CompanyId);

            var options = new List<KeyValuePair<string, string>>();

            foreach (var type in flowKinds)
            {
                options.Add(new KeyValuePair<string, string>("@" + type.Value, type.Name));
                options.AddRange(flows.Where(t => t.Type == int.Parse(type.Value)).Select(t => new KeyValuePair<string, string>(t.Id.ToString(), t.Name)).ToList());
            }

            return options;
        }

        public async Task<List<KeyValuePair<string, string>>> GetPublishedWorkFlowFormOptions()
        {
            var formKinds = dictionaryService.GetDictionary("WorkFlowFormKinds")["WorkFlowFormKinds"];

            var forms = await workFlowFormService.GetPublishedWorkFlowForms(UserIdentity.CompanyId);

            var options = new List<KeyValuePair<string, string>>();

            foreach (var type in formKinds)
            {
                options.Add(new KeyValuePair<string, string>("@" + type.Value, type.Value));
                options.AddRange(forms.Where(t => t.Type == int.Parse(type.Value)).Select(t => new KeyValuePair<string, string>(t.Id.ToString(), t.Name)).ToList());
            }

            return options;
        }

        public async Task<List<KeyValuePair<string, string>>> GetSelectUserGroupOptions()
        {
            var data = await repositoryContext.GetRepository<Guid,UserGroup>().FindAllAsync(t => t.CompanyId == UserIdentity.CompanyId);

            return data.Select(t => new KeyValuePair<string, string>(t.Id.ToString(), t.GroupName)).ToList();
        }

        public async Task<List<KeyValuePair<string, string>>> GetSelectDepartmentOptions()
        {
            var data = await repositoryContext.GetRepository<Guid, Department>().FindAllAsync(t => t.CompanyId == UserIdentity.CompanyId);

            return data.Select(t => new KeyValuePair<string, string>(t.Id.ToString(), t.DepartmentName)).ToList();
        }

        public async Task<List<KeyValuePair<string, string>>> GetSelectUserOptions()
        {
            var users = await repositoryContext.GetRepository<Guid, User>().FindAllAsync(t => t.CompanyId == UserIdentity.CompanyId && t.IsDisabled == false);

            var departments = await GetSelectDepartmentOptions();

            var options = new List<KeyValuePair<string, string>>();

            if (departments.Count() > 0)
            {
                foreach (var department in departments)
                {
                    options.Add(new KeyValuePair<string, string>("@" + department.Key, department.Value));
                    options.AddRange(users.Where(t => t.DepartmentId == Guid.Parse(department.Key)).Select(t => new KeyValuePair<string, string>(t.Id.ToString(), t.NickName)).ToList());
                }
            }

            return options;
        }

        //public async Task<List<KeyValuePair<string, string>>> GetSelectCustomerOptions()
        //{
        //    var customers = await repositoryContext.GetRepository<Guid, Customer>().FindAllAsync(t => t.IsDisabled == false && t.CompanyId == UserIdentity.CompanyId, t => t.Desc(a => a.TimeStamp).Desc(b => b.Order));

        //    return customers.Select(t => new KeyValuePair<string, string>(t.Id.ToString(), t.CustomerName)).ToList();
        //}

        //public async Task<List<KeyValuePair<string, string>>> GetSelectSupplierOptions()
        //{
        //    var suppliers = await repositoryContext.GetRepository<Guid, Supplier>().FindAllAsync(t => t.IsDisabled == false && t.CompanyId == UserIdentity.CompanyId, t => t.Desc(a => a.TimeStamp).Desc(b => b.Order));

        //    return suppliers.Select(t => new KeyValuePair<string, string>(t.Id.ToString(), t.SupplierName)).ToList();
        //}

        //public async Task<List<KeyValuePair<string, string>>> GetSelectWarehouseOptions()
        //{
        //    var warehouses = await repositoryContext.GetRepository<Guid, Warehouse>().FindAllAsync(t => t.IsDisabled == false && t.CompanyId == UserIdentity.CompanyId, t => t.Desc(a => a.TimeStamp).Desc(b => b.Order));

        //    return warehouses.Select(t => new KeyValuePair<string, string>(t.Id.ToString(), t.WarehouseName)).ToList();
        //}
    }
}
