using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NK.ENum;
using NK.Entity;
using NK.Event;
using System.ComponentModel;
using System.Reflection;
using System.Data.Common;
using LinqToDB.Mapping;
using LinqToDB;

namespace NK.Data
{
    /// <summary>
    /// 数据库T-SQL操作
    /// </summary> 
    public class IDataBase : ControllerHelper, IDisposable
    {

        #region 构造函数

        private void init()
        {
            initialization();
            Init();
            if (this.DBOper != null)
            {
                this.DBOper.KeepAlive = this.KeepAlive;
                if (this.Connect != null)
                    DBOper.Connect += this.Connect;
                if (this.DisConnect != null)
                    DBOper.DisConnect += this.DisConnect;
                if (this.log != null)
                    DBOper.log += this.log;
                if (this.HasError != null)
                    DBOper.HasError += this.HasError;
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        protected virtual void Init()
        {
            
        }

        /// <summary>
        /// 数据库T-SQL基本操作
        /// </summary>
        public IDataBase():base()
        {
            ClassName = this.GetType().ToString();
            this.language = Language.Chinese;
        }

        /// <summary>
        /// 数据库T-SQL基本操作
        /// </summary>
        public IDataBase(DBInfo info = null) : base(info)
        {
            ClassName = this.GetType().ToString();
            this.language = Language.Chinese;
        }

        /// <summary>
        /// 数据库T-SQL基本操作
        /// </summary>
        /// <param name="ConnectionType">数据库类型</param>
        /// <param name="ConnectionString">连接串</param>
        /// <param name="Timeout">超时时间，毫秒</param>
        public IDataBase(DBType ConnectionType, string ConnectionString, int Timeout = 60) : base(ConnectionType, ConnectionString, Timeout)
        {
            ClassName = this.GetType().ToString();
            this.language = Language.Chinese;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        ~IDataBase()
        {
            Dispose(false);
        }

        #endregion

        #region 方法

        /// <summary>
        /// 查询表是否存在
        /// </summary>
        /// <param name="TableName">表名称</param>
        /// <returns>是否存在</returns>
        [DisplayName("TableIsExist")]
        [Description("查询表是否存在")]
        public bool TableIsExist(string TableName)
        {

            MethodName = "";
            try
            {
                MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            bool res = false;
            try
            {
                init();
                res = DBOper.TableIsExist(TableName);
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

            MethodName = "";
            try
            {
                MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            bool res = false;
            FieldType = typeof(object);
            CanBeNull = false;
            IsPrimaryKey = false;
            try
            {
                init();
                res = DBOper.CheckField(TableName, Field, out FieldType, out CanBeNull, out IsPrimaryKey);
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
        /// 判断是否存在记录
        /// </summary>
        /// <param name="sql">T-SQL</param>
        /// <returns>TRUE为存在，False为不存在</returns>
        [DisplayName("IsExist")]
        [Description("判断是否存在记录")]
        public bool IsExist(string sql)
        {

            MethodName = "";
            try
            {
                MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            bool res = false;
            try
            {
                init();
                res = DBOper.IsExist(sql);
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
        /// 查询数据记录
        /// </summary>
        /// <param name="sql">T-SQL</param>
        /// <returns>TRUE为存在，False为不存在</returns>
        [DisplayName("Find")]
        [Description("查询数据记录")]
        public Dictionary<string, object> Find(string sql)
        {
            MethodName = "";
            try
            {
                MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            Dictionary<string, object> res = null;
            try
            {
                init();
                res = DBOper.Find(sql);
            }
            catch (Exception ex)
            {
                if (HasError != null)
                    HasError(ClassName, MethodName, ex);
                else
                    throw ex;
            }
            if (res == null) res = new Dictionary<string, object>();
            return res;
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

            MethodName = "";
            try
            {
                MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            object res = null;
            try
            {
                init();
                res = DBOper.ExecuteScalar(sql);
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
        /// 获取查询数据
        /// </summary>
        /// <param name="sql">T-SQL</param>
        /// <returns>DataTable</returns>
        [DisplayName("getDataTable")]
        [Description("获取查询数据")]
        public DataTable getDataTable(string sql)
        {

            MethodName = "";
            try
            {
                MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            int PageSize = 0;
            int RecCount, PageCount;
            DataTable res = null;
            try
            {
                init();
                res = getDataTable(sql, PageSize, out RecCount, out PageCount);
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

            MethodName = "";
            try
            {
                MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            RecCount = 0;
            PageCount = 0;
            DataTable res = null;
            try
            {
                init();
                res = DBOper.getDataTable(sql, PageSize, out RecCount, out PageCount, TableName);
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
        /// 获取查询数据
        /// </summary>
        /// <param name="sql">T-SQL</param>
        /// <param name="PageIndex">当前页</param>
        /// <param name="PageSize">分页大小</param>
        /// <param name="RecCount">返回查询记录数</param>
        /// <param name="PageCount">返回总页数</param>
        /// <param name="TableName">表名</param>
        /// <returns>DataTable</returns>
        [DisplayName("getDataTable")]
        [Description("获取查询数据")]
        public DataTable getDataTable(string sql, int PageIndex, int PageSize, out int RecCount, out int PageCount, string TableName = "Query")
        {
            MethodName = "";
            try
            {
                MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            RecCount = 0;
            PageCount = 0;
            DataTable res = null;
            try
            {
                init();
                res = DBOper.getDataTable(sql, PageIndex, PageSize, out RecCount, out PageCount, TableName);
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

            MethodName = "";
            try
            {
                MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            RecodeCount = 0;
            PageCount = 0;
            DataTable res = null;
            try
            {
                init();
                res = DBOper.getDataTableByRam(PageIndex, PageSize, DisplayField, TableName, Where, OrderBy, GroupBy, out RecodeCount, out PageCount);
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

            MethodName = "";
            try
            {
                MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            RecodeCount = 0;
            PageCount = 0;
            DataTable res = null;
            try
            {
                init();
                res = DBOper.getDataTableByDB(PageIndex, PageSize, DisplayField, TableName, Where, OrderField, OrderBy, GroupBy, out RecodeCount, out PageCount);
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
        /// 获取所有表
        /// </summary>
        [DisplayName("Tables")]
        [Description("获取所有表")]
        public List<string> Tables
        {
            get
            {
                MethodName = "";
                try
                {
                    MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                    MethodName = method.Name;
                }
                catch { }
                List<string> res = null;
                try
                {
                    init();
                    res = DBOper.Tables;
                }
                catch (Exception ex)
                {
                    if (HasError != null)
                        HasError(ClassName, MethodName, ex);
                    else
                        throw ex;
                }
                if (res == null) res = new List<string>();
                return res;
            }
        }

        /// <summary>
        /// 获取所有视图
        /// </summary>
        [DisplayName("Views")]
        [Description("获取所有视图")]
        public List<string> Views
        {
            get
            {
                MethodName = "";
                try
                {
                    MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                    MethodName = method.Name;
                }
                catch { }
                List<string> res = null;
                try
                {
                    init();
                    res = DBOper.Views;
                }
                catch (Exception ex)
                {
                    if (HasError != null)
                        HasError(ClassName, MethodName, ex);
                    else
                        throw ex;
                }
                if (res == null) res = new List<string>();
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
        public Dictionary<DataColumn,bool> Columns(string TableName)
        {
            MethodName = "";
            try
            {
                MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            Dictionary<DataColumn, bool> res =new  Dictionary<DataColumn, bool>();
            try
            {
                init();
                Dictionary<ColumnAttribute, Type> cols = DBOper.Columns(TableName);
                List<ColumnAttribute> ls = cols.Keys.ToList();
                if (ls.Where(c => c.IsPrimaryKey).Count() <= 0)
                {
                    var index =  ls.FindIndex(c => c.Name.ToUpper().Contains("ID") || c.Name.ToUpper().Contains("INDEX") || c.IsIdentity);
                    if (index > -1)
                        ls[index].IsPrimaryKey = true;
                } 
                if (ls.Where(c => c.IsIdentity).Count() <= 0)
                {
                    var index = ls.FindIndex(c => c.IsPrimaryKey &&
                                                  (
                                                       c.DataType == DataType.Int16 || c.DataType == DataType.Int32 || c.DataType == DataType.Int64
                                                   || c.DataType == DataType.UInt16 || c.DataType == DataType.UInt32 || c.DataType == DataType.UInt64
                                                  ));
                    if (index > -1)
                        ls[index].IsIdentity = true;
                }
                foreach (var dic in ls)
                {
                    var dicm = cols.FirstOrDefault(c => c.Key.Name == dic.Name);
                    DataColumn dc = new DataColumn();
                    dc.AllowDBNull = dic.CanBeNull;
                    dc.ColumnName = dic.Name;
                    dc.DataType = dicm.Value;
                    dc.AutoIncrement = dic.IsIdentity;
                    res.Add(dc, dic.IsPrimaryKey);
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
                return DBOper.CheckConnection(out errmsg);
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
                MethodName = "";
                try
                {
                    MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                    MethodName = method.Name;
                }
                catch { }
                try
                {
                    init();
                    DbConnection Conn = DBOper.GetConnection();
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
            MethodName = "";
            try
            {
                MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            int res = -1;
            try
            {
                init();
                res = DBOper.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                CatchErr(ClassName, MethodName, ex);
                res = -2;
            }
            return res;
        }


        #endregion

        #region 事件

        /// <summary>
        /// 连接处理事件
        /// </summary>
        public DBEvent.Connect Connect { get; set; }

        /// <summary>
        /// 断开处理事件
        /// </summary>
        public DBEvent.DisConnect DisConnect { get; set; }

        #endregion

    }
}
