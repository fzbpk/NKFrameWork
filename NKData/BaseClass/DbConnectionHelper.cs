using NK.Entity;
using NK.ENum;
using NK.Event;
using NK.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NK.Data
{
    /// <summary>
    /// ADO.NET操作类
    /// </summary>
    public  class DbConnectionHelper
    {

        #region 事件
        /// <summary>
        /// 出错
        /// </summary>
        public  CommEvent.HasErrorEven HasError { get; set; }
        /// <summary>
        /// 操作日志
        /// </summary>
        public  CommEvent.LogEven log { get; set; }
        /// <summary>
        /// 数据库连接，长连接有效
        /// </summary>
        public  DBEvent.Connect Connect { get; set; }
        /// <summary>
        /// 数据库关闭，长连接有效
        /// </summary>
        public  DBEvent.DisConnect DisConnect { get; set; }

        #endregion

        #region 定义
        protected DBType dy = DBType.None;
        protected DBInfo DB = new DBInfo();
        protected DbConnection Conn = null; 
        protected bool m_disposed;
        protected string chksql = "";
        protected string ClassName = "";
        protected string MethodName = "";
        #endregion

        #region 构造函数

        /// <summary>
        /// 初始化
        /// </summary>
        protected virtual void initialization( )
        {

            if (Conn != null)
            {
                if (Conn.State == ConnectionState.Closed)
                {
                    Conn.Open();
                    try { if (this.Connect != null) this.Connect(Conn); }
                    catch { } 
                }
                else if (!string.IsNullOrEmpty(chksql))
                {
                    try
                    {
                        DbCommand cmd = Conn.CreateCommand();
                        cmd.CommandText = chksql;
                        cmd.CommandTimeout = this.Timeout * 1000;
                        object ss = cmd.ExecuteScalar();
                        cmd.Dispose();
                        cmd = null;
                    }
                    catch
                    {
                        try { Conn.Close(); }
                        catch { }
                        Conn.Open();
                        try { if (this.Connect != null) this.Connect(Conn); }
                        catch { }
                    }
                }
            }
        }

        /// <summary>
        /// 执行完成
        /// </summary>
        protected virtual void End()
        {
            if (Conn != null)
            { 
                if (!this.KeepAlive)
                {
                    if (Conn.State != ConnectionState.Closed)
                    {
                        try
                        { if (this.DisConnect != null) this.DisConnect(Conn); }
                        catch { }
                        Conn.Close();
                    }
                    Conn = null;
                }
            }
        }

        /// <summary>
        /// ADO.NET操作类
        /// </summary>
        /// <param name="info"></param>
        public DbConnectionHelper(DBInfo info)
        {
            DB = info;
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

        /// <summary>
        /// ADO.NET操作类
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="Timeouts"></param>
        public DbConnectionHelper(string connection = "", int Timeouts = 60)
        {
            this.Connection = connection;
            this.Timeout = Timeouts;
        }

        /// <summary>
        /// ADO.NET操作类
        /// </summary>
        /// <param name="Connection"></param>
        public DbConnectionHelper(DbConnection Connection)
        {
            if (Connection != null)
            {
                Conn = Connection;
                this.Connection = Connection.ConnectionString;
                this.Timeout = Connection.ConnectionTimeout;
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
        /// 释放资源
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            lock (this)
            {
                if (disposing && !m_disposed)
                {
                    if (Conn != null)
                    {
                        try
                        {
                            if (Conn.State != ConnectionState.Closed)
                            {
                                try { if (this.DisConnect != null) this.DisConnect(Conn); }
                                catch { }
                                Conn.Close();
                            }
                            Conn = null;
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
        /// <summary>
        /// 数据库配置
        /// </summary>
        public virtual DBInfo DataBase
        {
            get {
                return DB != null ? DB : (new DBInfo() {
                Mode = dy,
                ConnStr = this.Connection,
                TimeOut = this.Timeout
            });
            }
            set
            {
                DB = value;
                if (DB != null)
                {
                    if (string.IsNullOrEmpty(DB.ConnStr))
                        this.Connection = DB.ConnectionString();
                    else
                        this.Connection = value.ConnStr;
                    this.Timeout = value.TimeOut;
                }
                else
                {
                    this.Connection = "";
                    this.Timeout = 60;
                }
            }
        }
        /// <summary>
        /// 连接串
        /// </summary>
        public virtual string Connection { get; set; }
        /// <summary>
        /// 超时
        /// </summary>
        public virtual int Timeout { get; set; }
        /// <summary>
        /// 长连接
        /// </summary>
        public virtual bool KeepAlive { get; set; }

        #endregion

        #region 公共方法
         
        /// <summary>
        /// 获取查询数据
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        public virtual DataTable getDataTable(string sql)
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
        public virtual DataTable getDataTable(string sql, out int RecCount)
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
        public virtual DataTable getDataTable(string sql, int PageSize, out int PageCount)
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
        public virtual DataTable getDataTable(string sql, int PageSize, out int RecCount, out int PageCount, string TableName = "")
        {
            PageCount = 0;
            RecCount = 0;
            return null;
        }

        /// <summary>
        /// sql内存分页
        /// </summary>
        /// <param name="sql">查询SQL</param>
        /// <param name="PageIndex">当前页</param>
        /// <param name="PageSize">分页大小</param>
        /// <param name="RecCount">返回查询记录数</param>
        /// <param name="PageCount">返回总页数</param>
        /// <param name="TableName">表名</param>
        /// <returns></returns>
        public virtual DataTable getDataTable(string sql, int PageIndex, int PageSize, out int RecCount, out int PageCount, string TableName = "")
        {
            PageCount = 0;
            RecCount = 0;
            return null;
        }

        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        /// <param name="conn">数据库连接</param>
        public virtual void CloseConnection(DbConnection conn)
        {
            MethodName = "";
            try
            {
                MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            try
            {
                if (conn != null)
                {
                    if (log != null) log(ClassName, MethodName, Log_Type.Infomation, " DbConnection check");
                    if (conn.State != ConnectionState.Closed)
                    {
                        if (log != null) log(ClassName, MethodName, Log_Type.Infomation, " DbConnection close");
                        conn.Close();
                        if (this.DisConnect != null)
                            DisConnect(conn);
                    }
                    if (log != null) log(ClassName, MethodName, Log_Type.Infomation, " DbConnection Dispose");
                    conn.Dispose();
                    conn = null;
                } 
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, MethodName, Log_Type.Error, ex.Message);
                if (this.HasError != null)
                    HasError(ClassName, MethodName, ex);
                else
                    throw ex;
            }
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 获取分页部分
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="Where"></param>
        /// <param name="GroupBy"></param>
        /// <param name="PageSize"></param>
        /// <param name="RecodeCount"></param>
        /// <param name="PageCount"></param>
        protected virtual void Page(string TableName,string Where,string GroupBy,int PageSize, out int RecodeCount, out int PageCount)
        { 
            string CountSql = ""; 
            if (!string.IsNullOrEmpty(GroupBy))
                CountSql = "select count(1) as num from ( select count(1) as xx from " + TableName + " " + Where + " " + GroupBy + " ) DERIVEDTBL";
            else
                CountSql = "select count(1) as num from  " + TableName + " " + Where;
            if (log != null) log(ClassName, MethodName, Log_Type.Test, CountSql);
            DbCommand cmd = Conn.CreateCommand();
            cmd.CommandTimeout = this.Timeout * 1000;
            cmd.CommandText = CountSql;
            RecodeCount = Convert.ToInt32(cmd.ExecuteScalar());
            if (PageSize == 0)
                PageCount = RecodeCount;
            else if (RecodeCount % PageSize == 0)
                PageCount = RecodeCount / PageSize;
            else
                PageCount = (RecodeCount / PageSize) + 1;
        }
         
        #endregion 

    }
}
