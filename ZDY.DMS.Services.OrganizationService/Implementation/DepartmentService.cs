using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZDY.DMS.Models;
using ZDY.DMS.Repositories;
using ZDY.DMS.Services.OrganizationService.ServiceContracts;

namespace ZDY.DMS.Services.OrganizationService.Implementation
{
    public class DepartmentService: IDepartmentService
    {
        private readonly IRepositoryContext repositoryContext;
        private readonly IRepository<Guid, Department> departmentFlowRepository;

        public DepartmentService(IRepositoryContext repositoryContext)
        {
            this.repositoryContext = repositoryContext;
            this.departmentFlowRepository = this.repositoryContext.GetRepository<Guid, Department>();
        }

        public async Task<IEnumerable<Department>> GetAllDepartmentAsync(Guid companyID)
        {
            return await departmentFlowRepository.FindAllAsync(t => t.CompanyId == companyID, o => o.Desc(t => t.TimeStamp));
        }
    }
}
