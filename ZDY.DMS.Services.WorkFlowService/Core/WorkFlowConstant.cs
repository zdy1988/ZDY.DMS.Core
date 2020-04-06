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
        public static TResult ExecuteCustomMethod<TArgs, TResult>(string name, TArgs args)
        {
            var reflection = name.Split(',');
            var dllName = reflection[0];
            var typeName = System.IO.Path.GetFileNameWithoutExtension(reflection[1]);
            var methodName = System.IO.Path.GetExtension(reflection[1]).Substring(1);

            var assembly = Assembly.Load(dllName);
            var type = assembly.GetType(typeName, true);
            var instance = Activator.CreateInstance(type, false);
            var method = type.GetMethod(methodName);

            if (method != null)
            {
                var result = method.Invoke(instance, new object[] { args });

                if (result is TResult)
                {
                    return (TResult)result;
                }
                else
                {
                    throw new TypeUnloadedException($"{name} 提供的返回值类型不符合规定");
                }
            }
            else
            {
                throw new MissingMethodException(typeName, methodName);
            }
        }
    }
}
