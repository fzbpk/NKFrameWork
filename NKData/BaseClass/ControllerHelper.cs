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
using NK.Message;
using NK.Interface; 

namespace NK.Data
{
    /// <summary>
    /// TSQL处理基类
    /// </summary>
    public  class ControllerHelper
    {

        #region 定义
        protected DBInfo DB = new DBInfo();
        protected iDataBase DBOper = null;
        protected string connstr = "";
        protected bool m_disposed;
        protected string ClassName = "";
        protected string MethodName = "";
        #endregion

        #region 构造函数

        /// <summary>
        /// 初始化
        /// </summary>
        protected void initialization()
        { 
            if (DBOper == null)
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
                    if (DBOper != null)
                    {
                        DBOper.Dispose();
                        DBOper = null;
                    }
                    m_disposed = true;
                }
            }
        }

        /// <summary>
        /// TSQL处理基类
        /// </summary>
        protected ControllerHelper()
        {
            DB = new DBInfo();
            DB.Mode = DBType.None;
            DB.ConnStr = "";
            DB.TimeOut = 60;
        }

        /// <summary>
        /// TSQL处理基类
        /// </summary>
        protected ControllerHelper(DBInfo db)
        {
            DB = db;
            if (DB == null)
            {
                DB = new DBInfo();
                DB.Mode = DBType.None;
                DB.ConnStr = "";
                DB.TimeOut = 60;
            }
            else if(string.IsNullOrEmpty(DB.ConnStr))
            {
                DB.ConnStr = DB.ConnectionString();
            }
        }

        /// <summary>
        /// TSQL处理基类
        /// </summary>
        protected ControllerHelper(DBType ConnectionType, string ConnectionString, int Timeout = 60)
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
        /// 错误事件
        /// </summary>
        /// <param name="Class"></param>
        /// <param name="func"></param>
        /// <param name="ex"></param>
        protected void CatchErr(string Class, string func, Exception ex)
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

        #region 私有

        /// <summary>
        /// 获取字段
        /// </summary>
        /// <param name="Table"></param>
        /// <returns></returns>
        protected List<ColumnAttribute> Cols(string Table)
        {
            List<ColumnAttribute> cos = new List<ColumnAttribute>();
            cos = DBOper.Columns(Table).Keys.ToList(); 
            if (cos.Where(c => c.IsPrimaryKey).Count() <= 0)
            {
                var index = cos.FindIndex(c => c.Name.ToUpper().Contains("ID") || c.Name.ToUpper().Contains("INDEX") || c.IsIdentity);
                if (index > -1)
                    cos[index].IsPrimaryKey = true;
            }
            if (cos.Where(c => c.IsPrimaryKey).Count() <= 0)
                throw new NullReferenceException(SystemMessage.RefNullOrEmpty("PrimaryKey", language));
            if (cos.Where(c => c.IsIdentity).Count() <= 0)
            {
                var index = cos.FindIndex(c => c.IsPrimaryKey && 
                                              (
                                                   c.DataType== DataType.Int16 || c.DataType == DataType.Int32 || c.DataType == DataType.Int64
                                               || c.DataType == DataType.UInt16 || c.DataType == DataType.UInt32 || c.DataType == DataType.UInt64
                                              ));
                if (index > -1)
                    cos[index].IsIdentity = true;
            }
            return cos;
        }
 
        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="Table"></param>
        /// <param name="Column"></param>
        /// <param name="SCOL"></param>
        protected void Insert(string Table, Dictionary<string, object> Column, List<ColumnAttribute> SCOL)
        {
            Dictionary<string, object> cols = new Dictionary<string, object>(); 
            foreach (var col in SCOL)
            {
                if (Column.Where(c => c.Key.ToUpper().Trim() == col.Name.ToUpper().Trim()).Count() < 0 && !col.CanBeNull && !col.IsIdentity)
                    throw new NullReferenceException(SystemMessage.RefNullOrEmpty(col.Name, language));
            }
            foreach (var col in Column)
            { 
                var tmp = SCOL.FirstOrDefault(c => c.Name.ToUpper().Trim() == col.Key.ToUpper().Trim());
                if (tmp != null)
                {
                    if (tmp.IsIdentity) continue;
                    if (col.Value == null)
                    {
                        if (!tmp.CanBeNull && !tmp.IsPrimaryKey)
                            throw new NullReferenceException(SystemMessage.RefNullOrEmpty(col.Key, language));
                    }
                    cols.Add(col.Key, col.Value);
                } 
            }
            if (cols.Count > 0)
            {
                string Sql = Table.InsertToSQL(DataBaseType, cols);
                DBOper.ExecuteNonQuery(Sql);
            }
        }
        
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="Table"></param>
        /// <param name="Column"></param>
        /// <param name="SCOL"></param>
        protected void Update(string Table, Dictionary<string, object> Column,List<ColumnAttribute> SCOL)
        { 
            Dictionary<string, object> cols = new Dictionary<string, object>();
            string Where = "";
            foreach (var col in Column)
            {
                var tmp = SCOL.FirstOrDefault(c => c.Name.ToUpper().Trim() == col.Key.ToUpper().Trim());
                if (col.Value == null)
                {
                    if (!tmp.CanBeNull && !tmp.IsPrimaryKey)
                        throw new NullReferenceException(SystemMessage.RefNullOrEmpty(col.Key, language));
                }
                if (tmp.IsPrimaryKey)
                    continue;
                else if (tmp.IsIdentity)
                    continue;
                cols.Add(col.Key, col.Value);
            }
            if (SCOL.Where(c => c.IsPrimaryKey).Count() > 0)
            {
                var tmp = SCOL.FirstOrDefault(c => c.IsPrimaryKey);
                if (Column.Where(c => c.Key.ToUpper().Trim() == tmp.Name.ToUpper().Trim()).Count() > 0)
                {
                    var tmpID = Column.FirstOrDefault(c => c.Key.ToUpper().Trim() == tmp.Name.ToUpper().Trim());
                    Type t = tmpID.Value.GetType();
                    if (t == typeof(string))
                        Where = " where " + tmpID.Key + " ='" + tmpID.Value.ToString() + "' ";
                    else if (t == typeof(DateTime))
                        Where = " where " + tmpID.Key + " ='" + tmpID.Value.ToString() + "' ";
                    else if (t.IsValueType)
                        Where = " where " + tmpID.Key + " =" + tmpID.Value.ToString() + "";
                }
            }
            else
                throw new KeyNotFoundException(SystemMessage.RefNullOrEmpty("PriKey", language));
            if (cols.Count > 0 && !string.IsNullOrEmpty(Where))
            {
                string Sql = Table.UpdateToSQL(DataBaseType, cols) + Where;
                DBOper.ExecuteNonQuery(Sql);
            }
        }

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="Table"></param>
        /// <param name="Column"></param>
        /// <param name="SCOL"></param>
        protected void Delete(string Table, Dictionary<string, object> Column, List<ColumnAttribute> SCOL)
        { 
            string Where = ""; 
            if (SCOL.Where(c => c.IsPrimaryKey).Count() > 0)
            {
                var tmp = SCOL.FirstOrDefault(c => c.IsPrimaryKey);
                if (Column.Where(c => c.Key.ToUpper().Trim() == tmp.Name.ToUpper().Trim()).Count() > 0)
                {
                    var tmpID = Column.FirstOrDefault(c => c.Key.ToUpper().Trim() == tmp.Name.ToUpper().Trim());
                    Type t = tmpID.Value.GetType();
                    if (t == typeof(string))
                        Where = " where " + tmpID.Key + " ='" + tmpID.Value.ToString() + "' ";
                    else if (t == typeof(DateTime))
                        Where = " where " + tmpID.Key + " ='" + tmpID.Value.ToString() + "' ";
                    else if (t.IsValueType)
                        Where = " where " + tmpID.Key + " =" + tmpID.Value.ToString() + "";
                }
            }
            else
                throw new KeyNotFoundException(SystemMessage.RefNullOrEmpty("PriKey", language));
            if ( !string.IsNullOrEmpty(Where))
            {
                string Sql = Table.DeleteToSQL() + Where;
                DBOper.ExecuteNonQuery(Sql);
            }
        }
        
        /// <summary>
        /// 创建表
        /// </summary>
        /// <param name="Table"></param>
        /// <param name="SCOL"></param>
        /// <returns></returns>
        protected string Creat(string Table, List<ColumnAttribute> SCOL)
        {
            string tab = Table;
            if (!DBOper.TableIsExist(tab))
            { 
                List<string> sqlbat = tab.CreatToSql(DB.Mode, SCOL);
                foreach (var tsql in sqlbat)
                    DBOper.ExecuteNonQuery(tsql);
            }
            return DBOper.TableIsExist(tab) ? tab : "";
        }

        /// <summary>
        /// 创建历史表
        /// </summary>
        /// <param name="Table"></param>
        /// <param name="SCOL"></param>
        /// <param name="DT"></param>
        /// <returns></returns>
        protected string CreatHis(string Table, List<ColumnAttribute> SCOL, DateTime DT)
        {
            string tab = Table + DT.ToString("yyyyMM");
            if (!DBOper.TableIsExist(tab))
            { 
                List<string> sqlbat = tab.CreatToSql(DB.Mode, SCOL);
                foreach (var tsql in sqlbat)
                    DBOper.ExecuteNonQuery(tsql);
            }
            return DBOper.TableIsExist(tab) ? tab : "";
        }
        
        #endregion
         
    }
}
