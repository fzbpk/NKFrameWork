using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using System.ComponentModel;
using LinqToDB.Mapping;
using System.Runtime.Serialization.Json;
using NK.Attribut;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;
namespace UIWeb.Entity
{
     internal static  class UIHelper
    {

        public static T Deserialize<T>(this string json)
        {
            T obj = Activator.CreateInstance<T>();
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
                return (T)serializer.ReadObject(ms);
            }
        }

        public static string Serialize(this object obj)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            MemoryStream stream = new MemoryStream();
            serializer.WriteObject(stream, obj);
            byte[] dataBytes = new byte[stream.Length];
            stream.Position = 0;
            stream.Read(dataBytes, 0, (int)stream.Length);
            return Encoding.UTF8.GetString(dataBytes);
        }


        public static Dictionary<PropertyInfo, object> ToDictionary(this Type obj)
        {
            Dictionary<PropertyInfo, object> listItems = new Dictionary<PropertyInfo, object>();
            PropertyInfo[] properties = obj.GetProperties(BindingFlags.Public | BindingFlags.Instance);
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
                    if (t.IsValueType || t.IsEnum　|| t==typeof(string) || t == typeof(bool) || t == typeof(DateTime))
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

        public static string EntityToForm<T>(Dictionary<PropertyInfo, object> pls, string postDataKey,string LineCSS="",string HeadCSS="",string ValueCSS = "",string TextBoxCSS="", string ComBoxCSS = "",string TimeCSS="",string  CheckBoxCSS="") where T : class, new()
        { 
            string HTML = "<div style='display:none'><input type='text' value='DBInfo' name='" + postDataKey + "' /></div>\r\n ";
            foreach (var key in pls)
            {
                string DispName = key.Key.ToDisplayfiled();
                if (string.IsNullOrEmpty(DispName))
                    DispName = key.Key.ToDisplayColumn();
                if (string.IsNullOrEmpty(DispName))
                    DispName = key.Key.Name;
                string ControlID = postDataKey + "_" + key.Key.Name;
                string ControlName = postDataKey+"_"+ key.Key.Name;
                bool IsIdentity = false;
                bool CanBeNull = true;
                bool CanDisp = true;
                string JS = "";
                string CSS = string.IsNullOrEmpty(LineCSS) ? "" : LineCSS;
                ColumnAttribute[] EnumAttributes = (ColumnAttribute[])key.Key.GetCustomAttributes(typeof(ColumnAttribute), false);
                if (EnumAttributes.Length > 0)
                {
                    IsIdentity = EnumAttributes[0].IsIdentity;
                    CanBeNull = EnumAttributes[0].CanBeNull;
                }
                DisplayColumnAttribute[] DCAttributes = (DisplayColumnAttribute[])key.Key.GetCustomAttributes(typeof(DisplayColumnAttribute), false);
                if (DCAttributes.Length > 0)
                {
                    JS = DCAttributes[0].JS;
                    CanDisp = DCAttributes[0].CanDeitail;
                    if (!string.IsNullOrEmpty(DCAttributes[0].CSS))
                    {
                        CSS = DCAttributes[0].CSS;
                        HeadCSS = "";
                        ValueCSS = "";
                        TextBoxCSS = "";
                        ComBoxCSS = "";
                        TimeCSS = "";
                        CheckBoxCSS = ""; 
                    }
                }
                Type t = key.Key.PropertyType;

                HTML += "<div " + (string.IsNullOrEmpty(CSS) ? "" : "class=\"" + CSS + "\"") + (IsIdentity || !CanDisp ? "   style=\"display:none\"" : "") + " >" +
                       "   <label " + (string.IsNullOrEmpty(HeadCSS) ? "" : "class=\"" + HeadCSS + "\"") + " for=\"" + ControlName + "\">" + DispName + "</label>\r\n" +
                       "   <div " + (string.IsNullOrEmpty(ValueCSS) ? "" : "class=\"" + ValueCSS + "\"") + ">\r\n";
                string vals = key.Value == null ? "" : key.Value.ToString();
                if (t.IsEnum)
                {
                    if (!string.IsNullOrEmpty(JS))
                        JS = "onchange='javascript:" + JS + "(this);'";
                    Dictionary<string, int> listItems = new Dictionary<string, int>();
                    FieldInfo[] fields = t.GetFields();
                    foreach (var field in fields)
                    {
                        if (field.Name.Equals("value__")) continue;
                        string Key = field.Name;
                        int val = Convert.ToInt32(field.GetRawConstantValue());
                        if (vals == Key)
                            vals = val.ToString();
                        if (listItems.Where(c => c.Key == Key).Count() <= 0)
                            listItems.Add(Key, val);
                    }
                    HTML += "     <select name=\"" + ControlName + "\" id=\"" + ControlID + "\" " + (string.IsNullOrEmpty(ComBoxCSS) ? "" : "class=\"" + ComBoxCSS + "\"") + " >\r\n";
                    foreach (var DIC in listItems)
                        HTML += "      <option value=\"" + DIC.Value.ToString() + "\" " + (DIC.Value.ToString() == vals ? "selected='selected'" : "") + ">" + DIC.Key + "</option>\r\n";
                    HTML += "     </select >\r\n";
                }
                else if (t == typeof(bool))
                {
                    if (!string.IsNullOrEmpty(JS))
                        JS = "onclick='javascript:" + JS + "(this);'";
                    bool bval = Convert.ToBoolean(key.Value);
                    HTML += "    <label>\r\n" +
                           "      <input name = \"" + ControlName + "\" type = \"checkbox\" id = \"" + ControlID + "\" " + (string.IsNullOrEmpty(CheckBoxCSS) ? "" : "class=\"" + CheckBoxCSS + "\"") + " " + (bval ? " checked=\"checked\"" : "") + " value=\"true\" />\r\n";
                    HTML += "     </label>\r\n";
                }
                else if (t.IsValueType)
                {
                    if (CanBeNull && !(t.GetType().IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>))))
                        CanBeNull = false;
                    if (string.IsNullOrEmpty(JS))
                        JS = (CanBeNull ? "" : "onblur=\"javascript:if(isNaN(this.value)||this.value.length==0){document.getElementById('errtips_" + ControlID + "').style.display='';}else{document.getElementById('errtips_" + ControlID + "').style.display='none';}\" ");
                    else
                        JS = "onblur='javascript:" + JS + "(this);'";
                    HTML += "     <input name=\"" + ControlName + "\" type=\"text\" id=\"" + ControlID + "\" " + (string.IsNullOrEmpty(TextBoxCSS) ? "" : "class=\"" + TextBoxCSS + "\"") + " value=\"" + vals + "\" " + JS + " />\r\n";
                    HTML += "     <span id=\"errtips_" + ControlID + "\"   style=\"display:none;color:#dd4b39;\">" + DispName + "不能为空或必须为数字</span>\r\n";
                }
                else if (t == typeof(string))
                {
                    if (string.IsNullOrEmpty(JS))
                        JS = (CanBeNull ? "" : "onblur=\"javascript:if(this.value.length==0){document.getElementById('errtips_" + ControlID + "').style.display='';}else{document.getElementById('errtips_" + ControlID + "').style.display='none';}\" ");
                    else
                        JS = "onblur='javascript:" + JS + "(this);'";
                    HTML += "     <input name=\"" + ControlName + "\" type=\"text\" id=\"" + ControlID + "\" " + (string.IsNullOrEmpty(TextBoxCSS) ? "" : "class=\"" + TextBoxCSS + "\"") + "  value=\"" + vals + "\" " + JS + " />\r\n";
                    HTML += "     <span id=\"errtips_" + ControlID + "\"   style=\"display:none;color:#dd4b39;\">" + DispName + "不能为空</span>\r\n";
                }
                else if (t == typeof(DateTime))
                {
                    HTML += "     <input name=\"" + ControlName + "\" type=\"text\" id=\"" + ControlID + "\" " + (string.IsNullOrEmpty(TimeCSS) ? "" : "class=\"" + TimeCSS + "\"") + " value=\"" + vals + "\" data-date-format=\"yyyy - mm - dd\" />\r\n";
                }

                HTML += "  </div>\r\n" +
                     "</div>\r\n" +
                     "<div class=\"space - 4\"></div>\r\n"; 
            }
            return HTML;
        }


        public static T FormToEntity<T>(string postDataKey, NameValueCollection postCollection,out string Errmsg) where T : class, new()
        {
            Errmsg = "";
            T org = new T();
            try
            {
                PropertyInfo[] properties = org.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo p in properties)
                {
                    string DispName = p.ToDisplayfiled();
                    if (string.IsNullOrEmpty(DispName))
                        DispName = p.ToDisplayColumn();
                    if (string.IsNullOrEmpty(DispName))
                        DispName = p.Name;
                    string postKey = postDataKey + "_" + p.Name;
                    string postedValue = postCollection[postKey];
                    bool CanBeNull = true;
                    bool IsPri = false;
                    bool IsIdentity = false;
                    string Format = "";
                    Type t = p.PropertyType;
                    ColumnAttribute[] EnumAttributes = (ColumnAttribute[])p.GetCustomAttributes(typeof(ColumnAttribute), false);
                    if (EnumAttributes.Length > 0)
                    {
                        CanBeNull = EnumAttributes[0].CanBeNull;
                        IsPri = EnumAttributes[0].IsPrimaryKey;
                        IsIdentity = EnumAttributes[0].IsIdentity;
                    }
                    DisplayColumnAttribute[] DCAttributes = (DisplayColumnAttribute[])p.GetCustomAttributes(typeof(DisplayColumnAttribute), false);
                    if (DCAttributes.Length > 0)
                    {
                        Format = DCAttributes[0].Format; 
                    }
                    if (CanBeNull)
                    {
                        if (IsIdentity)
                            CanBeNull = true;
                        else if (IsPri & !IsIdentity)
                            CanBeNull = false;
                    }
                    if (t.IsEnum)
                    {
                        int val = 0;
                        if (int.TryParse(postedValue, out val))
                        {
                            var em = Enum.ToObject(t, val);
                            p.SetValue(org, em, null);
                        }
                        else
                        {
                            Errmsg = DispName + "输入格式有误";
                            return null;
                        }
                    } 
                    else if (t == typeof(bool))
                    {
                        bool val = false;
                        bool.TryParse(postedValue, out val);
                        p.SetValue(org, val, null);
                    }
                    else if (t.IsValueType)
                    {
                        Type colType = t;
                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                            colType = colType.GetGenericArguments()[0];
                        try
                        { p.SetValue(org, Convert.ChangeType(postedValue, colType), null); }
                        catch
                        {
                            Errmsg = DispName + "输入格式有误";
                            return null;
                        }
                    }
                    else if (t == typeof(string))
                    {
                        if (!CanBeNull && string.IsNullOrEmpty(postedValue))
                        {
                            Errmsg = DispName + "不能为空";
                            return null;
                        }
                        else if (!string.IsNullOrEmpty(Format))
                        {
                            if (!Regex.IsMatch(postedValue, Format))
                            {
                                Errmsg = DispName + "输入格式有误";
                                return null;
                            }
                        }
                        p.SetValue(org, postedValue, null);
                    }
                    else if (t == typeof(DateTime))
                    {
                        DateTime val = DateTime.Now;
                        if (DateTime.TryParse(postedValue, out val))
                        {
                            p.SetValue(org, postedValue, null);
                        }
                        else
                        {
                            Errmsg = DispName + "输入格式有误";
                            return null;
                        } 
                    } 
                }
                return org;
            }
            catch (Exception ex)
            {
                Errmsg = ex.Message;
                return null;
            } 
        }

        public static bool Found<M, N>(this Dictionary<M, N> Dict, Func<KeyValuePair<M, N>, bool> where)
        {
            return Dict.Where(where).Count() > 0;
        }

        public static string ToColumnName(this PropertyInfo property)
        {
            if (property == null)
                return "";
            ColumnAttribute[] EnumAttributes = (ColumnAttribute[])property.GetCustomAttributes(typeof(ColumnAttribute), false);
            if (EnumAttributes.Length > 0)
                return EnumAttributes[0].Name;
            return property.Name;
        }


    }
}
