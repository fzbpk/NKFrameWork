using System;
using System.Collections.Generic;
using System.Linq;
using System.Data; 
using NK.ENum;
using NK.Entity; 
using NK.Message;
using System.ComponentModel;

namespace NK.Data
{
    /// <summary>
    /// UI控件
    /// </summary>
    public class DbUIControl : ControllerHelper, IDisposable
    {

        #region 构造函数

        /// <summary>
        /// 初始化
        /// </summary>
        protected virtual void Init()
        {
            
        }

        private void init()
        {
            initialization();
            Init();
        }
        /// <summary>
        ///  Linq数据库处理
        /// </summary>
        public DbUIControl() : base()
        {
            ClassName = this.GetType().ToString();
            this.language = Language.Chinese;
        }

        /// <summary>
        ///  Linq数据库处理
        /// </summary>
        /// <param name="info">数据库参数</param>
        public DbUIControl(DBInfo info) : base(info)
        {
            ClassName = this.GetType().ToString();
            this.language = Language.Chinese;
        }

        /// <summary>
        /// Linq数据库处理
        /// </summary>
        /// <param name="ConnectionType">数据库类型</param>
        /// <param name="ConnectionString">连接串</param>
        /// <param name="Timeout">超时时间，毫秒</param>
        public DbUIControl(DBType ConnectionType, string ConnectionString, int Timeout = 60) : base(ConnectionType, ConnectionString, Timeout)
        {
            ClassName = this.GetType().ToString();
            this.language = Language.Chinese;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        ~DbUIControl()
        {
            Dispose(false);
        }

        #endregion



        #region 方法

        private DataTable getDataTable(string sql)
        {
            init();
            if (DBOper != null)
                return DBOper.getDataTable(sql);
            else
                throw new NotSupportedException(SystemMessage.NotSupported("DBMODE", language));
        }

        private Dictionary<string, object> getDataRow(string sql)
        {
            init();
            if (DBOper != null)
                return DBOper.Find(sql);
            else
                throw new NotSupportedException(SystemMessage.NotSupported("DBMODE", language));
        }

        /// <summary>
        /// 绑定数据到FORM CheckBoxList
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="Class"></param>
        /// <param name="TextField"></param>
        /// <param name="ValueField"></param>
        /// <param name="defaultValue"></param>
        [DisplayName("bingCheckButtonList")]
        [Description("绑定数据到CheckBoxList")]
        public void bingCheckButtonList(string sql, System.Windows.Forms.CheckedListBox Class, string TextField, string ValueField, object defaultValue = null)
        {
            try
            {
                if (Class.Items.Count > 0)
                    Class.Items.Clear();
                DataTable dt = getDataTable(sql);
                if (dt != null)
                {
                    Class.DataSource = dt;
                    Class.DisplayMember = TextField;
                    Class.ValueMember = ValueField;
                    if (defaultValue != null)
                    {
                        var index = Class.Items.IndexOf(defaultValue);
                        if (index > -1)
                            Class.SetItemChecked(index, true);
                    }
                }
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, "bingCheckedListBox", Log_Type.Error, ex.Message);
                if (HasError != null)
                    HasError(ClassName, "bingCheckedListBox", ex);
                else
                    throw ex;
            }
        }

        /// <summary>
        /// 绑定数据到WEB CheckBoxList
        /// </summary>
        /// <param name="sql">T-SQL</param>
        /// <param name="Class">控件</param>
        /// <param name="TextField">显示列</param>
        /// <param name="ValueField">值列</param>
        /// <param name="defaultValue">默认值</param>
        [DisplayName("bindCheckButtonList")]
        [Description("绑定数据到CheckBoxList")]
        public void bindCheckButtonList(string sql, System.Web.UI.WebControls.CheckBoxList Class, string TextField, string ValueField, string defaultValue = "")
        {
            try
            {
                if (Class.Items.Count > 0)
                    Class.Items.Clear();
                DataTable dt = getDataTable(sql);
                if (dt != null)
                {
                    Class.DataSource = dt;
                    Class.DataTextField = TextField;
                    Class.DataValueField = ValueField;
                    Class.DataBind();
                }
                if (string.IsNullOrEmpty(defaultValue))
                    Class.Items.Insert(0, new System.Web.UI.WebControls.ListItem(ContorlsMessage.Select("", language), ""));
                else
                {
                    try
                    { Class.Items.FindByValue(defaultValue).Selected = true; }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, "bindCheckButtonList", Log_Type.Error, ex.Message);
                if (HasError != null)
                    HasError(ClassName, "bindCheckButtonList", ex);
                else
                    throw ex;
            }

        }

