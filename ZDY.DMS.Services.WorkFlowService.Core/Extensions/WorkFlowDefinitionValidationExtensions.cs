using System;
using System.Collections.Generic;
using System.Linq;
using ZDY.DMS.Services.WorkFlowService.Core.Enums;
using ZDY.DMS.Services.WorkFlowService.Core.Models;

namespace ZDY.DMS.Services.WorkFlowService.Core.Extensions
{
    public static class WorkFlowDefinitionValidationExtensions
    {
        public static bool Valid(this WorkFlowDefinition definition, out List<Tuple<Guid, string, string, string>> results)
        {
            results = CheckFlow(definition);

            return !results.Any();
        }

        private static List<Tuple<Guid, string, string, string>> CheckFlow( WorkFlowDefinition definition)
        {
            //验证流程名称
            if (string.IsNullOrEmpty(definition.Name))
            {
                throw new InvalidOperationException("流程名称未填写");
            }

            //验证数据存在
            if (definition.Steps.Count <= 0 || definition.Transits.Count <= 0)
            {
                throw new InvalidOperationException("流程数据解析异常");
            }

            //验证首尾存在
            var start = definition.Steps.Find(t => t.StepId == WorkFlowConstant.StartStepId);
            if (start == null)
            {
                throw new InvalidOperationException("流程数据解析异常");
            }

            var end = definition.Steps.Find(t => t.StepId == WorkFlowConstant.EndStepId);
            if (end == null)
            {
                throw new InvalidOperationException("流程数据解析异常");
            }

            List<Tuple<Guid, string, string, string>> results = new List<Tuple<Guid, string, string, string>>();

            var stepArray = definition.Steps.Select(t => t.StepId).ToArray();

            var fromTransitPointArray = definition.Transits.Select(t => t.FromStepId).ToArray();

            var toTransitPointArray = definition.Transits.Select(t => t.ToStepId).ToArray();

            var transitPointArray = fromTransitPointArray.Concat(toTransitPointArray).ToArray();

            //验证首尾已连接
            if (!transitPointArray.Contains(WorkFlowConstant.StartStepId))
            {
                results.Add(new Tuple<Guid, string, string, string>(WorkFlowConstant.StartStepId, "开始", "步骤", "开始步骤未连接"));
            }

            if (!transitPointArray.Contains(WorkFlowConstant.EndStepId))
            {
                results.Add(new Tuple<Guid, string, string, string>(WorkFlowConstant.EndStepId, "结束", "步骤", "结束步骤未连接"));
            }

            //验证结束节点不允许有下一出口
            if (fromTransitPointArray.Contains(WorkFlowConstant.EndStepId))
            {
                results.Add(new Tuple<Guid, string, string, string>(WorkFlowConstant.EndStepId, "结束", "步骤", "结束步骤不能再有下一出口"));
            }


            //验证步骤节点
            var results1 = CheckSteps(definition.Steps, stepArray, transitPointArray, fromTransitPointArray, toTransitPointArray);

            if (results1.Count > 0)
            {
                results.AddRange(results1);
            }

            //验证条件链接
            var results2 = CheckTransits(definition.Transits, stepArray, transitPointArray);
            if (results2.Count > 0)
            {
                results.AddRange(results2);
            }

            return results;
        }

        private static List<Tuple<Guid, string, string, string>> CheckSteps(List<WorkFlowStep> steps, Guid[] stepArray, Guid[] transitPointArray, Guid[] fromTransitPointArray, Guid[] toTransitPointArray)
        {
            List<Tuple<Guid, string, string, string>> results = new List<Tuple<Guid, string, string, string>>();

            //验证步骤节点
            foreach (var step in steps)
            {
                //验证节点ID唯一
                var count = stepArray.Where(t => t == step.StepId).Count();
                if (count > 1)
                {
                    results.Add(new Tuple<Guid, string, string, string>(step.StepId, step.StepName, "步骤", "条件节点ID存在重复情况"));
                }

                //验证节点名称必须填写
                if (string.IsNullOrEmpty(step.StepName))
                {
                    results.Add(new Tuple<Guid, string, string, string>(step.StepId, step.StepName, "步骤", "步骤名称是必须填写的"));
                }

                //验证节点必须都链接
                if (!transitPointArray.Contains(step.StepId))
                {
                    results.Add(new Tuple<Guid, string, string, string>(step.StepId, step.StepName, "步骤", "步骤没有被连接到"));
                }

                //验证节点出口
                if (!fromTransitPointArray.Contains(step.StepId) && !step.IsEnd())
                {
                    results.Add(new Tuple<Guid, string, string, string>(step.StepId, step.StepName, "步骤", "步骤出口没有被连接到"));
                }

                //验证节点入库
                if (!toTransitPointArray.Contains(step.StepId) && !step.IsStart())
                {
                    results.Add(new Tuple<Guid, string, string, string>(step.StepId, step.StepName, "步骤", "步骤入库没有被连接到"));
                }

                if (step.StepId == WorkFlowConstant.StartStepId || step.StepId == WorkFlowConstant.EndStepId)
                {
                    continue;
                }

                //验证规则
                if (step.IsHandleTacticBy(WorkFlowHandleTacticKinds.PercentageAgree) && step.Percentage == 0)
                {
                    results.Add(new Tuple<Guid, string, string, string>(step.StepId, step.StepName, "步骤", $"步骤处理策略为【依据人数比例】，但策略百分比不可以设置为 0 %"));
                }

                if (step.IsCountersignatureTacticBy(WorkFlowCountersignatureTacticKinds.PercentageAgree) && step.CountersignaturePercentage == 0)
                {
                    results.Add(new Tuple<Guid, string, string, string>(step.StepId, step.StepName, "步骤", $"步骤处理策略为【依据同意步骤比例】，但会签策略百分比不可以设置为 0 %"));
                }
            }

            return results;
        }

        private static List<Tuple<Guid, string, string, string>> CheckTransits(List<WrokFlowTransit> transits, Guid[] stepArray, Guid[] transitPointArray)
        {
            List<Tuple<Guid, string, string, string>> results = new List<Tuple<Guid, string, string, string>>();

            //验证条件链接
            foreach (var transit in transits)
            {
                if (string.IsNullOrEmpty(transit.TransitName))
                {
                    results.Add(new Tuple<Guid, string, string, string>(transit.TransitId, transit.TransitName, "条件", "条件名称是必须填写的"));
                }

                if (transit.ConditionIsNot(WorkFlowTransitConditionKinds.None) && string.IsNullOrEmpty(transit.ConditionValue))
                {
                    var conditionTypeName = transit.GetConditionTypeName();
                    results.Add(new Tuple<Guid, string, string, string>(transit.TransitId, transit.TransitName, "条件", $"条件类型为【{conditionTypeName}】，但条件表达式不存在"));
                }

                if (!stepArray.Contains(transit.FromStepId) || !stepArray.Contains(transit.ToStepId))
                {
                    results.Add(new Tuple<Guid, string, string, string>(transit.TransitId, transit.TransitName, "条件", "条件存在未连接端点"));
                }

                if (transits.Where(t => t.FromStepId == transit.FromStepId && t.ToStepId == transit.ToStepId).Count() > 1)
                {
                    results.Add(new Tuple<Guid, string, string, string>(transit.TransitId, transit.TransitName, "条件", "条件两端只能同方向只能连接一次，此条件两端被连接多次"));
                }
            }

            return results;
        }
    }
}
