using System;
using System.Reflection;

namespace ZDY.DMS.Services.WorkFlowService.Core
{
    public static class WorkFlowConstant
    {
        public static Guid StartStepId
        {
            get
            {
                return Guid.Parse("00000000-0000-0000-0000-000000000001");
            }
        }

        public static Guid EndStepId
        {
            get
            {
                return Guid.Parse("00000000-0000-0000-0000-000000000002");
            }
        }

        /// <summary>
        /// 执行自定义方法
        /// </summary>
        /// <returns></returns>
        public static void ExecuteMethod<TArgs, TResult>(string methodAssembly, TArgs args, out TResult result)
        {
            var reflection = methodAssembly.Split(',');
            var dllName = reflection[0];
            var typeName = System.IO.Path.GetFileNameWithoutExtension(reflection[1]);
            var methodName = System.IO.Path.GetExtension(reflection[1]).Substring(1);

            var assembly = Assembly.Load(dllName);
            Type type = assembly.GetType(typeName, true);
            var instance = Activator.CreateInstance(type, false);
            var method = type.GetMethod(methodName);

            if (method != null)
            {
                var r = method.Invoke(instance, new object[] { args });

                if (r is TResult)
                {
                    result = (TResult)r;
                }
                else
                {
                    throw new TypeUnloadedException($"{methodAssembly} 提供的返回值类型不符合规定");
                }
            }
            else
            {
                throw new MissingMethodException(typeName, methodName);
            }
        }
    }
}
