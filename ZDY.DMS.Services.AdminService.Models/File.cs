using System;
using ZDY.DMS.Services.Shared.Models;

namespace ZDY.DMS.Services.AdminService.Models
{
    public class File : BaseEntity
    {
        public string OriginalName { get; set; }
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public string ExtensionName { get; set; }
        public string Path { get; set; }
        public string Type { get; set; }
        public Guid BusinessID { get; set; } = default(Guid);
        public int Order { get; set; } = 0;
    }
}
