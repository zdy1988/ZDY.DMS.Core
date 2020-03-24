using System;
using System.ComponentModel.DataAnnotations;

namespace ZDY.DMS.Models
{
    public class Warehouse : BaseEntity
    {
        /// <summary>
        /// 公司ID
        /// </summary>
        public Guid CompanyId { get; set; } = default(Guid);
        /// <summary>
        /// 仓库名称
        /// </summary>
        [Required(ErrorMessage = "请填写仓库名称")]
        public string WarehouseName { get; set; }
        /// <summary>
        /// 电话 
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 省市区域
        /// </summary>
        public string Provinces { get; set; } = "北京 北京";
        /// <summary>
        /// 地址 
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Note { get; set; }
        /// <summary>
        /// 是否禁用
        /// </summary>
        public bool IsDisabled { get; set; } = false;
        /// <summary>
        /// 排序字段
        /// </summary>
        public int Order { get; set; }
    }
}
