using System;
using System.Collections.Generic; 
using System.Data; 
using System.ComponentModel; 
using NK.ENum;
using NK.Entity;
using NK.Event;
using NK.Interface;
using NK.Message;
namespace NK.Data
{
    /// <summary>
    /// 数据库事务
    /// </summary> 
    [DisplayName("DBTransnction")]
    [Description("数据库事务")]
    public class DBTransnction  :IDisposable 
    {

        #region 定义

        private iTransaction DBOper = null;
        private DBInfo DB = new DBInfo();
        private bool m_disposed;
        private string ClassName = "";

        #endregion

        #region 构造函数

        public DBTransnction()
        {
            DB = new DBInfo();
            DB.Mode = DBType.None;
            DB.ConnStr = "";
            DB.TimeOut = 60;
        }

        /// <summary>
        /// 数据库T-SQL基本操作
        /// </summary>
        public DBTransnction(DBInfo info)
        {
            DB = info;
            if (DB == null)
            {
                DB = new DBInfo();
                DB.Mode = DBType.None;
                DB.ConnStr = "";
                DB.TimeOut = 60;
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
        public DBTransnction(DBType ConnectionType, string ConnectionString, int Timeout = 60)
        {
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
        ~DBTransnction()
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

        #region 方法

        /// <summary>
        /// 使用事务
        /// </summary> 
        /// <returns>是否存在</returns>
        [DisplayName("Transaction")]
        [Description("使用事务")]
        public   void Transaction( )
        {
            if (DBOper != null)
               DBOper.Transaction();
        }

        /// <summary>
        /// 提交事务
        /// </summary> 
        /// <returns>是否存在</returns>
        [DisplayName("SaveChange")]
        [Description("提交事务")]
        public void SaveChange(bool Rollback=true)
        {
            if (DBOper != null)
                DBOper.SaveChange(Rollback);
        }

        /// <summary>
        /// 取消事务
        /// </summary> 
        /// <returns>是否存在</returns>
        [DisplayName("Cancel")]
        [Description("取消事务")]
        public void Cancel()
        {
            if (DBOper != null)
                DBOper.Cancel();
        }

        /// <summary>
        /// 检查数据库连接
        /// </summary>
        /// <param name="ErrorMessage">错误信息</param>
        /// <returns>是否连接成功</returns>
        [DisplayName("CheckConnection")]
        [Description("检查数据库连接")]
        public bool CheckConnection(out string ErrorMessage)
        {
            ErrorMessage = "";
            bool res = false;
            if (DBOper == null)
            {
                ErrorMessage = SystemMessage.Badsequencecommands(language);
                return false;
            }
            if (DBOper != null)
                res = DBOper.CheckConnection(out ErrorMessage);
            else
                ErrorMessage = SystemMessage.NotSupported("DBMODE", language); 
            return res; 
        }

        /// <summary>
        /// 查询表是否存在
        /// </summary>
        /// <param name="TableName">表名称</param>
        /// <returns>是否存在</returns>
        [DisplayName("TableIsExist")]
        [Description("查询表是否存在")]
        public bool TableIsExist(string TableName)
        {
            bool res = false;
            if (DBOper == null)
                throw new Exception(SystemMessage.Badsequencecommands(language));
            if (DBOper != null)
                res = DBOper.TableIsExist(TableName); 
            return res;
        }

        /// <summary>
        /// 查询字段属性
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <param name="Field">字段名</param>
        /// <param name="FieldType">字段类型</param>
        /// <param name="CanBeNull">可空</param>
        /// <param name="IsPrimaryKey">主键</param>
        /// <returns>true 字段存在，false 字段不存在</returns>
        [DisplayName("CheckField")]
        [Description("查询字段属性")]
        public bool CheckField(string TableName, string Field, out System.Type FieldType, out bool CanBeNull, out bool IsPrimaryKey)
        {
            bool res = false;
            FieldType = typeof(object);
            CanBeNull = false;
            IsPrimaryKey = false;
            if (DBOper == null)
                throw new Exception(SystemMessage.Badsequencecommands(language));
            if (DBOper != null)
                res = DBOper.CheckField(TableName, Field, out FieldType, out CanBeNull, out IsPrimaryKey);
            return res;
        }

        /// <summary>
        /// 数据库Insert,update,delete带返回执行数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>影响数量</returns>
        [DisplayName("ExecuteNonQuery")]
        [Description("数据库Insert,update,delete带返回执行数")]
        public void ExecuteNonQuery(string sql)
        { 
            if (DBOper == null)
                throw new Exception(SystemMessage.Badsequencecommands(language));
            if (DBOper != null)
                 DBOper.ExecuteNonQuery(sql); 
        }

        /// <summary>
        /// 判断是否存在记录
        /// </summary>
        /// <param name="sql">T-SQL</param>
        /// <returns>TRUE为存在，False为不存在</returns>
        [DisplayName("IsExist")]
        [Description("判断是否存在记录")]
        public bool IsExist(string sql)
        {
            bool res = false;
            if (DBOper == null)
                throw new Exception(SystemMessage.Badsequencecommands(language));
            if (DBOper != null)
                res = DBOper.IsExist(sql); 
            return res;
        }

        /// <summary>
        /// 查询数据记录
        /// </summary>
        /// <param name="sql">T-SQL</param>
        /// <returns>TRUE为存在，False为不存在</returns>
        [DisplayName("Find")]
        [Description("查询数据记录")]
        public Dictionary<string, object> Find(string sql)
        {
            Dictionary<string, object> key = null;
            if (DBOper == null)
                throw new Exception(SystemMessage.Badsequencecommands(language));
            if (DBOper != null)
                key = DBOper.Find(sql); 
            return key;
        }

        /// <summary>
        /// 返回第一行第一列数据
        /// </summary>
        /// <param name="sql">T-SQL</param>
        /// <returns></returns>
        [DisplayName("ExecuteScalar")]
        [Description("返回第一行第一列数据")]
        public object ExecuteScalar(string sql)
        {
            object res = null;
            if (DBOper == null)
                throw new Exception(SystemMessage.Badsequencecommands(language));
            if (DBOper != null)
                res = DBOper.ExecuteScalar(sql); 
            return res;
        }

        /// <summary>
        /// 获取查询数据
        /// </summary>
        /// <param name="sql">T-SQL</param>
        /// <returns>DataTable</returns>
        [DisplayName("getDataTable")]
        [Description("获取查询数据")]
        public DataTable getDataTable(string sql)
        {
            int PageSize = 0;
            int RecCount, PageCount;
            return getDataTable(sql, PageSize, out RecCount, out PageCount);
        }

        /// <summary>
        /// 获取查询数据
        /// </summary>
        /// <param name="sql">T-SQL</param>
        /// <param name="PageSize">分页大小</param>
        /// <param name="RecCount">返回查询记录数</param>
        /// <param name="PageCount">返回总页数</param>
        /// <param name="TableName">表名</param>
        /// <returns>DataTable</returns>
        [DisplayName("getDataTable")]
        [Description("获取查询数据")]
        public DataTable getDataTable(string sql, int PageSize, out int RecCount, out int PageCount, string TableName = "Query")
        {
            RecCount = 0;
            PageCount = 0;
            DataTable dt = null;
            if (DBOper == null)
                throw new Exception(SystemMessage.Badsequencecommands(language));
            if (DBOper != null)
                dt = DBOper.getDataTable(sql, PageSize, out RecCount, out PageCount, TableName); 
            return dt;
        }

        /// <summary>
        /// 内存分页
        /// </summary>
        /// <param name="PageIndex">当前页</param>
        /// <param name="PageSize">分页大小</param>
        /// <param name="DisplayField">字段列，每个字段用,分开</param>
        /// <param name="TableName">表名，支持（） k 视图方式</param>
        /// <param name="Where">查询条件</param>
        /// <param name="OrderBy">排序语句</param>
        /// <param name="GroupBy">GROUP BY 字段</param>
        /// <param name="RecodeCount">返回记录数</param>
        /// <param name="PageCount">返回页数</param>
        /// <returns>查询结果</returns>
        [DisplayName("getDataTableByRam")]
        [Description("内存分页")]
        public DataTable getDataTableByRam(int PageIndex, int PageSize, string DisplayField, string TableName, string Where, string OrderBy, string GroupBy, out int RecodeCount, out int PageCount)
        {
            RecodeCount = 0;
            PageCount = 0;
            DataTable dt = null;
            if (DBOper == null)
                throw new Exception(SystemMessage.Badsequencecommands(language));
            if (DBOper != null)
                dt = DBOper.getDataTableByRam(PageIndex, PageSize, DisplayField, TableName, Where, OrderBy, GroupBy, out RecodeCount, out PageCount);
            return dt;
        }

        /// <summary>
        /// 数据库分页
        /// </summary>
        /// <param name="PageIndex">当前页</param>
        /// <param name="PageSize">分页大小</param>
        /// <param name="DisplayField">字段列，每个字段用,分开</param>
        /// <param name="TableName">表名，支持（） k 视图方式</param>
        /// <param name="Where">查询条件</param>
        /// <param name="OrderField">排序字段</param>
        /// <param name="OrderBy">排序语句</param>
        /// <param name="GroupBy">GROUP BY 字段</param>
        /// <param name="RecodeCount">返回记录数</param>
        /// <param name="PageCount">返回页数</param>
        /// <returns>查询结果</returns>
        [DisplayName("getDataTableByDB")]
        [Description("数据库分页")]
        public DataTable getDataTableByDB(int PageIndex, int PageSize, string DisplayField, string TableName, string Where, string OrderField, string OrderBy, string GroupBy, out int RecodeCount, out int PageCount)
        {
            RecodeCount = 0;
            PageCount = 0;
            DataTable dt = null;
            if (DBOper == null)
                throw new Exception(SystemMessage.Badsequencecommands(language));
            if (DBOper != null)
                dt = DBOper.getDataTableByDB(PageIndex, PageSize, DisplayField, TableName, Where, OrderField, OrderBy, GroupBy, out RecodeCount, out PageCount);
            return dt;
        }

        /// <summary>
        /// 获取所有表
        /// </summary>
        [DisplayName("Tables")]
        [Description("获取所有表")]
        List<string> Tables
        {
            get
            {
                List<string> res = new List<string>();
                if (DBOper == null)
                    throw new Exception(SystemMessage.Badsequencecommands(language));
                if (DBOper != null)
                    res = DBOper.Tables; 
                return res;
            }
        }

        /// <summary>
        /// 获取所有视图
        /// </summary>
        [DisplayName("Views")]
        [Description("获取所有视图")]
        List<string> Views
        {
            get
            {
                List<string> res = new List<string>();
                if (DBOper == null)
                    throw new Exception(SystemMessage.Badsequencecommands(language));
                if (DBOper != null)
                    res = DBOper.Views; 
                return res;
            }
        }

        /// <summary>
        /// 列出表字段
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <returns></returns>
        [DisplayName("Columns")]
        [Description("列出表字段")]
        List<string> Columns(string TableName)
        {
            List<string> res = new List<string>();
            if (DBOper == null)
                throw new Exception(SystemMessage.Badsequencecommands(language));
            if (DBOper != null)
                res = DBOper.Columns(TableName); 
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
        /// <summary>
        /// 连接事件
        /// </summary>
        public event DBEvent.Connect Connect = null;
        /// <summary>
        /// 连接断开
        /// </summary>
        public event DBEvent.DisConnect DisConnect = null;

        #endregion

    }
}
