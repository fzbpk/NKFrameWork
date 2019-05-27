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
    /// 实体转T-SQL
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DisplayName("DBQuety")]
    [Description("实体转T-SQL")]
    public class DBQuety<T> : IDisposable where T : class,
        new()
    {
        #region 定义

        private DBInfo DB = null;
        private bool m_disposed;
        private string ClassName = "";

        #endregion

        #region 构造函数

        /// <summary>
        /// 数据库T-SQL基本操作
        /// </summary>
        public DBQuety(DBInfo info = null)
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
        public DBQuety(DBType ConnectionType, string ConnectionString, int Timeout = 60)
        {
            this.Connection = ConnectionString;
            this.DataBaseType = ConnectionType;
            this.Timeout = Timeout;
            DB = new DBInfo();
            DB.Mode = ConnectionType;
            DB.ConnStr = ConnectionString;
            DB.TimeOut = Timeout;
            ClassName = this.GetType().ToString();
            this.language = Language.Chinese;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        ~DBQuety()
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

        #region 方法

        private iDataBase MakeConnection()
        {
            iDataBase DBOper = null;
            switch (this.DataBaseType)
            {
                case DBType.Access:
                    DBOper = new Access(Connection, Timeout);
                    break;
                case DBType.MYSQL:
                    DBOper = new MySql(Connection, Timeout);
                    break;
                case DBType.MSSQL:
                    DBOper = new MSSql(Connection, Timeout);
                    break;
                case DBType.Oracle:
                    DBOper = new Oracle(Connection, Timeout);
                    break;
                case DBType.SQLite:
                    DBOper = new SQLite(Connection, Timeout);
                    break;
                case DBType.PostgreSQL:
                    DBOper = new PostgreSQL(Connection, Timeout);
                    break;
                case DBType.OleDB:
                    DBOper = new OleDb(Connection, Timeout);
                    break;
                default:
                    DBOper = null;
                    break;
            }
            if (DBOper != null)
            {
                if (this.log != null)
                    DBOper.log += this.log;
            }
            return DBOper;
        }

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
                    if (t.IsEnum || t.IsValueType || t == typeof(DateTime) || t == typeof(string) || t == typeof(bool) || t == typeof(byte[]) || t == typeof(char))
                        Col.Add(res, p.GetValue(entity, null));
                }
            }
            return Col;
        }

        /// <summary>
        /// 连接属性
        /// </summary>
        [DisplayName("CreateConnection")]
        [Description("连接属性")]
        public DbConnection CreateConnection
        {
            get
            {
                try
                {
                    iDataBase DBOper = MakeConnection();
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
        /// 插入实体
        /// </summary>
        /// <param name="entity">实体</param>
        [DisplayName("Insert")]
        [Description("插入实体")]
        public virtual bool Insert(T entity)
        {
            try
            {
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
                string sql = TableName.InsertToSQL(ColSVal);
                bool res = false;
                iDataBase DBOper = MakeConnection();
                if (DBOper != null)
                    res = DBOper.ExecuteNonQuery(sql) > 0;
                return res;
            }
            catch (Exception ex)
            {
                if (HasError != null)
                    HasError(ClassName, "Insert", ex);
                else
                    throw ex;
                return false;
            }
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        [DisplayName("Update")]
        [Description("更新实体")]
        public virtual bool Update(T entity)
        {
            try
            {
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
                bool res = false;
                string SQL = TableName.UpdateToSQL(ColSVal) + where;
                iDataBase DBOper = MakeConnection();
                if (DBOper != null)
                    res = DBOper.ExecuteNonQuery(SQL) > 0;
                return res;
            }
            catch (Exception ex)
            {
                if (HasError != null)
                    HasError(ClassName, "Update", ex);
                else
                    throw ex;
                return false;
            }
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity"></param>
        [DisplayName("Delete")]
        [Description("删除实体")]
        public virtual bool Delete(T entity)
        {
            try
            {
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
                bool res = false;
                iDataBase DBOper = MakeConnection();
                string SQL = TableName.DeleteToSQL() + where;
                if (DBOper != null)
                    res = DBOper.ExecuteNonQuery(SQL) > 0;
                return res;
            }
            catch (Exception ex)
            {
                if (HasError != null)
                    HasError(ClassName, "Delete", ex);
                else
                    throw ex;
                return false;
            }
        }

        /// <summary>
        /// 查询所有结果
        /// </summary>
        /// <returns>所有结果</returns>
        [DisplayName("List")]
        [Description("查询所有实体")]
        public IList<T> List()
        {
            try
            {
                T org = new T();
                TableAttribute[] TableAttributes = (TableAttribute[])org.GetType().GetCustomAttributes(typeof(TableAttribute), false);
                string TableName = (TableAttributes == null ? "" : (TableAttributes.Length > 0 ? TableAttributes.ToList().First().Name.Trim() : ""));
                if (string.IsNullOrEmpty(TableName))
                    TableName = org.GetType().Name;
                DataTable DT = null;
                iDataBase DBOper = MakeConnection();
                if (DBOper != null)
                    DT = DBOper.getDataTable("select * from " + TableName);
                return DataTableToEntity(DT);
            }
            catch (Exception ex)
            {
                if (HasError != null)
                    HasError(ClassName, "List", ex);
                else
                    throw ex;
            }
            return null;
        }

        /// <summary>
        /// 查询所有结果
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns>所有结果</returns>
        [DisplayName("List")]
        [Description("查询所有实体")]
        public IList<T> Query(string where)
        {
            try
            { 
                if (!string.IsNullOrEmpty(where))
                    where = " where " + where;
                T org = new T();
                TableAttribute[] TableAttributes = (TableAttribute[])org.GetType().GetCustomAttributes(typeof(TableAttribute), false);
                string TableName = (TableAttributes == null ? "" : (TableAttributes.Length > 0 ? TableAttributes.ToList().First().Name.Trim() : ""));
                if (string.IsNullOrEmpty(TableName))
                    TableName = org.GetType().Name;
                DataTable DT = null;
                iDataBase DBOper = MakeConnection();
                if (DBOper != null)
                    DT = DBOper.getDataTable("select * from " + TableName + where);
                return DataTableToEntity(DT);
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
        /// <param name="where">查询条件</param>
        /// <param name="orderby">排序条件</param>
        /// <param name="PageCount">总页数</param>
        /// <param name="RecordCount">总记录数</param>
        /// <param name="ASCDESC">顺序倒叙</param>
        /// <returns>结果</returns>
        [DisplayName("Page")]
        [Description("分页查询所有实体")]
        public IList<T> Select(int PageIndex, int PageSize,string where, string orderby, out int PageCount, out int RecordCount, bool ASCDESC = false)
        {
            PageCount = 1;
            RecordCount = 0;
            try
            { 
                if (!string.IsNullOrEmpty(where))
                    where = " where " + where; 
                if (!string.IsNullOrEmpty(orderby))
                    orderby = " order " + orderby + " " + (ASCDESC ? "DESC" : "");
                T org = new T();
                TableAttribute[] TableAttributes = (TableAttribute[])org.GetType().GetCustomAttributes(typeof(TableAttribute), false);
                string TableName = (TableAttributes == null ? "" : (TableAttributes.Length > 0 ? TableAttributes.ToList().First().Name.Trim() : ""));
                if (string.IsNullOrEmpty(TableName))
                    TableName = org.GetType().Name;
                DataTable DT = null;
                iDataBase DBOper = MakeConnection();
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
        /// <param name="where">查询条件</param>
        /// <returns>找到为实体，找不到为NULL</returns>
        [DisplayName("Find")]
        [Description("查找")]
        public T Find(string where)
        {

            try
            { 
                if (!string.IsNullOrEmpty(where))
                    where = " where " + where;
                T org = new T();
                TableAttribute[] TableAttributes = (TableAttribute[])org.GetType().GetCustomAttributes(typeof(TableAttribute), false);
                string TableName = (TableAttributes == null ? "" : (TableAttributes.Length > 0 ? TableAttributes.ToList().First().Name.Trim() : ""));
                if (string.IsNullOrEmpty(TableName))
                    TableName = org.GetType().Name;
                PropertyInfo[] properties = org.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                DataTable DT = null;
                iDataBase DBOper = MakeConnection();
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
        /// 创建表
        /// </summary>
        /// <returns>执行结果</returns>
        [DisplayName("CreatTable")]
        [Description("创建表")]
        public bool CreatTable()
        {
            try
            {
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
                        tmp.DataType = (ColumnAttributes == null ? LinqToDB.DataType.Undefined : (ColumnAttributes.Length > 0 ? ColumnAttributes.ToList().First().DataType : LinqToDB.DataType.Undefined));
                        if (tmp.DataType == LinqToDB.DataType.Undefined)
                            tmp.DataType = t.ToDataType();
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
                iDataBase DBOper = MakeConnection();
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
        public bool ModifyTable()
        {
            try
            {
                T org = new T();
                TableAttribute[] TableAttributes = (TableAttribute[])org.GetType().GetCustomAttributes(typeof(TableAttribute), false);
                string TableName = (TableAttributes == null ? "" : (TableAttributes.Length > 0 ? TableAttributes.ToList().First().Name.Trim() : ""));
                if (string.IsNullOrEmpty(TableName))
                    TableName = org.GetType().Name;
                PropertyInfo[] properties = org.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

                int n = 0;
                iDataBase DBOper = MakeConnection();

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
                    {
                        if (DBOper.ExecuteNonQuery(tmp) > 0)
                            n++;
                    }
                }
                return n > 0;
            }
            catch (Exception ex)
            {
                if (HasError != null)
                    HasError(ClassName, "ModifyTable", ex);
                else
                    throw ex;
                return false;
            }
        }

        /// <summary>
        /// 删除表格
        /// </summary>
        /// <returns></returns>
        [DisplayName("DropTable")]
        [Description("删除表格")]
        public bool DropTable()
        {
            try
            {
                T org = new T();
                TableAttribute[] TableAttributes = (TableAttribute[])org.GetType().GetCustomAttributes(typeof(TableAttribute), false);
                string TableName = (TableAttributes == null ? "" : (TableAttributes.Length > 0 ? TableAttributes.ToList().First().Name.Trim() : ""));
                if (string.IsNullOrEmpty(TableName))
                    TableName = org.GetType().Name;
                iDataBase DBOper = MakeConnection();
                string sql = TableName.DropToSql();
                return DBOper.ExecuteNonQuery(sql) > 0;
            }
            catch (Exception ex)
            {
                if (HasError != null)
                    HasError(ClassName, "DropTable", ex);
                else
                    throw ex;
                return false;
            }
        }

        /// <summary>
        /// 表是否存在
        /// </summary>
        /// <returns>是否存在</returns>
        [DisplayName("TableIsExist")]
        [Description("表是否存在")]
        public bool TableIsExist()
        {
            try
            {
                T org = new T();
                TableAttribute[] TableAttributes = (TableAttribute[])org.GetType().GetCustomAttributes(typeof(TableAttribute), false);
                string TableName = (TableAttributes == null ? "" : (TableAttributes.Length > 0 ? TableAttributes.ToList().First().Name.Trim() : ""));
                if (string.IsNullOrEmpty(TableName))
                    TableName = org.GetType().Name;
                bool res = false;
                iDataBase DBOper = MakeConnection();
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
        /// 检测数据库连接
        /// </summary>
        /// <param name="errmsg">错误信息</param>
        /// <returns>连接结果</returns>
        [DisplayName("CheckDataBase")]
        [Description("检测数据库连接")]
        public bool CheckDataBase(out string errmsg)
        {
            try
            {
                errmsg = "";
                bool res = false;
                iDataBase DBOper = MakeConnection();
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
        /// 数据库Insert,update,delete带返回执行数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>影响数量</returns>
        [DisplayName("ExecuteNonQuery")]
        [Description("数据库Insert,update,delete带返回执行数")]
        public int ExecuteNonQuery(string sql)
        {
            int res = -1;
            try
            { 
                iDataBase DBOper = MakeConnection(); 
                return DBOper.ExecuteNonQuery(sql) ;
            }
            catch (Exception ex)
            {
                res = -2;
                if (HasError != null)
                    HasError(ClassName, "ExecuteNonQuery", ex);
                else
                    throw ex; 
            }
            return res;
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
