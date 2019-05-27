using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data;
using System.Data.Common;
using LinqToDB;
using System.ComponentModel;
using LinqToDB.Mapping;
using NK.ENum;
using NK.Entity;
using NK.Event;
using NK.Message;
using System.Reflection;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data.Odbc;
using System.Data.SQLite;
using Oracle.ManagedDataAccess.Client;
using MySql.Data.MySqlClient;
using Npgsql;
using NK.Attribut;
using NpgsqlTypes;

namespace NK.Data
{ 
    /// <summary>
    /// Linq处理基类
    /// </summary>
    public class DataHelper
    {
        #region 定义
        protected  DBInfo DB = new DBInfo();
        protected DataContext context = null;
        protected string connstr = "";
        protected bool m_disposed;
        protected string ClassName = "";
        protected string MethodName = "";
        #endregion

        #region 构造函数

        /// <summary>
        /// 初始化
        /// </summary>
        protected void init()
        {
            if (context == null)
            {
                if (DB != null)
                {
                    if (string.IsNullOrEmpty(DB.ConnStr))
                        connstr = DB.ConnectionString();
                    else
                        connstr = DB.ConnStr;
                }
                if (string.IsNullOrEmpty(connstr))
                    CatchErr(ClassName, "init", new NullReferenceException(SystemMessage.RefNullOrEmpty(connstr, language)));
                switch (DB.Mode)
                {
                    case DBType.Access:
                        context = new DataContext(new LinqToDB.DataProvider.Access.AccessDataProvider(), connstr);
                        break;
                    case DBType.MSSQL:
                        context = new DataContext(new LinqToDB.DataProvider.SqlServer.SqlServerDataProvider("", LinqToDB.DataProvider.SqlServer.SqlServerVersion.v2008), connstr);
                        break;
                    case DBType.MYSQL:
                        context = new DataContext(new LinqToDB.DataProvider.MySql.MySqlDataProvider(), connstr);
                        break;
                    case DBType.Oracle:
                        context = new DataContext(new LinqToDB.DataProvider.Oracle.OracleDataProvider(), connstr);
                        break;
                    case DBType.SQLite:
                        context = new DataContext(new LinqToDB.DataProvider.SQLite.SQLiteDataProvider(), connstr);
                        break;
                    case DBType.PostgreSQL:
                        context = new DataContext(new LinqToDB.DataProvider.PostgreSQL.PostgreSQLDataProvider(), connstr);
                        break;
                    default:
                        CatchErr(ClassName, "init", new NotSupportedException(SystemMessage.NotSupported("DBMODE", language)));
                        break;
                }
            } 
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
        /// 释放连接处理
        /// </summary>
        /// <param name="disposing">是否释放</param>
        protected virtual void Dispose(bool disposing)
        {
            lock (this)
            {
                if (disposing && !m_disposed)
                {
                    if (context != null)
                        context = null;
                    m_disposed = true;
                }
            }
        }

        /// <summary>
        /// Linq处理基类
        /// </summary>
        protected DataHelper()
        {
            DB = new DBInfo();
            DB.Mode = DBType.None;
            DB.ConnStr = "";
            DB.TimeOut = 60;
        }

        /// <summary>
        /// Linq处理基类
        /// </summary>
        /// <param name="db"></param>
        protected DataHelper(DBInfo db)
        {
            DB = db;
            if (DB == null) 
            {
                DB = new DBInfo();
                DB.Mode = DBType.None;
                DB.ConnStr = "";
                DB.TimeOut = 60; 
            }
        }

        /// <summary>
        /// Linq处理基类
        /// </summary>
        /// <param name="ConnectionType"></param>
        /// <param name="ConnectionString"></param>
        /// <param name="Timeout"></param>
        protected DataHelper(DBType ConnectionType, string ConnectionString, int Timeout = 60)
        {
            DB = new DBInfo();
            DB.Mode = ConnectionType;
            DB.ConnStr = ConnectionString;
            DB.TimeOut = Timeout;
        }

        #endregion

        #region 属性

        /// <summary>
        /// 长连接/短连接
        /// </summary>
        [DisplayName("KeepAlive")]
        [Description("连接")]
        public bool KeepAlive { get; set; }

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
                if (DB == null)
                {
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
        public DBType DataBaseType
        {
            get { return DB.Mode; }
            set { DB.Mode = value; }
        }

        /// <summary>
        /// 数据库连接串
        /// </summary>
        [DisplayName("Connection")]
        [Description("数据库连接串")]
        public string Connection
        {
            get { return DB.ConnStr; }
            set { DB.ConnStr = value; }
        }
        /// <summary>
        /// 数据库操作超时时间
        /// </summary>
        [DisplayName("Timeout")]
        [Description("数据库操作超时时间")]
        public int Timeout
        {
            get { return DB.TimeOut; }
            set { DB.TimeOut = value; }
        }
        /// <summary>
        /// 显示语言
        /// </summary>
        [DisplayName("language")]
        [Description("显示语言")]
        public Language language { get; set; }


        #endregion

        #region 事件

        /// <summary>
        /// 出现错误
        /// </summary>
        /// <param name="Class"></param>
        /// <param name="func"></param>
        /// <param name="ex"></param>
        protected void CatchErr(string Class,string func, Exception ex)
        {
                if (HasError != null)
                    HasError(Class, func, ex);
                else
                    throw ex; 
        }

        /// <summary>
        /// 调试信息
        /// </summary>
        public CommEvent.LogEven log { get; set; }
        /// <summary>
        /// 错误出现事件，性能参数内DEBUG设置为EVENT有效
        /// </summary>
        public CommEvent.HasErrorEven HasError { get; set; }

        #endregion

        #region 私有方法

        /// <summary>
        /// DATATable转对象
        /// </summary>
        /// <param name="DT"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected IList<object> DataTableToObj(DataTable DT, Type obj)
        {
            IList<object> entities = new List<object>();
            if (DT != null)
            {
                foreach (DataRow row in DT.Rows)
                {
                    object entity = Activator.CreateInstance(obj, true);
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

        /// <summary>
        /// DATATable转实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="DT"></param>
        /// <returns></returns>
        protected IList<T> DataTableToEntity<T>(DataTable DT) where T :class,new()
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

        /// <summary>
        /// DataRow转对象
        /// </summary>
        /// <param name="DR"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected object DataRowToobj(DataRow DR, Type obj)
        {
            object org = Activator.CreateInstance(obj, true);
            if (DR != null)
            {
                foreach (var item in obj.GetProperties())
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

        /// <summary>
        /// DataRow转实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="DR"></param>
        /// <returns></returns>
        protected T DataRowToEntity<T>(DataRow DR) where T : class, new()
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
        
        /// <summary>
        ///对象转字段
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected Dictionary<ColumnAttribute, object> objInfoToCol(object entity)
        {
            Dictionary<ColumnAttribute, object> Col = new Dictionary<ColumnAttribute, object>();
            PropertyInfo[] properties = entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo p in properties)
            {
                if (p != null)
                {
                    Type t = p.PropertyType;
                    ColumnAttribute[] ColumnAttributes = (ColumnAttribute[])p.GetCustomAttributes(typeof(ColumnAttribute), false);
                    string ColumnName = (ColumnAttributes == null ? "" : (ColumnAttributes.Length > 0 ? (ColumnAttributes.ToList().First().Name == null ? "" : ColumnAttributes.ToList().First().Name.Trim()) : ""));
                    if (string.IsNullOrEmpty(ColumnName))
                        ColumnName = p.Name;
                    bool IsPrimaryKey = (ColumnAttributes == null ? false : (ColumnAttributes.Length > 0 ? ColumnAttributes.ToList().First().IsPrimaryKey : false));
                    bool IsIdentity = (ColumnAttributes == null ? false : (ColumnAttributes.Length > 0 ? ColumnAttributes.ToList().First().IsIdentity : false));
                    bool CanBeNull = (ColumnAttributes == null ? false : (ColumnAttributes.Length > 0 ? ColumnAttributes.ToList().First().CanBeNull : false));
                    ColumnAttribute res = new ColumnAttribute();
                    res.Name = ColumnName;
                    res.CanBeNull = CanBeNull;
                    res.IsPrimaryKey = IsPrimaryKey;
                    res.IsIdentity = IsIdentity;
                    res.DataType = (ColumnAttributes == null ? LinqToDB.DataType.Undefined : (ColumnAttributes.Length > 0 ? ColumnAttributes.ToList().First().DataType : LinqToDB.DataType.Undefined));
                    if (res.DataType == LinqToDB.DataType.Undefined)
                        res.DataType = t.ToDataType(); 
                    if (t.SupportedType())
                        Col.Add(res, p.GetValue(entity, null));
                }
            }
            return Col;
        }

        /// <summary>
        /// 实体转字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected Dictionary<ColumnAttribute, object> PropertyInfoToCol<T>(T entity) where T : class, new()
        { 
            return objInfoToCol(entity);
        }

        /// <summary>
        /// 获取字段
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="TableName"></param>
        /// <returns></returns>
        protected List<ColumnAttribute> OToSqlCreat(Type obj, out string TableName)
        {
            List<ColumnAttribute> Columns = new List<ColumnAttribute>();
            TableAttribute[] TableAttributes = (TableAttribute[])obj.GetCustomAttributes(typeof(TableAttribute), false);
            TableName = (TableAttributes == null ? "" : (TableAttributes.Length > 0 ? TableAttributes.ToList().First().Name.Trim() : ""));
            if (string.IsNullOrEmpty(TableName))
                TableName = obj.Name;
            PropertyInfo[] properties = obj.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo p in properties)
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
            if (Columns.Where(c => c.IsPrimaryKey).Count() <= 0)
            {
                var index = Columns.FindIndex(c => c.Name.ToUpper().Contains("ID") || c.Name.ToUpper().Contains("INDEX") || c.IsIdentity);
                if (index > -1)
                    Columns[index].IsPrimaryKey = true;
            }
            if (Columns.Where(c => c.IsPrimaryKey).Count() <= 0)
                throw new NullReferenceException(SystemMessage.RefNullOrEmpty("PrimaryKey", language));
            if (Columns.Where(c => c.IsIdentity).Count() <= 0)
            {
                var index = Columns.FindIndex(c => c.IsPrimaryKey &&
                                              (
                                                   c.DataType == DataType.Int16 || c.DataType == DataType.Int32 || c.DataType == DataType.Int64
                                               || c.DataType == DataType.UInt16 || c.DataType == DataType.UInt32 || c.DataType == DataType.UInt64
                                              ));
                if (index > -1)
                    Columns[index].IsIdentity = true;
            }
            return Columns;
        }
        
        /// <summary>
        /// 获取字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="TableName"></param>
        /// <returns></returns>
        protected List<ColumnAttribute> EToSqlCreat<T>(out string TableName) where T : class, new()
        {
            TableName = "";
            T org = new T();
            List<ColumnAttribute> Columns = OToSqlCreat(org.GetType(),out TableName);
            if (Columns == null) Columns = new List<ColumnAttribute>();
            return Columns;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="TableName"></param>
        /// <param name="Cols"></param>
        /// <param name="orgcol"></param>
        protected void OTosqlModify(Type obj, out string TableName, out List<ColumnAttribute> Cols, out List<ColumnAttribute> orgcol) 
        {
            Cols = new List<ColumnAttribute>();
            orgcol = new List<ColumnAttribute>();
        
            TableAttribute[] TableAttributes = (TableAttribute[])obj.GetCustomAttributes(typeof(TableAttribute), false);
            TableName = (TableAttributes == null ? "" : (TableAttributes.Length > 0 ? TableAttributes.ToList().First().Name.Trim() : ""));
            if (string.IsNullOrEmpty(TableName))
                TableName = obj.Name;
            PropertyInfo[] properties = obj.GetProperties(BindingFlags.Public | BindingFlags.Instance);
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
                if (CheckField(TableName, ColumnName, out t, out CanBeNull, out IsPrimaryKey))
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
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="TableName"></param>
        /// <param name="Cols"></param>
        /// <param name="orgcol"></param>
        protected void ETosqlModify<T>(out string TableName,out List<ColumnAttribute> Cols,out List<ColumnAttribute> orgcol) where T : class, new()
        {
            TableName = "";
            Cols = new List<ColumnAttribute>();
            orgcol = new List<ColumnAttribute>();
            T org = new T();
            OTosqlModify(org.GetType(), out TableName, out Cols, out orgcol);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="TableName"></param>
        /// <returns></returns>
        protected Dictionary<string, object> OToSqlInsert(object entity, out string TableName)  
        {
            Dictionary<string, object> ColSVal = new Dictionary<string, object>();
            TableAttribute[] TableAttributes = (TableAttribute[])entity.GetType().GetCustomAttributes(typeof(TableAttribute), false);
            TableName = (TableAttributes == null ? "" : (TableAttributes.Length > 0 ? TableAttributes.ToList().First().Name.Trim() : ""));
            if (string.IsNullOrEmpty(TableName))
                TableName = entity.GetType().Name;
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
            return ColSVal;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="TableName"></param>
        /// <returns></returns>
        protected Dictionary<string, object> EToSqlInsert<T>(T entity,out string TableName) where T : class, new()
        { 
            return OToSqlInsert(entity, out TableName);
        }


        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="TableName"></param>
        /// <returns></returns>
        protected Dictionary<KeyValuePair<string, DataType>, object> OToSqlInsertEX(object entity, out string TableName)
        {
            Dictionary<KeyValuePair<string, DataType>, object> ColSVal = new Dictionary<KeyValuePair<string, DataType>, object>();
            TableAttribute[] TableAttributes = (TableAttribute[])entity.GetType().GetCustomAttributes(typeof(TableAttribute), false);
            TableName = (TableAttributes == null ? "" : (TableAttributes.Length > 0 ? TableAttributes.ToList().First().Name.Trim() : ""));
            if (string.IsNullOrEmpty(TableName))
                TableName = entity.GetType().Name;
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
                            ColSVal.Add(new KeyValuePair<string, DataType>(col.Key.Name, col.Key.DataType), col.Value);
                        else
                            continue;
                    }
                    else
                        continue;
                }
                else if (col.Value != null)
                    ColSVal.Add(new KeyValuePair<string, DataType>(col.Key.Name, col.Key.DataType), col.Value);
                else if (col.Key.CanBeNull)
                    ColSVal.Add(new KeyValuePair<string, DataType>(col.Key.Name, col.Key.DataType), null);
                else
                    throw new NullReferenceException(SystemMessage.RefNullOrEmpty(col.Key.Name, language));
            }
            return ColSVal;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="TableName"></param>
        /// <returns></returns>
        protected Dictionary<KeyValuePair<string, DataType>, object> EToSqlInsertEX<T>(T entity, out string TableName) where T : class, new()
        {
            return OToSqlInsertEX(entity, out TableName);
        }
         
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="TableName"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        protected Dictionary<string, object> OToSqlUpdate(object entity, out string TableName, out string where) 
        {
            Dictionary<string, object> ColSVal = new Dictionary<string, object>();
            TableAttribute[] TableAttributes = (TableAttribute[])entity.GetType().GetCustomAttributes(typeof(TableAttribute), false);
            TableName = (TableAttributes == null ? "" : (TableAttributes.Length > 0 ? TableAttributes.ToList().First().Name.Trim() : ""));
            if (string.IsNullOrEmpty(TableName))
                TableName = entity.GetType().Name;
            where = "";
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
            return ColSVal;
        }

        protected Dictionary<KeyValuePair<string, DataType>, object> OToSqlUpdateEX(object entity, out string TableName, out string where)
        {
            Dictionary<KeyValuePair<string, DataType>, object> ColSVal = new Dictionary<KeyValuePair<string, DataType>, object>();
            TableAttribute[] TableAttributes = (TableAttribute[])entity.GetType().GetCustomAttributes(typeof(TableAttribute), false);
            TableName = (TableAttributes == null ? "" : (TableAttributes.Length > 0 ? TableAttributes.ToList().First().Name.Trim() : ""));
            if (string.IsNullOrEmpty(TableName))
                TableName = entity.GetType().Name;
            where = "";
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
                    ColSVal.Add(new KeyValuePair<string, DataType>(col.Key.Name, col.Key.DataType), col.Value);
                else if (col.Key.CanBeNull)
                    ColSVal.Add(new KeyValuePair<string, DataType>(col.Key.Name, col.Key.DataType), null);
                else
                    throw new NullReferenceException(SystemMessage.RefNullOrEmpty(col.Key.Name, language));
            }
            return ColSVal;
        }
         
        /// <summary>
        /// 编辑
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="TableName"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        protected Dictionary<string, object> EToSqlUpdate<T>(T entity, out string TableName,out string where) where T : class, new()
        {
            return OToSqlUpdate(entity, out TableName, out where);
        }

        protected Dictionary<KeyValuePair<string, DataType>, object> EToSqlUpdateEX<T>(T entity, out string TableName, out string where) where T : class, new()
        {
            return OToSqlUpdateEX(entity, out TableName, out where);
        }
         
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="TableName"></param>
        /// <param name="where"></param>
        protected void OToSqlDelete(object entity, out string TableName, out string where)  
        {
            TableAttribute[] TableAttributes = (TableAttribute[])entity.GetType().GetCustomAttributes(typeof(TableAttribute), false);
            TableName = (TableAttributes == null ? "" : (TableAttributes.Length > 0 ? TableAttributes.ToList().First().Name.Trim() : ""));
            if (string.IsNullOrEmpty(TableName))
                TableName = entity.GetType().Name;
            where = "";
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
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="TableName"></param>
        /// <param name="where"></param>
        protected void EToSqlDelete<T>(T entity, out string TableName, out string where) where T : class, new()
        {
            OToSqlDelete(entity, out TableName, out where);
        }
         
        /// <summary>
        /// 获取表名
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected string Table<T>() where T : class, new()
        {
            T org = new T();
            TableAttribute[] TableAttributes = (TableAttribute[])org.GetType().GetCustomAttributes(typeof(TableAttribute), false);
            string TableName = (TableAttributes == null ? "" : (TableAttributes.Length > 0 ? TableAttributes.ToList().First().Name.Trim() : ""));
            if (string.IsNullOrEmpty(TableName))
                TableName = org.GetType().Name;
            return TableName;
        }

        /// <summary>
        /// 获取表名
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected string Table(Type obj)
        { 
            TableAttribute[] TableAttributes = (TableAttribute[])obj.GetCustomAttributes(typeof(TableAttribute), false);
            string TableName = (TableAttributes == null ? "" : (TableAttributes.Length > 0 ? TableAttributes.ToList().First().Name.Trim() : ""));
            if (string.IsNullOrEmpty(TableName))
                TableName = obj.Name;
            return TableName;
        }

        /// <summary>
        /// 执行单条操作
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        protected virtual int Execute(string sql)
        { 
            int res = -1;
            bool conns = false;
            IDbConnection conn = null;
            try
            {
                var DataProvider = context.DataProvider;
                conn = DataProvider.CreateConnection(context.ConnectionString);
                if (conn.State != ConnectionState.Open)
                {
                    conns = true;
                    conn.Open();
                }
                DbCommand cmd = conn.CreateCommand() as DbCommand;
                cmd.CommandText = sql;
                cmd.CommandTimeout = this.Timeout * 1000;
                cmd.ExecuteNonQuery();
                res = 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conns && conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
                conn = null;
            }
            return res;
        }


        private DbParameter getParameter(string Key,LinqToDB.DataType DType)
        {
            int ress = DType.DBDataType(DB.Mode);
            switch (DB.Mode)
            {
                case DBType.MSSQL:
                    SqlDbType MSTYPE = (SqlDbType)ress; 
                    return new SqlParameter(Key, MSTYPE);
                case DBType.Access:
                case DBType.OleDB:
                    OleDbType OLTYPE = (OleDbType)ress;
                    return new OleDbParameter(Key, OLTYPE);
                case DBType.ODBC:
                    OdbcType ODTYPE = (OdbcType)ress;
                    return new OdbcParameter(Key, ODTYPE);
                case DBType.Oracle:
                    OracleDbType OTYPE = (OracleDbType)ress;
                    return new OracleParameter(Key, OTYPE);
                case DBType.MYSQL:
                    MySqlDbType MYTYPE = (MySqlDbType)ress;
                    return new MySqlParameter(Key, MYTYPE);
                case DBType.PostgreSQL:
                    NpgsqlDbType PTYPE = (NpgsqlDbType)ress;
                    return new NpgsqlParameter(Key, PTYPE);
                case DBType.SQLite:
                    DbType SLTYPE = (DbType)ress;
                    return new SQLiteParameter(Key, SLTYPE);
                default:
                    return null;
            }
        }
         
        /// <summary>
        /// 执行单条操作
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="Parameter"></param>
        /// <returns></returns>
        protected virtual int Execute(string sql, Dictionary<KeyValuePair<string, DataType>, object> Parameter)
        {
            int res = -1;
            bool conns = false;
            IDbConnection conn = null;
            try
            {
                var DataProvider = context.DataProvider;
                conn = DataProvider.CreateConnection(context.ConnectionString);
                if (conn.State != ConnectionState.Open)
                {
                    conns = true;
                    conn.Open();
                }
                 
                DbCommand cmd = conn.CreateCommand() as DbCommand;
                cmd.CommandText = sql;
                cmd.CommandTimeout = this.Timeout * 1000;
                if (Parameter != null)
                {
                    foreach (var par in Parameter)
                    {
                        DbParameter pp = getParameter(par.Key.Key,par.Key.Value);
                        pp.Value = par.Value;
                        cmd.Parameters.Add(pp);
                    }
                }
                cmd.ExecuteNonQuery();
                res = 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conns && conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
                conn = null;
            }
            return res;
        }
         
        /// <summary>
        /// 执行批量处理
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        protected virtual int ExecuteNonQuery(List<string> sql)
        {
            int res = 0;
            if (sql == null) sql = new List<string>();
            bool conns = false;
            IDbConnection conn = null;
            try
            {
                var DataProvider = context.DataProvider;
                conn = DataProvider.CreateConnection(context.ConnectionString);
                if (conn.State != ConnectionState.Open)
                {
                    conns = true;
                    conn.Open();
                }
                DbCommand cmd = conn.CreateCommand() as DbCommand;
                cmd.CommandTimeout = this.Timeout * 1000;
                foreach (var tmp in sql)
                {
                    cmd.CommandText = tmp;
                    cmd.ExecuteNonQuery();
                    res++;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conns && conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
                conn = null;
            }
            return res;
        }

        /// <summary>
        /// 表是否存在
        /// </summary>
        /// <param name="Table"></param>
        /// <returns></returns>
        protected virtual bool TableIsExist(string Table)
        {
            bool res = false;
            bool conns = false;
            IDbConnection conn = null;
            try
            {
                var DataProvider = context.DataProvider;
                conn = DataProvider.CreateConnection(context.ConnectionString);
                if (conn.State != ConnectionState.Open)
                {
                    conns = true;
                    conn.Open();
                }
                DbCommand cmd = conn.CreateCommand() as DbCommand;
                cmd.CommandText = "select count(1) as num from " + Table + " where 1<>1";
                cmd.CommandTimeout = this.Timeout * 1000;
                try
                {
                    cmd.ExecuteScalar();
                    res = true;
                }
                catch
                { }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conns && conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
                conn = null;
            }
            return res;
        }

        private DbDataAdapter getData()
        {
            switch (DB.Mode)
            {
                case DBType.MSSQL:
                    return  new SqlDataAdapter(); 
                case DBType.Access:
                case DBType.OleDB:
                    return new OleDbDataAdapter(); 
                case DBType.ODBC:
                    return new OdbcDataAdapter();
                case DBType.Oracle:
                    return new OracleDataAdapter();
                case DBType.MYSQL:
                    return new MySqlDataAdapter();
                case DBType.PostgreSQL:
                    return new NpgsqlDataAdapter();
                case DBType.SQLite:
                    return new SQLiteDataAdapter();
                default:
                    return null;
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="TableName"></param>
        /// <returns></returns>
        protected virtual DataTable getTable(string sql,string TableName="")
        { 
            DataTable res = null;
            bool conns = false;
            IDbConnection conn = null;
            try
            {
                var DataProvider = context.DataProvider;
                conn = DataProvider.CreateConnection(context.ConnectionString);
                if (conn.State != ConnectionState.Open)
                {
                    conns = true;
                    conn.Open();
                }
                DbCommand cmd = conn.CreateCommand() as DbCommand;
                cmd.CommandText = sql;
                cmd.CommandTimeout = this.Timeout * 1000;
                DbDataAdapter da = getData();  
                if (da != null)
                {
                    DataSet ds = new DataSet();
                    da.SelectCommand = cmd;
                    if (string.IsNullOrEmpty(TableName))
                        da.Fill(ds);
                    else
                        da.Fill(ds, TableName);
                    if (ds.Tables.Count > 0)
                        res = ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conns && conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
                conn = null;
            }
            return res;
        }

        protected virtual DataTable getTable(int PageIndex, int PageSize, string sql, out int RecodeCount, out int PageCount,string TableName="")
        {
            RecodeCount = 0;
            PageCount = 0;
            DataTable res = null;
            bool conns = false;
            IDbConnection conn = null;
            try
            {
                var DataProvider = context.DataProvider;
                conn = DataProvider.CreateConnection(context.ConnectionString);
                if (conn.State != ConnectionState.Open)
                {
                    conns = true;
                    conn.Open();
                }
                DbCommand cmd = conn.CreateCommand() as DbCommand;
                cmd.CommandTimeout = this.Timeout * 1000;
                cmd.CommandText = "select count(1) from (" + sql + ") tabs";
                RecodeCount = Convert.ToInt32(cmd.ExecuteScalar());
                if (PageSize == 0)
                    PageCount = RecodeCount;
                else if (RecodeCount % PageSize == 0)
                    PageCount = RecodeCount / PageSize;
                else
                    PageCount = (RecodeCount / PageSize) + 1;
                DbDataAdapter da = getData();
                if (da != null)
                {
                    DataSet ds = new DataSet();
                    cmd.CommandText = sql;
                    da.SelectCommand = cmd;
                    da.Fill(ds, (PageIndex - 1) * PageSize, PageSize, "Query");
                    if (ds.Tables.Count > 0)
                        res = ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conns && conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
                conn = null;
            }
            return res;
        }
         
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="DisplayField"></param>
        /// <param name="TableName"></param>
        /// <param name="Where"></param>
        /// <param name="OrderBy"></param>
        /// <param name="GroupBy"></param>
        /// <param name="RecodeCount"></param>
        /// <param name="PageCount"></param>
        /// <returns></returns>
        protected virtual DataTable getTable(int PageIndex, int PageSize, string DisplayField, string TableName, string Where, string OrderBy, string GroupBy, out int RecodeCount, out int PageCount)
        {
            DataTable res = null;
            bool conns = false;
            IDbConnection conn = null;
            try
            {
                string Sql = "";
                string CountSql = "";
                if (!string.IsNullOrEmpty(Where))
                {
                    if (!Where.ToLower().Trim().Contains("where"))
                        Where = " WHERE " + Where;
                }
                if (string.IsNullOrEmpty(DisplayField))
                    DisplayField = "*";
                if (!string.IsNullOrEmpty(OrderBy))
                {
                    if (!OrderBy.ToLower().Trim().Contains("order"))
                        OrderBy = " ORDER BY " + OrderBy;
                }
                if (!string.IsNullOrEmpty(GroupBy))
                {
                    if (!GroupBy.ToLower().Trim().Contains("group"))
                        GroupBy = " GROUP BY " + GroupBy;
                    CountSql = "select count(1) as num from ( select count(1) as xx from  " + TableName + "  " + Where + " " + GroupBy + " ) DERIVEDTBL";
                }
                else
                    CountSql = "select count(1) as num from   " + TableName + "  " + Where;
                Sql = "SELECT " + DisplayField + " FROM  " + TableName + " " + Where + " " + GroupBy + " " + OrderBy;
                var DataProvider = context.DataProvider;
                conn = DataProvider.CreateConnection(context.ConnectionString);
                if (conn.State != ConnectionState.Open)
                {
                    conns = true;
                    conn.Open();
                }
                DbCommand cmd = conn.CreateCommand() as DbCommand; 
                cmd.CommandTimeout = this.Timeout * 1000;
                cmd.CommandText = CountSql;
                RecodeCount = Convert.ToInt32(cmd.ExecuteScalar());
                if (PageSize == 0)
                    PageCount = RecodeCount;
                else if (RecodeCount % PageSize == 0)
                    PageCount = RecodeCount / PageSize;
                else
                    PageCount = (RecodeCount / PageSize) + 1;
                DbDataAdapter da = getData();
                if (da != null)
                {
                    DataSet ds = new DataSet();
                    cmd.CommandText = Sql;
                    da.SelectCommand = cmd;
                    da.Fill(ds, (PageIndex - 1) * PageSize, PageSize, TableName);
                    if (ds.Tables.Count > 0)
                        res = ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conns && conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
                conn = null;
            }
            return res;
        }

        /// <summary>
        /// 字段查询
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="Field"></param>
        /// <param name="FieldType"></param>
        /// <param name="CanBeNull"></param>
        /// <param name="IsPrimaryKey"></param>
        /// <returns></returns>
        protected virtual bool CheckField(string TableName, string Field, out System.Type FieldType, out bool CanBeNull, out bool IsPrimaryKey)
        {
            FieldType = typeof(object);
            CanBeNull = false;
            IsPrimaryKey = false;
            if (string.IsNullOrEmpty(TableName))
                throw new NullReferenceException("TableName Is  Null Or Empty");
            else if (string.IsNullOrEmpty(Field))
                throw new NullReferenceException("Field Is  Null Or Empty");
            bool res = false;
            bool conns = false;
            IDbConnection conn = null;
            try
            {
                var DataProvider = context.DataProvider;
                conn = DataProvider.CreateConnection(context.ConnectionString);
                if (conn.State != ConnectionState.Open)
                {
                    conns = true;
                    conn.Open();
                }
                DbCommand cmd = conn.CreateCommand() as DbCommand; 
                cmd.CommandTimeout = this.Timeout * 1000;
                cmd.CommandText = "select * from " + TableName + " where 1<>1";
                DbDataAdapter da = getData(); 
                if (da != null)
                {
                    DataTable dt = null;
                    DataSet ds = new DataSet();
                    da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                    da.SelectCommand = cmd;
                    da.Fill(ds);
                    dt = ds.Tables[0];
                    var PriKeys = dt.PrimaryKey;
                    DataColumn[] dcs = new DataColumn[dt.Columns.Count];
                    dt.Columns.CopyTo(dcs, 0);
                    var dc = dcs.FirstOrDefault(c => c.ColumnName.ToUpper() == Field.ToUpper());
                    if (dc != null)
                    {
                        CanBeNull = dc.AllowDBNull;
                        if (PriKeys.FirstOrDefault(c => c.ColumnName.ToUpper().Trim() == Field.ToUpper().Trim()) != null)
                            IsPrimaryKey = true;
                        else
                            IsPrimaryKey = false;
                        FieldType = dc.DataType;
                        res = true;
                    }
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conns && conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
                conn = null;
            }
            return res;
        }

        /// <summary>
        /// 列出字段
        /// </summary>
        /// <param name="TableName"></param>
        /// <returns></returns>
        protected virtual List<DisplayColumnAttribute> Columns(string TableName )
        { 
            if (string.IsNullOrEmpty(TableName))
                throw new NullReferenceException("TableName Is  Null Or Empty");
            List<DisplayColumnAttribute> res = new List<DisplayColumnAttribute>();
            bool conns = false;
            IDbConnection conn = null;
            try
            {
                var DataProvider = context.DataProvider;
                conn = DataProvider.CreateConnection(context.ConnectionString);
                if (conn.State != ConnectionState.Open)
                {
                    conns = true;
                    conn.Open();
                }
                DbCommand cmd = conn.CreateCommand() as DbCommand;
                cmd.CommandTimeout = this.Timeout * 1000;
                cmd.CommandText = "select * from " + TableName + " where 1<>1";
                DbDataAdapter da = getData();
                if (da != null)
                {
                    DataTable dt = null;
                    DataSet ds = new DataSet();
                    da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                    da.SelectCommand = cmd;
                    da.Fill(ds);
                    dt = ds.Tables[0];
                    var PriKeys = dt.PrimaryKey;
                    DataColumn[] dcs = new DataColumn[dt.Columns.Count];
                    dt.Columns.CopyTo(dcs, 0);
                    foreach (var cl in dcs)
                    {
                        DisplayColumnAttribute disp = new DisplayColumnAttribute();
                        disp.Table = TableName;
                        disp.Column = cl.ColumnName;
                        disp.Name = cl.ColumnName;
                        disp.JS = "";
                        disp.CSS = "";
                        disp.Format = "";
                        disp.Unit = "";
                        disp.Caption = "";
                        disp.index = 0;
                        disp.Seqencing = cl.Ordinal;
                        disp.CanCount = false;
                        disp.CanHead = true;
                        disp.CanSearch = true;
                        disp.CanImpExp = false;
                        disp.IsUnique = false;
                        disp.Displaylanguage = this.language; 
                        if (PriKeys.FirstOrDefault(c => c.ColumnName.ToUpper().Trim() == cl.ColumnName.ToUpper().Trim()) != null)
                            disp.CanDeitail = false;
                        else
                            disp.CanDeitail = true;
                        res.Add(disp);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conns && conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
                conn = null;
            }
            return res;
        }

        /// <summary>
        /// 列出字段
        /// </summary>
        /// <param name="Type"></param>
        /// <returns></returns>
        protected virtual List<string> Structure(string Type)
        {
            if (string.IsNullOrEmpty(Type))
                throw new NullReferenceException("Type Is  Null Or Empty");
            List<string> res = new List<string>();
            bool conns = false;
            IDbConnection conn = null;
            try
            {
                DataTable dt = null;
                var DataProvider = context.DataProvider;
                conn = DataProvider.CreateConnection(context.ConnectionString);
                if (conn.State != ConnectionState.Open)
                {
                    conns = true;
                    conn.Open();
                }
                switch (DB.Mode)
                {
                    case DBType.MSSQL: 
                        dt = ((SqlConnection)conn).GetSchema(Type);
                        break;
                    case DBType.Access:
                    case DBType.OleDB: 
                        dt = ((OleDbConnection)conn).GetSchema(Type);
                        break;
                    case DBType.ODBC:
                        dt = ((OdbcConnection)conn).GetSchema(Type);
                        break;
                    case DBType.MYSQL:
                        dt = ((MySqlConnection)conn).GetSchema(Type);
                        break;
                    case DBType.Oracle:
                        dt = ((OracleConnection)conn).GetSchema(Type);
                        break;
                    case DBType.PostgreSQL:
                        dt = ((NpgsqlConnection)conn).GetSchema(Type);
                        break;
                    case DBType.SQLite:
                        dt = ((SQLiteConnection)conn).GetSchema(Type);
                        break;
                }
                if (dt != null)
                {
                    int m = dt.Columns.IndexOf("TABLE_NAME");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        res.Add(dr.ItemArray.GetValue(m).ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conns && conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
                conn = null;
            }
            return res;
        }


        #endregion

        #region 公开方法

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
                init();
                errmsg = "";
                var DataProvider = context.DataProvider;
                IDbConnection conn = DataProvider.CreateConnection(context.ConnectionString);
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                    conn.Close();
                    return true;
                }
                else 
                  return true; 
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 连接属性
        /// </summary>
        [DisplayName("CreateConnection")]
        [Description("连接属性")]
        public virtual DbConnection CreateConnection
        {
            get
            {
                init();
                MethodName = "";
                try
                {
                    MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                    MethodName = method.Name;
                }
                catch { }
                try
                {
                    var DataProvider = context.DataProvider;
                    DbConnection Conn = (DbConnection)DataProvider.CreateConnection(context.ConnectionString); 
                    return Conn;
                }
                catch (Exception ex)
                {
                    if (HasError != null)
                        HasError(ClassName, MethodName, ex);
                    else
                        throw ex;
                }
                return null;
            }
        }
        
        /// <summary>
        /// 数据库Insert,update,delete带返回执行数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>影响数量</returns>
        [DisplayName("ExecuteNonQuery")]
        [Description("数据库Insert,update,delete带返回执行数")]
        public virtual int ExecuteNonQuery(string sql)
        {
            init();
            MethodName = "";
            try
            {
                MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            int res = -1;
            bool conns = false;
            IDbConnection conn = null;
            try
            {
                var DataProvider = context.DataProvider;
                conn = DataProvider.CreateConnection(context.ConnectionString);
                if (conn.State != ConnectionState.Open)
                {
                    conns = true;
                    conn.Open();
                }
                DbCommand cmd = conn.CreateCommand() as DbCommand;
                cmd.CommandText = sql;
                cmd.CommandTimeout = this.Timeout * 1000;
                cmd.ExecuteNonQuery();
                res = 1;
            }
            catch (Exception ex)
            {
                CatchErr(ClassName, MethodName, ex);
                res = -2;
            }
            finally
            {
                if (conns && conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
                conn = null;
            }
            return res;
        }

        /// <summary>
        /// 数据库Insert,update,delete带返回执行数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>影响数量</returns>
        [DisplayName("ExecuteNonQuery")]
        [Description("数据库Insert,update,delete带返回执行数")]
        public virtual int ExecuteNonQuery(string sql, Dictionary<string, object> Parameter)
        {
            init();
            MethodName = "";
            try
            {
                MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            int res = -1;
            bool conns = false;
            IDbConnection conn = null;
            try
            {
                var DataProvider = context.DataProvider;
                conn = DataProvider.CreateConnection(context.ConnectionString);
                if (conn.State != ConnectionState.Open)
                {
                    conns = true;
                    conn.Open();
                }
                DbCommand cmd = conn.CreateCommand() as DbCommand;
                cmd.CommandText = sql;
                cmd.CommandTimeout = this.Timeout * 1000;
                cmd.ExecuteNonQuery();
                res = 1;
            }
            catch (Exception ex)
            {
                CatchErr(ClassName, MethodName, ex);
                res = -2;
            }
            finally
            {
                if (conns && conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
                conn = null;
            }
            return res;
        }


        #endregion
    }
}
