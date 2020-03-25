using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using ZDY.DMS.Services.WorkFlowService.Exception;
using ZDY.DMS.Services.WorkFlowService.DataObjects;
using ZDY.DMS.Domain.Enums;
using ZDY.DMS.Repositories;
using ZDY.DMS.Models;
using ZDY.DMS.Extensions.DependencyInjection.Autofac;

namespace ZDY.DMS.Services.WorkFlowService
{
    public static class WorkFlowAnalysis
    {
        public static Guid StartStepID = new Guid("00000000-0000-0000-0000-000000000000");
        public static Guid EndStepID = new Guid("00000000-0000-0000-0000-000000000001");

        public static WorkFlowInstalled AnalyticWorkFlowInstalledData(string flowJson)
        {
            WorkFlowInstalled workFlowInstalled = null;

            try
            {
                workFlowInstalled = JsonConvert.DeserializeObject<WorkFlowInstalled>(flowJson);
            }
            catch(System.Exception e)
            {
                throw new AnalyzeMistakesException();
            }

            return workFlowInstalled;
        }

        public static List<Tuple<Guid, string, string, string>> CheckFlow(this WorkFlowInstalled workFlowInstalled)
        {
            //验证流程名称
            if (string.IsNullOrEmpty(workFlowInstalled.Name))
            {
                throw new InvalidOperationException("流程名称未填写");
            }

            //验证数据存在
            if (workFlowInstalled.Steps.Count <= 0 || workFlowInstalled.Transits.Count <= 0)
            {
                throw new AnalyzeMistakesException();
            }

            //验证首尾存在
            var start = workFlowInstalled.Steps.Find(t => t.StepId == StartStepID);
            if (start == null)
            {
                throw new AnalyzeMistakesException();
            }

            var end = workFlowInstalled.Steps.Find(t => t.StepId == EndStepID);
            if (end == null)
            {
                throw new AnalyzeMistakesException();
            }

            List<Tuple<Guid, string, string, string>> messages = new List<Tuple<Guid, string, string, string>>();

            var stepArray = workFlowInstalled.Steps.Select(t => t.StepId).ToArray();

            var fromTransitPointArray = workFlowInstalled.Transits.Select(t => t.FromStepId).ToArray();

            var toTransitPointArray = workFlowInstalled.Transits.Select(t => t.ToStepId).ToArray();

            var transitPointArray = fromTransitPointArray.Concat(toTransitPointArray).ToArray();

            //验证首尾已连接
            if (!transitPointArray.Contains(StartStepID))
            {
                messages.Add(new Tuple<Guid, string, string, string>(StartStepID, "开始", "步骤", "开始步骤未连接"));
            }

            if (!transitPointArray.Contains(EndStepID))
            {
                messages.Add(new Tuple<Guid, string, string, string>(EndStepID, "结束", "步骤", "结束步骤未连接"));
            }

            //验证结束节点不允许有下一出口
            if (fromTransitPointArray.Contains(EndStepID))
            {
                messages.Add(new Tuple<Guid, string, string, string>(EndStepID, "结束", "步骤", "结束步骤不能再有下一出口"));
            }


            //验证步骤节点
            var messages1 = CheckSteps(workFlowInstalled.Steps, stepArray, transitPointArray, fromTransitPointArray, toTransitPointArray);

            if (messages1.Count > 0)
            {
                messages.AddRange(messages1);
            }

            //验证条件链接
            var messages2 = CheckTransits(workFlowInstalled.Transits, stepArray, transitPointArray);
            if (messages2.Count > 0)
            {
                messages.AddRange(messages2);
            }

            return messages;
        }

