using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.UI.WebControls;
namespace NK.UI
{
    /// <summary>
    /// UIWeb扩展
    /// </summary>
    public  static class UIWebEx
    {

        /// <summary>
        /// 枚举转控件
        /// </summary>
        /// <param name="Class">控件</param>
        /// <param name="t">枚举类型</param>
        public static void EnumToUI(this RadioButtonList Class, Type t)
        {
            if (t != null)
            {
                if(t.IsEnum)
                {
                    Dictionary<string, int> listItems = new Dictionary<string, int>();
                    FieldInfo[] fields =t.GetFields();
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
                    foreach (var dic in listItems)
                        Class.Items.Add(new ListItem(dic.Key, dic.Value.ToString())); 
                }
            }
        }

        /// <summary>
        /// 枚举转控件
        /// </summary>
        /// <param name="Class">控件</param>
        /// <param name="t">枚举类型</param>
        public static void EnumToUI(this DropDownList Class, Type t)
        {
            if (t != null)
            {
                if (t.IsEnum)
                {
                    Dictionary<string, int> listItems = new Dictionary<string, int>();
                    FieldInfo[] fields = t.GetFields();
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
                    foreach (var dic in listItems)
                        Class.Items.Add(new ListItem(dic.Key, dic.Value.ToString()));
                }
            }
        }

        /// <summary>
        /// 枚举显示
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="t"></param>
        public static string EnumToDisplayName(this string Value, Type t)
        {
            if (t != null  && !string.IsNullOrEmpty(Value))
            {
                if (t.IsEnum)
                {
                    
                    FieldInfo[] fields = t.GetFields();
                    foreach (var field in fields)
                    {
                        if (field.Name.Equals("value__")) continue;
                        string Key = field.Name;
                        DescriptionAttribute[] EnumAttributes = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);
                        string disp = "";
                        if (EnumAttributes.Length > 0)
                            disp = EnumAttributes[0].Description;
                        int val = Convert.ToInt32(field.GetRawConstantValue());
                        if (Key.ToUpper().Trim() == Value.ToUpper().Trim()) return Key;
                    } 
                }
            }
            return "";
        }

        /// <summary>
        /// 枚举显示
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="t"></param>
        public static string EnumToDisplayDescription(this string Value, Type t)
        {
            if (t != null && !string.IsNullOrEmpty(Value))
            {
                if (t.IsEnum)
                {

                    FieldInfo[] fields = t.GetFields();
                    foreach (var field in fields)
                    {
                        if (field.Name.Equals("value__")) continue;
                        string Key = field.Name;
                        DescriptionAttribute[] EnumAttributes = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);
                        string disp = "";
                        if (EnumAttributes.Length > 0)
                            disp = EnumAttributes[0].Description;
                        int val = Convert.ToInt32(field.GetRawConstantValue());
                        if (disp.ToUpper().Trim() == Value.ToUpper().Trim()) return disp;
                    }
                }
            }
            return "";
        }

    }
}
