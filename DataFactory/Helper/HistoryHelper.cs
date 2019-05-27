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
    /// 历史表处理类,需要依赖主表
    /// </summary>
    [DisplayName("HistoryHelper")]
    [Description("历史表处理类")]
    public   class HistoryHelper : IDisposable
    {
        #region 定义
        private DBInfo DB = new DBInfo();
        private Dictionary<ColumnAttribute, Type> cols = new Dictionary<ColumnAttribute, Type>();
        private bool m_disposed;
        private string ClassName = "";
        #endregion

        #region 构造函数

        /// <summary>
        /// 表操作
        /// </summary>
        public HistoryHelper()
        {
            if (cols.Count > 0)
                cols.Clear();
            ClassName = this.GetType().ToString();
            this.language = Language.Chinese;
        }

        /// <summary>
        /// 表操作
        /// </summary>
        /// <param name="TableName">表</param>
        /// <param name="info">数据库信息</param>
        public HistoryHelper(string TableName, DBInfo info = null)
        {
            Table = TableName;
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
            if (cols.Count > 0)
                cols.Clear();
            ClassName = this.GetType().ToString();
            this.language = Language.Chinese;
        }

        /// <summary>
        /// 表操作
        /// </summary>
        /// <param name="TableName">表</param>
        /// <param name="ConnectionType">数据库类型</param>
        /// <param name="ConnectionString">连接串</param>
        /// <param name="Timeout">超时时间，毫秒</param>
        public HistoryHelper(string TableName, DBType ConnectionType, string ConnectionString, int Timeout = 60)
        {
            Table = TableName;
            this.Connection = ConnectionString;
            this.DataBaseType = ConnectionType;
            this.Timeout = Timeout;
            DB = new DBInfo();
            DB.Mode = ConnectionType;
            DB.ConnStr = ConnectionString;
            DB.TimeOut = Timeout;
            if (cols.Count > 0)
                cols.Clear();
            ClassName = this.GetType().ToString();
            this.language = Language.Chinese;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        ~HistoryHelper()
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
        /// 表
        /// </summary>
        [DisplayName("Table")]
        [Description("表")]
        public string Table { get; set; }

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
            if (string.IsNullOrEmpty(this.Table)) return null;
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

        private Dictionary<ColumnAttribute, Type> GetCol()
        {
            Dictionary<ColumnAttribute, Type> res = new Dictionary<ColumnAttribute, Type>();
            if (!string.IsNullOrEmpty(this.Table))
            {
                iDataBase DBOper = MakeConnection();
                if (DBOper != null)
                {
                    res = DBOper.Columns(this.Table); 
                }
            }
            return res;
        }

        private string CheckTable(ref iDataBase DB, int Year, int Month)
        {
            if (string.IsNullOrEmpty(this.Table)) return "";
            string TableName = "";
            DateTime DT = DateTime.Now;
            if (!DateTime.TryParse(Year.ToString() + "-" + (Month > 9 ? Month.ToString() : "0" + Month.ToString()), out DT))
            { DT = DateTime.Now; }
            if (DB.TableIsExist(this.Table))
            {
                if (DB.TableIsExist(this.Table + DT.Year.ToString() + (DT.Month > 9 ? DT.Month.ToString() : "0" + DT.Month.ToString())))
                    TableName = this.Table + DT.Year.ToString() + (DT.Month > 9 ? DT.Month.ToString() : "0" + DT.Month.ToString());
            }
            return TableName;
        }

        private string CheckMTable(ref iDataBase DB, int Year, int Month)
        {
            if (string.IsNullOrEmpty(this.Table)) return "";
            string TableName = "";
            DateTime DT = DateTime.Now;
            if (!DateTime.TryParse(Year.ToString() + "-" + (Month > 9 ? Month.ToString() : "0" + Month.ToString()), out DT))
            { DT = DateTime.Now; }
            if (DB.TableIsExist(this.Table))
                TableName = this.Table + DT.Year.ToString() + (DT.Month > 9 ? DT.Month.ToString() : "0" + DT.Month.ToString());
            return TableName;
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
        /// 主表是否存在
        /// </summary>
        /// <returns>是否存在</returns>
        [DisplayName("TableIsExist")]
        [Description("表是否存在")]
        public bool TableIsExist()
        {
            try
            {
                bool res = false;
                iDataBase DBOper = MakeConnection();
                if (DBOper != null)
                    res = DBOper.TableIsExist(this.Table);
                return res;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 历史表是否存在
        /// </summary>
        /// <param name="Year">年</param>
        /// <param name="Month">月</param>
        /// <returns>是否存在,主表不存在也会返回不存在</returns>
        [DisplayName("HistoryIsExist")]
        [Description("历史表是否存在")]
        public bool HistoryIsExist(int Year,int Month)
        {
            DateTime DT = DateTime.Now;
            if (!DateTime.TryParse(Year.ToString() + "-" + (Month > 9 ? Month.ToString() : "0" + Month.ToString()), out DT))
            { DT = DateTime.Now; }
            try
            {
                string TableName = this.Table + DT.Year.ToString() + (DT.Month > 9 ? DT.Month.ToString() : "0" + DT.Month.ToString());
                bool res = false;
                iDataBase DBOper = MakeConnection();
                if (DBOper != null)
                {
                    if (DBOper.TableIsExist(this.Table))
                        return DBOper.TableIsExist(TableName);
                }
                return res;
            }
            catch
            {
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
                    return GetCol();
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
            set { cols = value; }
        }

        /// <summary>
        /// 创建表
        /// </summary>
        /// <param name="Year">年</param>
        /// <param name="Month">月</param>
        /// <returns></returns>
        [DisplayName("CreatTable")]
        [Description("建表")]
        public bool CreatTable(int Year, int Month)
        { 
            try
            {
                iDataBase DBOper = MakeConnection();
                if (DBOper != null)
                {
                    string TableName = CheckMTable(ref DBOper, Year, Month);
                    if (string.IsNullOrEmpty(TableName)) return false;
                    var OrgCol = GetCol();
                    List<ColumnAttribute> cos = new List<ColumnAttribute>();
                    foreach (var col in OrgCol)
                    {
                        Type tp = col.Value; 
                        if (!tp.SupportedType())
                            continue;
                        col.Key.DataType = tp.ToDataType();
                        cos.Add(col.Key);
                    }
                    if (cos.Where(c => c.IsPrimaryKey).Count() <= 0)
                    {
                        var index = cos.FindIndex(c => c.Name.ToUpper().Contains("ID") || c.Name.ToUpper().Contains("INDEX"));
                        if (index > -1)
                        {
                            cos[index].IsPrimaryKey = true;
                            cos[index].IsIdentity = true;
                        }
                    }
                    if (cos.Where(c => c.IsPrimaryKey).Count() <= 0)
                        throw new NullReferenceException(SystemMessage.RefNullOrEmpty("PrimaryKey", language));
                    ColumnAttribute YearCol = new ColumnAttribute();
                    YearCol.Name = "YearCol";
                    YearCol.CanBeNull = false;
                    YearCol.DataType = DataType.Int64;
                    YearCol.IsPrimaryKey = false;
                    cos.Add(YearCol);
                    ColumnAttribute MonthCol = new ColumnAttribute();
                    MonthCol.Name = "MonthCol";
                    MonthCol.CanBeNull = false;
                    MonthCol.DataType = DataType.Int64;
                    MonthCol.IsPrimaryKey = false;
                    cos.Add(MonthCol); 
                    List<string> sqlbat = TableName.CreatToSql(DB.Mode, cos);
                    foreach (var tsql in sqlbat)
                        DBOper.ExecuteNonQuery(tsql);
                    return DBOper.TableIsExist(TableName);
                }
                return false;
            }
            catch (Exception ex)
            {
                if (HasError != null)
                    HasError(ClassName, "Creat", ex);
                else
                    throw ex;
                return false;
            }
        }

        /// <summary>
        /// 删除表
        /// </summary>
        /// <param name="Year">年</param>
        /// <param name="Month">月</param>
        /// <returns></returns>
        [DisplayName("DropTable")]
        [Description("删除表")]
        public bool DropTable(int Year, int Month)
        { 
            try
            {
                iDataBase DBOper = MakeConnection();
                if (DBOper != null)
                {
                    string TableName = CheckTable(ref DBOper, Year, Month);
                    if (string.IsNullOrEmpty(TableName)) return false;
                    List<string> sqlbat = TableName.DropToSql();
                    foreach (var tsql in sqlbat)
                    {
                        if (DBOper.ExecuteNonQuery(tsql) <= 0)
                            throw new Exception(tsql);
                    }
                    return !DBOper.TableIsExist(TableName);
                }
                return false;
            }
            catch (Exception ex)
            {
                if (HasError != null)
                    HasError(ClassName, "Drop", ex);
                else
                    throw ex;
                return false;
            }
        }

        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="Column">字段及字段值</param>
        /// <param name="Year">年</param>
        /// <param name="Month">月</param>
        /// <returns></returns>
        [DisplayName("Insert")]
        [Description("添加记录")]
        public bool Insert(Dictionary<string, object> Column, int Year, int Month)
        {
            if (Column == null) return false;
            if (Column.Count <= 0) return false;
            try
            {
                iDataBase DBOper = MakeConnection();
                if (DBOper != null)
                {
                    string TableName = CheckTable(ref DBOper, Year, Month);
                    if (string.IsNullOrEmpty(TableName)) return false;

                    Dictionary<string, object> cols = new Dictionary<string, object>();
                    Dictionary<ColumnAttribute, Type> SCOL = GetCol();
                    foreach (var col in SCOL)
                    {
                        if (Column.Where(c => c.Key.ToUpper().Trim() == col.Key.Name.ToUpper().Trim()).Count() < 0 && !col.Key.CanBeNull && !col.Key.IsPrimaryKey)
                            throw new NullReferenceException(SystemMessage.RefNullOrEmpty(col.Key.Name,language));
                    }
                    foreach (var col in Column)
                    {
                        if (SCOL.Where(c => c.Key.Name.ToUpper().Trim() == col.Key.ToUpper().Trim()).Count() > 0)
                        {
                            var tmp = SCOL.FirstOrDefault(c => c.Key.Name.ToUpper().Trim() == col.Key.ToUpper().Trim());
                            if (col.Value == null)
                            {
                                if (!tmp.Key.CanBeNull && !tmp.Key.IsPrimaryKey)
                                    throw new NullReferenceException(SystemMessage.RefNullOrEmpty(col.Key, language));
                            }
                            cols.Add(col.Key, col.Value);
                        }
                    }
                    if (cols.Count > 0)
                    {
                        cols.Add("YearCol", Year);
                        cols.Add("MonthCol", Month);
                        string Sql = TableName.InsertToSQL(DataBaseType, cols);
                        return DBOper.ExecuteNonQuery(Sql) > 0;
                    }
                }
                return false; 
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
        /// 修改记录
        /// </summary>
        /// <param name="Column">字段及字段值</param>
        /// <param name="Year">年</param>
        /// <param name="Month">月</param>
        /// <returns></returns>
        [DisplayName("Update")]
        [Description("修改记录")]
        public bool Update(Dictionary<string, object> Column, int Year, int Month)
        {
            if (Column == null) return false;
            if (Column.Count <= 0) return false;
            string Where = "";
            try
            {
                iDataBase DBOper = MakeConnection();
                if (DBOper != null)
                {
                    string TableName = CheckTable(ref DBOper, Year, Month);
                    if (string.IsNullOrEmpty(TableName)) return false;

                    Dictionary<string, object> cols = new Dictionary<string, object>();
                    Dictionary<ColumnAttribute, Type> SCOL = GetCol();
                    foreach (var col in SCOL)
                    {
                        if (Column.Where(c => c.Key.ToUpper().Trim() == col.Key.Name.ToUpper().Trim()).Count() < 0 && !col.Key.CanBeNull)
                            throw new NullReferenceException(SystemMessage.RefNullOrEmpty(col.Key.Name, language));
                    }
                    foreach (var col in Column)
                    {
                        if (SCOL.Where(c => c.Key.Name.ToUpper().Trim() == col.Key.ToUpper().Trim()).Count() > 0)
                        {
                            var tmp = SCOL.FirstOrDefault(c => c.Key.Name.ToUpper().Trim() == col.Key.ToUpper().Trim());
                            if (col.Value == null)
                                throw new NullReferenceException(SystemMessage.RefNullOrEmpty(col.Key, language));
                            cols.Add(col.Key, col.Value);
                        }
                    }

                    if (SCOL.Where(c => c.Key.IsPrimaryKey).Count() > 0)
                    {
                        var tmp = SCOL.FirstOrDefault(c => c.Key.IsPrimaryKey);
                        if (cols.Where(c => c.Key.ToUpper().Trim() == tmp.Key.Name.ToUpper().Trim()).Count() > 0)
                        {
                            var tmpID = cols.FirstOrDefault(c => c.Key.ToUpper().Trim() == tmp.Key.Name.ToUpper().Trim());
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
                        Where += " and YearCol=" + Year.ToString() + " and MonthCol=" + Month.ToString();
                        string Sql = TableName.UpdateToSQL(DataBaseType, cols) + Where;
                        return DBOper.ExecuteNonQuery(Sql) > 0;
                    }
                }  
                return false;
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
        /// 删除记录
        /// </summary>
        /// <param name="Column">字段及字段值</param>
        /// <param name="Year">年</param>
        /// <param name="Month">月</param>
        /// <returns></returns>
        [DisplayName("Delete")]
        [Description("删除记录")]
        public bool Delete(Dictionary<string, object> Column, int Year, int Month)
        {
            if (Column == null) return false;
            if (Column.Count <= 0) return false;
            string Where = "";
            try
            {
                iDataBase DBOper = MakeConnection();
                if (DBOper != null)
                {
                    string TableName = CheckTable(ref DBOper, Year, Month);
                    if (string.IsNullOrEmpty(TableName)) return false;
                    Dictionary<ColumnAttribute, Type> SCOL = GetCol();
                    if (SCOL.Where(c => c.Key.IsPrimaryKey).Count() > 0)
                    {
                        var tmp = SCOL.FirstOrDefault(c => c.Key.IsPrimaryKey);
                        if (Column.Where(c => c.Key.ToUpper().Trim() == tmp.Key.Name.ToUpper().Trim()).Count() > 0)
                        {
                            var tmpID = Column.FirstOrDefault(c => c.Key.ToUpper().Trim() == tmp.Key.Name.ToUpper().Trim());
                            if (tmpID.Value == null)
                                throw new NullReferenceException(SystemMessage.RefNullOrEmpty(tmp.Key.Name, language));
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
                    if (cols.Count > 0 && string.IsNullOrEmpty(Where))
                    {
                        Where += " and YearCol=" + Year.ToString() + " and MonthCol=" + Month.ToString();
                        string Sql = TableName.DeleteToSQL() + Where;
                        return DBOper.ExecuteNonQuery(Sql) > 0;
                    }
                }
                return false;
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
        /// 查找记录
        /// </summary>
        /// <param name="WHERE">查询条件语句</param>
        /// <param name="Year">年</param>
        /// <param name="Month">月</param>
        /// <returns>字段及值</returns>
        [DisplayName("Find")]
        [Description("查找记录")]
        public Dictionary<string, object> Find(string WHERE, int Year, int Month)
        {
            if (string.IsNullOrEmpty(this.Table)) return null;
            if (string.IsNullOrEmpty(WHERE)) return null;
            if (!WHERE.ToUpper().Contains("WHERE")) WHERE = " WHERE " + WHERE;
            try
            {
                iDataBase DBOper = MakeConnection();
                if (DBOper != null)
                {
                    string TableName = CheckTable(ref DBOper, Year, Month);
                    if (string.IsNullOrEmpty(TableName)) return null; 
                    if(string.IsNullOrEmpty(WHERE))
                        WHERE = " WHERE YearCol=" + Year.ToString() + " and MonthCol=" + Month.ToString();
                    else
                        WHERE += " and YearCol=" + Year.ToString() + " and MonthCol=" + Month.ToString();
                    string Sql = TableName.SelectToSql(null) + WHERE;
                    Dictionary<string, object> res = DBOper.Find(Sql);
                    return res;
                }
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
        public DataSet Query(string WHERE, DateTime SDT, DateTime EDT, bool AllInOne = true ,bool IsUnique=false )
        {
            if (string.IsNullOrEmpty(this.Table)) return null;
            if (!string.IsNullOrEmpty(WHERE))
            { if (!WHERE.ToUpper().Contains("WHERE")) WHERE = " WHERE " + WHERE; }
            else
            { WHERE = ""; } 
            try
            {
                Dictionary<ColumnAttribute, Type> SCOL = GetCol();
                List<string> Mothtable = new List<string>();
                iDataBase DBOper = MakeConnection();
                if (DBOper != null)
                {
                    DateTime DT = DateTime.Now;
                    int i = 0;
                    while ((DT = SDT.AddMonths(i)) < EDT)
                    {
                        string TableName = CheckTable(ref DBOper, DT.Year, DT.Month);
                        if (!string.IsNullOrEmpty(TableName)) Mothtable.Add(TableName);
                        i++;
                    }
                    DataSet DS = new DataSet();
                    if (AllInOne)
                    {
                        string Sql = Mothtable.UnionToSql(null, IsUnique) + WHERE;
                        DataTable dt = DBOper.getDataTable(Sql);
                        dt.TableName = this.Table;
                        DS.Tables.Add(dt);
                    }
                    else
                    {
                        foreach (var sll in Mothtable)
                        {
                            string Sql = sll.SelectToSql(null) + WHERE;
                            DataTable dt = DBOper.getDataTable(Sql);
                            dt.TableName = sll;
                            DS.Tables.Add(dt);
                        } 
                    }
                    return DS;
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
        /// 查询分页
        /// </summary>
        /// <param name="PageIndex">当前页</param>
        /// <param name="PageSize">分页大小</param>
        /// <param name="SDT">开始时间</param>
        /// <param name="EDT">结束时间</param>
        /// <param name="WHERE">查询条件</param>
        /// <param name="Orderby">排序</param>
        /// <param name="PageCount">总页数</param>
        /// <param name="RecordCount">总记录数</param>
        /// <returns></returns>
        [DisplayName("Select")]
        [Description("查询分页")]
        public DataSet Select(int PageIndex, int PageSize, DateTime SDT, DateTime EDT, string WHERE, string Orderby, out int PageCount, out int RecordCount , bool AllInOne = true, bool IsUnique = false)
        {
            PageCount = 0;
            RecordCount = 0;
            if (string.IsNullOrEmpty(this.Table)) return null;
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
                    List<string> Mothtable = new List<string>();
                    DateTime DT = DateTime.Now;
                    int i = 0;
                    while ((DT = SDT.AddMonths(i)) < EDT)
                    {
                        string TableName = CheckTable(ref DBOper, DT.Year, DT.Month);
                        if (!string.IsNullOrEmpty(TableName)) Mothtable.Add(TableName);
                    }
                    DataSet DS = new DataSet();
                    if (AllInOne)
                    {
                        string Sql = Mothtable.UnionToSql(null, IsUnique) ;
                        Sql = Sql.SimViewToSql();
                        DataTable dt = DBOper.getDataTableByRam(PageIndex, PageSize, "", Sql, WHERE, Orderby, "", out RecordCount, out PageCount);
                        dt.TableName = this.Table;
                        DS.Tables.Add(dt);
                    }
                    else
                    {
                        foreach (var sll in Mothtable)
                        {
                            DataTable dt = DBOper.getDataTableByRam(PageIndex, PageSize, "", sll, WHERE, Orderby, "", out RecordCount, out PageCount);
                            dt.TableName = sll;
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
