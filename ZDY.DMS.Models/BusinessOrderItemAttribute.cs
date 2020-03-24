using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZDY.DMS.Models
{
    [Table("Business_Order_Item_Attribute")]
    public class BusinessOrderItemAttribute : BaseEntity
    {
        public Guid BusinessOrderItemId { get; set; }
        public string Attribute { get; set; }
    }
}
