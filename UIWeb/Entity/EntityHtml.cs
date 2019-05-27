using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using NK.Attribut;
using LinqToDB.Mapping;
using System.Text.RegularExpressions;
using System.Collections.Specialized;

namespace UIWeb.Entity
{
    public   partial class EntityHtml
    {
        private string Errmsg = "";
        /// <summary>
        /// 行样式
        /// </summary>
        public string LineCSS { get; set; }
        /// <summary>
        /// 标题列样式
        /// </summary>
        public string HeadCSS { get; set; }
        /// <summary>
        /// 值列样式
        /// </summary>
        public string ValueCSS { get; set; } 
        /// <summary>
        /// 文本框样式
        /// </summary>
        public string TextBoxCSS { get; set; }
        /// <summary>
        /// 下拉列表样式
        /// </summary>
        public string ComBoxCSS { get; set; }
        /// <summary>
        /// 时间控件样式
        /// </summary>
        public  string TimeCSS { get; set; }
        /// <summary>
        /// CheckBox样式
        /// </summary>
        public string CheckBoxCSS { get; set; }
        /// <summary>
        /// 获取错误信息
        /// </summary>
        public string GetError { get {
                string err = Errmsg;
                Errmsg = "";
                return err;
            } }

        /// <summary>
        /// 字段排序
        /// </summary>
        /// <param name="Column">字段</param>
        /// <param name="Style">排序</param>
        /// <returns></returns>
        public Dictionary<PropertyInfo, object> OrderBy(Dictionary<PropertyInfo, object> Column, List<DisplayColumnAttribute> Style)
        {
            if (Style == null || Column==null) return Column;
            if (Style.Count <= 0 ) return Column;
            var ls = Style.OrderBy(c => c.Seqencing);
            Dictionary<PropertyInfo, object> res = new Dictionary<PropertyInfo, object>();
            foreach (var p in ls)
            {
                if (Column.Where(c => c.Key.Name.ToUpper().Trim() == p.Column.ToUpper().Trim()).Count() > 0)
                {
                    var dic = Column.FirstOrDefault(c => c.Key.Name.ToUpper().Trim() == p.Column.ToUpper().Trim());
                    res.Add(dic.Key, dic.Value);
                } 
            }
            foreach (var dic in Column)
            {
                if (res.Where(c => c.Key.Name.ToUpper().Trim() == dic.Key.Name.ToUpper().Trim()).Count() <= 0)
                 res.Add(dic.Key, dic.Value);
            }
            return res;
        }

