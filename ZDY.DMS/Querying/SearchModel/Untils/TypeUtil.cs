using System;

namespace ZDY.DMS.Querying.SearchModel
{
    /// <summary>
    /// Type类的处理工具类
    /// </summary>
    public class TypeUtil
    {
        /// <summary>
        /// 获取可空类型的实际类型
        /// </summary>
        /// <param name="conversionType"></param>
        /// <returns></returns>
        public static Type GetUnNullableType(Type conversionType)
        {
            if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                //如果是泛型方法，且泛型类型为Nullable<>则视为可空类型
                //并使用NullableConverter转换器进行转换
                var nullableConverter = new System.ComponentModel.NullableConverter(conversionType);
                conversionType = nullableConverter.UnderlyingType;
            }
            return conversionType;
        }

        public static object ConvertValue(object value, Type conversionType)
        {
            var elementType = GetUnNullableType(conversionType);

            if (elementType == typeof(Guid))
            {
                return Guid.Parse(value.ToString());
            }

            return Convert.ChangeType(value, elementType);
        }
    }
}
