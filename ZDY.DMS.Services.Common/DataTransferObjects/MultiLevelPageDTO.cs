using System;
using System.Collections.Generic;
using ZDY.DMS.Services.Common.Models;

namespace ZDY.DMS.Services.Common.DataTransferObjects
{
    public class MultiLevelPageDTO: Page
    {
        public IEnumerable<MultiLevelPageDTO> ChildLevelPages { get; set; }
    }
}
