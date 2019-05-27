using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel; 
namespace NK
{
    /// <summary>
    /// 实体属性扩展
    /// </summary>
    public static partial class EntityProperty
    {
        
        /// <summary>
        /// 获取实体属性名
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static string ToName(this PropertyInfo property)
        {
            return property.Name;
        }

        /// <summary>
        /// 字段是否可读
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static bool CanRead(this PropertyInfo property)
        {
            return property.CanRead;
        }

        /// <summary>
        /// 字段是否可写
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static bool CanWrite(this PropertyInfo property)
        {
            return property.CanWrite;
        }

        /// <summary>
        /// 字段值类型
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static Type ValueType(this PropertyInfo property)
        { 
            return property.PropertyType;
        }
        /// <summary>
        /// 获取属性类型的描述
        /// </summary>
        /// <param name="property">属性字段</param>
        /// <returns>返回对象描述，没有字段属性则显示属性名</returns>
        public static string ToDescripfiled(this PropertyInfo property)
        {
            if (property == null)
                return "";
            DescriptionAttribute[] EnumAttributes = (DescriptionAttribute[])property.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (EnumAttributes.Length > 0)
                return EnumAttributes[0].Description;
            return "";
        }

        /// <summary>
        /// 获取属性类型的显示名
        /// </summary>
        /// <param name="property">属性字段</param>
        /// <returns>返回属性类型的显示名，没有字段属性则显示属性名</returns>
        public static string ToDisplayName(this PropertyInfo property)
        {
            if (property == null)
                return "";
            DisplayNameAttribute[] EnumAttributes = (DisplayNameAttribute[])property.GetCustomAttributes(typeof(DisplayNameAttribute), false);
            if (EnumAttributes.Length > 0)
                return EnumAttributes[0].DisplayName;
            return "";
        }
        

    }
}
