using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ZDY.DMS;

namespace ZDY.DMS.Models
{
    [Table("Business_Order_Item")]
    public class BusinessOrderItem : BaseEntity
    {
        /// <summary>
        /// 订单ID
        /// </summary>
        public Guid BusinessOrderId { get; set; }
        /// <summary>
        /// 商品ID
        /// </summary>
        public Guid ProductId { get; set; }
        /// <summary>
        /// 商品编码
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Count { get; set; } = 0;
        /// <summary>
        /// 属性
        /// </summary>
        public string Attributes { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public decimal Price { get; set; } = 0;
        /// <summary>
        /// 小计
        /// </summary>
        public decimal SubTotal { get; set; } = 0;
    }
}
