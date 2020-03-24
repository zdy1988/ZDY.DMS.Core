using System;
using System.Collections.Generic;
using System.Text;
using ZDY.DMS.Models;

namespace ZDY.DMS.DataTransferObjects
{
    public class MultiLevelPageDTO: Page
    {
        public IEnumerable<MultiLevelPageDTO> ChildLevelPages { get; set; }
    }
}
