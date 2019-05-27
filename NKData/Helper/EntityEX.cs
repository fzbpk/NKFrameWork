using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using LinqToDB.Mapping;
using System.Reflection;
using NK.Attribut;
namespace NK
{
    /// <summary>
    /// 实体扩展类
    /// </summary>
    public static partial class EntityEX
    {

        /// <summary>
        /// 获取表名
        /// </summary>
        /// <param name="obj">实体</param>
        /// <returns></returns>
        public static string ToTableName(this object obj)
        {
            Type ObjType = obj.GetType();
            TableAttribute[] EnumAttributes = (TableAttribute[])ObjType.GetCustomAttributes(typeof(TableAttribute), false);
            if (EnumAttributes.Length > 0)
                return EnumAttributes[0].Name;
            return ObjType.Name;
        }

        /// <summary>
        /// 获取表名
        /// </summary>
        /// <param name="obj">实体</param>
        /// <returns></returns>
        public static string ToTableName(this Type obj)
        { 
            TableAttribute[] EnumAttributes = (TableAttribute[])obj.GetCustomAttributes(typeof(TableAttribute), false);
            if (EnumAttributes.Length > 0)
                return EnumAttributes[0].Name;
            return obj.Name;
        }

        /// <summary>
        /// 获取实体属性和值
        /// </summary>
        /// <param name="obj">实体</param>
        /// <returns></returns>
        public static List<ColumnAttribute> ToProperty(this object obj)
        {
            Type ObjType = obj.GetType();
            List<ColumnAttribute> listItems = new List<ColumnAttribute>();
            PropertyInfo[] properties = ObjType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo p in properties)
            {
                Type t = p.PropertyType;
                ColumnAttribute col = null;
                ColumnAttribute[] EnumAttributes = (ColumnAttribute[])p.GetCustomAttributes(typeof(ColumnAttribute), false);
                if (EnumAttributes.Length > 0)
                    col = EnumAttributes[0];
                else
                {
                    col = new ColumnAttribute();
                    col.Name = p.Name;
                    col.CanBeNull = (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)));
                    col.DataType = LinqToDB.DataType.Undefined;
                }
                if (col.DataType == LinqToDB.DataType.Undefined)
                    col.DataType = t.ToDataType();
                listItems.Add(col);
            }
            return listItems;
        }

        /// <summary>
        /// 获取实体属性和值
        /// </summary>
        /// <param name="obj">实体</param>
        /// <returns></returns>
        public static List<ColumnAttribute> ToProperty(this Type obj)
        { 
            List<ColumnAttribute> listItems = new List<ColumnAttribute>();
            PropertyInfo[] properties = obj.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo p in properties)
            {
                Type t = p.PropertyType;
                ColumnAttribute col = null;
                ColumnAttribute[] EnumAttributes = (ColumnAttribute[])p.GetCustomAttributes(typeof(ColumnAttribute), false);
                if (EnumAttributes.Length > 0)
                    col = EnumAttributes[0];
                else
                {
                    col = new ColumnAttribute();
                    col.Name = p.Name;
                    col.CanBeNull = (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)));
                    col.DataType = LinqToDB.DataType.Undefined;
                }
                if (col.DataType == LinqToDB.DataType.Undefined)
                    col.DataType = t.ToDataType();
                listItems.Add(col);
            }
            return listItems;
        }

        /// <summary>
        /// 获取实体显示属性和值
        /// </summary>
        /// <param name="obj">实体</param>
        /// <returns></returns>
        public static List<DisplayColumnAttribute> ToDispProperty(this object obj)
        {
            string Tab = "";
            Type ObjType = obj.GetType();
            TableAttribute[] EnumAttributes = (TableAttribute[])ObjType.GetCustomAttributes(typeof(TableAttribute), false);
            if (EnumAttributes.Length > 0)
                Tab = EnumAttributes[0].Name;
            else
                Tab = ObjType.Name;
            List<DisplayColumnAttribute> listItems = new List<DisplayColumnAttribute>();
            PropertyInfo[] properties = ObjType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo p in properties)
            {
                string Col = "";
                Type t = p.PropertyType;
                ColumnAttribute[] ColumnAttributes = (ColumnAttribute[])p.GetCustomAttributes(typeof(ColumnAttribute), false);
                if (ColumnAttributes.Length > 0)
                    Col = ColumnAttributes[0].Name;
                else
                    Col = p.Name;
                DisplayColumnAttribute col = null;
                DisplayColumnAttribute[] ColAttributes = (DisplayColumnAttribute[])p.GetCustomAttributes(typeof(DisplayColumnAttribute), false);
                if (ColAttributes.Length > 0)
                    col = ColAttributes[0];
                else
                {
                    col = new DisplayColumnAttribute();
                    col.CSS = "";
                    col.Format = "";
                    col.JS = "";
                    col.Name = "";
                    col.Unit = "";
                }
                col.Table = Tab;
                col.Column = Col;
                listItems.Add(col);
            }
            return listItems;
        }

        /// <summary>
        /// 获取实体显示属性和值
        /// </summary>
        /// <param name="obj">实体</param>
        /// <returns></returns>
        public static List<DisplayColumnAttribute> ToDispProperty(this Type obj)
        {
            string Tab = ""; 
            TableAttribute[] EnumAttributes = (TableAttribute[])obj.GetCustomAttributes(typeof(TableAttribute), false);
            if (EnumAttributes.Length > 0)
                Tab = EnumAttributes[0].Name;
            else
                Tab = obj.Name;
            List<DisplayColumnAttribute> listItems = new List<DisplayColumnAttribute>();
            PropertyInfo[] properties = obj.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo p in properties)
            {
                string Col = "";
                Type t = p.PropertyType;
                ColumnAttribute[] ColumnAttributes = (ColumnAttribute[])p.GetCustomAttributes(typeof(ColumnAttribute), false);
                if (ColumnAttributes.Length > 0)
                    Col = ColumnAttributes[0].Name;
                else
                    Col = p.Name;
                DisplayColumnAttribute col = null;
                DisplayColumnAttribute[] ColAttributes = (DisplayColumnAttribute[])p.GetCustomAttributes(typeof(DisplayColumnAttribute), false);
                if (ColAttributes.Length > 0)
                    col = ColAttributes[0];
                else
                {
                    col = new DisplayColumnAttribute();
                    col.CSS = "";
                    col.Format = "";
                    col.JS = "";
                    col.Name = "";
                    col.Unit = "";
                }
                col.Table = Tab;
                col.Column = Col;
                listItems.Add(col);
            }
            return listItems;
        }


        /// <summary>
        /// 获取实体属性和值
        /// </summary>
        /// <param name="obj">实体</param>
        /// <returns></returns>
        public static Dictionary<ColumnAttribute, object> ToColumnDictionary(this object obj)
        {
            Type ObjType = obj.GetType();
            Dictionary<ColumnAttribute, object> listItems = new Dictionary<ColumnAttribute, object>();
            PropertyInfo[] properties = ObjType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo p in properties)
            {
                Type t = p.PropertyType;
                ColumnAttribute col = null;
                ColumnAttribute[] EnumAttributes = (ColumnAttribute[])p.GetCustomAttributes(typeof(ColumnAttribute), false);
                if (EnumAttributes.Length > 0)
                    col = EnumAttributes[0];
                else
                {
                    col = new ColumnAttribute();
                    col.Name = p.Name;
                    col.CanBeNull = (t.GetType() == typeof(string)) || (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)));
                    col.DataType = LinqToDB.DataType.Undefined;
                }
                if (col.DataType == LinqToDB.DataType.Undefined)
                    col.DataType = t.ToDataType();
                listItems.Add(col, p.GetValue(obj, null));
            }
            return listItems;
        }
        
        /// <summary>
        /// 获取实体显示属性和值
        /// </summary>
        /// <param name="obj">实体</param>
        /// <returns></returns>
        public static Dictionary<DisplayColumnAttribute, object> ToDispDictionary(this object obj)
        {
            string Tab = "";
            Type ObjType = obj.GetType();
            TableAttribute[] EnumAttributes = (TableAttribute[])ObjType.GetCustomAttributes(typeof(TableAttribute), false);
            if (EnumAttributes.Length > 0)
                Tab = EnumAttributes[0].Name;
            else
                Tab = ObjType.Name;
            Dictionary<DisplayColumnAttribute, object> listItems = new Dictionary<DisplayColumnAttribute, object>();
            PropertyInfo[] properties = ObjType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo p in properties)
            {
                string Col = "";
                Type t = p.PropertyType;
                ColumnAttribute[] ColumnAttributes = (ColumnAttribute[])p.GetCustomAttributes(typeof(ColumnAttribute), false);
                if (ColumnAttributes.Length > 0)
                    Col = ColumnAttributes[0].Name;
                else
                    Col = p.Name;
                DisplayColumnAttribute col = null;
                DisplayColumnAttribute[] ColAttributes = (DisplayColumnAttribute[])p.GetCustomAttributes(typeof(DisplayColumnAttribute), false);
                if (ColAttributes.Length > 0)
                    col = ColAttributes[0];
                else
                {
                    col = new DisplayColumnAttribute();
                    col.CSS = "";
                    col.Format = "";
                    col.JS = "";
                    col.Name = "";
                    col.Unit = "";
                }
                col.Table = Tab;
                col.Column = Col;
                if (t.IsValueType || t.IsEnum || t == typeof(string) || t == typeof(bool) || t == typeof(DateTime) || t.IsArray)
                {
                    if (p.GetValue(obj, null) != null)
                        listItems.Add(col, p.GetValue(obj, null));
                    else
                        listItems.Add(col, null);
                } 
            }
            return listItems;
        }

        /// <summary>
        /// 实体赋值
        /// </summary>
        /// <param name="obj">实体</param>
        /// <param name="Value">值</param>
        public static void FromDictionary(this object obj, Dictionary<DisplayColumnAttribute, object> Value)
        {
            string Tab = "";
            Type ObjType = obj.GetType();
            TableAttribute[] EnumAttributes = (TableAttribute[])ObjType.GetCustomAttributes(typeof(TableAttribute), false);
            if (EnumAttributes.Length > 0)
                Tab = EnumAttributes[0].Name;
            else
                Tab = ObjType.Name;
            PropertyInfo[] properties = ObjType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo p in properties)
            {
                string Col = "";
                Type t = p.PropertyType;
                DisplayColumnAttribute[] ColumnAttributes = (DisplayColumnAttribute[])p.GetCustomAttributes(typeof(DisplayColumnAttribute), false);
                if (ColumnAttributes.Length > 0)
                    Col = ColumnAttributes[0].Name;
                else
                    Col = p.Name;
                if (Value.Where(c => c.Key.Name.ToUpper().Trim() == Col.ToUpper().Trim() && c.Key.Table.ToUpper().Trim()== Tab.ToUpper().Trim()).Count() > 0)
                {
                    var dic = Value.FirstOrDefault(c => c.Key.Name.ToUpper().Trim() == Col.ToUpper().Trim() && c.Key.Table.ToUpper().Trim() == Tab.ToUpper().Trim() );
                    p.SetValue(obj, dic.Value, null);
                }
                else if (Value.Where(c => c.Key.Name.ToUpper().Trim() == p.Name.ToUpper().Trim() && c.Key.Table.ToUpper().Trim() == Tab.ToUpper().Trim() ).Count() > 0)
                {
                    var dic = Value.FirstOrDefault(c => c.Key.Name.ToUpper().Trim() == p.Name.ToUpper().Trim() && c.Key.Table.ToUpper().Trim() == Tab.ToUpper().Trim() );
                    p.SetValue(obj, dic.Value, null);
                }
            }
        }

        /// <summary>
        /// 实体赋值
        /// </summary>
        /// <param name="obj">实体</param>
        /// <param name="Value">值</param>
        public static void FromDictionary(this object obj, Dictionary<ColumnAttribute, object> Value)
        {
            Type ObjType = obj.GetType();
            PropertyInfo[] properties = ObjType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo p in properties)
            {
                string Col = "";
                Type t = p.PropertyType;
                ColumnAttribute[] ColumnAttributes = (ColumnAttribute[])p.GetCustomAttributes(typeof(ColumnAttribute), false);
                if (ColumnAttributes.Length > 0)
                    Col = ColumnAttributes[0].Name;
                else
                    Col = p.Name;
                if (Value.Where(c => c.Key.Name.ToUpper().Trim() == Col.ToUpper().Trim()).Count() > 0)
                {
                    var dic = Value.FirstOrDefault(c => c.Key.Name.ToUpper().Trim() == Col.ToUpper().Trim());
                    p.SetValue(obj, dic.Value, null);
                }
                else if (Value.Where(c => c.Key.Name.ToUpper().Trim() == p.Name.ToUpper().Trim()).Count() > 0)
                {
                    var dic = Value.FirstOrDefault(c => c.Key.Name.ToUpper().Trim() == p.Name.ToUpper().Trim());
                    p.SetValue(obj, dic.Value, null);
                }
            }
        }

    }
}
