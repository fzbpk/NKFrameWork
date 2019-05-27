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
using NK.Interface;
using LinqToDB.Mapping;
using System.Reflection;

namespace NK.Data.Manager
{
    /// <summary>
    /// 描述记录
    /// </summary>
    [DisplayName("ColumnManager")]
    [Description("描述记录")]
    public   class ColumnManager : DataHelper, IDisposable
    {
        
        #region 构造函数

        /// <summary>
        ///  Linq数据库处理
        /// </summary>
        public ColumnManager():base()
        {
            this.language = Language.Chinese;
            ClassName = this.GetType().ToString();
        }

        /// <summary>
        ///  Linq数据库处理
        /// </summary>
        /// <param name="info">数据库参数</param>
        public ColumnManager(DBInfo info = null):base(info)
        { 
            this.language = Language.Chinese;
            ClassName = this.GetType().ToString();
        }

        /// <summary>
        /// Linq数据库处理
        /// </summary>
        /// <param name="ConnectionType">数据库类型</param>
        /// <param name="ConnectionString">连接串</param>
        /// <param name="Timeout">超时时间，毫秒</param>
        public ColumnManager(DBType ConnectionType, string ConnectionString, int Timeout = 60):base(ConnectionType, ConnectionString, Timeout)
        { 
            this.language = Language.Chinese;
            ClassName = this.GetType().ToString();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        ~ColumnManager()
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
            List<ColumnAttribute> Columns = EToSqlCreat<DisplayColumnAttribute>(out TableName);
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
            string TableName = Table<DisplayColumnAttribute>();
            List<string> sqlbat = TableName.DropToSql();
            foreach (var tsql in sqlbat)
            {
                if (Execute(tsql) <= 0)
                    throw new Exception(tsql);
            }
        }

