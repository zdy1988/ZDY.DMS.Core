using System;
using System.ComponentModel.DataAnnotations;
using ZDY.DMS.Models.Validations;

namespace ZDY.DMS.Models
{
    /// <summary>
    /// 供应商
    /// </summary>
    public class Supplier : BaseEntity
    {
        /// <summary>
        /// 公司Id
        /// </summary>
        public Guid CompanyId { get; set; } = default(Guid);
        /// <summary>
        /// 厂商名称
        /// </summary>
        [Required(ErrorMessage = "请填写厂商名称")]
        public string SupplierName { get; set; }
        /// <summary>
        /// 手机 
        /// </summary>
        [Required(ErrorMessage = "请填写手机号码")]
        [Mobile]
        public string Mobile { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string Contact { get; set; }
        /// <summary>
        /// 省市区域
        /// </summary>
        public string Provinces { get; set; } = "北京 北京";
        /// <summary>
        /// 地址 
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 电话 
        /// </summary>
        [Validations.Phone]
        public string Phone { get; set; }
        /// <summary>
        /// 传真 
        /// </summary>
        public string Fax { get; set; }
        /// <summary>
        ///  邮编
        /// </summary>
        public string ZipCode { get; set; }
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
