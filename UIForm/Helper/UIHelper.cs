using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ComponentModel;
using LinqToDB.Mapping;
using NK.Attribut;
namespace UIForm.Entity
{
    internal static class UIHelper
    {

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
                    if (t.IsValueType || t.IsEnum || t == typeof(string) || t == typeof(bool) || t == typeof(DateTime))
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

        public static string ToDisplayColumn(this PropertyInfo property)
        {
            if (property == null)
                return "";
            DisplayColumnAttribute[] EnumAttributes = (DisplayColumnAttribute[])property.GetCustomAttributes(typeof(DisplayColumnAttribute), false);
            if (EnumAttributes.Length > 0)
                return (!string.IsNullOrEmpty(EnumAttributes[0].Name) ? EnumAttributes[0].Name : property.Name);
            return property.Name;
        }

        public static string ToDisplayfiled(this PropertyInfo property)
        {
            if (property == null)
                return "";
            DisplayNameAttribute[] EnumAttributes = (DisplayNameAttribute[])property.GetCustomAttributes(typeof(DisplayNameAttribute), false);
            if (EnumAttributes.Length > 0)
                return EnumAttributes[0].DisplayName;
            return property.Name;
        }

        public static Dictionary<string, int> EnumToList(Enum em)
        {
            Dictionary<string, int> listItems = new Dictionary<string, int>();
            FieldInfo[] fields = em.GetType().GetFields();
            foreach (var field in fields)
            {
                if (field.Name.Equals("value__")) continue;
                string Key = field.Name;
                int val = Convert.ToInt32(field.GetRawConstantValue());
                if (listItems.Where(c => c.Key == Key).Count() <= 0)
                    listItems.Add(Key, val);
            }
            return listItems;

        }
    }
}
