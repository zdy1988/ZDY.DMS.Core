using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZDY.DMS.Services.WorkFlowService.DataObjects
{
    public class WorkFlowDefinition
    {       
        public string Name { get; set; }
        public List<WorkFlowStep> Steps { get; set; }
        public List<WrokFlowTransit> Transits { get; set; }

        public static WorkFlowDefinition Parse(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<WorkFlowDefinition>(json);
            }
            catch
            {
                throw new InvalidOperationException("流程数据解析异常");
            }
        }
    }
}
