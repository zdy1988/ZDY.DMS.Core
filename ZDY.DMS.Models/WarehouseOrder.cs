using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZDY.DMS.Models
{
    [Table("Warehouse_Order")]
    public class WarehouseOrder : BaseEntity
    {
        /// <summary>
        /// 公司ID
        /// </summary>
        public Guid CompanyId { get; set; } = default(Guid);
        /// <summary>
        /// 流水号
        /// </summary>
        public string SerialCode { get; set; }
        /// <summary>
        /// 被操作的仓库ID
        /// 入库单 入库到某个仓库
        /// </summary>
        public Guid WarehouseId { get; set; } = default(Guid);
        /// <summary>
        /// 供应商ID
        /// 入库单 货物由某个供应商提供
        /// 退库单 退还给某个供应商
        /// </summary>
        public Guid SupplierId { get; set; } = default(Guid);
        /// <summary>
        /// 客户ID
        /// 出库单 出库给某个客户
        /// 返货单 返回是某个客户提供
        /// </summary>
        public Guid CustomerId { get; set; } = default(Guid);
        /// <summary>
        /// 业务订单ID
        /// 出库单 出库是因为某个订单出库
        /// </summary>
        public Guid BusinessOrderId { get; set; } = default(Guid);
        /// <summary>
        /// 仓库单据类型
        /// </summary>
        public int WarehouseOrderType { get; set; } = 0;
        /// <summary>
        /// 仓库单据状态
        /// </summary>
        public int WarehouseOrderState { get; set; } = 0;
        /// <summary>
        /// 货物总数
        /// </summary>
        public int TotalCount { get; set; } = 0;
        /// <summary>
        /// 金额总计
        /// </summary>
        public decimal TotalAmount { get; set; } = 0;
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
