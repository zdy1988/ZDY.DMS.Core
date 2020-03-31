using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZDY.DMS.AspNetCore;
using ZDY.DMS.Repositories;
using ZDY.DMS.Services.Common.Models;
using ZDY.DMS.Services.OrganizationService.ServiceContracts;

namespace ZDY.DMS.Services.OrganizationService.Implementation
{
    public class DepartmentService: ServiceBase<OrganizationServiceModule>, IDepartmentService
    {
        private readonly IRepository<Guid, Department> departmentFlowRepository;

        public DepartmentService(Func<Type, IRepositoryContext> repositoryContextFactory)
            : base(repositoryContextFactory)
        {
            this.departmentFlowRepository = this.GetRepository<Guid, Department>();
        }

        public async Task<IEnumerable<Department>> GetAllDepartmentAsync(Guid companyID)
        {
            return await departmentFlowRepository.FindAllAsync(t => t.CompanyId == companyID, o => o.Desc(t => t.TimeStamp));
        }
    }
}
