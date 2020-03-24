using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ZDY.DMS.Models.Validations;

namespace ZDY.DMS.Models
{
    [Table("Business_Order")]
    public class BusinessOrder : BaseEntity
    {
        /// <summary>
        /// 公司ID
        /// </summary>
        public Guid CompanyId { get; set; }
        /// <summary>
        /// 流水号
        /// </summary>
        public string SerialCode { get; set; }
        /// <summary>
        /// 销售人员
        /// </summary>
        public Guid SalerId { get; set; } = default(Guid);
        /// <summary>
        /// 客户ID
        /// </summary>
        public Guid CustomerId { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        [Required(ErrorMessage = "请填写客户名称！")]
        public string CustomerName { get; set; }
        /// <summary>
        /// 客户手机
        /// </summary>
        [Required(ErrorMessage = "请填写手机号码")]
        [Mobile]
        public string CustomerMobile { get; set; }
        /// <summary>
        /// 客户地址
        /// </summary>
        public string CustomerAddress { get; set; }
        /// <summary>
        /// 订单类型
        /// </summary>
        public int BusinessOrderType { get; set; } = 0;
        /// <summary>
        /// 货物总数
        /// </summary>
        public int TotalCount { get; set; } = 0;
        /// <summary>
        /// 金额总计
        /// </summary>
        public decimal TotalAmount { get; set; } = 0;
        /// <summary>
        /// 业务订单状态
        /// </summary>
        public int BusinessOrderState { get; set; } = 0;
        /// <summary>
        /// 备注
        /// </summary>
        public string Note { get; set; }
        /// <summary>
        /// 是否禁用
        /// </summary>
        public bool IsDisabled { get; set; } = false;
    }
}
