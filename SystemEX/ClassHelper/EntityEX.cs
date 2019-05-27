using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ComponentModel;
namespace NK
{
    /// <summary>
    /// 实体扩展
    /// </summary>
    public static partial class EntityEX
    {

       /// <summary>
       /// 获取实体名
       /// </summary>
       /// <param name="obj">实体</param>
       /// <returns>类名</returns>
        public static string Name(this object obj)
        {
            Type ObjType = obj.GetType();
            return ObjType.Name;
        }

        /// <summary>
        /// 获取实体名
        /// </summary>
        /// <param name="obj">实体</param>
        /// <returns>类名</returns>
        public static string Name(this Type obj)
        { 
            return obj.Name;
        }

        /// <summary>
        /// 获取实体全名
        /// </summary>
        /// <param name="obj">实体</param>
        /// <returns>类全名</returns>
        public static string FullName(this object obj)
        {
            Type ObjType = obj.GetType();
            return ObjType.FullName;
        }

        /// <summary>
        /// 获取实体全名
        /// </summary>
        /// <param name="obj">实体</param>
        /// <returns>类全名</returns>
        public static string FullName(this Type obj)
        { 
            return obj.FullName;
        }

        /// <summary>
        /// 获取类型的描述
        /// </summary>
        /// <param name="obj">实体</param>
        /// <returns>返回对象描述</returns>
        public static string ToDescription(this object obj)
        {
            Type ObjType = obj.GetType();
            DescriptionAttribute[] EnumAttributes = (DescriptionAttribute[])ObjType.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (EnumAttributes.Length > 0)
                return EnumAttributes[0].Description;
            return "";
        }

        /// <summary>
        /// 获取类型的描述
        /// </summary>
        /// <param name="obj">实体</param>
        /// <returns>返回对象描述</returns>
        public static string ToDescription(this Type obj)
        { 
            DescriptionAttribute[] EnumAttributes = (DescriptionAttribute[])obj.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (EnumAttributes.Length > 0)
                return EnumAttributes[0].Description;
            return "";
        }

        /// <summary>
        /// 获取类型的显示名称
        /// </summary>
        /// <param name="obj">实体</param>
        /// <returns>返回对象描述</returns>
        public static string ToDisplayName(this object obj)
        {
            Type ObjType = obj.GetType();
            DisplayNameAttribute[] EnumAttributes = (DisplayNameAttribute[])ObjType.GetCustomAttributes(typeof(DisplayNameAttribute), false);
            if (EnumAttributes.Length > 0)
                return EnumAttributes[0].DisplayName;
            return "";
        }

        /// <summary>
        /// 获取类型的显示名称
        /// </summary>
        /// <param name="obj">实体</param>
        /// <returns>返回对象描述</returns>
        public static string ToDisplayName(this Type obj)
        { 
            DisplayNameAttribute[] EnumAttributes = (DisplayNameAttribute[])obj.GetCustomAttributes(typeof(DisplayNameAttribute), false);
            if (EnumAttributes.Length > 0)
                return EnumAttributes[0].DisplayName;
            return "";
        }

        /// <summary>
        /// 获取所有属性
        /// </summary>
        /// <param name="obj">实体</param>
        /// <returns></returns>
        public static List<PropertyInfo> ToPropertys(this object obj)
        {
            Type ObjType = obj.GetType();
            return ObjType.GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList();
        }

        /// <summary>
        /// 获取所有属性
        /// </summary>
        /// <param name="obj">实体</param>
        /// <returns></returns>
        public static List<PropertyInfo> ToPropertys(this Type obj)
        { 
            return obj.GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList();
        }

        /// <summary>
        /// 获取属性
        /// </summary>
        /// <param name="obj">实体</param>
        /// <param name="filed">字段</param>
        /// <param name="Ignore">忽略大小写</param>
        /// <returns></returns>
        public static PropertyInfo ToProperty(this object obj,string filed,bool Ignore=false)
        {
            Type ObjType = obj.GetType();
            List<PropertyInfo> properties = ObjType.GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList();
            if (Ignore)
                properties = properties.Where(c => c.Name.ToUpper() == filed.ToUpper()).ToList();
            else
                properties = properties.Where(c => c.Name == filed).ToList();
            return properties.FirstOrDefault();
        }

        /// <summary>
        /// 获取属性
        /// </summary>
        /// <param name="obj">实体</param>
        /// <param name="filed">字段</param>
        /// <param name="Ignore">忽略大小写</param>
        /// <returns></returns>
        public static PropertyInfo ToProperty(this Type obj, string filed, bool Ignore = false)
        { 
            List<PropertyInfo> properties = obj.GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList();
            if (Ignore)
                properties = properties.Where(c => c.Name.ToUpper() == filed.ToUpper()).ToList();
            else
                properties = properties.Where(c => c.Name == filed).ToList();
            return properties.FirstOrDefault();
        }

        /// <summary>
        /// 列出类所有属性
        /// </summary>
        /// <param name="obj">实体</param>
        /// <returns>属性和值</returns>
        public static Dictionary<PropertyInfo, object> ToDictionary(this object obj)
        {
            Type ObjType = obj.GetType();
            Dictionary<PropertyInfo, object> listItems = new Dictionary<PropertyInfo, object>();
            PropertyInfo[] properties = ObjType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo p in properties)
            {
                if (p != null)
                {
                    Type t = p.PropertyType;
                    if (t.IsValueType || t.IsEnum || t == typeof(string) || t == typeof(bool) || t == typeof(DateTime) || t.IsArray )
                    {
                        if (p.GetValue(obj, null) != null)
                            listItems.Add(p, p.GetValue(obj, null));
                        else
                            listItems.Add(p, null);
                    }
                }
            }
            return listItems;
        } 
         

        /// <summary>
        /// 实体赋值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="Value"></param>
        public static void FromDictionary(this object obj, Dictionary<PropertyInfo, object> Value)
        {
            if (Value.IsNotNull())
            {
                Type ObjType = obj.GetType();
                Dictionary<PropertyInfo, string> res = new Dictionary<PropertyInfo, string>();
                PropertyInfo[] properties = ObjType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo p in properties)
                {
                    if (Value.Where(c => c.Key.Name == p.Name).Count() > 0)
                    { 
                        Type t = p.PropertyType;
                        var key = Value.FirstOrDefault(c => c.Key.Name == p.Name); 
                        p.SetValue(obj, key.Value, null);
                    }
                }
            }
        }

    }
 

}