        private bool TableIsExist()
        {
            string TableName = Table<DisplayColumnAttribute>();
            if (TableIsExist(TableName))
            {
                try
                {
                    var tmp = context.GetTable<DisplayColumnAttribute>().FirstOrDefault();
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

        private List<DisplayColumnAttribute> Columns(string Name)
        {
            Init();
            List<DisplayColumnAttribute> res = Columns(Name); 
            return res;
        }
         
        #endregion
 
        #region 方法
         
        /// <summary>
        /// 获取字段描述
        /// </summary>
        /// <param name="Name">表或视图名</param>
        /// <returns></returns>
        [DisplayName("GetDispColumn")]
        [Description("获取字段描述")]
        public List<DisplayColumnAttribute> GetDispColumnTable(string Name)
        {
            Init();
            MethodName = "";
            try
            {
                System.Reflection.MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            List<DisplayColumnAttribute> res = new List<DisplayColumnAttribute>();
            if (string.IsNullOrEmpty(Name))
                return res;
            try
            {
                if (TableIsExist(Name))
                {
                    var cols = Columns(Name);
                    var LS = context.GetTable<DisplayColumnAttribute>().Where(c => c.Table.ToUpper().Trim() == Name.ToUpper().Trim() && c.Displaylanguage == this.language).ToList();
                    foreach (var col in cols)
                    {
                        var tmp = LS.FirstOrDefault(c => c.Column.ToUpper().Trim() == col.Column.ToUpper().Trim());
                        if (tmp != null)
                        {
                            col.Table = tmp.Table;
                            col.Column = tmp.Column;
                            col.Name = tmp.Name;
                            col.JS = (string.IsNullOrEmpty(tmp.JS) ? "" : tmp.JS);
                            col.CSS = (string.IsNullOrEmpty(tmp.CSS) ? "" : tmp.CSS);
                            col.Format = (string.IsNullOrEmpty(tmp.Format) ? "" : tmp.Format);
                            col.Unit = (string.IsNullOrEmpty(tmp.Unit) ? "" : tmp.Unit);
                            col.index = tmp.index;
                            col.Seqencing = tmp.Seqencing;
                            col.CanCount = tmp.CanCount;
                            col.CanHead = tmp.CanHead;
                            col.CanSearch = tmp.CanSearch;
                            col.CanImpExp = tmp.CanImpExp;
                            col.IsUnique = tmp.IsUnique;
                            col.CanDeitail = tmp.CanDeitail;
                            col.Caption = tmp.Caption;
                            if (col.CanDeitail)
                            {
                                if (col.Column.ToUpper() == "ID" || col.Column.ToUpper() == "INDEX")
                                    col.CanDeitail = false;
                            }
                            col.Displaylanguage = tmp.Displaylanguage;
                        }
                        res.Add(col);
                    }
                }
            }
            catch (Exception ex)
            { 
                if (HasError != null)
                    HasError(ClassName, MethodName, ex);
                else
                    throw ex; 
            }
            return res;
        }

        /// <summary>
        /// 设置字段描述
        /// </summary>
        /// <param name="Name">表或视图名称</param>
        ///  <param name="Column">修改的字段</param>
        /// <returns></returns>
        [DisplayName("SetDispColumn")]
        [Description("设置字段描述")]
        public bool SetDispColumnTable(string Name, List<DisplayColumnAttribute> Column)
        {
            if (string.IsNullOrEmpty(Name))
                return false;
            else if (Column == null)
                return false;
            else if (Column.Count <= 0)
                return false;
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
                foreach (var col in Column.Where(c => c.Table.ToUpper().Trim() == Name.ToUpper().Trim() && c.Displaylanguage==this.language))
                {
                    var tmp = context.GetTable<DisplayColumnAttribute>().FirstOrDefault(c => c.Table.ToUpper().Trim() == Name.ToUpper().Trim() && col.Column.ToUpper().Trim() == c.Column.ToUpper().Trim() && c.Displaylanguage==this.language);
                    if (tmp != null)
                    {
                        tmp.Table = col.Table;
                        tmp.Column = col.Column;
                        tmp.Name = col.Name;
                        tmp.JS = (string.IsNullOrEmpty(col.JS) ? "" : col.JS);
                        tmp.CSS = (string.IsNullOrEmpty(col.CSS) ? "" : col.CSS);
                        tmp.Format = (string.IsNullOrEmpty(col.Format) ? "" : col.Format);
                        tmp.Unit = (string.IsNullOrEmpty(col.Unit) ? "" : col.Unit);
                        tmp.Seqencing = col.Seqencing;
                        tmp.CanCount = col.CanCount;
                        tmp.CanHead = col.CanHead;
                        tmp.CanSearch = col.CanSearch;
                        tmp.CanImpExp = col.CanImpExp;
                        tmp.IsUnique = col.IsUnique;
                        tmp.CanDeitail = col.CanDeitail;
                        tmp.Displaylanguage = col.Displaylanguage;
                        tmp.Caption = col.Caption;
                        context.Update(tmp);
                    }
                    else
                    {
                        tmp=new DisplayColumnAttribute();
                        tmp.Table = col.Table;
                        tmp.Column = col.Column;
                        tmp.Name = col.Name;
                        tmp.JS = (string.IsNullOrEmpty(col.JS) ? "" : col.JS);
                        tmp.CSS = (string.IsNullOrEmpty(col.CSS) ? "" : col.CSS);
                        tmp.Format = (string.IsNullOrEmpty(col.Format) ? "" : col.Format);
                        tmp.Unit = (string.IsNullOrEmpty(col.Unit) ? "" : col.Unit);
                        tmp.Seqencing = col.Seqencing;
                        tmp.CanCount = col.CanCount;
                        tmp.CanHead = col.CanHead;
                        tmp.CanSearch = col.CanSearch;
                        tmp.CanImpExp = col.CanImpExp;
                        tmp.IsUnique = col.IsUnique;
                        tmp.CanDeitail = col.CanDeitail;
                        tmp.Caption = col.Caption;
                        tmp.Displaylanguage = col.Displaylanguage;
                        context.Insert(col);
                    }
                }
                return true; 
            }
            catch (Exception ex)
            { 
                if (HasError != null)
                    HasError(ClassName, MethodName, ex);
                else
                    throw ex;
                return false;
            }
        }

        /// <summary>
        /// 删除设置
        /// </summary>
        /// <param name="Name">表或视图名称</param>
        /// <returns></returns>
        [DisplayName("DelDispColumn")]
        [Description("删除设置")]
        public bool DelDispColumnTable(string Name)
        { 
            if (string.IsNullOrEmpty(Name))
                return false;
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
                var LS = context.GetTable<DisplayColumnAttribute>().Where(c => c.Table.ToUpper().Trim() == Name.ToUpper().Trim() && c.Displaylanguage==this.language).ToList();
                foreach (var col in LS)
                    context.Delete(col);
                return true;
            }
            catch (Exception ex)
            { 
                if (HasError != null)
                    HasError(ClassName, MethodName, ex);
                else
                    throw ex;
                return false;
            }
        }

        /// <summary>
        /// 实体显示属性
        /// </summary>
        /// <param name="Class"></param>
        /// <returns></returns>
        [DisplayName("GetDisplayColumnEntity")]
        [Description("实体显示属性")]
        public List<DisplayColumnAttribute> GetDisplayColumnEntity(Type Class)
        {
            List<DisplayColumnAttribute> res = new List<DisplayColumnAttribute>();
            string TableName = Class.FullName;
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
                int n = 0;
                PropertyInfo[] properties = Class.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo p in properties)
                {
                    string Column = "";
                    ColumnAttribute[] ColumnAttributes = (ColumnAttribute[])p.GetCustomAttributes(typeof(ColumnAttribute), false);
                    Column = (ColumnAttributes == null ? p.Name : (ColumnAttributes.Length > 0 ? ColumnAttributes.ToList().First().Name.Trim() : p.Name));
                    DisplayColumnAttribute disp = context.GetTable<DisplayColumnAttribute>().FirstOrDefault(c =>c.Table.ToLower().Trim() == TableName.ToLower().Trim() && c.Column.ToLower().Trim()== Column.ToLower().Trim() && c.Displaylanguage==this.language);
                    if (disp == null)
                    {
                        disp = new DisplayColumnAttribute();
                        disp.Table = TableName;
                        disp.Column = Column;
                        disp.Name = p.Name;
                        disp.JS = "";
                        disp.CSS = "";
                        disp.Format = "";
                        disp.Unit = "";
                        disp.Caption = "";
                        disp.index = n;
                        disp.Seqencing = n;
                        disp.CanCount = false;
                        disp.CanHead = true;
                        disp.CanSearch = true;
                        disp.CanImpExp = false;
                        disp.CanDeitail = false;
                        disp.IsUnique = false;
                        List<DisplayColumnAttribute> EnumAttributes = ((DisplayColumnAttribute[])p.GetCustomAttributes(typeof(DisplayColumnAttribute), false)).ToList();
                        var ens = EnumAttributes.FirstOrDefault(c => c.Displaylanguage == this.language);
                        if (ens == null && EnumAttributes.Count > 0)
                            ens = EnumAttributes[0];
                        if (ens != null)
                        {
                            if (string.IsNullOrEmpty(ens.Name))
                            {
                                List<DescriptionAttribute> Attributes = ((DescriptionAttribute[])p.GetCustomAttributes(typeof(DescriptionAttribute), false)).ToList();
                                disp.Name = (Attributes.Count() > 0 ? (string.IsNullOrEmpty(Attributes[0].Description) ? p.Name : Attributes[0].Description) : p.Name);
                            }
                            else
                                disp.Name = ens.Name;
                            disp.JS = (string.IsNullOrEmpty(ens.JS) ? "" : ens.JS);
                            disp.CSS = (string.IsNullOrEmpty(ens.CSS) ? "" : ens.CSS);
                            disp.Format = (string.IsNullOrEmpty(ens.Format) ? "" : ens.Format);
                            disp.Unit = (string.IsNullOrEmpty(ens.Unit) ? "" : ens.Unit);
                            disp.index = ens.index;
                            disp.Seqencing = ens.Seqencing;
                            disp.CanCount = ens.CanCount;
                            disp.CanHead = ens.CanHead;
                            disp.CanSearch = ens.CanSearch;
                            disp.CanImpExp = ens.CanImpExp;
                            disp.CanDeitail = ens.CanDeitail;
                            disp.Caption = ens.Caption;
                            disp.IsUnique = ens.IsUnique;
                        } 
                    } 
                    if (disp.CanDeitail)
                    {
                        if (p.Name.ToUpper() == "ID" || p.Name.ToUpper() == "INDEX")
                            disp.CanDeitail = false;
                    }
                    res.Add(disp);
                }
            }
            catch (Exception ex)
            {
                if (HasError != null)
                    HasError(ClassName, MethodName, ex);
                else
                    throw ex; 
            } 
            return res;
        }