        /// <summary>
        /// 绑定数据到HTML CheckBoxList
        /// </summary>
        /// <param name="sql">T-SQL</param>
        /// <param name="Class">控件</param>
        /// <param name="TextField">显示列</param>
        /// <param name="ValueField">值列</param>
        /// <param name="defaultValue">默认值</param>
        /// <param name="CSS">CSS</param>
        /// <returns></returns>
        [DisplayName("bindCheckButtonList")]
        [Description("绑定数据到CheckBoxList")]
        public string bindCheckButtonList(string sql, string Class, string TextField, string ValueField, string defaultValue = "", string CSS = "")
        {
            try
            {
                string HTML = "<div name=\"CheckButtonList_" + Class + "\" id=\"CheckButtonList_" + Class + "\" class=\"" + CSS + "\" ><ul>";
                DataTable dt = getDataTable(sql);
                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i][ValueField].ToString() == defaultValue)
                            HTML += "<li>< input name =\"" + Class + "\" id=\"" + Class + "\"  type=\"checkbox\" checked=\"checked\"  >" + dt.Rows[i][TextField].ToString() + "</li>";
                        else
                            HTML += "<li>< input name =\"" + Class + "\" id=\"" + Class + "\"  type=\"checkbox\" >" + dt.Rows[i][TextField].ToString() + "</li>";
                    }
                }
                HTML += "</ul></div>";
                return HTML;
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, "bindDropDownList", Log_Type.Error, ex.Message);
                if (HasError != null)
                    HasError(ClassName, "bindDropDownList", ex);
                else
                    throw ex;
                return "";
            }
        }

        /// <summary>
        /// 绑定数据到WEB DropDownList
        /// </summary>
        /// <param name="sql">T-SQL</param>
        /// <param name="Class">控件</param>
        /// <param name="TextField">显示列</param>
        /// <param name="ValueField">值列</param>
        /// <param name="defaultValue">默认值</param> 
        [DisplayName("bindDropDownList")]
        [Description("绑定数据到DropDownList")]
        public void bindDropDownList(string sql, System.Windows.Forms.ComboBox Class, string TextField, string ValueField, object defaultValue = null)
        {
            try
            {
                if (Class.Items.Count > 0)
                    Class.Items.Clear();
                DataTable dt = getDataTable(sql);
                if (dt != null)
                {
                    Class.DataSource = dt;
                    Class.DisplayMember = TextField;
                    Class.ValueMember = ValueField;
                    if (defaultValue != null)
                    {
                        var index = Class.Items.IndexOf(defaultValue);
                        if (index > -1)
                            Class.SelectedIndex = index;
                    }
                    else
                        Class.Items.Insert(0, ContorlsMessage.Select("", language));
                }
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, "bindDropDownList", Log_Type.Error, ex.Message);
                if (HasError != null)
                    HasError(ClassName, "bindDropDownList", ex);
                else
                    throw ex;
            }
        }

        /// <summary>
        /// 绑定数据到WEB DropDownList
        /// </summary>
        /// <param name="sql">T-SQL</param>
        /// <param name="Class">控件</param>
        /// <param name="TextField">显示列</param>
        /// <param name="ValueField">值列</param>
        /// <param name="defaultValue">默认值</param> 
        [DisplayName("bindDropDownList")]
        [Description("绑定数据到DropDownList")]
        public void bindDropDownList(string sql, System.Web.UI.WebControls.DropDownList Class, string TextField, string ValueField, string defaultValue = "")
        {
            try
            {
                DataTable dt = getDataTable(sql);
                if (dt != null)
                {
                    Class.DataSource = dt;
                    Class.DataTextField = TextField;
                    Class.DataValueField = ValueField;
                    Class.DataBind();
                }
                if (string.IsNullOrEmpty(defaultValue))
                    Class.Items.Insert(0, new System.Web.UI.WebControls.ListItem(ContorlsMessage.Select("", language), ""));
                else
                {
                    try
                    { Class.Items.FindByValue(defaultValue).Selected = true; }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, "bindDropDownList", Log_Type.Error, ex.Message);
                if (HasError != null)
                    HasError(ClassName, "bindDropDownList", ex);
                else
                    throw ex;
            }
        }

        /// <summary>
        /// 绑定数据到HTML DropDownList
        /// </summary>
        /// <param name="sql">T-SQL</param>
        /// <param name="Class">控件</param>
        /// <param name="TextField">显示列</param>
        /// <param name="ValueField">值列</param>
        /// <param name="defaultValue">默认值</param>
        /// <param name="CSS">CSS</param>
        [DisplayName("bindDropDownList")]
        [Description("绑定数据到DropDownList")]
        public string bindDropDownList(string sql, string Class, string TextField, string ValueField, string defaultValue = "", string CSS = "")
        {
            try
            {
                string HTML = "<select name=\"" + Class + "\" id=\"" + Class + "\" class=\"" + CSS + "\" >";
                DataTable dt = getDataTable(sql);
                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i][ValueField].ToString() == defaultValue)
                            HTML += "<option value=\"" + dt.Rows[i][ValueField].ToString() + "\" selected>" + dt.Rows[i][TextField].ToString() + "</option>";
                        else
                            HTML += "<option value=\"" + dt.Rows[i][ValueField].ToString() + "\" >" + dt.Rows[i][TextField].ToString() + "</option>";
                    }
                }
                if (string.IsNullOrEmpty(defaultValue))
                    HTML += "<option value='' selected>" + ContorlsMessage.Select("", language) + "</option>";
                HTML += "</select>";
                return HTML;
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, "bindDropDownList", Log_Type.Error, ex.Message);
                if (HasError != null)
                    HasError(ClassName, "bindDropDownList", ex);
                else
                    throw ex;
                return "";
            }

        }

        /// <summary>
        /// 绑定数据到FORM RadioButtonList
        /// </summary>
        /// <param name="sql">T-SQL</param>
        /// <param name="Class">控件</param>
        /// <param name="TextField">显示列</param>
        /// <param name="ValueField">值列</param>
        /// <param name="defaultValue">默认值</param> 
        [DisplayName("bindRadioButtonList")]
        [Description("绑定数据到RadioButtonList")]
        public void bindRadioButtonList(string sql, System.Windows.Forms.Panel Class, string TextField, string ValueField, string defaultValue = "")
        {
            try
            {
                if (Class.Controls.Count > 0)
                    Class.Controls.Clear();
                DataTable dt = getDataTable(sql);
                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        System.Windows.Forms.RadioButton raid = new System.Windows.Forms.RadioButton();
                        raid.Name = Class.Name + "_" + i.ToString();
                        raid.Text = dt.Rows[i][TextField].ToString();
                        if (!string.IsNullOrEmpty(defaultValue))
                        {
                            if (dt.Rows[i][ValueField].ToString() == defaultValue)
                                raid.Checked = true;
                        }
                        Class.Controls.Add(raid);
                    }
                }
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, "bindRadioButtonList", Log_Type.Error, ex.Message);
                if (HasError != null)
                    HasError(ClassName, "bindRadioButtonList", ex);
                else
                    throw ex;
            }
        }

        /// <summary>
        /// 绑定数据到WEB RadioButtonList
        /// </summary>
        /// <param name="sql">T-SQL</param>
        /// <param name="Class">控件</param>
        /// <param name="TextField">显示列</param>
        /// <param name="ValueField">值列</param>
        /// <param name="defaultValue">默认值</param>
        [DisplayName("bindRadioButtonList")]
        [Description("绑定数据到RadioButtonList")]
        public void bindRadioButtonList(string sql, System.Web.UI.WebControls.RadioButtonList Class, string TextField, string ValueField, string defaultValue = "")
        {
            try
            {
                DataTable dt = getDataTable(sql);
                if (dt != null)
                {
                    Class.DataSource = dt;
                    Class.DataTextField = TextField;
                    Class.DataValueField = ValueField;
                    Class.DataBind();
                }
                if (string.IsNullOrEmpty(defaultValue))
                    Class.Items.Insert(0, new System.Web.UI.WebControls.ListItem(ContorlsMessage.Select("", language), ""));
                else
                {
                    try
                    { Class.Items.FindByValue(defaultValue).Selected = true; }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, "bindRadioButtonList", Log_Type.Error, ex.Message);
                if (HasError != null)
                    HasError(ClassName, "bindRadioButtonList", ex);
                else
                    throw ex;
            }

        }

        /// <summary>
        /// 绑定数据到HTML RadioButtonList
        /// </summary>
        /// <param name="sql">T-SQL</param>
        /// <param name="Class">控件</param>
        /// <param name="TextField">显示列</param>
        /// <param name="ValueField">值列</param>
        /// <param name="defaultValue">默认值</param>
        /// <param name="CSS">样式</param>
        [DisplayName("bindRadioButtonList")]
        [Description("绑定数据到RadioButtonList")]
        public string bindRadioButtonList(string sql, string Class, string TextField, string ValueField, string defaultValue = "", string CSS = "")
        {
            try
            {
                string HTML = "<div name=\"RadioButtonList_" + Class + "\" id=\"RadioButtonList_" + Class + "\" class=\"" + CSS + "\" ><ul>";
                DataTable dt = getDataTable(sql);
                if (dt != null)
                {
                    bool sel = false;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i][ValueField].ToString() == defaultValue)
                        {
                            sel = true;
                            HTML += " <li><input type=\"radio\" name=\"" + Class + "\" id=\"" + Class + "_" + i.ToString() + "\" value=\"" + dt.Rows[i][ValueField].ToString() + "\" checked=\"checked\" />" + dt.Rows[i][TextField].ToString() + "</li>";
                        }
                        else
                        {
                            if (i == (dt.Rows.Count - 1) && !sel)
                                HTML += " <li><input type=\"radio\" name=\"" + Class + "\" id=\"" + Class + "_" + i.ToString() + "\" value=\"" + dt.Rows[i][ValueField].ToString() + "\" />" + dt.Rows[i][TextField].ToString() + "</li>";
                            else
                                HTML += " <li><input type=\"radio\" name=\"" + Class + "\" id=\"" + Class + "_" + i.ToString() + "\" value=\"" + dt.Rows[i][ValueField].ToString() + "\" />" + dt.Rows[i][TextField].ToString() + "</li>";
                        }
                    }
                }
                HTML += "</ul></div>";
                return HTML;
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, "bindRadioButtonList", Log_Type.Error, ex.Message);
                if (HasError != null)
                    HasError(ClassName, "bindRadioButtonList", ex);
                else
                    throw ex;
                return "";
            }

        }

        /// <summary>
        /// 绑定数据到FORM ListBox
        /// </summary>
        /// <param name="sql">T-SQL</param>
        /// <param name="Class">控件</param>
        /// <param name="TextField">显示列</param>
        /// <param name="ValueField">值列</param>
        /// <param name="defaultValue">默认值</param> 
        [DisplayName("bindListBox")]
        [Description("绑定数据到ListBox")]
        public void bindListBox(string sql, System.Windows.Forms.ListBox Class, string TextField, string ValueField, object defaultValue = null)
        {
            try
            {
                if (Class.Items.Count > 0)
                    Class.Items.Clear();
                DataTable dt = getDataTable(sql);
                if (dt != null)
                {
                    Class.DataSource = dt;
                    Class.DisplayMember = TextField;
                    Class.ValueMember = ValueField;
                    if (defaultValue != null)
                    {
                        var index = Class.Items.IndexOf(defaultValue);
                        if (index > -1)
                            Class.SelectedIndex = index;
                    }
                }
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, "bindListBox", Log_Type.Error, ex.Message);
                if (HasError != null)
                    HasError(ClassName, "bindListBoxt", ex);
                else
                    throw ex;
            }
        }

        /// <summary>
        /// 绑定数据到WEB ListBoxt
        /// </summary>
        /// <param name="sql">T-SQL</param>
        /// <param name="Class">控件</param>
        /// <param name="TextField">显示列</param>
        /// <param name="ValueField">值列</param>
        /// <param name="defaultValue">默认值</param> 
        [DisplayName("bindListBox")]
        [Description("绑定数据到ListBoxt")]
        public void bindListBox(string sql, System.Web.UI.WebControls.ListBox Class, string TextField, string ValueField, string defaultValue = null)
        {
            try
            {
                if (Class.Items.Count > 0)
                    Class.Items.Clear();
                DataTable dt = getDataTable(sql);
                if (dt != null)
                {
                    Class.DataSource = dt;
                    Class.DataTextField = TextField;
                    Class.DataValueField = ValueField;
                    Class.DataBind();
                }
                if (string.IsNullOrEmpty(defaultValue))
                    Class.Items.Insert(0, new System.Web.UI.WebControls.ListItem(ContorlsMessage.Select("", language), ""));
                else
                {
                    try
                    { Class.Items.FindByValue(defaultValue).Selected = true; }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, "bindListBox", Log_Type.Error, ex.Message);
                if (HasError != null)
                    HasError(ClassName, "bindListBoxt", ex);
                else
                    throw ex;
            }
        }

        /// <summary>
        /// 绑定数据到HTML ListBoxt
        /// </summary>
        /// <param name="sql">T-SQL</param>
        /// <param name="Class">控件</param>
        /// <param name="TextField">显示列</param>
        /// <param name="ValueField">值列</param>
        /// <param name="defaultValue">默认值</param>
        /// <param name="CSS">样式</param>
        [DisplayName("bindListBox")]
        [Description("绑定数据到ListBoxt")]
        public string bindListBox(string sql, string Class, string TextField, string ValueField, string defaultValue = "", string CSS = "")
        {
            try
            {
                string HTML = "<div name=\"ListBoxt_" + Class + "\" id=\"ListBoxt_" + Class + "\" class=\"" + CSS + "\" ><ul>";
                string selv = "";
                DataTable dt = getDataTable(sql);
                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(defaultValue))
                        {
                            if (dt.Rows[i][ValueField].ToString() == defaultValue)
                                selv = dt.Rows[i][ValueField].ToString();
                        }
                        HTML += "<li onclick=\"javascript:document.getElementById(\"" + Class + "\").value='" + dt.Rows[i][ValueField].ToString() + "';\">" + dt.Rows[i][TextField].ToString() + "</li>";

                    }
                }
                HTML += "</ul><input type=\"hidden\" name=\"" + Class + "\" id=\"" + Class + "\" value=\"" + selv + "\" /></div>";
                return HTML;
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, "bindListBox", Log_Type.Error, ex.Message);
                if (HasError != null)
                    HasError(ClassName, "bindListBoxt", ex);
                else
                    throw ex;
                return "";
            }
        }

        /// <summary>
        /// 绑定数据到FORM TextBox
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="Class"></param> 
        /// <param name="ValueField"></param>
        /// <param name="defaultValue"></param>
        [DisplayName("bindTextBox")]
        [Description("绑定数据到TextBox")]
        public void bindTextBox(string sql, System.Windows.Forms.TextBox Class, string ValueField, string defaultValue = "")
        {
            try
            {
                Class.Text = "";
                Dictionary<string, object> dr = getDataRow(sql);
                if (dr != null)
                {
                    var tmp = dr.FirstOrDefault(c => c.Key.ToUpper().Trim() == ValueField.ToUpper().Trim());
                    if (!string.IsNullOrEmpty(tmp.Key))
                        Class.Text = tmp.Value.ToString();
                }
                if (string.IsNullOrEmpty(Class.Text) && !string.IsNullOrEmpty(defaultValue))
                    Class.Text = defaultValue;
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, "bindTextBox", Log_Type.Error, ex.Message);
                if (HasError != null)
                    HasError(ClassName, "bingTextBox", ex);
                else
                    throw ex;
            }
        }

        /// <summary>
        /// 绑定数据到WEB TextBox
        /// </summary>
        /// <param name="sql">T-SQL</param>
        /// <param name="Class">控件</param>
        /// <param name="ValueField">值列</param>
        /// <param name="defaultValue">默认值</param>
        [DisplayName("bindTextBox")]
        [Description("绑定数据到TextBox")]
        public void bindTextBox(string sql, System.Web.UI.WebControls.TextBox Class, string ValueField, string defaultValue = "")
        {
            try
            {
                Class.Text = "";
                Dictionary<string, object> dr = getDataRow(sql);
                if (dr != null)
                {
                    var tmp = dr.FirstOrDefault(c => c.Key.ToUpper().Trim() == ValueField.ToUpper().Trim());
                    if (!string.IsNullOrEmpty(tmp.Key))
                        Class.Text = tmp.Value.ToString();
                }
                if (string.IsNullOrEmpty(Class.Text) && !string.IsNullOrEmpty(defaultValue))
                    Class.Text = defaultValue;
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, "bindTextBox", Log_Type.Error, ex.Message);
                if (HasError != null)
                    HasError(ClassName, "bindTextBox", ex);
                else
                    throw ex;
            }
        }

        /// <summary>
        /// 绑定数据到HTML TextBox
        /// </summary>
        /// <param name="sql">T-SQL</param>
        /// <param name="Class">控件</param>
        /// <param name="ValueField">值列</param>
        /// <param name="defaultValue">默认值</param>
        /// <param name="CSS">CSS</param>
        /// <returns></returns>
        [DisplayName("bindTextBox")]
        [Description("绑定数据到TextBox")]
        public string bindTextBox(string sql, string Class, string ValueField, string defaultValue = "", string CSS = "")
        {
            try
            {
                string selv = "";
                Dictionary<string, object> dr = getDataRow(sql);
                if (dr != null)
                {
                    var tmp = dr.FirstOrDefault(c => c.Key.ToUpper().Trim() == ValueField.ToUpper().Trim());
                    if (!string.IsNullOrEmpty(tmp.Key))
                        selv = tmp.Value.ToString();
                }
                if (string.IsNullOrEmpty(selv) && !string.IsNullOrEmpty(defaultValue))
                    selv = defaultValue;
                string HTML = "< input name =\"" + Class + "\" id=\"" + Class + "\"  type=\"text\"  value=\"" + selv + "\" class=\"" + CSS + "\" />";
                return HTML;
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, "bindTextBox", Log_Type.Error, ex.Message);
                if (HasError != null)
                    HasError(ClassName, "bindTextBox", ex);
                else
                    throw ex;
                return "";
            }
        }

        /// <summary>
        /// 绑定数据到FORM CheckBox
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="Class"></param> 
        /// <param name="TextField"></param>
        /// <param name="defaultValue"></param>
        [DisplayName("bindCheckBox")]
        [Description("绑定数据到CheckBox")]
        public void bindCheckBox(string sql, System.Windows.Forms.CheckBox Class, string TextField, bool defaultValue = false)
        {
            try
            {
                Class.Text = "";
                Dictionary<string, object> dr = getDataRow(sql);
                if (dr != null)
                {
                    var tmp = dr.FirstOrDefault(c => c.Key.ToUpper().Trim() == TextField.ToUpper().Trim());
                    if (!string.IsNullOrEmpty(tmp.Key))
                        Class.Text = tmp.Value.ToString();
                }
                Class.Checked = defaultValue;
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, "bindCheckBox", Log_Type.Error, ex.Message);
                if (HasError != null)
                    HasError(ClassName, "bindCheckBox", ex);
                else
                    throw ex;
            }
        }

        /// <summary>
        /// 绑定数据到WEB CheckBox
        /// </summary>
        /// <param name="sql">T-SQL</param>
        /// <param name="Class">控件</param>
        /// <param name="TextField">值列</param>
        /// <param name="defaultValue">默认值</param>
        [DisplayName("bindCheckBox")]
        [Description("绑定数据到CheckBox")]
        public void bindCheckBox(string sql, System.Web.UI.WebControls.CheckBox Class, string TextField, bool defaultValue = false)
        {
            try
            {
                Class.Text = "";
                Dictionary<string, object> dr = getDataRow(sql);
                if (dr != null)
                {
                    var tmp = dr.FirstOrDefault(c => c.Key.ToUpper().Trim() == TextField.ToUpper().Trim());
                    if (!string.IsNullOrEmpty(tmp.Key))
                        Class.Text = tmp.Value.ToString();
                }
                Class.Checked = defaultValue;
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, "bindCheckBox", Log_Type.Error, ex.Message);
                if (HasError != null)
                    HasError(ClassName, "bindCheckBox", ex);
                else
                    throw ex;
            }
        }

        /// <summary>
        /// 绑定数据到HTML CheckBox
        /// </summary>
        /// <param name="sql">T-SQL</param>
        /// <param name="Class">控件</param>
        /// <param name="TextField">显示列</param>
        /// <param name="ValueField">值列</param>
        /// <param name="defaultValue">默认值</param>
        /// <param name="CSS">CSS</param>
        /// <returns></returns>
        [DisplayName("bindCheckBox")]
        [Description("绑定数据到CheckBox")]
        public string bindCheckBox(string sql, string Class, string TextField, string ValueField, bool defaultValue = false, string CSS = "")
        {
            try
            {
                string selv = "", txt = "", val = "";
                Dictionary<string, object> dr = getDataRow(sql);
                if (dr != null)
                {
                    var tmp = dr.FirstOrDefault(c => c.Key.ToUpper().Trim() == TextField.ToUpper().Trim());
                    if (!string.IsNullOrEmpty(tmp.Key))
                        txt = tmp.Value.ToString();
                    var tmpv = dr.FirstOrDefault(c => c.Key.ToUpper().Trim() == ValueField.ToUpper().Trim());
                    if (!string.IsNullOrEmpty(tmp.Key))
                        val = tmpv.Value.ToString();
                }
                if (defaultValue)
                    selv = "checked=\"checked\"";
                string HTML = "<div class=\"" + CSS + "\" id=\"CheckBox_" + Class + " name=\"CheckBox_" + Class + " \">< input name =\"" + Class + "\" id=\"" + Class + "\"  type=\"checkbox\"  value=\"" + val + "\"  " + selv + "  />" + txt + "</div>";
                return HTML;
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, "bindCheckBox", Log_Type.Error, ex.Message);
                if (HasError != null)
                    HasError(ClassName, "bindCheckBox", ex);
                else
                    throw ex;
                return "";
            }
        }

        /// <summary>
        /// 绑定数据到FORM RadioButton
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="Class"></param> 
        /// <param name="TextField"></param>
        /// <param name="defaultValue"></param>
        [DisplayName("bindRadioButton")]
        [Description("绑定数据到RadioButton")]
        public void bindRadioButton(string sql, System.Windows.Forms.RadioButton Class, string TextField, bool defaultValue = false)
        {
            try
            {
                Class.Text = "";
                Dictionary<string, object> dr = getDataRow(sql);
                if (dr != null)
                {
                    var tmp = dr.FirstOrDefault(c => c.Key.ToUpper().Trim() == TextField.ToUpper().Trim());
                    if (!string.IsNullOrEmpty(tmp.Key))
                        Class.Text = tmp.Value.ToString();
                }
                Class.Checked = defaultValue;
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, "bindRadioButton", Log_Type.Error, ex.Message);
                if (HasError != null)
                    HasError(ClassName, "bindRadioButton", ex);
                else
                    throw ex;
            }
        }

        /// <summary>
        /// 绑定数据到WEB RadioButton
        /// </summary>
        /// <param name="sql">T-SQL</param>
        /// <param name="Class">控件</param>
        /// <param name="TextField">值列</param>
        /// <param name="defaultValue">默认值</param>
        [DisplayName("bindRadioButton")]
        [Description("绑定数据到RadioButton")]
        public void bindRadioButton(string sql, System.Web.UI.WebControls.RadioButton Class, string TextField, bool defaultValue = false)
        {
            try
            {
                Class.Text = "";
                Dictionary<string, object> dr = getDataRow(sql);
                if (dr != null)
                {
                    var tmp = dr.FirstOrDefault(c => c.Key.ToUpper().Trim() == TextField.ToUpper().Trim());
                    if (!string.IsNullOrEmpty(tmp.Key))
                        Class.Text = tmp.Value.ToString();
                }
                Class.Checked = defaultValue;
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, "bindRadioButton", Log_Type.Error, ex.Message);
                if (HasError != null)
                    HasError(ClassName, "bindRadioButton", ex);
                else
                    throw ex;
            }
        }

        /// <summary>
        /// 绑定数据到HTML RadioButton
        /// </summary>
        /// <param name="sql">T-SQL</param>
        /// <param name="Class">控件</param>
        /// <param name="TextField">显示列</param>
        /// <param name="ValueField">值列</param>
        /// <param name="defaultValue">默认值</param>
        /// <param name="CSS">CSS</param>
        /// <returns></returns>
        [DisplayName("bindRadioButton")]
        [Description("绑定数据到RadioButton")]
        public string bindRadioButton(string sql, string Class, string TextField, string ValueField, bool defaultValue = false, string CSS = "")
        {
            try
            {
                string selv = "", txt = "", val = "";
                Dictionary<string, object> dr = getDataRow(sql);
                if (dr != null)
                {
                    var tmp = dr.FirstOrDefault(c => c.Key.ToUpper().Trim() == TextField.ToUpper().Trim());
                    if (!string.IsNullOrEmpty(tmp.Key))
                        txt = tmp.Value.ToString();
                    var tmpv = dr.FirstOrDefault(c => c.Key.ToUpper().Trim() == ValueField.ToUpper().Trim());
                    if (!string.IsNullOrEmpty(tmp.Key))
                        val = tmpv.Value.ToString();
                }
                if (defaultValue)
                    selv = "checked=\"checked\"";
                string HTML = "<div class=\"" + CSS + "\" id=\"Radio_" + Class + " name=\"Radio_" + Class + " \">< input name =\"" + Class + "\" id=\"" + Class + "\"  type=\"radio\"  value=\"" + val + "\"  " + selv + "  />" + txt + "</div>";
                return HTML;
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, "bindRadioButton", Log_Type.Error, ex.Message);
                if (HasError != null)
                    HasError(ClassName, "bindRadioButton", ex);
                else
                    throw ex;
                return "";
            }
        }

        /// <summary>
        /// 绑定数据到FORM Label
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="Class"></param> 
        /// <param name="TextField"></param> 
        [DisplayName("bindLabel")]
        [Description("绑定数据到Label")]
        public void bindLabel(string sql, System.Windows.Forms.Label Class, string TextField)
        {
            try
            {
                Class.Text = "";
                Dictionary<string, object> dr = getDataRow(sql);
                if (dr != null)
                {
                    var tmp = dr.FirstOrDefault(c => c.Key.ToUpper().Trim() == TextField.ToUpper().Trim());
                    if (!string.IsNullOrEmpty(tmp.Key))
                        Class.Text = tmp.Value.ToString();
                }
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, "bindLabel", Log_Type.Error, ex.Message);
                if (HasError != null)
                    HasError(ClassName, "bindLabel", ex);
                else
                    throw ex;
            }
        }

        /// <summary>
        /// 绑定数据到WEB Label
        /// </summary>
        /// <param name="sql">T-SQL</param>
        /// <param name="Class">控件</param>
        /// <param name="TextField">值列</param>
        [DisplayName("bindLabel")]
        [Description("绑定数据到Label")]
        public void bindLabel(string sql, System.Web.UI.WebControls.Label Class, string TextField)
        {
            try
            {
                Class.Text = "";
                Dictionary<string, object> dr = getDataRow(sql);
                if (dr != null)
                {
                    var tmp = dr.FirstOrDefault(c => c.Key.ToUpper().Trim() == TextField.ToUpper().Trim());
                    if (!string.IsNullOrEmpty(tmp.Key))
                        Class.Text = tmp.Value.ToString();
                }
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, "bindLabel", Log_Type.Error, ex.Message);
                if (HasError != null)
                    HasError(ClassName, "bindLabel", ex);
                else
                    throw ex;
            }
        }

        /// <summary>
        /// 绑定数据到HTML Label
        /// </summary>
        /// <param name="sql">T-SQL</param>
        /// <param name="Class">控件</param>
        /// <param name="TextField">显示列</param> 
        /// <param name="CSS">CSS</param>
        /// <returns></returns>
        [DisplayName("bindLabel")]
        [Description("绑定数据到Label")]
        public string bindLabel(string sql, string Class, string TextField, string CSS = "")
        {
            try
            {
                string txt = "";
                Dictionary<string, object> dr = getDataRow(sql);
                if (dr != null)
                {
                    var tmp = dr.FirstOrDefault(c => c.Key.ToUpper().Trim() == TextField.ToUpper().Trim());
                    if (!string.IsNullOrEmpty(tmp.Key))
                        txt = tmp.Value.ToString();
                }
                string HTML = "<div class=\"" + CSS + "\" id=\"" + Class + "\">" + txt + "</div>";
                return HTML;
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, "bindLabel", Log_Type.Error, ex.Message);
                if (HasError != null)
                    HasError(ClassName, "bindLabel", ex);
                else
                    throw ex;
                return "";
            }
        }

        /// <summary>
        /// 绑定数据到FORM HyLinkLabel
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="Class"></param> 
        /// <param name="TextField"></param> 
        /// <param name="ValueField"></param> 
        [DisplayName("bindHyLinkLabel")]
        [Description("绑定数据到HyLinkLabel")]
        public void bindHyLinkLabel(string sql, System.Windows.Forms.LinkLabel Class, string TextField, string ValueField)
        {
            try
            {
                Class.Text = "";
                Dictionary<string, object> dr = getDataRow(sql);
                if (dr != null)
                {
                    var tmp = dr.FirstOrDefault(c => c.Key.ToUpper().Trim() == TextField.ToUpper().Trim());
                    if (!string.IsNullOrEmpty(tmp.Key))
                        Class.Text = tmp.Value.ToString();
                    var val = dr.FirstOrDefault(c => c.Key.ToUpper().Trim() == ValueField.ToUpper().Trim());
                    if (!string.IsNullOrEmpty(val.Key))
                        Class.Links.Add(0, tmp.Value.ToString().Length, val.Value.ToString());
                }
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, "bindHyLinkLabel", Log_Type.Error, ex.Message);
                if (HasError != null)
                    HasError(ClassName, "bindHyLinkLabel", ex);
                else
                    throw ex;
            }
        }

        /// <summary>
        /// 绑定数据到WEB HyperLink
        /// </summary>
        /// <param name="sql">T-SQL</param>
        /// <param name="Class">控件</param>
        /// <param name="TextField">显示列</param>
        /// <param name="ValueField">值列</param>
        [DisplayName("bindHyLinkLabel")]
        [Description("绑定数据到HyLinkLabel")]
        public void bindHyLinkLabel(string sql, System.Web.UI.WebControls.HyperLink Class, string TextField, string ValueField)
        {
            try
            {
                Class.Text = "";
                Dictionary<string, object> dr = getDataRow(sql);
                if (dr != null)
                {
                    var tmp = dr.FirstOrDefault(c => c.Key.ToUpper().Trim() == TextField.ToUpper().Trim());
                    if (!string.IsNullOrEmpty(tmp.Key))
                        Class.Text = tmp.Value.ToString();
                    var val = dr.FirstOrDefault(c => c.Key.ToUpper().Trim() == ValueField.ToUpper().Trim());
                    if (!string.IsNullOrEmpty(val.Key))
                        Class.NavigateUrl = val.Value.ToString();
                }
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, "bindHyLinkLabel", Log_Type.Error, ex.Message);
                if (HasError != null)
                    HasError(ClassName, "bindHyLinkLabel", ex);
                else
                    throw ex;
            }
        }

        /// <summary>
        /// 绑定数据到HTML HyLinkLabel
        /// </summary>
        /// <param name="sql">T-SQL</param>
        /// <param name="Class">控件</param>
        /// <param name="TextField">显示列</param> 
        /// <param name="ValueField">值列</param> 
        /// <param name="CSS">CSS</param>
        /// <returns></returns>
        [DisplayName("bindHyLinkLabel")]
        [Description("绑定数据到HyLinkLabel")]
        public string bindHyLinkLabel(string sql, string Class, string TextField, string ValueField, string CSS = "")
        {
            try
            {
                string txt = "", vala = "";
                Dictionary<string, object> dr = getDataRow(sql);
                if (dr != null)
                {
                    var tmp = dr.FirstOrDefault(c => c.Key.ToUpper().Trim() == TextField.ToUpper().Trim());
                    if (!string.IsNullOrEmpty(tmp.Key))
                        txt = tmp.Value.ToString();
                    var val = dr.FirstOrDefault(c => c.Key.ToUpper().Trim() == ValueField.ToUpper().Trim());
                    if (!string.IsNullOrEmpty(val.Key))
                        vala = val.Value.ToString();
                }
                string HTML = "<a herf=\"" + vala + "\" class=\"" + CSS + "\" id=\"" + Class + "\"  name=\"" + Class + "\"  >" + txt + "</a>";
                return HTML;
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, "bindHyLinkLabel", Log_Type.Error, ex.Message);
                if (HasError != null)
                    HasError(ClassName, "bindHyLinkLabel", ex);
                else
                    throw ex;
                return "";
            }
        }

        /// <summary>
        /// 绑定数据到FORM Label
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="Class"></param> 
        /// <param name="ValueField"></param> 
        /// <param name="defaultValue">默认值</param>
        [DisplayName("bindHidenLabel")]
        [Description(" 绑定数据到HiddenField")]
        public void bindHidenLabel(string sql, System.Windows.Forms.Label Class, string ValueField, string defaultValue = "")
        {
            try
            {
                Class.Visible = false;
                Class.Text = "";
                Dictionary<string, object> dr = getDataRow(sql);
                if (dr != null)
                {
                    var tmp = dr.FirstOrDefault(c => c.Key.ToUpper().Trim() == ValueField.ToUpper().Trim());
                    if (!string.IsNullOrEmpty(tmp.Key))
                        Class.Text = tmp.Value.ToString();
                }
                if (string.IsNullOrEmpty(Class.Text) && !string.IsNullOrEmpty(defaultValue))
                    Class.Text = defaultValue;
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, "bindHidenLabel", Log_Type.Error, ex.Message);
                if (HasError != null)
                    HasError(ClassName, "bindHidenLabel", ex);
                else
                    throw ex;
            }
        }

        /// <summary>
        /// 绑定数据到WEB TextBox
        /// </summary>
        /// <param name="sql">T-SQL</param>
        /// <param name="Class">控件</param>
        /// <param name="ValueField">值列</param>
        /// <param name="defaultValue">默认值</param>
        [DisplayName("bindHidenLabel")]
        [Description(" 绑定数据到HiddenField")]
        public void bindHidenLabel(string sql, System.Web.UI.WebControls.HiddenField Class, string ValueField, string defaultValue = "")
        {
            try
            {
                Class.Value = "";
                Dictionary<string, object> dr = getDataRow(sql);
                if (dr != null)
                {
                    var tmp = dr.FirstOrDefault(c => c.Key.ToUpper().Trim() == ValueField.ToUpper().Trim());
                    if (!string.IsNullOrEmpty(tmp.Key))
                        Class.Value = tmp.Value.ToString();
                }
                if (string.IsNullOrEmpty(Class.Value) && !string.IsNullOrEmpty(defaultValue))
                    Class.Value = defaultValue;
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, "bindHidenLabel", Log_Type.Error, ex.Message);
                if (HasError != null)
                    HasError(ClassName, "bindHidenLabel", ex);
                else
                    throw ex;
            }
        }

        /// <summary>
        /// 绑定数据到HTML TextBox
        /// </summary>
        /// <param name="sql">T-SQL</param>
        /// <param name="Class">控件</param>
        /// <param name="ValueField">显示列</param> 
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        [DisplayName("bindHidenLabel")]
        [Description(" 绑定数据到HiddenField")]
        public string bindHidenLabel(string sql, string Class, string ValueField, string defaultValue = "")
        {
            try
            {
                string txt = "";
                Dictionary<string, object> dr = getDataRow(sql);
                if (dr != null)
                {
                    var tmp = dr.FirstOrDefault(c => c.Key.ToUpper().Trim() == ValueField.ToUpper().Trim());
                    if (!string.IsNullOrEmpty(tmp.Key))
                        txt = tmp.Value.ToString();
                }
                if (string.IsNullOrEmpty(txt) && !string.IsNullOrEmpty(defaultValue))
                    txt = defaultValue;
                string HTML = "< input name =\"" + Class + "\" id=\"" + Class + "\"  type=\"hidden\"  value=\"" + txt + "\"   />";
                return HTML;
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, "bindHidenLabel", Log_Type.Error, ex.Message);
                if (HasError != null)
                    HasError(ClassName, "bindHidenLabel", ex);
                else
                    throw ex;
                return "";
            }
        }


        #endregion

    }
}