        /// <summary>
        /// 生成HTML
        /// </summary>
        /// <param name="ClassName">类名</param>
        /// <param name="Column">字段</param>
        /// <param name="Style">显示属性</param>
        /// <returns></returns>
        public  string EntityToHtml(string ClassName, Dictionary<PropertyInfo, object> Column,List<DisplayColumnAttribute> Style=null)
        {
            if (Style == null) Style = new List<DisplayColumnAttribute>();
            if (string.IsNullOrEmpty(ClassName)) return "";
            if (Column == null) return "";
            else if (Column.Count <= 0) return "";
            string HTML = "";
            foreach (var key in Column)
            {
                string KeyName = key.Key.Name;
                string DispName = key.Key.ToDisplayfiled();
                if (string.IsNullOrEmpty(DispName))
                    DispName = key.Key.ToDisplayColumn();
                if (string.IsNullOrEmpty(DispName))
                    DispName = key.Key.Name; 
                bool IsIdentity = false;
                bool CanBeNull = true;
                bool CanDisp = true;
                bool IsPri = false;
                string JS = "";
                string CSS = string.IsNullOrEmpty(LineCSS) ? "" : LineCSS;
                ColumnAttribute[] EnumAttributes = (ColumnAttribute[])key.Key.GetCustomAttributes(typeof(ColumnAttribute), false);
                if (EnumAttributes.Length > 0)
                {
                    IsPri = EnumAttributes[0].IsPrimaryKey;
                    IsIdentity = EnumAttributes[0].IsIdentity;
                    CanBeNull = EnumAttributes[0].CanBeNull;
                    if (!string.IsNullOrEmpty(EnumAttributes[0].Name))
                        KeyName = EnumAttributes[0].Name;
                }
                string ControlID = ClassName + "_" + KeyName;
                string ControlName = ClassName + "_" + KeyName;
                var style = Style.FirstOrDefault(c => c.Table == ClassName && c.Column == KeyName);
                if (style != null)
                {
                    JS = style.JS;
                    CanDisp = style.CanDeitail;
                    if (!string.IsNullOrEmpty(style.CSS))
                    {
                        CSS = style.CSS;
                        HeadCSS = "";
                        ValueCSS = "";
                        TextBoxCSS = "";
                        ComBoxCSS = "";
                        TimeCSS = "";
                        CheckBoxCSS = "";
                    }
                }
                else
                {
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
                }
                if (CanDisp && IsIdentity)
                    CanDisp = false;
                if (IsPri )
                {
                    if (!IsIdentity)
                    {
                        CanBeNull = false;
                        CanDisp = true;
                    } 
                }
                 
                
                
                Type t = key.Key.PropertyType;

                HTML += "<div " + (string.IsNullOrEmpty(CSS) ? "" : "class=\"" + CSS + "\"") + (!CanDisp ? "   style=\"display:none\"" : "") + " >" +
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
                           "      <input name = \"" + ControlName + "\" type = \"checkbox\" id = \"" + ControlID + "\" " + (string.IsNullOrEmpty(CheckBoxCSS) ? "" : "class=\"" + CheckBoxCSS + "\"") + " value='true' " + (bval ? " checked=\"checked\"" : "") + " />\r\n";
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

        /// <summary>
        /// 生成数据
        /// </summary>
        /// <param name="ClassName">类名</param>
        /// <param name="Value">值</param>
        /// <param name="Column">字段</param>
        /// <param name="Style">显示属性</param>
        /// <returns></returns>
        public Dictionary<PropertyInfo, object> HtmlToEntity(string ClassName,  NameValueCollection postCollection, List<PropertyInfo> Column, List<DisplayColumnAttribute> Style = null)
        {
            Dictionary<PropertyInfo, object> res = new Dictionary<PropertyInfo, object>();
            if (Style == null) Style = new List<DisplayColumnAttribute>();
            if (string.IsNullOrEmpty(ClassName)) return null;
            if (postCollection == null) return null;
            else if (postCollection .Count<= 0) return null;
            if (Column == null) return null;
            else if (Column.Count <= 0) return null;
            foreach (var key in Column)
            {
                string KeyName = key.Name;
                string DispName = key.ToDisplayfiled();
                if (string.IsNullOrEmpty(DispName))
                    DispName = key.ToDisplayColumn();
                if (string.IsNullOrEmpty(DispName))
                    DispName = key.Name;
                bool IsIdentity = false;
                bool CanBeNull = true;
                bool IsPri = false;
                string Format = "";
                string postKey = "";
                string postedValue ="";
                ColumnAttribute[] EnumAttributes = (ColumnAttribute[])key.GetCustomAttributes(typeof(ColumnAttribute), false);
                if (EnumAttributes.Length > 0)
                {
                    IsPri =   EnumAttributes[0].IsPrimaryKey;
                    IsIdentity = EnumAttributes[0].IsIdentity;
                    CanBeNull = EnumAttributes[0].CanBeNull;
                    if (!string.IsNullOrEmpty(EnumAttributes[0].Name))
                        KeyName = EnumAttributes[0].Name;
                }
                var style = Style.FirstOrDefault(c => c.Table == ClassName && c.Column == KeyName);
                if (style != null)
                    Format = style.Format;
                else
                {
                    DisplayColumnAttribute[] DCAttributes = (DisplayColumnAttribute[])key.GetCustomAttributes(typeof(DisplayColumnAttribute), false);
                    if (DCAttributes.Length > 0)
                        Format = DCAttributes[0].Format;
                }
                if (IsIdentity)
                    continue;
                Type t = key.PropertyType;
                postKey = ClassName + "_" + KeyName;
                postedValue = string.IsNullOrEmpty(postCollection[postKey])?"": postCollection[postKey];
                if (t.IsEnum)
                {
                    int val = 0;
                    if (int.TryParse(postedValue, out val))
                    {
                        var em = Enum.ToObject(t, val);
                        res.Add(key, em);
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
                    res.Add(key, val);
                }
                else if (t.IsValueType)
                {
                    Type colType = t;
                    if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                        colType = colType.GetGenericArguments()[0];
                    try
                    { res.Add(key, Convert.ChangeType(postedValue, colType)); }
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
                    res.Add(key, postedValue);
                }
                else if (t == typeof(DateTime))
                {
                    DateTime val = DateTime.Now;
                    if (DateTime.TryParse(postedValue, out val))
                        res.Add(key, val);
                    else
                    {
                        Errmsg = DispName + "输入格式有误";
                        return null;
                    }
                }
            }
            return res;
        }

    }
}
