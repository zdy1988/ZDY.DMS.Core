using System;
using ZDY.DMS.Services.Common.Models;

namespace ZDY.DMS.Services.AdminService.Models
{
    public class Page : BaseEntity
    {
        public string PageName { get; set; }
        public string PageCode { get; set; }
        public Guid ParentId { get; set; } = default(Guid);
        public bool IsInMenu { get; set; } = true;
        public string MenuName { get; set; }
        public string Src { get; set; }
        public string Icon { get; set; }
        public int Order { get; set; } = 0;
        /// <summary>
        /// 加入菜单时，是否需要权限
        /// </summary>
        public bool IsPermissionRequired { get; set; } = true;
        /// <summary>
        /// default 全局通用 ，其他值则为某个公司独有
        /// </summary>
        public Guid CompanyId { get; set; } = default(Guid);
        /// <summary>
        /// 是否禁用
        /// </summary>
        public bool IsDisabled { get; set; } = false;
    }
}
