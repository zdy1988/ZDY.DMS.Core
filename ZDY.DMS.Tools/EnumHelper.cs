using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace ZDY.DMS.Tools
{
    public static class EnumHelper
    {
        /// <summary>
        /// 获取类型中所有的枚举
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <returns>{ value = value, desription = desription, displayName = displayName, category = category }</returns>
        public static IEnumerable<EnumInformation> GetEnums(Type type)
        {
            if (!type.IsEnum)
            {
                throw new InvalidOperationException("错误的枚举类型");
            }
            var enumList = new List<EnumInformation>();
            foreach (FieldInfo item in type.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                if (!item.IsStatic) continue;

                var value = item.GetValue(null);
                var displayName = string.Empty;
                var desription = string.Empty;
                var category = string.Empty;
                var defaultValue = string.Empty;

                if (Attribute.GetCustomAttribute(item, typeof(DisplayNameAttribute)) is DisplayNameAttribute dna && !String.IsNullOrEmpty(dna.DisplayName)) displayName = dna.DisplayName;

                if (Attribute.GetCustomAttribute(item, typeof(DescriptionAttribute)) is DescriptionAttribute da && !String.IsNullOrEmpty(da.Description)) desription = da.Description;

                if (Attribute.GetCustomAttribute(item, typeof(CategoryAttribute)) is CategoryAttribute ca && !String.IsNullOrEmpty(ca.Category)) category = ca.Category;

                if (Attribute.GetCustomAttribute(item, typeof(DefaultValueAttribute)) is DefaultValueAttribute dva && dva.Value != null) defaultValue = dva.Value.ToString();

                enumList.Add(new EnumInformation
                {
                    Value = string.IsNullOrEmpty(defaultValue) ? ((int)value).ToString() : defaultValue,
                    Desription = desription,
                    DisplayName = displayName,
                    Category = category
                });
            }
            return enumList;
        }

        /// <summary>
        /// 枚举字典类型
        /// </summary>
        public class EnumInformation
        {
            public string Value { get; set; }

            public string Desription { get; set; }

            public string DisplayName { get; set; }

            public string Category { get; set; }
        }

        /// <summary>
        /// 获取枚举类型的描述
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription(Enum value)
        {
            FieldInfo item = value.GetType().GetField(value.ToString(), BindingFlags.Public | BindingFlags.Static);
            if (item == null) return null;
            if (Attribute.GetCustomAttribute(item, typeof(DescriptionAttribute)) is DescriptionAttribute attribute && !String.IsNullOrEmpty(attribute.Description)) return attribute.Description;
            return null;
        }
    }
}
