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
    /// 历史表模拟视图
    /// </summary>
    [DisplayName("SimViewHistoryHelper")]
    [Description("历史表模拟视图")]
    public class SimViewHistoryHelper : IDisposable
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
        public SimViewHistoryHelper()
        {
            ClassName = this.GetType().ToString();
            this.language = Language.Chinese;
        }


        /// <summary>
        /// 表操作
        /// </summary>
        /// <param name="TSql">查询语句</param>
        /// <param name="info">数据库信息</param>
        public SimViewHistoryHelper(string TSql, DBInfo info = null)
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
        public SimViewHistoryHelper(string TSql, DBType ConnectionType, string ConnectionString, int Timeout = 60)
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
        ~SimViewHistoryHelper()
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

        private List<string> MakeMonthTable(iDataBase Oper, DateTime SDT, DateTime EDT)
        {
            List<string> res = new List<string>();
            if (!string.IsNullOrEmpty(this.TSQL) && SDT != null & EDT != null && Oper != null)
            {
                DateTime DT = DateTime.Now;
                int i = 0;
                while ((DT = SDT.AddMonths(i)) < EDT)
                {
                    string DTS = DT.Year.ToString() + (DT.Month > 9 ? DT.Month.ToString() : "0" + DT.Month.ToString());
                    string tab = string.Join(TSQL, new object[] { DTS });
                    string sql = "SELECT * From (" + TSQL + ") Tab Where 1<>1 ";
                    try
                    {
                        Oper.getDataTable(sql);
                        res.Add(tab);
                    }
                    catch
                    { }
                    i++;
                }
            }
            return res;
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
        /// 查找记录
        /// </summary>
        /// <param name="WHERE">查询条件</param>
        /// <param name="SDT">开始时间</param>
        /// <param name="EDT">结束时间</param>
        /// <param name="IsUnique">是否唯一</param>
        /// <returns></returns>
        [DisplayName("Find")]
        [Description("查找记录")]
        public Dictionary<string, object> Find(string WHERE, DateTime SDT, DateTime EDT , bool IsUnique = false)
        {
            if (string.IsNullOrEmpty(this.TSQL)) return null;
            else if (!TSQL.Contains("{0}")) return null;
            if (string.IsNullOrEmpty(WHERE)) return null; 
            if (!WHERE.ToUpper().Contains("WHERE")) WHERE = " WHERE " + WHERE;
            try
            {
                iDataBase DBOper = MakeConnection();
                if (DBOper != null)
                {
                    List<string> MonthTab = MakeMonthTable(DBOper, SDT, EDT); 
                    string view = "";
                    foreach (string Month in MonthTab)
                    {
                        if (string.IsNullOrEmpty(view))
                            view = "SELECT * FROM (" + Month + ") Tab \r\n";
                        else
                            view += (IsUnique ? " UNION \r\n" : " UNION ALL \r\n") + "SELECT * FROM (" + Month + ") Tab \r\n"; 
                    } 
                    string Sql = "SELECT * From (" + view + ") SimView  " + WHERE;
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
        /// <param name="WHERE">查询条件语句</param>
        /// <param name="SDT">开始时间</param>
        /// <param name="EDT">结束时间</param>
        /// <param name="AllInOne">整合到一个DT</param>
        /// <param name="IsUnique">是否可重复</param>
        /// <returns></returns>
        [DisplayName("Query")]
        [Description("查询记录")]
        public DataSet Query(string WHERE, DateTime SDT, DateTime EDT, bool AllInOne = true, bool IsUnique = false)
        {
            if (string.IsNullOrEmpty(this.TSQL)) return null;
            else if (!TSQL.Contains("{0}")) return null;
            if (!string.IsNullOrEmpty(WHERE))
            { if (!WHERE.ToUpper().Contains("WHERE")) WHERE = " WHERE " + WHERE; }
            else
            { WHERE = ""; }
            try
            { 
                iDataBase DBOper = MakeConnection();
                if (DBOper != null)
                {
                    List<string> MonthTab = MakeMonthTable(DBOper, SDT, EDT); 
                    DataSet DS = new DataSet();
                    if (AllInOne)
                    {
                        string view = "";
                        foreach (string Month in MonthTab)
                        {
                            if (string.IsNullOrEmpty(view))
                                view = "SELECT * FROM (" + Month + ") Tab \r\n";
                            else
                                view += (IsUnique ? " UNION \r\n" : " UNION ALL \r\n") + "SELECT * FROM (" + Month + ") Tab \r\n";
                        }
                        string Sql = "SELECT * From (" + view + ") SimView  " + WHERE;
                        DataTable dt = DBOper.getDataTable(Sql);
                        DS.Tables.Add(dt);
                    }
                    else
                    {
                        foreach (string Month in MonthTab)
                        {
                            string Sql = "SELECT * From (" + Month + ") SimView  " + WHERE;
                            DataTable dt = DBOper.getDataTable(Sql);
                            DS.Tables.Add(dt);
                        }
                    }
                    return DS;
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
        /// <param name="SDT">开始时间</param>
        /// <param name="EDT">结束时间</param>
        /// <param name="PageIndex">当前页</param>
        /// <param name="PageSize">分页大小</param>
        /// <param name="WHERE">查询条件</param>
        /// <param name="Orderby">排序</param>
        /// <param name="PageCount">总页数</param>
        /// <param name="RecordCount">总记录数</param>
        /// <param name="AllInOne">整合到一个DT</param>
        /// <param name="IsUnique">是否可重复</param>
        /// <returns></returns>
        [DisplayName("Select")]
        [Description("查询分页")]
        public DataSet Select(DateTime SDT, DateTime EDT,int PageIndex, int PageSize, string WHERE, string Orderby, out int PageCount, out int RecordCount, bool AllInOne = true, bool IsUnique = false)
        {
            PageCount = 0;
            RecordCount = 0;
            if (string.IsNullOrEmpty(this.TSQL)) return null;
            else if (!TSQL.Contains("{0}")) return null;
            if (!string.IsNullOrEmpty(WHERE))
            { if (!WHERE.ToUpper().Contains("WHERE")) WHERE = " WHERE " + WHERE; }
            else
            { WHERE = ""; }
            if (!string.IsNullOrEmpty(Orderby))
            { if (!Orderby.ToUpper().Contains("Order")) Orderby = " Order By " + Orderby; }
            else
            { Orderby = ""; }
            try
            {
                iDataBase DBOper = MakeConnection();
                if (DBOper != null)
                {
                    List<string> MonthTab = MakeMonthTable(DBOper, SDT, EDT);
                    DataSet DS = new DataSet();
                    if (AllInOne)
                    {
                        string view = "";
                        foreach (string Month in MonthTab)
                        {
                            if (string.IsNullOrEmpty(view))
                                view = "SELECT * FROM (" + Month + ") Tab \r\n";
                            else
                                view += (IsUnique ? " UNION \r\n" : " UNION ALL \r\n") + "SELECT * FROM (" + Month + ") Tab \r\n";
                        } 
                        DataTable dt = DBOper.getDataTableByRam(PageIndex, PageSize, "*", view, WHERE, Orderby, "", out RecordCount, out PageCount);
                        DS.Tables.Add(dt);
                    }
                    else
                    {
                        foreach (string Month in MonthTab)
                        { 
                            DataTable dt = DBOper.getDataTableByRam(PageIndex, PageSize, "*", Month, WHERE, Orderby, "", out RecordCount, out PageCount);
                            DS.Tables.Add(dt);
                        }
                    }
                    return DS;
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
