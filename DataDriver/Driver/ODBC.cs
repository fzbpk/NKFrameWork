﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using NK.ENum;
using NK.Entity;
using NK.Event;
using NK.Interface;

namespace NK.Data
{
    public partial class ODBC : IDisposable, iDataBase
    {

        #region 事件

        /// <summary>
        /// 错误事件
        /// </summary>
        public event CommEvent.HasErrorEven HasError = null;
        /// <summary>
        /// 日志事件
        /// </summary>
        public event CommEvent.LogEven log = null;
        /// <summary>
        /// 数据库连接成功事件
        /// </summary>
        public event DBEvent.Connect Connect = null;
        /// <summary>
        /// 数据库关闭事件
        /// </summary>
        public event DBEvent.DisConnect DisConnect = null;

        #endregion

        #region 定义

        private DbConnection DBConn = null; 
        private DBInfo DB = null; 
        private bool m_disposed;
        private string ClassName = "";

        #endregion

        #region 构造函数

        public ODBC(DBInfo info)
        { 
            if (info != null)
            {
                DB = info;
                if (string.IsNullOrEmpty(DB.ConnStr))
                    this.Connection = DB.ConnectionString();
                else
                    this.Connection = DB.ConnStr;
                this.Timeout = DB.TimeOut;
            }
            else
            {
                this.Connection = "";
                this.Timeout = 60;
            }
            ClassName = this.GetType().ToString();
        }