        public static List<Tuple<Guid, string, string, string>> CheckSteps(List<WorkFlowStep> steps, Guid[] stepArray, Guid[] transitPointArray,Guid[] fromTransitPointArray,Guid[] toTransitPointArray)
        {
            List<Tuple<Guid, string, string, string>> messages = new List<Tuple<Guid, string, string, string>>();

            //验证步骤节点
            foreach (var step in steps)
            {
                //验证节点ID唯一
                var count = stepArray.Where(t => t == step.StepId).Count();
                if (count > 1)
                {
                    messages.Add(new Tuple<Guid, string, string, string>(step.StepId, step.StepName, "步骤", "条件节点ID存在重复情况"));
                }

                //验证节点名称必须填写
                if (string.IsNullOrEmpty(step.StepName))
                {
                    messages.Add(new Tuple<Guid, string, string, string>(step.StepId, step.StepName, "步骤", "步骤名称是必须填写的"));
                }

                //验证节点必须都链接
                if (!transitPointArray.Contains(step.StepId))
                {
                    messages.Add(new Tuple<Guid, string, string, string>(step.StepId, step.StepName, "步骤", "步骤没有被连接到"));
                }

                //验证节点出口
                if (!fromTransitPointArray.Contains(step.StepId) && !step.IsEnd())
                {
                    messages.Add(new Tuple<Guid, string, string, string>(step.StepId, step.StepName, "步骤", "步骤出口没有被连接到"));
                }

                //验证节点入库
                if (!toTransitPointArray.Contains(step.StepId) && !step.IsStart())
                {
                    messages.Add(new Tuple<Guid, string, string, string>(step.StepId, step.StepName, "步骤", "步骤入库没有被连接到"));
                }

                if (step.StepId == StartStepID || step.StepId == EndStepID)
                {
                    continue;
                }

                //验证规则
                var handlers = step.Handlers.Split(',').ToArray();
                var handlerTypeName = step.GetHandleTypeName();
                var len = handlers.Length;

                if (step.IsHandleBy(WorkFlowHandlerKinds.User, WorkFlowHandlerKinds.DirectorForHandler, WorkFlowHandlerKinds.LeaderForHandler))
                {
                    var len2 = ServiceLocator.GetService<IRepositoryContext>().GetRepository<Guid, User>().Count(t => t.IsDisabled == false && handlers.Contains(t.Id.ToString()));
                    if (len != len2)
                    {
                        messages.Add(new Tuple<Guid, string, string, string>(step.StepId, step.StepName, "步骤", $"步骤处理者类型为【{handlerTypeName}】，但所选人员与实际人员不符，即实际人员离职或数据丢失"));
                    }
                }
                else if (step.IsHandleBy(WorkFlowHandlerKinds.Department))
                {
                    var len2 = ServiceLocator.GetService<IRepositoryContext>().GetRepository<Guid, Department>().Count(t => handlers.Contains(t.Id.ToString()));
                    if (len != len2)
                    {
                        messages.Add(new Tuple<Guid, string, string, string>(step.StepId, step.StepName, "步骤", $"步骤处理者类型为【{handlerTypeName}】，但所选部门不存在"));
                    }
                }
                else if (step.IsHandleBy(WorkFlowHandlerKinds.UserGroup))
                {
                    var len2 = ServiceLocator.GetService<IRepositoryContext>().GetRepository<Guid, UserGroup>().Count(t => handlers.Contains(t.Id.ToString()));
                    if (len != len2)
                    {
                        messages.Add(new Tuple<Guid, string, string, string>(step.StepId, step.StepName, "步骤", $"步骤处理者类型为【{handlerTypeName}】，但所选角色不存在"));
                    }
                }

                if (step.IsHandleTacticBy(WorkFlowHandleTacticKinds.PercentageAgree) && step.Percentage == 0)
                {
                    messages.Add(new Tuple<Guid, string, string, string>(step.StepId, step.StepName, "步骤", $"步骤处理策略为【依据人数比例】，但策略百分比不可以设置为 0 %"));
                }

                if (step.IsCountersignatureTacticBy(WorkFlowCountersignatureTacticKinds.PercentageAgree) && step.CountersignaturePercentage == 0)
                {
                    messages.Add(new Tuple<Guid, string, string, string>(step.StepId, step.StepName, "步骤", $"步骤处理策略为【依据同意步骤比例】，但会签策略百分比不可以设置为 0 %"));
                }
            }

            return messages;
        }

        public static List<Tuple<Guid, string, string, string>> CheckTransits(List<WrokFlowTransit> transits, Guid[] stepArray, Guid[] transitPointArray)
        {
            List<Tuple<Guid, string, string, string>> messages = new List<Tuple<Guid, string, string, string>>();

            //验证条件链接
            foreach (var transit in transits)
            {
                if (string.IsNullOrEmpty(transit.TransitName))
                {
                    messages.Add(new Tuple<Guid, string, string, string>(transit.TransitId, transit.TransitName, "条件", "条件名称是必须填写的"));
                }

                if (transit.ConditionIsNot(WorkFlowTransitConditionKinds.None) && string.IsNullOrEmpty(transit.ConditionValue))
                {
                    var conditionTypeName = transit.GetConditionTypeName();
                    messages.Add(new Tuple<Guid, string, string, string>(transit.TransitId, transit.TransitName, "条件", $"条件类型为【{conditionTypeName}】，但条件表达式不存在"));
                }

                if (!stepArray.Contains(transit.FromStepId) || !stepArray.Contains(transit.ToStepId))
                {
                    messages.Add(new Tuple<Guid, string, string, string>(transit.TransitId, transit.TransitName, "条件", "条件存在未连接端点"));
                }

                if (transits.Where(t => t.FromStepId == transit.FromStepId && t.ToStepId == transit.ToStepId).Count() > 1)
                {
                    messages.Add(new Tuple<Guid, string, string, string>(transit.TransitId, transit.TransitName, "条件", "条件两端只能同方向只能连接一次，此条件两端被连接多次"));
                }
            }

            return messages;
        }
    }
}
