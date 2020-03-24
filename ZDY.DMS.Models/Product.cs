using System;
using System.ComponentModel.DataAnnotations;
using ZDY.DMS.Models.Validations;

namespace ZDY.DMS.Models
{
    public class Product : BaseEntity
    {
        /// <summary>
        /// 公司Id
        /// </summary>
        public Guid CompanyId { get; set; } = default(Guid);
        /// <summary>
        /// 封面
        /// </summary>
        public Guid Cover { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        [Required(ErrorMessage = "请填写商品名称")]
        public string ProductName { get; set; }
        /// <summary>
        /// 商品编码
        /// </summary>
        [Required(ErrorMessage = "请填写商品编码")]
        public string ProductCode { get; set; }
        /// <summary>
        /// 商品标签
        /// </summary>
        public string Tags { get; set; }
        /// <summary>
        /// 默认单价
        /// </summary>
        [Required(ErrorMessage = "请填写默认单价")]
        [Money]
        public decimal Price { get; set; } = 0;
        /// <summary>
        /// 成本
        /// </summary>
        [Money]
        public decimal Cost { get; set; } = 0;
        /// <summary>
        /// 默认供应商
        /// </summary>
        public Guid SupplierId { get; set; } = default(Guid);
        /// <summary>
        /// 默认仓库
        /// </summary>
        public Guid WarehouseId { get; set; } = default(Guid);
        /// <summary>
        /// 产品类型
        /// </summary>
        [Required(ErrorMessage = "请填写产品类型")]
        public string ProductType { get; set; }
        /// <summary>
        /// 产品颜色
        /// </summary>
        public string ProductColors { get; set; }
        /// <summary>
        /// 产品型号
        /// </summary>
        public string ProductModels { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Note { get; set; }
        /// <summary>
        /// 是否禁用
        /// </summary>
        public bool IsDisabled { get; set; } = false;
        /// <summary>
        /// 产品状态
        /// </summary>
        public int ProductState { get; set; } = 0;
        /// <summary>
        /// 产品介绍
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 排序字段
        /// </summary>
        public int Order { get; set; }
    }
}
