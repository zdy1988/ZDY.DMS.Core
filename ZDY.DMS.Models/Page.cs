using System;

namespace ZDY.DMS.Models
{
    public class Page : BaseEntity
    {
        public string PageName { get; set; }
        public string PageCode { get; set; }
        public int Level { get; set; } = 0;
        public Guid ParentId { get; set; } = default(Guid);
        public string Type { get; set; }
        public bool IsInMenu { get; set; } = true;
        public string MenuName { get; set; }
        public string Src { get; set; }
        public string Icon { get; set; }
        public int Order { get; set; } = 0;
        /// <summary>
        /// 是够不检查权限，直接放行
        /// </summary>
        public bool IsPassed { get; set; } = false;
        /// <summary>
        /// 0 通用，其他 只输入某个公司
        /// </summary>
        public Guid CompanyId { get; set; } = default(Guid);
        /// <summary>
        /// 是否禁用
        /// </summary>
        public bool IsDisabled { get; set; } = false;
    }
}
