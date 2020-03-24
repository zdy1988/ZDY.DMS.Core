using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZDY.DMS.Models
{
    [Table("Warehouse_Order_Item_Attribute")]
    public class WarehouseOrderItemAttribute : BaseEntity
    {
        public Guid WarehouseOrderItemId { get; set; }
        public string Attribute { get; set; }
    }
}
