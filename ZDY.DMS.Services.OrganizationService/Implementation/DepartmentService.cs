using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZDY.DMS.AspNetCore.Bootstrapper.Service;
using ZDY.DMS.Repositories;
using ZDY.DMS.Services.OrganizationService.Models;
using ZDY.DMS.Services.OrganizationService.ServiceContracts;

namespace ZDY.DMS.Services.OrganizationService.Implementation
{
    public class DepartmentService: ServiceBase<OrganizationServiceModule>, IDepartmentService
    {
        private readonly IRepository<Guid, Department> departmentFlowRepository;

        public DepartmentService()
        {
            this.departmentFlowRepository = this.GetRepository<Guid, Department>();
        }

        public async Task<IEnumerable<Department>> GetAllDepartmentAsync(Guid companyId)
        {
            return await departmentFlowRepository.FindAllAsync(t => t.CompanyId == companyId, o => o.Desc(t => t.TimeStamp));
        }
    }
}
