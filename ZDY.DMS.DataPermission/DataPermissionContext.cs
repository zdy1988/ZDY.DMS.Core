using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ZDY.DMS.DataPermission
{
    public class DataPermissionContext : IDataPermissionContext
    {
        private readonly Dictionary<string, List<string>> cachedDataPermissions = new Dictionary<string, List<string>>();

        private readonly IActionDescriptorCollectionProvider actionProvider;

        public DataPermissionContext(IActionDescriptorCollectionProvider actionProvider)
        {
            this.actionProvider = actionProvider;

            this.Load();
        }

        private void Load()
        {
            var actions = this.actionProvider.ActionDescriptors.Items.Where(t => t.GetType() == typeof(ControllerActionDescriptor)).Cast<ControllerActionDescriptor>().ToList();

            foreach (var aciton in actions)
            {
                if (aciton.RouteValues.ContainsKey("controller") && aciton.RouteValues.ContainsKey("action"))
                {
                    // 判断出入类型是 SearchModel
                    if (aciton.Parameters.Count == 1 && typeof(SearchModel).IsAssignableFrom(aciton.Parameters.First().ParameterType))
                    {
                        var key = $"{aciton.RouteValues["controller"]}/{aciton.RouteValues["action"]}";

                        var fields = new List<string>();

                        // 判断 Action 返回类型是 Task
                        if (aciton.MethodInfo.ReturnType.IsSubclassOf(typeof(Task)))
                        {
                            // 从 Task 中获取返回类型
                            var actionResult = aciton.MethodInfo.ReturnType.GetRuntimeProperty("Result");

                            // 判断返回类型是 Tuple<IEnumerable<IEntity<>>, int>
                            if (actionResult.PropertyType.GetInterfaces().Any(t => t.Name == "ITuple"))
                            {
                                var enumerableResult = actionResult.PropertyType.GetGenericArguments().First();

                                // 判断数据类型
                                if (typeof(IEnumerable<IEntity<Guid>>).IsAssignableFrom(enumerableResult))
                                {
                                    // 判断是否控制 Company
                                    if (typeof(IEnumerable<ICompanyEntity<Guid>>).IsAssignableFrom(enumerableResult))
                                    {
                                        fields.Add("CompanyId");
                                    }

                                    // 判断是否控制 Department
                                    if (typeof(IEnumerable<IDepartmentEntity<Guid>>).IsAssignableFrom(enumerableResult))
                                    {
                                        fields.Add("DepartmentId");
                                    }

                                    // 判断是否控制 Disabled
                                    if (typeof(IEnumerable<IDisabledEntity<Guid>>).IsAssignableFrom(enumerableResult))
                                    {
                                        fields.Add("IsDisabled");
                                    }

                                    cachedDataPermissions.Add(key, fields);
                                }
                            }
                        }
                    }
                }
            }
        }

        public List<string> GetSearchSpecifics(string key)
        {
            if (cachedDataPermissions.TryGetValue(key, out List<string> values))
            {
                return values;
            }

            return null;
        }

        public List<string> GetSearchSpecifics(string controllerName, string actionName)
        {
            string key = $"{controllerName}/{actionName}";

            return GetSearchSpecifics(key);
        }
    }
}