        public ODBC(string connection = "", int Timeouts = 60)
        {
            this.Connection = connection;
            this.Timeout = Timeouts;
            ClassName = this.GetType().ToString();
        }
        public ODBC(DbConnection Connection)
        { 
            if (Connection != null)
            {
                DBConn = Connection;
                this.Connection = Connection.ConnectionString;
                this.Timeout = Connection.ConnectionTimeout;
            }
            ClassName = this.GetType().ToString();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        ~ODBC()
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
                    if (DBConn != null)
                    {
                        try
                        {
                            if (DBConn.State != ConnectionState.Closed)
                                DBConn.Close();
                            DBConn = null;
                        }
                        catch
                        { }
                    }
                    m_disposed = true;
                }
            }
        }

        #endregion

        #region 属性
        public DBInfo DataBase
        {
            get { return DB; }
            set
            {
                DB = value;
                if (DB != null)
                {
                    if (string.IsNullOrEmpty(DB.ConnStr))
                        this.Connection = DB.ConnectionString();
                    else
                        this.Connection = DB.ConnStr;
                    this.Timeout = DB.TimeOut;
                }
                else
                {
                    this.Connection = "";
                    this.Timeout = 60;
                }
            }
        }
        public string Connection { get; set; }
        public int Timeout { get; set; }
        public bool KeepAlive { get; set; }

        #endregion

        #region 方法

        /// <summary>
        /// 打开数据库连接
        /// </summary>
        /// <returns>数据库连接</returns>
        public DbConnection GetConnection()
        {
            try
            {
                if (log != null) log(ClassName, "GetConnection", Log_Type.Infomation, "new DbConnection");
                DbConnection conn = null;
                if (this.KeepAlive)
                {
                    if (DBConn == null)
                        DBConn = new OdbcConnection(this.Connection);
                    conn = DBConn;
                }
                else
                    conn = new OdbcConnection(this.Connection);
                if (log != null) log(ClassName, "GetConnection", Log_Type.Infomation, "DbConnection check");
                if (conn.State == ConnectionState.Closed)
                {
                    if (log != null) log(ClassName, "GetConnection", Log_Type.Infomation, "DbConnection open");
                    conn.Open();
                    if (Connect != null)
                        Connect(conn);
                }
                if (log != null) log(ClassName, "GetConnection", Log_Type.Infomation, " DbConnection return");
                return conn;
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, "GetConnection", Log_Type.Error, ex.Message);
                if (this.HasError != null)
                    HasError(ClassName, "GetConnection", ex);
                else
                    throw ex;
                return null;
            }
        }

        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        /// <param name="conn">数据库连接</param>
        public void CloseConnection(DbConnection conn)
        {
            try
            {
                if (conn != null)
                {
                    if (log != null) log(ClassName, "CloseConnection", Log_Type.Infomation, " DbConnection check");
                    if (conn.State != ConnectionState.Closed)
                    {
                        if (log != null) log(ClassName, "CloseConnection", Log_Type.Infomation, " DbConnection close");
                        conn.Close();
                        if (this.DisConnect != null)
                            DisConnect(conn);
                    }
                    if (log != null) log(ClassName, "CloseConnection", Log_Type.Infomation, " DbConnection Dispose");
                    conn.Dispose();
                    conn = null;
                }
                else if (DBConn != null)
                {
                    if (log != null) log(ClassName, "CloseConnection", Log_Type.Infomation, " DbConnection check");
                    if (DBConn.State != ConnectionState.Closed)
                    {
                        if (log != null) log(ClassName, "CloseConnection", Log_Type.Infomation, " DbConnection close");
                        DBConn.Close();
                        if (this.DisConnect != null)
                            DisConnect(DBConn);
                    }
                    if (log != null) log(ClassName, "CloseConnection", Log_Type.Infomation, " DbConnection Dispose");
                    DBConn.Dispose();
                    DBConn = null;
                }
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, "CloseConnection", Log_Type.Error, ex.Message);
                if (this.HasError != null)
                    HasError(ClassName, "CloseConnection", ex);
                else
                    throw ex;
            }
        }

        /// <summary>
        /// 检查数据库连接是否连接成功
        /// </summary>
        /// <param name="ErrMsg">错误信息</param>
        /// <returns>是否连接成功</returns>
        public bool CheckConnection(out string ErrMsg)
        {
            ErrMsg = "";
            try
            {
                if (this.KeepAlive)
                {
                    if (DBConn == null)
                        DBConn = GetConnection();
                    if (DBConn.State == ConnectionState.Closed)
                        DBConn.Open();
                    return true;
                }
                else
                {
                    DbConnection conn = GetConnection();
                    CloseConnection(conn);
                    return true;
                }
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 数据库Insert,update,delete带返回执行数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>影响数量</returns>
        public int ExecuteNonQuery(string sql)
        {
            try
            {
                if (log != null) log(ClassName, "ExecuteNonQuery", Log_Type.Test, sql);
                OdbcCommand cmd = null;
                if (this.KeepAlive)
                {
                    if (DBConn == null)
                        DBConn = new OdbcConnection(this.Connection);
                    if (DBConn.State == ConnectionState.Closed)
                        DBConn.Open();
                    cmd = new OdbcCommand(sql, (OdbcConnection)DBConn);
                    cmd.CommandTimeout = this.Timeout * 1000;
                    int res = cmd.ExecuteNonQuery();
                    return res;
                }
                else
                {
                    OdbcConnection conn = (OdbcConnection)GetConnection();
                    cmd = new OdbcCommand(sql, conn);
                    cmd.CommandTimeout = this.Timeout * 1000;
                    int res = cmd.ExecuteNonQuery();
                    CloseConnection(conn);
                    return res;
                }
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, "ExecuteNonQuery", Log_Type.Error, ex.Message);
                if (this.HasError != null)
                    HasError(ClassName, "ExecuteNonQuery", ex);
                else
                    throw ex;
                return -1;
            }
        }

        /// <summary>
        /// 获取查询数据
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        public DataTable getDataTable(string sql)
        {
            int RecCount = 0;
            int PageSize = 0;
            int PageCount = 0;
            return getDataTable(sql, PageSize, out RecCount, out PageCount);
        }

        /// <summary>
        /// 获取查询数据
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="RecCount">返回记录数</param>
        /// <returns></returns>
        public DataTable getDataTable(string sql, out int RecCount)
        {
            int PageSize = 0;
            int PageCount = 0;
            return getDataTable(sql, PageSize, out RecCount, out PageCount);
        }

        /// <summary>
        /// 获取查询数据
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="PageSize">分页大小</param>
        /// <param name="PageCount">返回总页数</param>
        /// <returns></returns>
        public DataTable getDataTable(string sql, int PageSize, out int PageCount)
        {
            int RecCount = 0;
            return getDataTable(sql, PageSize, out RecCount, out PageCount);
        }

        /// <summary>
        /// 获取查询数据
        /// </summary>
        /// <param name="sql">查询SQL</param>
        /// <param name="PageSize">分页大小</param>
        /// <param name="RecCount">返回查询记录数</param>
        /// <param name="PageCount">返回总页数</param>
        /// <param name="TableName">表名</param>
        /// <returns>查询结果</returns>
        public DataTable getDataTable(string sql, int PageSize, out int RecCount, out int PageCount, string TableName = "")
        {
            if (log != null) log(ClassName, "getDataTable", Log_Type.Test, sql);
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                OdbcConnection conn = null;
                if (this.KeepAlive)
                {
                    if (DBConn == null)
                        DBConn = new OdbcConnection(this.Connection);
                    if (DBConn.State == ConnectionState.Closed)
                        DBConn.Open();
                }
                else
                    conn = (OdbcConnection)GetConnection();
                OdbcDataAdapter da = new OdbcDataAdapter();
                da.SelectCommand = new OdbcCommand();
                if (this.KeepAlive)
                    da.SelectCommand.Connection = (OdbcConnection)DBConn;
                else
                    da.SelectCommand.Connection = conn;
                da.SelectCommand.CommandTimeout = this.Timeout * 1000;
                da.SelectCommand.CommandText = sql;
                if (string.IsNullOrEmpty(TableName))
                    da.Fill(ds);
                else
                    da.Fill(ds, TableName);
                if (ds.Tables.Count > 0)
                {
                    RecCount = ds.Tables[0].Rows.Count;
                    dt = ds.Tables[0];
                }
                else
                {
                    RecCount = 0;
                    dt = null;
                }
                if (conn != null)
                    CloseConnection(conn);
                if (PageSize == 0)
                    PageCount = RecCount;
                else if (RecCount % PageSize == 0)
                    PageCount = RecCount / PageSize;
                else
                    PageCount = (RecCount / PageSize) + 1;
                return dt;
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, "getDataTable", Log_Type.Error, ex.Message);
                if (this.HasError != null)
                    HasError(ClassName, "getDataTable", ex);
                else
                    throw ex;
                PageCount = 0;
                RecCount = 0;
                return null;
            }
        }

        /// <summary>
        /// 判断是否存在记录
        /// </summary>
        /// <param name="sql">查询数据库</param>
        /// <returns>Boolean,TRUE为存在，False为不存在</returns>
        public bool IsExist(string sql)
        {
            if (log != null) log(ClassName, "IsExist", Log_Type.Test, sql);
            try
            {
                OdbcCommand cmd = null;
                OdbcConnection conn = null;
                if (this.KeepAlive)
                {
                    if (DBConn == null)
                        DBConn = new OdbcConnection(this.Connection);
                    if (DBConn.State == ConnectionState.Closed)
                        DBConn.Open();
                    cmd = new OdbcCommand(sql, (OdbcConnection)DBConn);
                }
                else
                {
                    conn = (OdbcConnection)GetConnection();
                    cmd = new OdbcCommand(sql, conn);
                }
                cmd.CommandTimeout = this.Timeout * 1000;
                OdbcDataReader da = cmd.ExecuteReader();
                bool res = da.Read();
                da.Close();
                if (conn != null)
                    CloseConnection(conn);
                return res;
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, "IsExist", Log_Type.Error, ex.Message);
                if (this.HasError != null)
                    HasError(ClassName, "IsExist", ex);
                else
                    throw ex;
                return false;
            }
        }

        /// <summary>
        /// 查询数据记录
        /// </summary>
        /// <param name="sql">查询SQL</param>
        /// <returns>返回第一行查询数据</returns>
        public Dictionary<string, object> Find(string sql)
        {
            if (log != null) log(ClassName, "Find", Log_Type.Test, sql);
            try
            {
                Dictionary<string, object> res = new Dictionary<string, object>();
                OdbcCommand cmd = null;
                OdbcConnection conn = null;
                if (this.KeepAlive)
                {
                    if (DBConn == null)
                        DBConn = new OdbcConnection(this.Connection);
                    if (DBConn.State == ConnectionState.Closed)
                        DBConn.Open();
                    cmd = new OdbcCommand(sql, (OdbcConnection)DBConn);
                }
                else
                {
                    conn = (OdbcConnection)GetConnection();
                    cmd = new OdbcCommand(sql, conn);
                }
                cmd.CommandTimeout = this.Timeout * 1000;
                OdbcDataReader da = cmd.ExecuteReader();
                if (da.Read())
                {
                    if (da.FieldCount > 0)
                    {
                        for (int i = 0; i < da.FieldCount; i++)
                        {
                            if (da.IsDBNull(i))
                                res.Add(da.GetName(i), null);
                            else
                                res.Add(da.GetName(i), da[i]);
                        }
                    }
                }
                da.Close();
                if (conn != null)
                    CloseConnection(conn);
                return res;
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, "Find", Log_Type.Error, ex.Message);
                if (this.HasError != null)
                    HasError(ClassName, "Find", ex);
                else
                    throw ex;
                return null;
            }
        }

        /// <summary>
        /// 查询记录
        /// </summary>
        /// <param name="sql">查询SQL</param>
        /// <returns>返回第一行第一列数据</returns>
        public object ExecuteScalar(string sql)
        {
            if (log != null) log(ClassName, "ExecuteScalar", Log_Type.Test, sql);
            try
            {
                OdbcCommand cmd = null;
                OdbcConnection conn = null;
                if (this.KeepAlive)
                {
                    if (DBConn == null)
                        DBConn = new OdbcConnection(this.Connection);
                    if (DBConn.State == ConnectionState.Closed)
                        DBConn.Open();
                    cmd = new OdbcCommand(sql, (OdbcConnection)DBConn);
                }
                else
                {
                    conn = (OdbcConnection)GetConnection();
                    cmd = new OdbcCommand(sql, conn);
                }
                cmd.CommandTimeout = this.Timeout * 1000;
                object res = null;
                try
                {
                    res = cmd.ExecuteScalar();
                    if (res is DBNull)
                        res = null;
                }
                catch
                { }
                if (conn != null)
                    CloseConnection(conn);
                return res;
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, "ExecuteScalar", Log_Type.Error, ex.Message);
                if (this.HasError != null)
                    HasError(ClassName, "ExecuteScalar", ex);
                else
                    throw ex;
                return null;
            }
        }

        /// <summary>
        /// 查询表是否存在
        /// </summary>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public bool TableIsExist(string TableName)
        {
            if (string.IsNullOrEmpty(TableName))
                return false;
            try
            { 
                OdbcCommand cmd = null;
                OdbcConnection conn = null;
                if (this.KeepAlive)
                {
                    if (DBConn == null)
                        DBConn = new OdbcConnection(this.Connection);
                    if (DBConn.State == ConnectionState.Closed)
                        DBConn.Open();
                    cmd = new OdbcCommand("select count(1) from " + TableName, (OdbcConnection)DBConn);
                }
                else
                {
                    conn = (OdbcConnection)GetConnection();
                    cmd = new OdbcCommand("select count(1) from " + TableName, conn);
                }
                cmd.CommandTimeout = this.Timeout * 1000;
                cmd.ExecuteScalar();
                if (conn != null)
                    CloseConnection(conn);
                return true;
            }
            catch 
            {
                return false;
            }

        }

        /// <summary>
        /// 利用内存分页因此适合数据小但SQL语句复杂的查询
        /// </summary>
        /// <param name="PageIndex">当前页</param>
        /// <param name="PageSize">分页大小</param>
        /// <param name="DisplayField">字段列，每个字段用,分开</param>
        /// <param name="TableName">表名，支持（） k 视图方式</param>
        /// <param name="Where">查询条件，不带关键字WHERE</param>
        /// <param name="OrderBy">排序语句，带order by</param>
        /// <param name="GroupBy">GROUP BY 字段，不带关键字GROUP BY</param>
        /// <param name="RecodeCount">返回总记录数</param>
        /// <param name="PageCount">返回总记录数</param>
        /// <returns>查询结果</returns>
        public DataTable getDataTableByRam(int PageIndex, int PageSize, string DisplayField, string TableName, string Where, string OrderBy, string GroupBy, out int RecodeCount, out int PageCount)
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
                CountSql = "select count(1) as num from ( select count(1) as xx from " + TableName + " " + Where + " " + GroupBy + " ) DERIVEDTBL";
            }
            else
                CountSql = "select count(1) as num from  " + TableName + " " + Where;
            Sql = "SELECT " + DisplayField + " FROM  " + TableName + " " + Where + " " + GroupBy + " " + OrderBy;
            if (log != null) log(ClassName, "getDataTableByRam", Log_Type.Test, CountSql);
            if (log != null) log(ClassName, "getDataTableByRam", Log_Type.Test, Sql);
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();

            try
            {
                OdbcCommand cmd = null;
                OdbcConnection conn = null;
                if (this.KeepAlive)
                {
                    if (DBConn == null)
                        DBConn = new OdbcConnection(this.Connection);
                    if (DBConn.State == ConnectionState.Closed)
                        DBConn.Open();
                    cmd = new OdbcCommand(CountSql, (OdbcConnection)DBConn);
                }
                else
                {
                    conn = (OdbcConnection)GetConnection();
                    cmd = new OdbcCommand(CountSql, conn);
                }
                cmd.CommandTimeout = this.Timeout * 1000;
                RecodeCount = Convert.ToInt32(cmd.ExecuteScalar());
                if (PageSize == 0)
                    PageCount = RecodeCount;
                else if (RecodeCount % PageSize == 0)
                    PageCount = RecodeCount / PageSize;
                else
                    PageCount = (RecodeCount / PageSize) + 1;
                OdbcDataAdapter da = new OdbcDataAdapter();
                da.SelectCommand = new OdbcCommand();
                if (this.KeepAlive)
                    da.SelectCommand.Connection = (OdbcConnection)DBConn;
                else
                    da.SelectCommand.Connection = conn;
                da.SelectCommand.CommandTimeout = this.Timeout * 1000;
                da.SelectCommand.CommandText = Sql;
                da.Fill(ds, (PageIndex - 1) * PageSize , PageSize, TableName);
                if (ds.Tables.Count > 0)
                    dt = ds.Tables[0];
                else
                    dt = null;
                if (conn != null)
                    CloseConnection(conn);
                return dt;
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, "getDataTableByRam", Log_Type.Error, ex.Message);
                if (this.HasError != null)
                    HasError(ClassName, "getDataTableByRam", ex);
                else
                    throw ex;
                RecodeCount = 0;
                PageCount = 0;
                return null;
            }
        }

        /// <summary>
        /// 利用数据库分页因此适合数据小但SQL语句复杂的查询
        /// </summary>
        /// <param name="PageIndex">当前页</param>
        /// <param name="PageSize">分页大小</param>
        /// <param name="DisplayField">字段列，每个字段用,分开</param>
        /// <param name="TableName">表名，支持（） k 视图方式</param>
        /// <param name="Where">查询条件，不带关键字WHERE</param>
        /// <param name="OrderBy">排序语句，带order by</param>
        /// <param name="OrderField">排序字段</param>
        /// <param name="GroupBy">GROUP BY 字段，不带关键字GROUP BY</param>
        /// <param name="RecodeCount">返回总记录数</param>
        /// <param name="PageCount">返回总记录数</param>
        /// <returns>查询结果</returns>
        public DataTable getDataTableByDB(int PageIndex, int PageSize, string DisplayField, string TableName, string Where, string OrderField, string OrderBy, string GroupBy, out int RecodeCount, out int PageCount)
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
                CountSql = "select count(1) as num from ( select count(1) as xx from " + TableName + " " + Where + " " + GroupBy + " ) DERIVEDTBL";
            }
            else
                CountSql = "select count(1) as num from  " + TableName + " " + Where;
            Sql = "select top " + PageSize.ToString() + " * from " + TableName + "  where " + OrderField + " not in(select top " + ((PageIndex - 1) * PageSize).ToString() + " " + OrderField + " from " + TableName + "  " + OrderBy + ") " + OrderBy + "";
            if (log != null) log(ClassName, "getDataTableByDB", Log_Type.Test, CountSql);
            if (log != null) log(ClassName, "getDataTableByDB", Log_Type.Test, Sql);
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();

            try
            {
                OdbcCommand cmd = null;
                OdbcConnection conn = null;
                if (this.KeepAlive)
                {
                    if (DBConn == null)
                        DBConn = new OdbcConnection(this.Connection);
                    if (DBConn.State == ConnectionState.Closed)
                        DBConn.Open();
                    cmd = new OdbcCommand(CountSql, (OdbcConnection)DBConn);
                }
                else
                {
                    conn = (OdbcConnection)GetConnection();
                    cmd = new OdbcCommand(CountSql, conn);
                }
                cmd.CommandTimeout = this.Timeout * 1000;
                RecodeCount = Convert.ToInt32(cmd.ExecuteScalar());
                if (PageSize == 0)
                    PageCount = RecodeCount;
                else if (RecodeCount % PageSize == 0)
                    PageCount = RecodeCount / PageSize;
                else
                    PageCount = (RecodeCount / PageSize) + 1;
                OdbcDataAdapter da = new OdbcDataAdapter();
                da.SelectCommand = new OdbcCommand();
                if (this.KeepAlive)
                    da.SelectCommand.Connection = (OdbcConnection)DBConn;
                else
                    da.SelectCommand.Connection = conn;
                da.SelectCommand.CommandTimeout = this.Timeout * 1000;
                da.SelectCommand.CommandText = Sql;
                da.Fill(ds, 0, PageSize, TableName);
                if (ds.Tables.Count > 0)
                    dt = ds.Tables[0];
                else
                    dt = null;
                if (conn != null)
                    CloseConnection(conn);
                return dt;
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, "getDataTableByDB", Log_Type.Error, ex.Message);
                if (this.HasError != null)
                    HasError(ClassName, "getDataTableByDB", ex);
                else
                    throw ex;
                RecodeCount = 0;
                PageCount = 0;
                return null;
            }
        }
 
        /// <summary>
        /// 查询字段属性
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <param name="Field">字段名</param>
        /// <param name="FieldType">字段类型</param>
        /// <returns>true 字段存在，false 字段不存在</returns>
        public bool CheckField(string TableName, string Field, out System.Type FieldType, out bool CanBeNull, out bool IsPrimaryKey)
        {
            FieldType = typeof(object);
            CanBeNull = false;
            IsPrimaryKey = false;
            if (string.IsNullOrEmpty(TableName))
            {
                if (log != null) log(ClassName, "CheckField", Log_Type.Error, "TableName Is  Null Or Empty");
                if (this.HasError != null)
                    HasError(ClassName, "CheckField", new NullReferenceException("TableName Is  Null Or Empty"));
                else
                    throw new NullReferenceException("TableName Is  Null Or Empty");
                return false;
            }
            else if (string.IsNullOrEmpty(Field))
            {
                if (log != null) log(ClassName, "CheckField", Log_Type.Error, "Field Is  Null Or Empty");
                if (this.HasError != null)
                    HasError(ClassName, "CheckField", new NullReferenceException("Field Is  Null Or Empty"));
                else
                    throw new NullReferenceException("Field Is  Null Or Empty");
                return false;
            }
            try
            {
                bool res = false; 
                OdbcConnection conn = null;
                DataTable dt = null;
                OdbcDataAdapter da = new OdbcDataAdapter();
                DataSet ds = new DataSet();
                if (this.KeepAlive)
                {
                    if (DBConn == null)
                        DBConn = new OdbcConnection(this.Connection);
                    if (DBConn.State == ConnectionState.Closed)
                        DBConn.Open(); 
                }
                else
                  conn = (OdbcConnection)GetConnection();
                OdbcCommand cmd = new OdbcCommand();
                cmd.Connection = conn;
                cmd.CommandTimeout = this.Timeout * 1000;
                cmd.CommandText = "select * from " + TableName + " where 1<>1";
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
                if (conn != null)
                    CloseConnection(conn);
                return res;
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, "CheckField", Log_Type.Error, ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 获取数据库所有表
        /// </summary>
        public List<string> Tables
        {
            get
            {
                List<string> res = new List<string>();
                try
                {
                    OdbcConnection conn = null;
                    DataTable dt = null;
                    if (this.KeepAlive)
                    {
                        if (DBConn == null)
                            DBConn = new OdbcConnection(this.Connection);
                        if (DBConn.State == ConnectionState.Closed)
                            DBConn.Open();
                        dt = DBConn.GetSchema("Tables");
                    }
                    else
                    {
                        conn = (OdbcConnection)GetConnection();
                        dt = conn.GetSchema("Tables");
                    }
                    int m = dt.Columns.IndexOf("TABLE_NAME");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        res.Add(dr.ItemArray.GetValue(m).ToString());
                    }
                    if (conn != null)
                        CloseConnection(conn);
                }
                catch (Exception ex)
                {
                    if (log != null) log(ClassName, "Tables", Log_Type.Error, ex.Message);
                    if (this.HasError != null)
                        HasError(ClassName, "Tables", ex);
                    else
                        throw ex;
                }
                return res;
            }
        }

        /// <summary>
        /// 获取所有视图
        /// </summary>
        public List<string> Views
        {
            get
            {
                List<string> res = new List<string>();
                try
                {
                    OdbcConnection conn = null;
                    DataTable dt = null;
                    if (this.KeepAlive)
                    {
                        if (DBConn == null)
                            DBConn = new OdbcConnection(this.Connection);
                        if (DBConn.State == ConnectionState.Closed)
                            DBConn.Open();
                        dt = DBConn.GetSchema("Views");
                    }
                    else
                    {
                        conn = (OdbcConnection)GetConnection();
                        dt = conn.GetSchema("Views");
                    }
                    int m = dt.Columns.IndexOf("TABLE_NAME");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        res.Add(dr.ItemArray.GetValue(m).ToString());
                    }
                    if (conn != null)
                        CloseConnection(conn);
                }
                catch (Exception ex)
                {
                    if (log != null) log(ClassName, "Views", Log_Type.Error, ex.Message);
                    if (this.HasError != null)
                        HasError(ClassName, "Views", ex);
                    else
                        throw ex;
                }
                return res;
            }
        }

        /// <summary>
        /// 表结构
        /// </summary>
        /// <param name="TableName">表</param>
        /// <returns>字段</returns>
        public List<string> Columns(string TableName)
        {
            List<string> res = new List<string>();
            if (string.IsNullOrEmpty(TableName))
            {
                if (this.HasError != null)
                    HasError(ClassName, "Columns", new NullReferenceException("TableName Is  Null Or Empty"));
                else
                    throw new NullReferenceException("TableName Is  Null Or Empty");
                return res;
            }
            else
            {
                try
                {
                    OdbcConnection conn = null;
                    DataTable dt = null;
                    OdbcDataAdapter da = new OdbcDataAdapter();
                    DataSet ds = new DataSet();
                    if (this.KeepAlive)
                    {
                        if (DBConn == null)
                            DBConn = new OdbcConnection(this.Connection);
                        if (DBConn.State == ConnectionState.Closed)
                            DBConn.Open();
                    }
                    else
                        conn = (OdbcConnection)GetConnection();
                    OdbcCommand cmd = new OdbcCommand();
                    cmd.Connection = conn;
                    cmd.CommandTimeout = this.Timeout * 1000;
                    cmd.CommandText = "select * from " + TableName + " where 1<>1";
                    da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                    da.SelectCommand = cmd;
                    da.Fill(ds);
                    dt = ds.Tables[0]; 
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        var col = dt.Columns[i];
                        res.Add(col.ColumnName);
                    }
                    if (conn != null)
                        CloseConnection(conn);
                }
                catch (Exception ex)
                {
                    if (log != null) log(ClassName, "Columns", Log_Type.Error, ex.Message);
                    if (this.HasError != null)
                        HasError(ClassName, "Columns", ex);
                    else
                        throw ex;
                }
                return res;
            }
        }

        #endregion
 
    }
}
