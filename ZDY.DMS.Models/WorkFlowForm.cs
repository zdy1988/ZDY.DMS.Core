using System;
using System.ComponentModel.DataAnnotations.Schema;
using ZDY.DMS.DataPermission;

namespace ZDY.DMS.Models
{
    [Table("Work_Flow_Form")]
    public class WorkFlowForm : BaseEntity, ICompanyEntity<Guid>, IDisabledEntity<Guid>
    {
        /// <summary>
        /// 表单名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 表单分类
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 创建人ID
        /// </summary>
        public Guid CreaterId { get; set; } = default;

        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime LastModifyTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 设计数据
        /// </summary>
        public string DesignJson { get; set; }

        /// <summary>
        /// 设计字段数据
        /// </summary>
        public string DesignFieldJson { get; set; }

        /// <summary>
        /// 状态：0 设计中 1 已发布 2 已删除
        /// </summary>
        public int State { get; set; } = 0;

        /// <summary>
        /// 备注
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// 是否禁用
        /// </summary>
        public bool IsDisabled { get; set; } = false;

        /// <summary>
        /// 公司ID
        /// </summary>
        public Guid CompanyId { get; set; } = default;
    }
}
