using System;
using System.ComponentModel.DataAnnotations;
using ZDY.DMS.Models.Validations;

namespace ZDY.DMS.Models
{
    /// <summary>
    /// 客户
    /// </summary>
    public class Customer : BaseEntity
    {
        /// <summary>
        /// 公司ID
        /// </summary>
        public Guid CompanyId { get; set; } = default(Guid);
        /// <summary>
        /// 客户名称
        /// </summary>
        [Required(ErrorMessage = "请填写客户名称")]
        public string CustomerName { get; set; }
        /// <summary>
        /// 电话 
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
        /// 传真 
        /// </summary>
        public string Fax { get; set; }
        /// <summary>
        /// 电子邮箱 
        /// </summary>
        [Email]
        public string Email { get; set; }
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
