using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using LinqToDB;
using System.ComponentModel;
using LinqToDB.Mapping;
using NK.ENum;
using NK.Entity;
using NK.Event;
using NK.Interface;
using NK.Message;
namespace NK.Data.Helper
{ 
    /// <summary>
    /// 表处理类
    /// </summary>
    [DisplayName("SimViewHelper")]
    [Description("模拟视图")]
    public class SimViewHelper : IDisposable
    {
        #region 定义
        private DBInfo DB = new DBInfo(); 
        private bool m_disposed;
        private string ClassName = "";
        #endregion

        #region 构造函数

        /// <summary>
        /// 表操作
        /// </summary>
        public SimViewHelper()
        {
            ClassName = this.GetType().ToString();
            this.language = Language.Chinese;
        }
         
        /// <summary>
        /// 表操作
        /// </summary>
        /// <param name="TSql">查询语句</param>
        /// <param name="info">数据库信息</param>
        public SimViewHelper(string TSql, DBInfo info = null)
        {
            TSQL = TSql;
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
                this.DataBaseType = DBType.None;
                this.Connection = "";
                this.Timeout = 60;
            } 
            ClassName = this.GetType().ToString();
            this.language = Language.Chinese;
        }

        /// <summary>
        /// 表操作
        /// </summary>
        /// <param name="TSql">查询语句</param>
        /// <param name="ConnectionType">数据库类型</param>
        /// <param name="ConnectionString">连接串</param>
        /// <param name="Timeout">超时时间，毫秒</param>
        public SimViewHelper(string TSql, DBType ConnectionType, string ConnectionString, int Timeout = 60)
        {
            TSQL = TSql;
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
        ~SimViewHelper()
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
        /// 查询语句
        /// </summary>
        [DisplayName("TSQL")]
        [Description("查询语句")]
        public string TSQL { get; set; }

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

        #region 私有方法

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
                case DBType.ODBC:
                    DBOper = new ODBC(Connection, Timeout);
                    break;
                default:
                    DBOper = null;
                    break;
            }
            if (DBOper != null)
            {
                if (this.log != null)
                    DBOper.log += log;
            }
            return DBOper;
        }
         
        #endregion

        #region 方法
         
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
        /// 显示表所有字段
        /// </summary>
        [DisplayName("Column")]
        [Description("表所有字段")]
        public Dictionary<ColumnAttribute, Type> Column
        {
            get
            {
                try
                {
                    Dictionary<ColumnAttribute, Type> res = new Dictionary<ColumnAttribute, Type>();
                    iDataBase DBOper = MakeConnection();
                    if (DBOper != null && !string.IsNullOrEmpty(TSQL))
                    {
                        string sql = "SELECT * FROM ("+TSQL+") Tab where 1<>1";
                        DataTable dt = DBOper.getDataTable(sql);
                        if (dt != null)
                        {
                            for (int i = 0; i < dt.Columns.Count; i++)
                            {
                                var Col = dt.Columns[i];
                                ColumnAttribute col = new ColumnAttribute();
                                col.Name = Col.ColumnName;
                                res.Add(col, Col.DataType);
                            }
                        }
                    }
                    return res;
                }
                catch (Exception ex)
                {
                    if (HasError != null)
                        HasError(ClassName, "Drop", ex);
                    else
                        throw ex;
                    return new Dictionary<ColumnAttribute, Type>();
                }
            } 
        }
         
        /// <summary>
        /// 查找记录
        /// </summary>
        /// <param name="WHERE">查询条件语句</param>
        /// <returns>字段及值</returns>
        [DisplayName("Find")]
        [Description("查找记录")]
        public Dictionary<string, object> Find(string WHERE)
        {
            if (string.IsNullOrEmpty(this.TSQL)) return null;
            if (string.IsNullOrEmpty(WHERE)) return null;
            if (!WHERE.ToUpper().Contains("WHERE")) WHERE = " WHERE " + WHERE;
            try
            { 
                iDataBase DBOper = MakeConnection();
                if (DBOper != null)
                {
                    string Sql = "SELECT * FROM (" + TSQL + ") Tab " + WHERE;
                    Dictionary<string, object> res = DBOper.Find(Sql);
                    return res;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                if (HasError != null)
                    HasError(ClassName, "Find", ex);
                else
                    throw ex;
                return null;
            }
        }

        /// <summary>
        /// 查询记录
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="order">排序条件</param>
        /// <returns></returns>
        [DisplayName("Query")]
        [Description("查询记录")]
        public DataTable Query(string where = "", string order = "")
        {
            if (string.IsNullOrEmpty(this.TSQL)) return null;
            if (!string.IsNullOrEmpty(where))
            { if (!where.ToUpper().Contains("WHERE")) where = " WHERE " + where; }
            else
            { where = ""; }
            if (!string.IsNullOrEmpty(order))
            { if (!order.ToUpper().Contains("OREDR")) order = " ORDER BY " + order; }
            else
            { order = ""; }
            try
            {
                string Sql = "SELECT * FROM (" + TSQL + ") Tab " + where+ order;
                iDataBase DBOper = MakeConnection();
                if (DBOper != null)
                { 
                    DataTable DT = DBOper.getDataTable(Sql); 
                    return DT;
                }
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
        /// 查询分页
        /// </summary>
        /// <param name="PageIndex">当前页</param>
        /// <param name="PageSize">分页大小</param>
        /// <param name="where">查询条件</param>
        /// <param name="orderby">排序条件</param>
        /// <param name="PageCount">总页数</param>
        /// <param name="RecordCount">总记录数</param>
        /// <param name="ASCDESC">顺序倒叙</param>
        /// <returns></returns>
        [DisplayName("Select")]
        [Description("查询分页")]
        public DataTable Select(int PageIndex, int PageSize, out int PageCount, out int RecordCount, string where = "", string orderby = "", bool ASCDESC = false)
        {
            PageCount = 0;
            RecordCount = 0;
            if (string.IsNullOrEmpty(this.TSQL)) return null;
            if (!string.IsNullOrEmpty(where))
            { if (!where.ToUpper().Contains("WHERE")) where = " WHERE " + where; }
            else
            { where = ""; }
            if (!string.IsNullOrEmpty(orderby))
            { if (!orderby.ToUpper().Contains("OREDR")) orderby = " ORDER BY " + orderby; }
            else
            { orderby = ""; }
            try
            {
                iDataBase DBOper = MakeConnection();
                if (DBOper != null)
                {
                    DataTable DT = DBOper.getDataTableByRam(PageIndex, PageSize, "", this.TSQL, where, orderby, "", out RecordCount, out PageCount); 
                    return DT;
                }
            }
            catch (Exception ex)
            {
                if (HasError != null)
                    HasError(ClassName, "Select", ex);
                else
                    throw ex;
            }
            return null;
        }

        #endregion

    }
}