        /// <summary>
        /// 实体显示属性
        /// </summary>
        /// <param name="Class"></param>
        /// <param name="Entity"></param>
        /// <returns></returns>
        [DisplayName("GetDisplayColumnEntity")]
        [Description("实体显示属性")]
        public Dictionary<DisplayColumnAttribute, object> GetDisplayColumnEntity(Type Class ,object Entity)
        {
            Dictionary<DisplayColumnAttribute, object> res = new Dictionary<DisplayColumnAttribute, object>();
            List<DisplayColumnAttribute> Column = GetDisplayColumnEntity(Class);
            if (Column == null) Column = new List<DisplayColumnAttribute>();
            Dictionary<string, object> Value = new Dictionary<string, object>();
            MethodName = "";
            try
            {
                System.Reflection.MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            try
            {
                if (Entity != null)
                {
                    PropertyInfo[] properties = Entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    foreach (PropertyInfo p in properties)
                    {
                        ColumnAttribute[] ColumnAttributes = (ColumnAttribute[])p.GetCustomAttributes(typeof(ColumnAttribute), false);
                        string ColumnName = (ColumnAttributes == null ? p.Name : (ColumnAttributes.Length > 0 ? ColumnAttributes.ToList().First().Name.Trim() : p.Name));
                        Type t = p.PropertyType;
                        if (t.IsValueType || t.IsEnum || t == typeof(string) || t == typeof(bool) || t == typeof(DateTime) || t.IsArray)
                        {
                            if (p.GetValue(Entity, null) != null)
                                Value.Add(ColumnName, p.GetValue(Entity, null));
                            else
                                Value.Add(ColumnName, null);
                        }
                    } 
                }
                foreach (var col in Column)
                {
                    if (Value.Where(c => c.Key == col.Column).Count() > 0)
                    {
                        var dic = Value.FirstOrDefault(c => c.Key == col.Name);
                        res.Add(col, dic.Value);
                    }
                    else
                        res.Add(col, null);
                }
            }
            catch (Exception ex)
            {
                if (HasError != null)
                    HasError(ClassName, MethodName, ex);
                else
                    throw ex;
            } 
            return res; 
        }

        /// <summary>
        /// 设置字段描述
        /// </summary>
        /// <param name="Class">实体类</param>
        /// <param name="Column">描述</param>
        /// <returns></returns>
        [DisplayName("SetDisplayColumnEntity")]
        [Description("设置字段描述")]
        public bool SetDisplayColumnEntity(Type Class, List<DisplayColumnAttribute> Column)
        {
             if (Column == null)
                return false;
            else if (Column.Count <= 0)
                return false;
            Init();
            string TableName = Class.FullName;
            MethodName = "";
            try
            {
                System.Reflection.MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { } 
            try
            {
                foreach (var col in Column.Where(c => c.Table.ToUpper().Trim() == TableName.ToUpper().Trim() && c.Displaylanguage == this.language))
                {
                    var tmp = context.GetTable<DisplayColumnAttribute>().FirstOrDefault(c => c.Table.ToUpper().Trim() == TableName.ToUpper().Trim() && col.Column.ToUpper().Trim() == c.Column.ToUpper().Trim() && c.Displaylanguage == this.language);
                    if (tmp != null)
                    {
                        tmp.Table = col.Table;
                        tmp.Column = col.Column;
                        tmp.Name = col.Name;
                        tmp.JS = (string.IsNullOrEmpty(col.JS) ? "" : col.JS);
                        tmp.CSS = (string.IsNullOrEmpty(col.CSS) ? "" : col.CSS);
                        tmp.Format = (string.IsNullOrEmpty(col.Format) ? "" : col.Format);
                        tmp.Unit = (string.IsNullOrEmpty(col.Unit) ? "" : col.Unit);
                        tmp.Seqencing = col.Seqencing;
                        tmp.CanCount = col.CanCount;
                        tmp.CanHead = col.CanHead;
                        tmp.CanSearch = col.CanSearch;
                        tmp.CanImpExp = col.CanImpExp;
                        tmp.IsUnique = col.IsUnique;
                        tmp.CanDeitail = col.CanDeitail;
                        tmp.Displaylanguage = col.Displaylanguage;
                        tmp.Caption = col.Caption;
                        context.Update(tmp);
                    }
                    else
                    {
                        tmp = new DisplayColumnAttribute();
                        tmp.Table = col.Table;
                        tmp.Column = col.Column;
                        tmp.Name = col.Name;
                        tmp.JS = (string.IsNullOrEmpty(col.JS) ? "" : col.JS);
                        tmp.CSS = (string.IsNullOrEmpty(col.CSS) ? "" : col.CSS);
                        tmp.Format = (string.IsNullOrEmpty(col.Format) ? "" : col.Format);
                        tmp.Unit = (string.IsNullOrEmpty(col.Unit) ? "" : col.Unit);
                        tmp.Seqencing = col.Seqencing;
                        tmp.CanCount = col.CanCount;
                        tmp.CanHead = col.CanHead;
                        tmp.CanSearch = col.CanSearch;
                        tmp.CanImpExp = col.CanImpExp;
                        tmp.IsUnique = col.IsUnique;
                        tmp.CanDeitail = col.CanDeitail;
                        tmp.Caption = col.Caption;
                        tmp.Displaylanguage = col.Displaylanguage;
                        context.Insert(col);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                if (HasError != null)
                    HasError(ClassName, MethodName, ex);
                else
                    throw ex;
                return false;
            }
        }

        /// <summary>
        /// 删除实体设置
        /// </summary>
        /// <param name="Class">表或视图名称</param>
        /// <returns></returns>
        [DisplayName("DelDispColumnEntity")]
        [Description("删除实体设置")]
        public bool DelDispColumnEntity(Type Class)
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
                string TableName = Class.FullName;
                var LS = context.GetTable<DisplayColumnAttribute>().Where(c => c.Table.ToUpper().Trim() == TableName.ToUpper().Trim() && c.Displaylanguage == this.language).ToList();
                foreach (var col in LS)
                    context.Delete(col);
                return true;
            }
            catch (Exception ex)
            {
                if (HasError != null)
                    HasError(ClassName, MethodName, ex);
                else
                    throw ex;
                return false;
            }
        }

        /// 获取字段描述
        /// </summary>
        /// <param name="Name">表或视图名</param>
        /// <returns></returns>
        [DisplayName("GetDispColumnUI")]
        [Description("获取字段描述")]
        public List<DisplayColumnAttribute> GetDispColumnUI(string Name)
        {
            Init();
            MethodName = "";
            try
            {
                System.Reflection.MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            List<DisplayColumnAttribute> res = new List<DisplayColumnAttribute>();
            if (string.IsNullOrEmpty(Name))
                return res;
            try
            {
                res = context.GetTable<DisplayColumnAttribute>().Where(c => c.Table.ToUpper().Trim() == Name.ToUpper().Trim() && c.Displaylanguage == this.language).ToList(); 
            }
            catch (Exception ex)
            {
                if (HasError != null)
                    HasError(ClassName, MethodName, ex);
                else
                    throw ex;
            }
            return res;
        }

        /// <summary>
        /// 设置字段描述
        /// </summary>
        /// <param name="Name">表或视图名称</param>
        ///  <param name="Column">修改的字段</param>
        /// <returns></returns>
        [DisplayName("SetDispColumnUI")]
        [Description("设置字段描述")]
        public bool SetDispColumnUI(string Name, List<DisplayColumnAttribute> Column)
        {
            if (string.IsNullOrEmpty(Name))
                return false;
            else if (Column == null)
                return false;
            else if (Column.Count <= 0)
                return false;
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
                foreach (var col in Column.Where(c => c.Table.ToUpper().Trim() == Name.ToUpper().Trim() && c.Displaylanguage == this.language))
                {
                    var tmp = context.GetTable<DisplayColumnAttribute>().FirstOrDefault(c => c.Table.ToUpper().Trim() == Name.ToUpper().Trim() && col.Column.ToUpper().Trim() == c.Column.ToUpper().Trim() && c.Displaylanguage == this.language);
                    if (tmp != null)
                    {
                        tmp.Table = col.Table;
                        tmp.Column = col.Column;
                        tmp.Name = col.Name;
                        tmp.JS = (string.IsNullOrEmpty(col.JS) ? "" : col.JS);
                        tmp.CSS = (string.IsNullOrEmpty(col.CSS) ? "" : col.CSS);
                        tmp.Format = (string.IsNullOrEmpty(col.Format) ? "" : col.Format);
                        tmp.Unit = (string.IsNullOrEmpty(col.Unit) ? "" : col.Unit);
                        tmp.Seqencing = col.Seqencing;
                        tmp.CanCount = col.CanCount;
                        tmp.CanHead = col.CanHead;
                        tmp.CanSearch = col.CanSearch;
                        tmp.CanImpExp = col.CanImpExp;
                        tmp.IsUnique = col.IsUnique;
                        tmp.CanDeitail = col.CanDeitail;
                        tmp.Displaylanguage = col.Displaylanguage;
                        tmp.Caption = col.Caption;
                        context.Update(tmp);
                    }
                    else
                    {
                        tmp = new DisplayColumnAttribute();
                        tmp.Table = col.Table;
                        tmp.Column = col.Column;
                        tmp.Name = col.Name;
                        tmp.JS = (string.IsNullOrEmpty(col.JS) ? "" : col.JS);
                        tmp.CSS = (string.IsNullOrEmpty(col.CSS) ? "" : col.CSS);
                        tmp.Format = (string.IsNullOrEmpty(col.Format) ? "" : col.Format);
                        tmp.Unit = (string.IsNullOrEmpty(col.Unit) ? "" : col.Unit);
                        tmp.Seqencing = col.Seqencing;
                        tmp.CanCount = col.CanCount;
                        tmp.CanHead = col.CanHead;
                        tmp.CanSearch = col.CanSearch;
                        tmp.CanImpExp = col.CanImpExp;
                        tmp.IsUnique = col.IsUnique;
                        tmp.CanDeitail = col.CanDeitail;
                        tmp.Caption = col.Caption;
                        tmp.Displaylanguage = col.Displaylanguage;
                        context.Insert(col);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                if (HasError != null)
                    HasError(ClassName, MethodName, ex);
                else
                    throw ex;
                return false;
            }
        }

        /// <summary>
        /// 删除设置
        /// </summary>
        /// <param name="Name">表或视图名称</param>
        /// <returns></returns>
        [DisplayName("DelDispColumnUI")]
        [Description("删除设置")]
        public bool DelDispColumnUI(string Name)
        {
            if (string.IsNullOrEmpty(Name))
                return false;
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
                var LS = context.GetTable<DisplayColumnAttribute>().Where(c => c.Table.ToUpper().Trim() == Name.ToUpper().Trim() && c.Displaylanguage == this.language).ToList();
                foreach (var col in LS)
                    context.Delete(col);
                return true;
            }
            catch (Exception ex)
            {
                if (HasError != null)
                    HasError(ClassName, MethodName, ex);
                else
                    throw ex;
                return false;
            }
        }
         
        #endregion

    }
}
