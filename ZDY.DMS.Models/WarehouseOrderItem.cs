using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZDY.DMS.Models
{
    [Table("Warehouse_Order_Item")]
    public class WarehouseOrderItem : BaseEntity
    {
        /// <summary>
        /// 仓库单据ID
        /// </summary>
        public Guid WarehouseOrderId { get; set; }
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
        /// 入库单价 或者 出库单价
        /// </summary>
        public decimal Price { get; set; } = 0;
        /// <summary>
        /// 金额小计
        /// </summary>
        public decimal SubTotal { get; set; } = 0;
    }
}
