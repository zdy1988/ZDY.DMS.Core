using System;
using System.Collections.Generic;
using System.Text;

namespace ZDY.DMS.Services.WorkFlowService.Core.Models
{
    public class WorkFlowUser
    {
        /// <summary>
        /// 人员ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 人员名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 人员公司ID
        /// </summary>
        public Guid CompanyId { get; set; }

        /// <summary>
        /// 人员部门ID
        /// </summary>
        public Guid DepartmentId { get; set; }

        /// <summary>
        /// 人员角色ID
        /// </summary>
        public Guid UserGroupId { get; set; }
    }
}
