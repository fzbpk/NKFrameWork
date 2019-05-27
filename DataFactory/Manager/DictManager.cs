using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Common;
using LinqToDB;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;
using System.ComponentModel;
using NK.Message;
using NK.ENum;
using NK.Entity;
using NK.Event;
using NK.Attribut;
using System.Web.UI.WebControls;
using LinqToDB.Mapping;

namespace NK.Data.Manager
{
    /// <summary>
    /// 字典管理
    /// </summary>
   public  class DictManager :DataHelper, IDisposable
    { 

        #region 构造函数
 
        /// <summary>
        /// 字典管理
        /// </summary>
        public DictManager():base()
        {
            this.language = Language.Chinese;
            ClassName = this.GetType().ToString(); 
        }

        /// <summary>
        /// 字典管理
        /// </summary>
        /// <param name="info">数据库参数</param>
        public DictManager(DBInfo info = null):base(info)
        { 
            this.language = Language.Chinese;
            ClassName = this.GetType().ToString();
        }

        /// <summary>
        ///字典管理
        /// </summary>
        /// <param name="ConnectionType">数据库类型</param>
        /// <param name="ConnectionString">连接串</param>
        /// <param name="Timeout">超时时间，毫秒</param>
        public DictManager(DBType ConnectionType, string ConnectionString, int Timeout = 60):base(ConnectionType, ConnectionString, Timeout)
        {   
            this.language = Language.Chinese;
            ClassName = this.GetType().ToString();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        ~DictManager()
        {
            Dispose(false);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region 私有方法

        private string Serialize(object obj)
        {
            try
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
                MemoryStream stream = new MemoryStream();
                serializer.WriteObject(stream, obj);
                byte[] dataBytes = new byte[stream.Length];
                stream.Position = 0;
                stream.Read(dataBytes, 0, (int)stream.Length);
                return Encoding.UTF8.GetString(dataBytes);
            }
            catch (Exception ex)
            { return ex.Message; }
        }

        private void Init()
        {
            init();
            if (!TableIsExist())
                CreatTable();
        }

        private void CreatTable()
        {
            string TableName = "";
            List<ColumnAttribute> Columns = EToSqlCreat<DictInfo>(out TableName);
            if (Columns.Where(c => c.IsPrimaryKey).Count() <= 0)
                throw new NullReferenceException(SystemMessage.RefNullOrEmpty("PrimaryKey", language));
            List<string> sqlbat = TableName.CreatToSql(DB.Mode, Columns);
            foreach (var tsql in sqlbat)
            {
                if (Execute(tsql) <= 0)
                    throw new Exception(tsql);
            }
        }

        private void DropTable()
        {
            string TableName = Table<DictInfo>();
            List<string> sqlbat = TableName.DropToSql();
            foreach (var tsql in sqlbat)
            {
                if (Execute(tsql) <= 0)
                    throw new Exception(tsql);
            }
        }

        private bool TableIsExist()
        {
            string TableName = Table<DictInfo>();
            if (TableIsExist(TableName))
            {
                try
                {
                    var tmp = context.GetTable<DictInfo>().FirstOrDefault();
                    return true;
                }
                catch
                {
                    DropTable();
                    return false;
                }
            }
            else
                return false;
        }

        #endregion
        
        #region 方法

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool Add(DictInfo info)
        {
            Init();
            MethodName = "";
            try
            {
                System.Reflection.MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            if (info == null)
            {
                if (this.HasError != null)
                    this.HasError(ClassName, MethodName, new NullReferenceException(SystemMessage.RefNullOrEmpty("info", language)));
                else
                    throw new NullReferenceException(SystemMessage.RefNullOrEmpty("info", language));
                return false;
            }
            else if (string.IsNullOrEmpty(info.Name))
            {
                if (this.HasError != null)
                    this.HasError(ClassName, MethodName, new NullReferenceException(SystemMessage.RefNullOrEmpty("info.Name", language)));
                else
                    throw new NullReferenceException(SystemMessage.RefNullOrEmpty("info.Name", language));
                return false;
            }
            else if (string.IsNullOrEmpty(info.Key))
            {
                if (this.HasError != null)
                    this.HasError(ClassName, MethodName, new NullReferenceException(SystemMessage.RefNullOrEmpty("info.Key", language)));
                else
                    throw new NullReferenceException(SystemMessage.RefNullOrEmpty("info.Key", language));
                return false;
            }
            try
            {
                info.language = this.language;
                context.Insert(info);
                return true;
            }
            catch (Exception ex)
            {
                if (this.HasError != null)
                    this.HasError(ClassName, MethodName, ex);
                else
                    throw ex;
                return false;
            } 
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool Update(DictInfo info)
        {
            Init();
            MethodName = "";
            try
            {
                System.Reflection.MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            if (info == null)
            {
                if (this.HasError != null)
                    this.HasError(ClassName, MethodName, new NullReferenceException(SystemMessage.RefNullOrEmpty("info", language)));
                else
                    throw new NullReferenceException(SystemMessage.RefNullOrEmpty("info", language));
                return false;
            }
            else if (string.IsNullOrEmpty(info.Name))
            {
                if (this.HasError != null)
                    this.HasError(ClassName, MethodName, new NullReferenceException(SystemMessage.RefNullOrEmpty("info.Name", language)));
                else
                    throw new NullReferenceException(SystemMessage.RefNullOrEmpty("info.Name", language));
                return false;
            }
            else if (string.IsNullOrEmpty(info.Key))
            {
                if (this.HasError != null)
                    this.HasError(ClassName, MethodName, new NullReferenceException(SystemMessage.RefNullOrEmpty("info.Key", language)));
                else
                    throw new NullReferenceException(SystemMessage.RefNullOrEmpty("info.Key", language));
                return false;
            }
            try
            {
                info.language = this.language;
                context.Update(info);
                return true;
            }
            catch (Exception ex)
            {
                if (this.HasError != null)
                    this.HasError(ClassName, MethodName, ex);
                else
                    throw ex;
                return false;
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool Delete(DictInfo info)
        {
            Init();
            MethodName = "";
            try
            {
                System.Reflection.MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            if (info == null)
            {
                if (this.HasError != null)
                    this.HasError(ClassName, MethodName, new NullReferenceException(SystemMessage.RefNullOrEmpty("Data", language)));
                else
                    throw new NullReferenceException(SystemMessage.RefNullOrEmpty("Data", language));
                return false;
            }
            try
            {
                context.Delete(info);
                return true;
            }
            catch (Exception ex)
            {
                if (this.HasError != null)
                    this.HasError(ClassName, MethodName, ex);
                else
                    throw ex;
                return false;
            }
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="TypeCode"></param>
        /// <param name="PageCount"></param>
        /// <param name="RecordCount"></param>
        /// <returns></returns>
        public IQueryable<string> Query(int PageIndex, int PageSize, string TypeCode, out int PageCount, out int RecordCount)
        {
            Init();
            MethodName = "";
            try
            {
                System.Reflection.MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            IQueryable<DictInfo> ls = null; 
            if (string.IsNullOrEmpty(TypeCode))
                ls = context.GetTable<DictInfo>().Where(c=>c.language==this.language);
            else
                ls = context.GetTable<DictInfo>().Where(c =>c.language==this.language &&  c.Name.ToUpper().Contains(TypeCode.ToUpper()));
            var q = from p in ls group p by p.Name into g select new { g.Key };
            List<string> res = new List<string>();
            foreach (var tmp in q)
                res.Add(tmp.Key);
            RecordCount = res.Count();
            if (PageSize < 0)
            { PageCount = 1; }
            else
            {
                if (RecordCount % PageSize == 0)
                { PageCount = RecordCount / PageSize; }
                else
                { PageCount = RecordCount / PageSize + 1; }
            }
            return res.Skip(PageSize * (PageIndex - 1))
                      .Take(PageSize)
                      .AsQueryable();
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="TypeCode"></param>
        /// <returns></returns>
        public List<string> List(string TypeCode = "")
        {
            Init();
            MethodName = "";
            try
            {
                System.Reflection.MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            List<DictInfo> ls = null;
            if (string.IsNullOrEmpty(TypeCode))
                ls = context.GetTable<DictInfo>().Where(c => c.language == this.language).ToList();
            else
                ls = context.GetTable<DictInfo>().Where(c => c.language == this.language && c.Name.ToUpper().Contains(TypeCode.ToUpper())).ToList();
            var q = from p in ls group p by p.Name into g select new { g.Key };
            List<string> res = new List<string>();
            foreach (var tmp in q)
                res.Add(tmp.Key);
            return res;
        }

        /// <summary>
        /// 查找字典类型
        /// </summary>
        /// <param name="TypeCode"></param>
        /// <returns></returns>
        public List<DictInfo> Find(string TypeCode)
        {
            Init();
            MethodName = "";
            try
            {
                System.Reflection.MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            if (string.IsNullOrEmpty(TypeCode))
                return new List<DictInfo>(); 
            List<DictInfo> ls = context.GetTable<DictInfo>().Where(c => c.language==this.language &&  c.Name.ToUpper().Contains(TypeCode.ToUpper())).ToList();
            return ls;
        }

        /// <summary>
        /// 查找记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DictInfo Find(int id)
        {
            Init();
            MethodName = "";
            try
            {
                System.Reflection.MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            var res = context.GetTable<DictInfo>().FirstOrDefault(c => c.ID == id);
            return res;
        }
         
        #endregion

        #region UI


        /// <summary>
        /// 绑定数据到CheckBoxList
        /// </summary>
        /// <param name="Class"></param>
        /// <param name="TypeCode"></param>
        [DisplayName("bingCheckButtonList")]
        [Description("绑定数据到CheckBoxList")]
        public void bingCheckButtonList(System.Windows.Forms.CheckedListBox Class, string TypeCode)
        {
            Init();
            MethodName = "";
            try
            {
                System.Reflection.MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            try
            {
                if (Class.Items.Count > 0)
                    Class.Items.Clear();
                List<DictInfo> ls = context.GetTable<DictInfo>().Where(c => c.language == this.language &&  c.Name.ToUpper().Contains(TypeCode.ToUpper()) && c.Enable ).ToList();
                var defaults = ls.FirstOrDefault(c => c.Default);
                Class.DataSource = ls;
                Class.DisplayMember ="Display" ;
                Class.ValueMember = "Value";
                if (defaults != null)
                {
                    var index = Class.Items.IndexOf(defaults.Value);
                    if (index > -1)
                        Class.SetItemChecked(index, true);
                }
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, MethodName, Log_Type.Error, ex.Message);
                if (HasError != null)
                    HasError(ClassName, MethodName, ex);
                else
                    throw ex;
            }
        }

        /// <summary>
        /// 绑定数据到CheckBoxList
        /// </summary>
        /// <param name="Class"></param>
        /// <param name="TypeCode"></param>
        [DisplayName("bindCheckButtonList")]
        [Description("绑定数据到CheckBoxList")]
        public void bindCheckButtonList(CheckBoxList Class, string TypeCode, string TypeName="")
        {
            Init();
            MethodName = "";
            try
            {
                System.Reflection.MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            try
            {
                if (Class.Items.Count > 0)
                    Class.Items.Clear();
                List<DictInfo> ls = context.GetTable<DictInfo>().Where(c => c.language == this.language && c.Name.ToUpper().Contains(TypeCode.ToUpper()) && c.Enable ).ToList();
                var defaults = ls.FirstOrDefault(c => c.Default);
                Class.DataSource = ls;
                Class.DataTextField = "Display";
                Class.DataValueField = "Value";
                Class.DataBind();
                if (defaults == null)
                    Class.Items.Insert(0, new System.Web.UI.WebControls.ListItem(ContorlsMessage.Select(TypeName,language), ""));
                else
                {
                    try
                    { Class.Items.FindByValue(defaults.Value).Selected = true; }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, MethodName, Log_Type.Error, ex.Message);
                if (HasError != null)
                    HasError(ClassName, MethodName, ex);
                else
                    throw ex;
            }

        }

        /// <summary>
        /// 绑定数据到CheckBoxList
        /// </summary>
        /// <param name="Class"></param>
        /// <param name="TypeCode"></param>
        /// <param name="CSS"></param>
        /// <returns></returns>
        [DisplayName("bindCheckButtonList")]
        [Description("绑定数据到CheckBoxList")]
        public string bindCheckButtonList( string Class, string TypeCode,   string CSS = "")
        {
            Init();
            MethodName = "";
            try
            {
                System.Reflection.MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            try
            {
                string HTML = "<div name=\"CheckButtonList_" + Class + "\" id=\"CheckButtonList_" + Class + "\" class=\"" + CSS + "\" ><ul>";
                List<DictInfo> ls = context.GetTable<DictInfo>().Where(c => c.language == this.language && c.Name.ToUpper().Contains(TypeCode.ToUpper()) && c.Enable).ToList();
                var defaults = ls.FirstOrDefault(c => c.Default);
                string defaultValue = defaults == null ? "" : defaults.Value;
                defaultValue = string.IsNullOrEmpty(defaultValue) ? "" : defaultValue;
                if (ls != null)
                {
                    for (int i = 0; i < ls.Count; i++)
                    {
                        DictInfo DIC = ls[i];
                        if (DIC.Value == defaultValue)
                            HTML += "<li>< input name =\"" + Class + "\" id=\"" + Class + "_"+ DIC.ID.ToString() + "\"  type=\"checkbox\" checked=\"checked\"  >" + DIC.Display+ "</li>";
                        else
                            HTML += "<li>< input name =\"" + Class + "\" id=\"" + Class + "_" + DIC.ID.ToString() + "\"  type=\"checkbox\" >" + DIC.Display + "</li>";
                    }
                }
                HTML += "</ul></div>";
                return HTML;
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, MethodName, Log_Type.Error, ex.Message);
                if (HasError != null)
                    HasError(ClassName, MethodName, ex);
                else
                    throw ex;
                return "";
            }
        }

        /// <summary>
        /// 绑定数据到DropDownList
        /// </summary>
        /// <param name="Class"></param>
        /// <param name="TypeCode"></param>
        [DisplayName("bindDropDownList")]
        [Description("绑定数据到DropDownList")]
        public void bindDropDownList(System.Windows.Forms.ComboBox Class, string TypeCode)
        {
            Init();
            MethodName = "";
            try
            {
                System.Reflection.MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            try
            {
                if (Class.Items.Count > 0)
                    Class.Items.Clear();
                List<DictInfo> ls = context.GetTable<DictInfo>().Where(c => c.language == this.language && c.Name.ToUpper().Contains(TypeCode.ToUpper()) && c.Enable).ToList();
                var defaults = ls.FirstOrDefault(c => c.Default);
                string defaultValue = defaults == null ? "" : defaults.Value;
                defaultValue = string.IsNullOrEmpty(defaultValue) ? "" : defaultValue;
                Class.DataSource = ls;
                Class.DisplayMember = "Display";
                Class.ValueMember = "Value";
                if (string.IsNullOrEmpty(defaultValue))
                    Class.Items.Insert(0, new System.Web.UI.WebControls.ListItem(ContorlsMessage.Select("",language), ""));
                else
                {
                    try
                    { Class.SelectedIndex=Class.Items.IndexOf(defaults.Value.ToString()); }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, MethodName, Log_Type.Error, ex.Message);
                if (HasError != null)
                    HasError(ClassName, MethodName, ex);
                else
                    throw ex;
            }
        }

        /// <summary>
        /// 绑定数据到DropDownList
        /// </summary>
        /// <param name="Class"></param>
        /// <param name="TypeCode"></param>
        [DisplayName("bindDropDownList")]
        [Description("绑定数据到DropDownList")]
        public void bindDropDownList( System.Web.UI.WebControls.DropDownList Class, string TypeCode)
        {
            Init();
            MethodName = "";
            try
            {
                System.Reflection.MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            try
            {
                List<DictInfo> ls = context.GetTable<DictInfo>().Where(c => c.language == this.language && c.Name.ToUpper().Contains(TypeCode.ToUpper()) && c.Enable).ToList();
                var defaults = ls.FirstOrDefault(c => c.Default);
                string defaultValue = defaults == null ? "" : defaults.Value;
                defaultValue = string.IsNullOrEmpty(defaultValue) ? "" : defaultValue;
                Class.DataSource = ls;
                Class.DataTextField = "Display";
                Class.DataValueField = "Value"; 
                Class.DataBind();
                if (string.IsNullOrEmpty(defaultValue))
                    Class.Items.Insert(0, new System.Web.UI.WebControls.ListItem(ContorlsMessage.Select("",language), ""));
                else
                {
                    try
                    { Class.Items.FindByValue(defaultValue).Selected = true; }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, MethodName, Log_Type.Error, ex.Message);
                if (HasError != null)
                    HasError(ClassName, MethodName, ex);
                else
                    throw ex;
            }
        }
         
        /// <summary>
        /// 绑定数据到DropDownList
        /// </summary>
        /// <param name="Class"></param>
        /// <param name="TypeCode"></param>
        /// <param name="CSS"></param>
        /// <returns></returns>
        [DisplayName("bindDropDownList")]
        [Description("绑定数据到DropDownList")]
        public string bindDropDownList(string Class, string TypeCode, string CSS = "")
        {
            Init();
            MethodName = "";
            try
            {
                System.Reflection.MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            try
            {
                string HTML = "<select name=\"" + ClassName + "\" id=\"" + ClassName + "\" class=\"" + CSS + "\" >";
                List<DictInfo> ls = context.GetTable<DictInfo>().Where(c => c.language == this.language && c.Name.ToUpper().Contains(TypeCode.ToUpper()) && c.Enable).ToList();
                var defaults = ls.FirstOrDefault(c => c.Default);
                string defaultValue = defaults == null ? "" : defaults.Value;
                defaultValue = string.IsNullOrEmpty(defaultValue) ? "" : defaultValue;
                for (int i = 0; i < ls.Count; i++)
                {
                    DictInfo DIC = ls[i];
                    if (DIC.Value == defaultValue)
                        HTML += "<option value=\"" + DIC.Value.ToString() + "\" selected>" + DIC.Display + "</option>";
                    else
                        HTML += "<option value=\"" + DIC.Value.ToString() + "\" >" + DIC.Display + "</option>";
                }
                if (string.IsNullOrEmpty(defaultValue))
                    HTML += "<option value='' selected>" + ContorlsMessage.Select("",language) + "</option>";
                HTML += "</select>";
                return HTML;
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, MethodName, Log_Type.Error, ex.Message);
                if (HasError != null)
                    HasError(ClassName, MethodName, ex);
                else
                    throw ex;
                return "";
            }

        }
         
        /// <summary>
        /// 绑定数据到RadioButtonList
        /// </summary>
        /// <param name="Class"></param>
        /// <param name="TypeCode"></param>
        [DisplayName("bindRadioButtonList")]
        [Description("绑定数据到RadioButtonList")]
        public void bindRadioButtonList( System.Windows.Forms.Panel   Class, string TypeCode)
        {
            Init();
            MethodName = "";
            try
            {
                System.Reflection.MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            try
            {
                if (Class.Controls.Count > 0)
                    Class.Controls.Clear();
                List<DictInfo> ls = context.GetTable<DictInfo>().Where(c => c.language == this.language && c.Name.ToUpper().Contains(TypeCode.ToUpper()) && c.Enable).ToList();
                var defaults = ls.FirstOrDefault(c => c.Default);
                string defaultValue = defaults == null ? "" : defaults.Value;
                defaultValue = string.IsNullOrEmpty(defaultValue) ? "" : defaultValue;
                for (int i = 0; i < ls.Count; i++)
                {
                    DictInfo DIC = ls[i];
                    System.Windows.Forms.RadioButton raid = new System.Windows.Forms.RadioButton();
                    raid.Name = Class.Name + "_" + i.ToString();
                    raid.Text = DIC.Display;
                    if (!string.IsNullOrEmpty(defaultValue))
                    {
                        if (DIC.Value == defaultValue)
                            raid.Checked = true;
                    }
                    Class.Controls.Add(raid);
                }
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, MethodName, Log_Type.Error, ex.Message);
                if (HasError != null)
                    HasError(ClassName, MethodName, ex);
                else
                    throw ex;
            }
        }

        /// <summary>
        /// 绑定数据到RadioButtonList
        /// </summary>
        /// <param name="Class"></param>
        /// <param name="TypeCode"></param>
        [DisplayName("bindRadioButtonList")]
        [Description("绑定数据到RadioButtonList")]
        public void bindRadioButtonList( System.Web.UI.WebControls.RadioButtonList Class, string TypeCode)
        {
            Init();
            MethodName = "";
            try
            {
                System.Reflection.MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            try
            {
                List<DictInfo> ls = context.GetTable<DictInfo>().Where(c => c.language == this.language && c.Name.ToUpper().Contains(TypeCode.ToUpper()) && c.Enable).ToList();
                var defaults = ls.FirstOrDefault(c => c.Default);
                string defaultValue = defaults == null ? "" : defaults.Value;
                defaultValue = string.IsNullOrEmpty(defaultValue) ? "" : defaultValue;
                Class.DataSource = ls;
                Class.DataTextField = "Display";
                Class.DataValueField = "Value";
                Class.DataBind();
                if (string.IsNullOrEmpty(defaultValue))
                    Class.Items.Insert(0, new System.Web.UI.WebControls.ListItem(ContorlsMessage.Select("",language), ""));
                else
                {
                    try
                    { Class.Items.FindByValue(defaultValue).Selected = true; }
                    catch { }
                } 
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, MethodName, Log_Type.Error, ex.Message);
                if (HasError != null)
                    HasError(ClassName, MethodName, ex);
                else
                    throw ex;
            }

        }

        /// <summary>
        /// 绑定数据到RadioButtonList
        /// </summary>
        /// <param name="Class"></param>
        /// <param name="TypeCode"></param>
        /// <param name="CSS"></param>
        /// <returns></returns>
        [DisplayName("bindRadioButtonList")]
        [Description("绑定数据到RadioButtonList")]
        public string bindRadioButtonList(string Class, string TypeCode, string CSS = "")
        {
            Init();
            MethodName = "";
            try
            {
                System.Reflection.MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            try
            {
                string HTML = "<div name=\"RadioButtonList_" + Class + "\" id=\"RadioButtonList_" + Class + "\" class=\"" + CSS + "\" ><ul>";
                List<DictInfo> ls = context.GetTable<DictInfo>().Where(c => c.language == this.language && c.Name.ToUpper().Contains(TypeCode.ToUpper()) && c.Enable).ToList();
                var defaults = ls.FirstOrDefault(c => c.Default);
                string defaultValue = defaults == null ? "" : defaults.Value;
                defaultValue = string.IsNullOrEmpty(defaultValue) ? "" : defaultValue;
                bool sel = false;
                for (int i = 0; i < ls.Count; i++)
                {
                    DictInfo DIC = ls[i];
                    if (DIC.Value == defaultValue)
                    {
                        sel = true;
                        HTML += " <li><input type=\"radio\" name=\"" + Class + "\" id=\"" + Class + "_" + i.ToString() + "\" value=\"" + DIC.Value + "\" checked=\"checked\" />" + DIC.Display + "</li>";
                    }
                    else
                    {
                        if (i == (ls.Count - 1) && !sel)
                            HTML += " <li><input type=\"radio\" name=\"" + Class + "\" id=\"" + Class + "_" + i.ToString() + "\" value=\"" + DIC.Value + "\" />" + DIC.Display + "</li>";
                        else
                            HTML += " <li><input type=\"radio\" name=\"" + Class + "\" id=\"" + Class + "_" + i.ToString() + "\" value=\"" + DIC.Value + "\" />" + DIC.Display + "</li>";
                    }
                }
                HTML += "</ul></div>";
                return HTML;
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, MethodName, Log_Type.Error, ex.Message);
                if (HasError != null)
                    HasError(ClassName, MethodName, ex);
                else
                    throw ex;
                return "";
            }

        }

        /// <summary>
        /// 绑定数据到ListBox
        /// </summary> 
        /// <param name="Class"></param>
        /// <param name="TypeCode"></param>
        [DisplayName("bindListBox")]
        [Description("绑定数据到ListBox")]
        public void bindListBox(  System.Windows.Forms.ListBox Class, string TypeCode)
        {
            Init();
            MethodName = "";
            try
            {
                System.Reflection.MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            try
            {
                if (Class.Items.Count > 0)
                    Class.Items.Clear();
                List<DictInfo> ls = context.GetTable<DictInfo>().Where(c => c.language == this.language && c.Name.ToUpper().Contains(TypeCode.ToUpper()) && c.Enable).ToList();
                var defaults = ls.FirstOrDefault(c => c.Default);
                string defaultValue = defaults == null ? "" : defaults.Value;
                defaultValue = string.IsNullOrEmpty(defaultValue) ? "" : defaultValue;
                Class.DataSource = ls;
                Class.DisplayMember = "Display";
                Class.ValueMember = "Value";
                if (defaultValue != null)
                {
                    var index = Class.Items.IndexOf(defaultValue);
                    if (index > -1)
                        Class.SelectedIndex = index;
                }
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, MethodName, Log_Type.Error, ex.Message);
                if (HasError != null)
                    HasError(ClassName, MethodName, ex);
                else
                    throw ex;
            }
        }

        /// <summary>
        /// 绑定数据到ListBoxt
        /// </summary>
        /// <param name="Class"></param>
        /// <param name="TypeCode"></param>
        [DisplayName("bindListBox")]
        [Description("绑定数据到ListBoxt")]
        public void bindListBox( System.Web.UI.WebControls.ListBox Class, string TypeCode)
        {
            Init();
            MethodName = "";
            try
            {
                System.Reflection.MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            try
            {
                if (Class.Items.Count > 0)
                    Class.Items.Clear();
                List<DictInfo> ls = context.GetTable<DictInfo>().Where(c => c.language == this.language && c.Name.ToUpper().Contains(TypeCode.ToUpper()) && c.Enable).ToList();
                var defaults = ls.FirstOrDefault(c => c.Default);
                string defaultValue = defaults == null ? "" : defaults.Value;
                defaultValue = string.IsNullOrEmpty(defaultValue) ? "" : defaultValue;
                Class.DataSource = ls ;
                Class.DataTextField = "Display";
                Class.DataValueField = "Value";
                Class.DataBind();
                if (string.IsNullOrEmpty(defaultValue))
                    Class.Items.Insert(0, new System.Web.UI.WebControls.ListItem(ContorlsMessage.Select("",language), ""));
                else
                {
                    try
                    { Class.Items.FindByValue(defaultValue).Selected = true; }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, MethodName, Log_Type.Error, ex.Message);
                if (HasError != null)
                    HasError(ClassName, MethodName, ex);
                else
                    throw ex;
            }
        }

        /// <summary>
        /// 绑定数据到ListBoxt
        /// </summary>
        /// <param name="Class"></param>
        /// <param name="TypeCode"></param>
        /// <param name="CSS"></param>
        /// <returns></returns>
        [DisplayName("bindListBox")]
        [Description("绑定数据到ListBoxt")]
        public string bindListBox(string Class, string TypeCode, string CSS = "")
        {
            Init();
            MethodName = "";
            try
            {
                System.Reflection.MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            try
            {
                string HTML = "<div name=\"ListBoxt_" + Class + "\" id=\"ListBoxt_" + Class + "\" class=\"" + CSS + "\" ><ul>";
                string selv = "";
                List<DictInfo> ls = context.GetTable<DictInfo>().Where(c => c.language == this.language && c.Name.ToUpper().Contains(TypeCode.ToUpper()) && c.Enable).ToList();
                var defaults = ls.FirstOrDefault(c => c.Default);
                string defaultValue = defaults == null ? "" : defaults.Value;
                defaultValue = string.IsNullOrEmpty(defaultValue) ? "" : defaultValue;
                for (int i = 0; i < ls.Count; i++)
                {
                    DictInfo DIC = ls[i];
                    if (!string.IsNullOrEmpty(defaultValue))
                    {
                        if (DIC.Value== defaultValue)
                            selv = DIC.Value;
                    }
                    HTML += "<li onclick=\"javascript:document.getElementById(\"" + Class + "\").value='" + DIC.Value + "';\">" + DIC.Display + "</li>";

                }
                HTML += "</ul><input type=\"hidden\" name=\"" + Class + "\" id=\"" + Class + "\" value=\"" + selv + "\" /></div>";
                return HTML;
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, MethodName, Log_Type.Error, ex.Message);
                if (HasError != null)
                    HasError(ClassName, MethodName, ex);
                else
                    throw ex;
                return "";
            }
        }
         
        #endregion

    }
}
