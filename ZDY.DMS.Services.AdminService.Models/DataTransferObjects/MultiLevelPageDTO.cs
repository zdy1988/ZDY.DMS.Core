using System;
using System.Collections.Generic;
using ZDY.DMS.Services.AdminService.Models;

namespace ZDY.DMS.Services.AdminService.DataTransferObjects
{
    public class MultiLevelPageDTO: Page
    {
        public IEnumerable<MultiLevelPageDTO> ChildLevelPages { get; set; }
    }
}
