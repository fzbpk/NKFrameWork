using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.ComponentModel;
using LinqToDB.Mapping;
using NK.ENum;
using NK.Entity;
using NK.Event;
using NK.Interface;
using NK.Message;
namespace NK.Data
{
    /// <summary>
    /// 实体转T-SQL事务
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DisplayName("DBRecovery")]
    [Description("实体转T-SQL事务")]
    public class  DBRecovery<T> : IDisposable where T : class,
                  new()
    {

        #region 定义

        private DBInfo DB = null;
        private bool m_disposed;
        private string ClassName = "";
        private iTransaction DBOper = null;

        #endregion

        #region 构造函数

        /// <summary>
        /// 数据库T-SQL基本操作
        /// </summary>
        public DBRecovery()
        {

            ClassName = this.GetType().ToString();
            this.language = Language.Chinese;
        }

        /// <summary>
        /// 数据库T-SQL基本操作
        /// </summary>
        /// <param name="info"></param>
        public DBRecovery(DBInfo info = null)
        {
            DB = info;
            if (DB != null)
            {
                this.DataBaseType = info.Mode;
                if (string.IsNullOrEmpty(DB.ConnStr))
                    this.Connection = DB.ConnectionString();
                else
                    this.Connection = DB.ConnStr;
                this.Timeout = info.TimeOut;
            }
            else
            {
                DB = new DBInfo();
                DB.Mode = DBType.None;
                DB.ConnStr = "";
                DB.TimeOut = 60;
                this.DataBaseType = DBType.None;
                this.Connection = "";
                this.Timeout = 60;
            }
            ClassName = this.GetType().ToString();
            this.language = Language.Chinese;
        }

        /// <summary>
        /// 数据库T-SQL基本操作
        /// </summary>
        /// <param name="ConnectionType">数据库类型</param>
        /// <param name="ConnectionString">连接串</param>
        /// <param name="Timeout">超时时间，毫秒</param>
        public DBRecovery(DBType ConnectionType, string ConnectionString, int Timeout = 60)
        {
            this.Connection = ConnectionString;
            this.DataBaseType = ConnectionType;
            this.Timeout = Timeout;
           
            ClassName = this.GetType().ToString();
            this.language = Language.Chinese;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        ~DBRecovery()
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

        /// <summary>
        /// 释放连接
        /// </summary>
        /// <param name="disposing">是否释放</param>
        protected virtual void Dispose(bool disposing)
        {
            lock (this)
            {
                if (disposing && !m_disposed)
                {
                    try
                    {
                        if (DBOper != null)
                        {
                            DBOper.SaveChange();
                            DBOper.Dispose();
                            DBOper = null;
                        }
                    }
                    catch { }
                    m_disposed = true;
                }
            }
        }


        #endregion

        #region 属性

        /// <summary>
        /// 数据库参数
        /// </summary>
        [DisplayName("DataBase")]
        [Description("数据库参数")]
        public DBInfo DataBase
        {
            get { return DB; }
            set
            {
                DB = value;
                if (DB != null)
                {
                    this.DataBaseType = DB.Mode;
                    if (string.IsNullOrEmpty(DB.ConnStr))
                        this.Connection = DB.ConnectionString();
                    else
                        this.Connection = DB.ConnStr;
                    this.Timeout = DB.TimeOut;
                }
                else
                {
                    this.DataBaseType = DBType.None;
                    this.Connection = "";
                    this.Timeout = 60;
                    DB = new DBInfo();
                    DB.Mode = DBType.None;
                    DB.ConnStr = "";
                    DB.TimeOut = 60;
                }
            }
        }
        /// <summary>
        /// 数据库连接类型
        /// </summary>
        [DisplayName("DataBaseType")]
        [Description("数据库连接类型")]
        public DBType DataBaseType { get; set; }

        /// <summary>
        /// 数据库连接串
        /// </summary>
        [DisplayName("Connection")]
        [Description("数据库连接串")]
        public string Connection { get; set; }

        /// <summary>
        /// 数据库操作超时时间
        /// </summary>
        [DisplayName("Timeout")]
        [Description("数据库操作超时时间")]
        public int Timeout { get; set; }
        /// <summary>
        /// 显示语言
        /// </summary>
        [DisplayName("language")]
        [Description("显示语言")]
        public Language language { get; set; }
        /// <summary>
        /// 实体类型
        /// </summary>
        [DisplayName("EntityType")]
        [Description("实体类型")]
        public Type EntityType
        {
            get
            {
                T org = new T();
                return org.GetType();
            }
        }
        #endregion

        #region 私有方法

        private IList<T> DataTableToEntity(DataTable DT)
        {
            IList<T> entities = new List<T>();
            if (DT != null)
            {
                foreach (DataRow row in DT.Rows)
                {
                    T entity = new T();
                    foreach (var item in entity.GetType().GetProperties())
                    {
                        Type PT = item.PropertyType;
                        ColumnAttribute[] ColumnAttributes = (ColumnAttribute[])item.GetCustomAttributes(typeof(ColumnAttribute), false);
                        string ColumnName = (ColumnAttributes == null ? "" : (ColumnAttributes.Length > 0 ? ColumnAttributes.ToList().First().Name.Trim() : ""));
                        if (ColumnName.Trim() != "")
                        {
                            if (PT.IsEnum)
                            {
                                if (row[ColumnName] is DBNull)
                                    continue;
                                else
                                {
                                    int val = int.Parse(row[ColumnName].ToString());
                                    var em = Enum.ToObject(PT, val);
                                    item.SetValue(entity, em, null);
                                }
                            }
                            else if (PT == typeof(byte[]))
                            {
                                if (row[ColumnName] is DBNull)
                                    item.SetValue(entity, null, null);
                                else
                                {
                                    if (!string.IsNullOrEmpty(row[ColumnName].ToString()))
                                    {
                                        byte[] val = Convert.FromBase64String(row[ColumnName].ToString());
                                        item.SetValue(entity, val, null);
                                    }
                                    else
                                        item.SetValue(entity, null, null);

                                }

                            }
                            else if (PT == typeof(object))
                            {
                                if (row[ColumnName] is DBNull)
                                    item.SetValue(entity, null, null);
                                else
                                {

                                }
                            }
                            else if (row[ColumnName] is DBNull)
                                item.SetValue(entity, null, null);
                            else
                                item.SetValue(entity, Convert.ChangeType(row[ColumnName], item.PropertyType), null);
                        }
                    }
                    entities.Add(entity);
                }
            }
            return entities;
        }

        private T DataRowToEntity(DataRow DR)
        {
            T org = new T();
            if (DR != null)
            {
                foreach (var item in org.GetType().GetProperties())
                {
                    Type PT = item.PropertyType;
                    ColumnAttribute[] ColumnAttributes = (ColumnAttribute[])item.GetCustomAttributes(typeof(ColumnAttribute), false);
                    string ColumnName = (ColumnAttributes == null ? "" : (ColumnAttributes.Length > 0 ? ColumnAttributes.ToList().First().Name.Trim() : ""));
                    if (ColumnName.Trim() != "")
                    {
                        if (PT.IsEnum)
                        {
                            if (DR[ColumnName] is DBNull)
                                continue;
                            else
                            {
                                int val = int.Parse(DR[ColumnName].ToString());
                                var em = Enum.ToObject(PT, val);
                                item.SetValue(org, em, null);
                            }
                        }
                        else if (PT == typeof(byte[]))
                        {
                            if (DR[ColumnName] is DBNull)
                                item.SetValue(org, null, null);
                            else
                            {
                                if (!string.IsNullOrEmpty(DR[ColumnName].ToString()))
                                {
                                    byte[] val = Convert.FromBase64String(DR[ColumnName].ToString());
                                    item.SetValue(org, val, null);
                                }
                                else
                                    item.SetValue(org, null, null);

                            }

                        }
                        else if (PT == typeof(object))
                        {
                            if (DR[ColumnName] is DBNull)
                                item.SetValue(org, null, null);
                            else
                            {

                            }
                        }
                        else if (DR[ColumnName] is DBNull)
                            item.SetValue(org, null, null);
                        else
                            item.SetValue(org, Convert.ChangeType(DR[ColumnName], item.PropertyType), null);
                    }
                }
            }
            return org;
        }

        private Dictionary<ColumnAttribute, object> PropertyInfoToCol(T entity)
        {
            Dictionary<ColumnAttribute, object> Col = new Dictionary<ColumnAttribute, object>();
            PropertyInfo[] properties = entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo p in properties)
            {
                if (p != null)
                {
                    ColumnAttribute[] ColumnAttributes = (ColumnAttribute[])p.GetCustomAttributes(typeof(ColumnAttribute), false);
                    string ColumnName = (ColumnAttributes == null ? "" : (ColumnAttributes.Length > 0 ? (ColumnAttributes.ToList().First().Name == null ? "" : ColumnAttributes.ToList().First().Name.Trim()) : ""));
                    if (string.IsNullOrEmpty(ColumnName))
                        ColumnName = p.Name;
                    bool IsPrimaryKey = (ColumnAttributes == null ? false : (ColumnAttributes.Length > 0 ? ColumnAttributes.ToList().First().IsPrimaryKey : false));
                    bool CanBeNull = (ColumnAttributes == null ? false : (ColumnAttributes.Length > 0 ? ColumnAttributes.ToList().First().CanBeNull : false));
                    ColumnAttribute res = new ColumnAttribute();
                    res.Name = ColumnName;
                    res.CanBeNull = CanBeNull;
                    res.IsPrimaryKey = IsPrimaryKey;
                    Type t = p.PropertyType;
                    if (t.SupportedType())
                        Col.Add(res, p.GetValue(entity, null));
                }
            }
            return Col;
        }


        #endregion

        #region 方法

        /// <summary>
        /// 连接属性
        /// </summary>
        [DisplayName("CreateConnection")]
        [Description("连接属性")]
        public virtual DbConnection CreateConnection
        {
            get
            {
                try
                {
                    if (DBOper != null)
                        return DBOper.GetConnection();
                }
                catch (Exception ex)
                {
                    if (HasError != null)
                        HasError(ClassName, "CreateConnection", ex);
                    else
                        throw ex;
                }
                return null;
            }
        }

        /// <summary>
        /// 检测数据库连接
        /// </summary>
        /// <param name="errmsg">错误信息</param>
        /// <returns>连接结果</returns>
        [DisplayName("CheckDataBase")]
        [Description("检测数据库连接")]
        public virtual bool CheckDataBase(out string errmsg)
        {
            try
            {
                if (DBOper == null)
                    throw new Exception(SystemMessage.Badsequencecommands(language));
                errmsg = "";
                bool res = false;
                if (DBOper != null)
                    res = DBOper.CheckConnection(out errmsg);
                return res;
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
        }
        
        /// <summary>
        /// 创建表
        /// </summary>
        /// <returns>执行结果</returns>
        [DisplayName("CreatTable")]
        [Description("创建表")]
        public virtual bool CreatTable()
        {
            try
            {
                if (DBOper == null)
                    throw new Exception(SystemMessage.Badsequencecommands(language));
                T org = new T();
                TableAttribute[] TableAttributes = (TableAttribute[])org.GetType().GetCustomAttributes(typeof(TableAttribute), false);
                string TableName = (TableAttributes == null ? "" : (TableAttributes.Length > 0 ? TableAttributes.ToList().First().Name.Trim() : ""));
                if (string.IsNullOrEmpty(TableName))
                    TableName = org.GetType().Name;
                List<ColumnAttribute> Columns = new List<ColumnAttribute>();
                PropertyInfo[] properties = org.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo p in properties)
                {
                    if (p != null)
                    {
                        Type t = p.PropertyType;
                        ColumnAttribute[] ColumnAttributes = (ColumnAttribute[])p.GetCustomAttributes(typeof(ColumnAttribute), false);
                        ColumnAttribute tmp = new ColumnAttribute();
                        tmp.CanBeNull = (ColumnAttributes == null ? false : (ColumnAttributes.Length > 0 ? ColumnAttributes.ToList().First().CanBeNull : false));
                        if (!tmp.CanBeNull)
                            tmp.CanBeNull = t.Name.ToUpper().Contains("NULL") ? true : false;
                        tmp.Name = (ColumnAttributes == null ? p.Name : (ColumnAttributes.Length > 0 ? ColumnAttributes.ToList().First().Name.Trim() : p.Name));
                        tmp.IsPrimaryKey = (ColumnAttributes == null ? false : (ColumnAttributes.Length > 0 ? ColumnAttributes.ToList().First().IsPrimaryKey : false));
                        tmp.IsIdentity = (ColumnAttributes == null ? false : (ColumnAttributes.Length > 0 ? ColumnAttributes.ToList().First().IsIdentity : false));
                        tmp.DataType = (ColumnAttributes == null ? LinqToDB.DataType.Undefined : (ColumnAttributes.Length > 0 ? ColumnAttributes.ToList().First().DataType : LinqToDB.DataType.Undefined));
                        if (tmp.DataType == LinqToDB.DataType.Undefined)
                            tmp.DataType = t.ToDataType();
                        if (t.SupportedType())
                            Columns.Add(tmp);
                    }
                }
                if (Columns.Where(c => c.IsPrimaryKey).Count() <= 0)
                {
                    var index = Columns.FindIndex(c => c.Name.ToUpper().Contains("ID") || c.Name.ToUpper().Contains("INDEX"));
                    if (index > -1)
                        Columns[index].IsPrimaryKey = true;
                }
                if (Columns.Where(c => c.IsPrimaryKey).Count() <= 0)
                    throw new NullReferenceException(SystemMessage.RefNullOrEmpty("PrimaryKey", language));
                List<string> sqlbat = TableName.CreatToSql(DB.Mode, Columns);
                if (DBOper != null)
                {
                    foreach (var tsql in sqlbat)
                        DBOper.ExecuteNonQuery(tsql);
                    return DBOper.TableIsExist(TableName); ;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                if (HasError != null)
                    HasError(ClassName, "CreatTable", ex);
                else
                    throw ex;
            }
            return false;
        }

        /// <summary>
        /// 修改表格
        /// </summary>
        /// <returns></returns>
        [DisplayName("ModifyTable")]
        [Description("修改表格")]
        public virtual void ModifyTable()
        {
            try
            {
                if (DBOper == null)
                    throw new Exception(SystemMessage.Badsequencecommands(language));
                T org = new T();
                TableAttribute[] TableAttributes = (TableAttribute[])org.GetType().GetCustomAttributes(typeof(TableAttribute), false);
                string TableName = (TableAttributes == null ? "" : (TableAttributes.Length > 0 ? TableAttributes.ToList().First().Name.Trim() : ""));
                if (string.IsNullOrEmpty(TableName))
                    TableName = org.GetType().Name;
                PropertyInfo[] properties = org.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                if (DBOper != null)
                {
                    List<ColumnAttribute> Cols = new List<ColumnAttribute>();
                    List<ColumnAttribute> orgcol = new List<ColumnAttribute>();
                    foreach (PropertyInfo p in properties)
                    {
                        Type t = p.PropertyType;
                        ColumnAttribute tmp = new ColumnAttribute();
                        ColumnAttribute[] ColumnAttributes = (ColumnAttribute[])p.GetCustomAttributes(typeof(ColumnAttribute), false);
                        tmp.CanBeNull = (ColumnAttributes == null ? false : (ColumnAttributes.Length > 0 ? ColumnAttributes.ToList().First().CanBeNull : false));
                        if (!tmp.CanBeNull)
                            tmp.CanBeNull = t.Name.ToUpper().Contains("NULL") ? true : false;
                        tmp.Name = (ColumnAttributes == null ? p.Name : (ColumnAttributes.Length > 0 ? ColumnAttributes.ToList().First().Name.Trim() : p.Name));
                        tmp.IsPrimaryKey = (ColumnAttributes == null ? false : (ColumnAttributes.Length > 0 ? ColumnAttributes.ToList().First().IsPrimaryKey : false));
                        tmp.DataType = (ColumnAttributes == null ? LinqToDB.DataType.Undefined : (ColumnAttributes.Length > 0 ? ColumnAttributes.ToList().First().DataType : LinqToDB.DataType.Undefined));
                        tmp.IsIdentity = (ColumnAttributes == null ? false : (ColumnAttributes.Length > 0 ? ColumnAttributes.ToList().First().IsIdentity : false));
                        if (tmp.DataType == LinqToDB.DataType.Undefined)
                            tmp.DataType = t.ToDataType();
                        if (t.SupportedType())
                            Cols.Add(tmp);
                        string ColumnName = tmp.Name;
                        bool CanBeNull = false, IsPrimaryKey = false;
                        if (DBOper.CheckField(TableName, ColumnName, out t, out CanBeNull, out IsPrimaryKey))
                        {
                            ColumnAttribute orgtmp = new ColumnAttribute();
                            orgtmp.Name = ColumnName;
                            orgtmp.IsPrimaryKey = IsPrimaryKey;
                            orgtmp.CanBeNull = CanBeNull;
                            orgtmp.DataType = t.ToDataType();
                            if (t.SupportedType())
                                orgcol.Add(orgtmp);
                        }
                    }
                    if (Cols.Where(c => c.IsPrimaryKey).Count() <= 0)
                    {
                        var index = Cols.FindIndex(c => c.Name.ToUpper().Contains("ID") || c.Name.ToUpper().Contains("INDEX"));
                        if (index > -1)
                            Cols[index].IsPrimaryKey = true;
                    }
                    if (Cols.Where(c => c.IsPrimaryKey).Count() <= 0)
                        throw new NullReferenceException(SystemMessage.RefNullOrEmpty("PrimaryKey", language));
                    List<string> sqlbat = TableName.ModifyToSql(DB.Mode, Cols, orgcol);
                    foreach (var tmp in sqlbat)
                        DBOper.ExecuteNonQuery(tmp);
                }
            }
            catch (Exception ex)
            {
                if (HasError != null)
                    HasError(ClassName, "ModifyTable", ex);
                else
                    throw ex;
            }
        }

        /// <summary>
        /// 删除表格
        /// </summary>
        /// <returns></returns>
        [DisplayName("DropTable")]
        [Description("删除表格")]
        public virtual void DropTable()
        {
            try
            {
                if (DBOper == null)
                    throw new Exception(SystemMessage.Badsequencecommands(language));
                T org = new T();
                TableAttribute[] TableAttributes = (TableAttribute[])org.GetType().GetCustomAttributes(typeof(TableAttribute), false);
                string TableName = (TableAttributes == null ? "" : (TableAttributes.Length > 0 ? TableAttributes.ToList().First().Name.Trim() : ""));
                if (string.IsNullOrEmpty(TableName))
                    TableName = org.GetType().Name;
                List<string> sqlbat = TableName.DropToSql();
                foreach (var tmp in sqlbat)
                    DBOper.ExecuteNonQuery(tmp);
            }
            catch (Exception ex)
            {
                if (HasError != null)
                    HasError(ClassName, "DropTable", ex);
                else
                    throw ex;
            }
        }

        /// <summary>
        /// 表是否存在
        /// </summary>
        /// <returns>是否存在</returns>
        [DisplayName("TableIsExist")]
        [Description("表是否存在")]
        public virtual bool TableIsExist()
        {
            try
            {
                if (DBOper == null)
                    throw new Exception(SystemMessage.Badsequencecommands(language));
                T org = new T();
                TableAttribute[] TableAttributes = (TableAttribute[])org.GetType().GetCustomAttributes(typeof(TableAttribute), false);
                string TableName = (TableAttributes == null ? "" : (TableAttributes.Length > 0 ? TableAttributes.ToList().First().Name.Trim() : ""));
                if (string.IsNullOrEmpty(TableName))
                    TableName = org.GetType().Name;
                bool res = false;
                if (DBOper != null)
                    res = DBOper.TableIsExist(TableName);
                return res;
            }
            catch
            {
                return false;
            }
        }
        
        /// <summary>
        /// 启用事务
        /// </summary> 
        [DisplayName("Transaction")]
        [Description("启用事务")]
        public virtual void Transaction()
        {
            try
            {
                switch (this.DataBaseType)
                {
                    case DBType.Access:
                        DBOper = new AccessT(Connection, Timeout);
                        break;
                    case DBType.MYSQL:
                        DBOper = new MySQLT(Connection, Timeout);
                        break;
                    case DBType.MSSQL:
                        DBOper = new MSSQLT(Connection, Timeout);
                        break;
                    case DBType.Oracle:
                        DBOper = new OracleT(Connection, Timeout);
                        break;
                    case DBType.SQLite:
                        DBOper = new SQLiteT(Connection, Timeout);
                        break;
                    case DBType.PostgreSQL:
                        DBOper = new PostgreSQLT(Connection, Timeout);
                        break;
                    case DBType.OleDB:
                        DBOper = new OleDbT(Connection, Timeout);
                        break;
                    default: 
                        throw new NotSupportedException(SystemMessage.NotSupported("DBMODE", language));
                }
                if (DBOper != null)
                {
                    if (this.log != null)
                        DBOper.log += this.log;
                    DBOper.Transaction();
                }
            }
            catch (Exception ex)
            {
                if (HasError != null)
                    HasError(ClassName, "Transaction", ex);
                else
                    throw ex;
            } 
        }

        /// <summary>
        /// 提交事务
        /// </summary> 
        [DisplayName("SaveChange")]
        [Description("提交事务")]
        public virtual void SaveChange(bool RollBack=true)
        {
            try
            {
                if (DBOper == null)
                    throw new Exception(SystemMessage.Badsequencecommands(language));
                else
                    DBOper.SaveChange(RollBack);
            }
            catch (Exception ex)
            {
                if (HasError != null)
                    HasError(ClassName, "SaveChange", ex);
                else
                    throw ex;
            }
        }

        /// <summary>
        /// 取消事务
        /// </summary> 
        [DisplayName("Cancel")]
        [Description("取消事务")]
        public virtual void Cancel()
        {
            try
            {
                if (DBOper == null)
                    throw new Exception(SystemMessage.Badsequencecommands(language));
                else
                    DBOper.Cancel();
            }
            catch (Exception ex)
            {
                if (HasError != null)
                    HasError(ClassName, "SaveChange", ex);
                else
                    throw ex;
            }
        }
         
        /// <summary>
        /// 插入实体
        /// </summary>
        /// <param name="entity">实体</param>
        [DisplayName("Insert")]
        [Description("插入实体")]
        public virtual void Insert(T entity)
        {
            try
            {
                if (DBOper == null)
                    throw new Exception(SystemMessage.Badsequencecommands(language));
                TableAttribute[] TableAttributes = (TableAttribute[])entity.GetType().GetCustomAttributes(typeof(TableAttribute), false);
                string TableName = (TableAttributes == null ? "" : (TableAttributes.Length > 0 ? TableAttributes.ToList().First().Name.Trim() : ""));
                if (string.IsNullOrEmpty(TableName))
                    TableName = entity.GetType().Name;
                Dictionary<string, object> ColSVal = new Dictionary<string, object>();
                Dictionary<ColumnAttribute, object> ColS = PropertyInfoToCol(entity);
                if (ColS.Where(c => c.Key.IsPrimaryKey).Count() <= 0)
                {
                    for (int i = 0; i < ColS.Count; i++)
                    {
                        if (ColS.ElementAt(i).Key.Name.ToUpper().Trim() == "ID" || ColS.ElementAt(i).Key.Name.ToUpper().Trim() == "INDEX")
                            ColS.ElementAt(i).Key.IsPrimaryKey = true;
                    }
                }
                foreach (var col in ColS)
                {
                    if (col.Key.IsIdentity)
                        continue;
                    if (col.Key.IsPrimaryKey)
                    {
                        if (col.Value == null)
                            continue;
                        else if (col.Value.GetType().IsValueType)
                        {
                            if (Convert.ToInt64(col.Value) > 0)
                                ColSVal.Add(col.Key.Name, col.Value);
                            else
                                continue;
                        }
                        else
                            continue;
                    }
                    else if (col.Value != null)
                        ColSVal.Add(col.Key.Name, col.Value);
                    else if (col.Key.CanBeNull)
                        ColSVal.Add(col.Key.Name, null);
                    else
                        throw new NullReferenceException(SystemMessage.RefNullOrEmpty(col.Key.Name, language));
                }
                string sql = TableName.InsertToSQL(DataBaseType,ColSVal); 
                if (DBOper != null)
                    DBOper.ExecuteNonQuery(sql); 
            }
            catch (Exception ex)
            {
                if (HasError != null)
                    HasError(ClassName, "Insert", ex);
                else
                    throw ex;
            }
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        [DisplayName("Update")]
        [Description("更新实体")]
        public virtual void Update(T entity)
        {
            try
            {
                if (DBOper == null)
                    throw new Exception(SystemMessage.Badsequencecommands(language));
                TableAttribute[] TableAttributes = (TableAttribute[])entity.GetType().GetCustomAttributes(typeof(TableAttribute), false);
                string TableName = (TableAttributes == null ? "" : (TableAttributes.Length > 0 ? TableAttributes.ToList().First().Name.Trim() : ""));
                if (string.IsNullOrEmpty(TableName))
                    TableName = entity.GetType().Name;
                Dictionary<string, object> ColSVal = new Dictionary<string, object>();
                string where = "";
                Dictionary<ColumnAttribute, object> ColS = PropertyInfoToCol(entity);
                if (ColS.Where(c => c.Key.IsPrimaryKey).Count() <= 0)
                {
                    for (int i = 0; i < ColS.Count; i++)
                    {
                        if (ColS.ElementAt(i).Key.Name.ToUpper().Trim() == "ID" || ColS.ElementAt(i).Key.Name.ToUpper().Trim() == "INDEX")
                            ColS.ElementAt(i).Key.IsPrimaryKey = true;
                    }
                }
                foreach (var col in ColS)
                {
                    if (col.Key.IsPrimaryKey)
                    {
                        if (col.Value == null)
                            throw new NullReferenceException(SystemMessage.RefNullOrEmpty(col.Key.Name, language));
                        where = col.Key.Name.KeyToSql(ExpressionType.Equal, col.Value);
                    }
                    else if (col.Key.IsIdentity)
                        continue;
                    else if (col.Value != null)
                        ColSVal.Add(col.Key.Name, col.Value);
                    else if (col.Key.CanBeNull)
                        ColSVal.Add(col.Key.Name, null);
                    else
                        throw new NullReferenceException(SystemMessage.RefNullOrEmpty(col.Key.Name, language));
                }
                if (string.IsNullOrEmpty(where))
                    throw new NullReferenceException(SystemMessage.RefNullOrEmpty("PriKey", language));
                else
                    where = " where " + where; 
                string SQL = TableName.UpdateToSQL(DataBaseType,ColSVal) + where; 
                if (DBOper != null)
                     DBOper.ExecuteNonQuery(SQL); 
            }
            catch (Exception ex)
            {
                if (HasError != null)
                    HasError(ClassName, "Update", ex);
                else
                    throw ex; 
            }
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity"></param>
        [DisplayName("Delete")]
        [Description("删除实体")]
        public virtual void Delete(T entity)
        {
            try
            {
                if (DBOper == null)
                    throw new Exception(SystemMessage.Badsequencecommands(language));
                TableAttribute[] TableAttributes = (TableAttribute[])entity.GetType().GetCustomAttributes(typeof(TableAttribute), false);
                string TableName = (TableAttributes == null ? "" : (TableAttributes.Length > 0 ? TableAttributes.ToList().First().Name.Trim() : ""));
                if (string.IsNullOrEmpty(TableName))
                    TableName = entity.GetType().Name;
                string where = "";
                Dictionary<ColumnAttribute, object> ColS = PropertyInfoToCol(entity);
                if (ColS.Where(c => c.Key.IsPrimaryKey).Count() <= 0)
                {
                    for (int i = 0; i < ColS.Count; i++)
                    {
                        if (ColS.ElementAt(i).Key.Name.ToUpper().Trim() == "ID" || ColS.ElementAt(i).Key.Name.ToUpper().Trim() == "INDEX")
                            ColS.ElementAt(i).Key.IsPrimaryKey = true;
                    }
                }
                foreach (var p in ColS)
                {
                    if (!p.Key.IsPrimaryKey)
                        continue;
                    if (p.Value != null)
                        where = p.Key.Name.KeyToSql(ExpressionType.Equal, p.Value);
                    else
                        throw new NullReferenceException(SystemMessage.RefNullOrEmpty(p.Key.Name, language));
                }
                if (string.IsNullOrEmpty(where))
                    throw new NullReferenceException(SystemMessage.RefNullOrEmpty("PriKey", language));
                else
                    where = " where " + where; 
                string SQL = TableName.DeleteToSQL() + where;
                if (DBOper != null)
                     DBOper.ExecuteNonQuery(SQL)  ; 
            }
            catch (Exception ex)
            {
                if (HasError != null)
                    HasError(ClassName, "Delete", ex);
                else
                    throw ex; 
            }
        }

        /// <summary>
        /// 查询所有结果
        /// </summary>
        /// <returns>所有结果</returns>
        [DisplayName("GetTable")]
        [Description("查询所有实体")]
        public virtual IList<T> GetTable(Expression<Func<T, bool>> whereLambda = null)
        {
            try
            { 
                if (DBOper == null)
                    throw new Exception(SystemMessage.Badsequencecommands(language));
                string where ="";
                if (whereLambda!=null)
                    where = " where " + whereLambda.WhereToSQL();
                T org = new T();
                TableAttribute[] TableAttributes = (TableAttribute[])org.GetType().GetCustomAttributes(typeof(TableAttribute), false);
                string TableName = (TableAttributes == null ? "" : (TableAttributes.Length > 0 ? TableAttributes.ToList().First().Name.Trim() : ""));
                if (string.IsNullOrEmpty(TableName))
                    TableName = org.GetType().Name;
                DataTable DT = null; 
                if (DBOper != null)
                    DT = DBOper.getDataTable("select * from " + TableName);
                return DataTableToEntity(DT);
            }
            catch (Exception ex)
            {
                if (HasError != null)
                    HasError(ClassName, "GetTable", ex);
                else
                    throw ex;
            }
            return null;
        }

        /// <summary>
        /// 查询所有结果
        /// </summary>
        /// <param name="whereLambda">查询条件</param>
        /// <param name="orderLambda">排序条件</param>
        /// <param name="ASCDESC">顺序倒叙</param>
        /// <returns>所有结果</returns>
        [DisplayName("Query")]
        [Description("查询所有实体")]
        public virtual List<T> Query(Expression<Func<T, bool>> whereLambda=null, Expression<Func<T, object>> orderLambda = null, bool ASCDESC = false)
        {
            try
            {
                if (DBOper == null)
                    throw new Exception(SystemMessage.Badsequencecommands(language));
                string where = "";
                if (whereLambda != null)
                    where = " where " + whereLambda.WhereToSQL();
                string orderby = "";
                if (orderLambda!=null)
                    orderby = " order " + orderLambda.OrderbyToSql() + " " + (ASCDESC ? "DESC" : "");
                T org = new T();
                TableAttribute[] TableAttributes = (TableAttribute[])org.GetType().GetCustomAttributes(typeof(TableAttribute), false);
                string TableName = (TableAttributes == null ? "" : (TableAttributes.Length > 0 ? TableAttributes.ToList().First().Name.Trim() : ""));
                if (string.IsNullOrEmpty(TableName))
                    TableName = org.GetType().Name;
                DataTable DT = null; 
                if (DBOper != null)
                    DT = DBOper.getDataTable("select * from " + TableName + where+ orderby);
                return DataTableToEntity(DT).ToList();
            }
            catch (Exception ex)
            {
                if (HasError != null)
                    HasError(ClassName, "Query", ex);
                else
                    throw ex;
            }
            return null;
        }

        /// <summary>
        /// 查询并分页
        /// </summary>
        /// <param name="PageIndex">当前页</param>
        /// <param name="PageSize">分页大小</param> 
        /// <param name="PageCount">总页数</param>
        /// <param name="RecordCount">总记录数</param>
        /// <param name="whereLambda">查询条件</param>
        /// <param name="orderLambda">排序条件</param>
        /// <param name="ASCDESC">顺序倒叙</param>
        /// <returns>结果</returns>
        [DisplayName("Page")]
        [Description("分页查询所有实体")]
        public virtual List<T> Select(int PageIndex, int PageSize, out int PageCount, out int RecordCount, Expression<Func<T, bool>> whereLambda=null,Expression<Func<T, object>> orderLambda = null, bool ASCDESC = false)
        {
            if (DBOper == null)
                throw new Exception(SystemMessage.Badsequencecommands(language));
            PageCount = 0;
            RecordCount = 0;
            try
            {
                string where = "";
                if (whereLambda != null)
                    where = " where " + whereLambda.WhereToSQL();
                string orderby = "";
                if (orderLambda != null)
                    orderby = " order " + orderLambda.OrderbyToSql() + " " + (ASCDESC ? "DESC" : "");
                T org = new T();
                TableAttribute[] TableAttributes = (TableAttribute[])org.GetType().GetCustomAttributes(typeof(TableAttribute), false);
                string TableName = (TableAttributes == null ? "" : (TableAttributes.Length > 0 ? TableAttributes.ToList().First().Name.Trim() : ""));
                if (string.IsNullOrEmpty(TableName))
                    TableName = org.GetType().Name;
                DataTable DT = null; 
                if (DBOper != null)
                    DT = DBOper.getDataTableByRam(PageIndex, PageSize, "", TableName, where, orderby, "", out RecordCount, out PageCount);
            }
            catch (Exception ex)
            {
                if (HasError != null)
                    HasError(ClassName, "Page", ex);
                else
                    throw ex;
            }
            return null;
        }
         
        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="whereLambda">查询条件</param>
        /// <returns>找到为实体，找不到为NULL</returns>
        [DisplayName("Find")]
        [Description("查找")]
        public virtual T Find(Expression<Func<T, bool>> whereLambda)
        { 
            try
            {
                if (DBOper == null)
                    throw new Exception(SystemMessage.Badsequencecommands(language));
                string where = whereLambda.WhereToSQL();
                if (!string.IsNullOrEmpty(where))
                    where = " where " + where;
                T org = new T();
                TableAttribute[] TableAttributes = (TableAttribute[])org.GetType().GetCustomAttributes(typeof(TableAttribute), false);
                string TableName = (TableAttributes == null ? "" : (TableAttributes.Length > 0 ? TableAttributes.ToList().First().Name.Trim() : ""));
                if (string.IsNullOrEmpty(TableName))
                    TableName = org.GetType().Name;
                PropertyInfo[] properties = org.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                DataTable DT = null; 
                if (DBOper != null)
                    DT = DBOper.getDataTable("select * from " + TableName + " " + where);
                if (DT != null)
                {
                    if (DT.Rows.Count > 0)
                        return DataRowToEntity(DT.Rows[0]);
                }
            }
            catch (Exception ex)
            {
                if (HasError != null)
                    HasError(ClassName, "Find", ex);
                else
                    throw ex;
            }
            return null;
        }
         
        /// <summary>
        /// 数据库Insert,update,delete带返回执行数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>影响数量</returns>
        [DisplayName("ExecuteNonQuery")]
        [Description("数据库Insert,update,delete带返回执行数")]
        public virtual void ExecuteNonQuery(string sql)
        {
            try
            {
                if (DBOper != null)
                    DBOper.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                if (HasError != null)
                    HasError(ClassName, "ExecuteNonQuery", ex);
                else
                    throw ex;
            }
        }

        #endregion

        #region 事件
        /// <summary>
        /// 调试信息
        /// </summary>
        public event CommEvent.LogEven log = null;
        /// <summary>
        /// 错误出现事件，性能参数内DEBUG设置为EVENT有效
        /// </summary>
        public event CommEvent.HasErrorEven HasError = null;

        #endregion

    }
}
