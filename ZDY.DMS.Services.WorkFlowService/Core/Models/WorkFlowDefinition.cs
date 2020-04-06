using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ZDY.DMS.Services.WorkFlowService.Core.Models
{
    public class WorkFlowDefinition
    {       
        public string Name { get; set; }
        public List<WorkFlowStep> Steps { get; set; }
        public List<WrokFlowTransit> Transits { get; set; }


        public static WorkFlowDefinition Parse(string source)
        {
            try
            {
                return JsonConvert.DeserializeObject<WorkFlowDefinition>(source);
            }
            catch
            {
                throw new InvalidOperationException("流程数据解析异常");
            }
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
