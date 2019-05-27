using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;
namespace NK
{
    /// <summary>
    /// 枚举类
    /// </summary>
    public static partial class EnumEx
    {
        /// <summary>
        /// 列出枚举及值
        /// </summary>
        /// <param name="em">枚举</param>
        /// <returns>名称和值</returns>
        public static Dictionary<string, int> EnumToList(this object em)
        {
            Dictionary<string, int> listItems = new Dictionary<string, int>();
            if (em.GetType().IsEnum)
            {
                FieldInfo[] fields = em.GetType().GetFields();
                foreach (var field in fields)
                {
                    if (field.Name.Equals("value__")) continue;
                    string Key = field.Name;
                    int val = Convert.ToInt32(field.GetRawConstantValue());
                    if (listItems.Where(c => c.Key == Key).Count() <= 0)
                        listItems.Add(Key, val);
                }
            } 
            return listItems;
        }

        /// <summary>
        ///  列出枚举描述及值
        /// </summary>
        /// <param name="em">枚举</param>
        /// <returns>枚举描述及值</returns>
        public static Dictionary<string, int> EnumDescToList(this object em)
        { 
            Dictionary<string, int> listItems = new Dictionary<string, int>();
            if (em.GetType().IsEnum)
            {
                FieldInfo[] fields = em.GetType().GetFields();
                foreach (var field in fields)
                {
                    if (field.Name.Equals("value__")) continue;
                    string Key = field.Name;
                    DescriptionAttribute[] EnumAttributes = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);
                    if (EnumAttributes.Length > 0)
                        Key = EnumAttributes[0].Description;
                    int val = Convert.ToInt32(field.GetRawConstantValue());
                    if (listItems.Where(c => c.Key == Key).Count() <= 0)
                        listItems.Add(Key, val);
                }
            } 
            return listItems; 
        }

        /// <summary>
        /// 列出枚举及值
        /// </summary>
        /// <param name="em">枚举</param>
        /// <returns>名称和值</returns>
        public static Dictionary<string, int> EnumToList(this Type em)
        {
            Dictionary<string, int> listItems = new Dictionary<string, int>();
            if (em.IsEnum)
            {
                FieldInfo[] fields = em.GetFields();
                foreach (var field in fields)
                {
                    if (field.Name.Equals("value__")) continue;
                    string Key = field.Name;
                    int val = Convert.ToInt32(field.GetRawConstantValue());
                    if (listItems.Where(c => c.Key == Key).Count() <= 0)
                        listItems.Add(Key, val);
                }
            }
            return listItems;
        }

        /// <summary>
        ///  列出枚举描述及值
        /// </summary>
        /// <param name="em">枚举</param>
        /// <returns>枚举描述及值</returns>
        public static Dictionary<string, int> EnumDescToList(this Type em)
        {
            Dictionary<string, int> listItems = new Dictionary<string, int>();
            if (em.IsEnum)
            {
                FieldInfo[] fields = em.GetFields();
                foreach (var field in fields)
                {
                    if (field.Name.Equals("value__")) continue;
                    string Key = field.Name;
                    DescriptionAttribute[] EnumAttributes = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);
                    if (EnumAttributes.Length > 0)
                        Key = EnumAttributes[0].Description;
                    int val = Convert.ToInt32(field.GetRawConstantValue());
                    if (listItems.Where(c => c.Key == Key).Count() <= 0)
                        listItems.Add(Key, val);
                }
            }
            return listItems;
        }

        /// <summary>
        /// 转枚举
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="enumtype"></param>
        /// <returns></returns>
        public static object ToENum(this string obj, Type enumtype)
        {
            if (enumtype.IsEnum && Enum.IsDefined(enumtype, obj))
                return Enum.ToObject(enumtype, obj);
            return null;
        }

        /// <summary>
        /// 转枚举
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="enumtype"></param>
        /// <returns></returns>
        public static object ToENum(this int obj, Type enumtype)
        {
            if (enumtype.IsEnum && Enum.IsDefined(enumtype, obj))
                return Enum.ToObject(enumtype, obj);
            return null;
        }

        /// <summary>
        /// 转枚举
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="enumtype"></param>
        /// <returns></returns>
        public static object ToENum(this object obj, Type enumtype)
        {
            if (enumtype.IsEnum && Enum.IsDefined(enumtype, obj))
                return Enum.ToObject(enumtype, obj);
            return null;
        }

    }
}
