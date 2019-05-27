using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using LinqToDB.Mapping; 
using NK.Attribut;
namespace NK
{
    /// <summary>
    /// 实体属性扩展
    /// </summary>
    public static partial class EntityProperty
    {
        /// <summary>
        /// 获取实体属性字段名
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static string ToColumnName(this PropertyInfo property)
        {
            if (property == null)
                return "";
            ColumnAttribute[] EnumAttributes = (ColumnAttribute[])property.GetCustomAttributes(typeof(ColumnAttribute), false);
            if (EnumAttributes.Length > 0)
                return EnumAttributes[0].Name;
            return property.Name;
        }

        /// <summary>
        /// 是否主键
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static bool IsPrimaryKey(this PropertyInfo property)
        {
            if (property == null)
                return false ;
            ColumnAttribute[] EnumAttributes = (ColumnAttribute[])property.GetCustomAttributes(typeof(ColumnAttribute), false);
            if (EnumAttributes.Length > 0)
                return EnumAttributes[0].IsPrimaryKey;
            return false ;
        }

        /// <summary>
        /// 是否自增量
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static bool IsIdentity(this PropertyInfo property)
        {
            if (property == null)
                return false;
            ColumnAttribute[] EnumAttributes = (ColumnAttribute[])property.GetCustomAttributes(typeof(ColumnAttribute), false);
            if (EnumAttributes.Length > 0)
                return EnumAttributes[0].IsIdentity;
            return false;
        }

        /// <summary>
        /// 是否可空
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static bool CanBeNull(this PropertyInfo property)
        {
            if (property == null)
                return false;
            ColumnAttribute[] EnumAttributes = (ColumnAttribute[])property.GetCustomAttributes(typeof(ColumnAttribute), false);
            if (EnumAttributes.Length > 0)
                return EnumAttributes[0].CanBeNull;
            return false;
        }

        /// <summary>
        /// 是否统计
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static bool CanCount(this PropertyInfo property)
        {
            if (property == null)
                return false;
            DisplayColumnAttribute[] EnumAttributes = (DisplayColumnAttribute[])property.GetCustomAttributes(typeof(ColumnAttribute), false);
            if (EnumAttributes.Length > 0)
                return EnumAttributes[0].CanCount;
            return false;
        }

        /// <summary>
        /// 是否详细页
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static bool CanDeitail(this PropertyInfo property)
        {
            if (property == null)
                return false;
            DisplayColumnAttribute[] EnumAttributes = (DisplayColumnAttribute[])property.GetCustomAttributes(typeof(ColumnAttribute), false);
            if (EnumAttributes.Length > 0)
                return EnumAttributes[0].CanDeitail;
            return false;
        }

        /// <summary>
        /// 是否用于表头
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static bool CanHead(this PropertyInfo property)
        {
            if (property == null)
                return false;
            DisplayColumnAttribute[] EnumAttributes = (DisplayColumnAttribute[])property.GetCustomAttributes(typeof(ColumnAttribute), false);
            if (EnumAttributes.Length > 0)
                return EnumAttributes[0].CanHead;
            return false;
        }

        /// <summary>
        /// 是否用于导入导出
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static bool CanImpExpead(this PropertyInfo property)
        {
            if (property == null)
                return false;
            DisplayColumnAttribute[] EnumAttributes = (DisplayColumnAttribute[])property.GetCustomAttributes(typeof(ColumnAttribute), false);
            if (EnumAttributes.Length > 0)
                return EnumAttributes[0].CanImpExp;
            return false;
        }

        /// <summary>
        /// 是否用于搜索
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static bool CanSearch(this PropertyInfo property)
        {
            if (property == null)
                return false;
            DisplayColumnAttribute[] EnumAttributes = (DisplayColumnAttribute[])property.GetCustomAttributes(typeof(ColumnAttribute), false);
            if (EnumAttributes.Length > 0)
                return EnumAttributes[0].CanSearch;
            return false;
        }

        /// <summary>
        /// 是否用于CSS
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static string CSS(this PropertyInfo property)
        {
            if (property == null)
                return "";
            DisplayColumnAttribute[] EnumAttributes = (DisplayColumnAttribute[])property.GetCustomAttributes(typeof(ColumnAttribute), false);
            if (EnumAttributes.Length > 0)
                return EnumAttributes[0].CSS;
            return "";
        }

        /// <summary>
        /// 显示格式
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static string Format(this PropertyInfo property)
        {
            if (property == null)
                return "";
            DisplayColumnAttribute[] EnumAttributes = (DisplayColumnAttribute[])property.GetCustomAttributes(typeof(ColumnAttribute), false);
            if (EnumAttributes.Length > 0)
                return EnumAttributes[0].Format;
            return "";
        }

        /// <summary>
        /// JS函数
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static string JS(this PropertyInfo property)
        {
            if (property == null)
                return "";
            DisplayColumnAttribute[] EnumAttributes = (DisplayColumnAttribute[])property.GetCustomAttributes(typeof(ColumnAttribute), false);
            if (EnumAttributes.Length > 0)
                return EnumAttributes[0].JS;
            return "";
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static int Seqencing(this PropertyInfo property)
        {
            if (property == null)
                return 0;
            DisplayColumnAttribute[] EnumAttributes = (DisplayColumnAttribute[])property.GetCustomAttributes(typeof(ColumnAttribute), false);
            if (EnumAttributes.Length > 0)
                return EnumAttributes[0].Seqencing;
            return 0;
        }

        /// <summary>
        /// 单位
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static string Unit(this PropertyInfo property)
        {
            if (property == null)
                return "";
            DisplayColumnAttribute[] EnumAttributes = (DisplayColumnAttribute[])property.GetCustomAttributes(typeof(ColumnAttribute), false);
            if (EnumAttributes.Length > 0)
                return EnumAttributes[0].Unit;
            return "";
        }

        /// <summary>
        /// 是否唯一
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static bool IsUnique(this PropertyInfo property)
        {
            if (property == null)
                return false;
            DisplayColumnAttribute[] EnumAttributes = (DisplayColumnAttribute[])property.GetCustomAttributes(typeof(ColumnAttribute), false);
            if (EnumAttributes.Length > 0) 
                return EnumAttributes[0].IsUnique;
            return false;
        }

    }
}
