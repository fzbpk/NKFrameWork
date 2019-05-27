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
    [DisplayName("TableHelper")]
    [Description("表处理类")]
    public   class TableHelper : IDisposable
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
        public TableHelper()
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
        public TableHelper(string TableName, DBInfo info = null)
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
        public TableHelper(string TableName,DBType ConnectionType, string ConnectionString, int Timeout = 60)
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
        ~TableHelper()
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
                    res = DBOper.Columns(this.Table);
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
        /// 表是否存在
        /// </summary>
        /// <returns>是否存在</returns>
        [DisplayName("TableIsExist")]
        [Description("表是否存在")]
        public bool TableIsExist()
        {
            try
            {
                if (string.IsNullOrEmpty(this.Table)) return false;
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
        /// 显示表所有字段
        /// </summary>
        [DisplayName("Column")]
        [Description("表所有字段")]
        public Dictionary<ColumnAttribute, Type> Column
        {
            get {
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
        /// <returns></returns>
        [DisplayName("CreatTable")]
        [Description("建表")]
        public bool  CreatTable()
        {
            if (string.IsNullOrEmpty(this.Table)) return false;
            if (cols.Count<=0) return false;
            try
            {
                List<ColumnAttribute> cos = new List<ColumnAttribute>();
                foreach (var col in cols)
                {
                    Type tp = col.Value;
                    if (!tp.SupportedType())
                        continue;
                    if(col.Key.DataType== DataType.Undefined)
                       col.Key.DataType = tp.ToDataType();
                    cos.Add(col.Key);
                }
                if (cos.Where(c => c.IsPrimaryKey).Count() <= 0)
                {
                    var index = cos.FindIndex(c => c.Name.ToUpper().Contains("ID") || c.Name.ToUpper().Contains("INDEX"));
                    if (index > -1)
                        cos[index].IsPrimaryKey = true;
                }
                if (cos.Where(c => c.IsPrimaryKey).Count() <= 0)
                    throw new NullReferenceException(SystemMessage.RefNullOrEmpty("PrimaryKey", language ));
                iDataBase DBOper = MakeConnection();
                if (DBOper != null)
                {
                    List<string> sqlbat = this.Table.CreatToSql(DB.Mode, cos);
                    foreach (var tsql in sqlbat)
                        DBOper.ExecuteNonQuery(tsql);
                    return DBOper.TableIsExist(this.Table);  
                }
                else
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
        /// <returns></returns>
        [DisplayName("DropTable")]
        [Description("删除表")]
        public bool DropTable()
        {
            if (string.IsNullOrEmpty(this.Table)) return false;
            try
            {
                iDataBase DBOper = MakeConnection();
                List<string> sqlbat = this.Table.DropToSql();
                foreach (var tsql in sqlbat)
                {
                    if (DBOper.ExecuteNonQuery(tsql) <= 0)
                        throw new Exception(tsql);
                }
                return !DBOper.TableIsExist(this.Table);
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
        /// 修改表
        /// </summary>
        /// <returns></returns>
        [DisplayName("ModifyTable")]
        [Description("修改表")]
        public bool ModifyTable()
        {
            if (string.IsNullOrEmpty(this.Table)) return false;
            if (cols.Count <= 0) return false;
            try
            {
                Dictionary<ColumnAttribute, Type> SCOL = GetCol();
                iDataBase DBOper = MakeConnection();
                int n = 0;
                if (DBOper != null)
                {
                    List<ColumnAttribute> Col = new List<ColumnAttribute>();
                    List<ColumnAttribute> OrgCol = new List<ColumnAttribute>();
                    foreach (var org in SCOL)
                        OrgCol.Add(org.Key);
                    foreach (var col in cols)
                    {
                        Type t = col.Value; 
                        if (!t.SupportedType())
                            continue;
                        DataType types = col.Key.DataType;
                        if (types == DataType.Undefined)
                            types = t.ToDataType();
                        string ColumnName = col.Key.Name;
                        bool CanBeNull = col.Key.CanBeNull;
                        bool IsPri = col.Key.IsPrimaryKey;
                        if(!CanBeNull)
                          CanBeNull = t.Name.ToUpper().Contains("NULL") ? true : false;
                        ColumnAttribute temp = new ColumnAttribute();
                        temp.Name = ColumnName;
                        temp.CanBeNull = CanBeNull;
                        temp.IsPrimaryKey = IsPri;
                        temp.DataType = types;
                        Col.Add(temp);
                    }
                    if (Col.Where(c => c.IsPrimaryKey).Count() <= 0)
                    {
                        var index = Col.FindIndex(c => c.Name.ToUpper().Contains("ID") || c.Name.ToUpper().Contains("INDEX"));
                        if (index > -1)
                            Col[index].IsPrimaryKey = true;
                    }
                    if (Col.Where(c => c.IsPrimaryKey).Count() <= 0)
                        throw new NullReferenceException(SystemMessage.RefNullOrEmpty("PrimaryKey", language));
                    List<string> sqlbat = this.Table.ModifyToSql(DB.Mode, Col, OrgCol);
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
                    HasError(ClassName, "Modify", ex);
                else
                    throw ex;
                return false;
            }
        }

        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="Column">字段及字段值</param>
        /// <returns></returns>
        [DisplayName("Insert")]
        [Description("添加记录")]
        public bool Insert(Dictionary<string, object> Column)
        {
            if (string.IsNullOrEmpty(this.Table)) return false;
            if (Column == null) return false;
            if (Column.Count <= 0) return false;
            Dictionary<string, object> cols = new Dictionary<string, object>();
            try
            {
                Dictionary<ColumnAttribute, Type> SCOL = GetCol();
                foreach (var col in SCOL)
                {
                    if (Column.Where(c => c.Key.ToUpper().Trim() == col.Key.Name.ToUpper().Trim()).Count() < 0 && !col.Key .CanBeNull && !col.Key.IsPrimaryKey )
                        throw new NullReferenceException(SystemMessage.RefNullOrEmpty(col.Key.Name,language));
                }
                foreach (var col in Column)
                { 
                    if (SCOL.Where(c => c.Key.Name.ToUpper().Trim() == col.Key.ToUpper().Trim()).Count() >0)
                    {
                        var tmp = SCOL.FirstOrDefault(c => c.Key.Name.ToUpper().Trim() == col.Key.ToUpper().Trim());
                        if (col.Value == null)
                        {
                            if (!tmp.Key.CanBeNull && !tmp.Key.IsPrimaryKey)
                                throw new NullReferenceException(SystemMessage.RefNullOrEmpty(col.Key, language));
                        } 
                        cols.Add(col.Key,col.Value);
                    }
                }
                if (cols.Count > 0)
                {
                    string Sql = Table.InsertToSQL(DataBaseType, cols);
                    iDataBase DBOper = MakeConnection();
                    if (DBOper != null)
                        return DBOper.ExecuteNonQuery(Sql) > 0;
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
        /// <returns></returns>
        [DisplayName("Update")]
        [Description("修改记录")]
        public bool Update(Dictionary<string, object> Column)
        {
            if (string.IsNullOrEmpty(this.Table)) return false;
            if (Column == null) return false;
            if (Column.Count <= 0) return false;
            Dictionary<string, object> cols = new Dictionary<string, object>();
            string Where = "";
            try
            {
                Dictionary<ColumnAttribute, Type> SCOL = GetCol();
                foreach (var col in SCOL)
                {
                    if (Column.Where(c => c.Key.ToUpper().Trim() == col.Key.Name.ToUpper().Trim()).Count() < 0 && !col.Key.CanBeNull )
                        throw new NullReferenceException(SystemMessage.RefNullOrEmpty(col.Key.Name, language));
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
                        if (tmp.Key.IsPrimaryKey)
                            continue;
                        cols.Add(col.Key, col.Value);
                    }
                }

                if (SCOL.Where(c => c.Key.IsPrimaryKey).Count() > 0)
                {
                    var tmp = SCOL.FirstOrDefault(c => c.Key.IsPrimaryKey);
                    if (Column.Where(c => c.Key.ToUpper().Trim() == tmp.Key.Name.ToUpper().Trim()).Count() > 0)
                    {
                        var tmpID = Column.FirstOrDefault(c => c.Key.ToUpper().Trim() == tmp.Key.Name.ToUpper().Trim());
                        Type t = tmpID.Value.GetType();
                        if (t == typeof(string))
                            Where = " where " + tmpID.Key + " ='" + tmpID.Value.ToString() + "' ";
                        else if(t==typeof(DateTime))
                            Where = " where " + tmpID.Key + " ='" + tmpID.Value.ToString() + "' ";
                        else if(t.IsValueType)
                            Where = " where " + tmpID.Key + " =" + tmpID.Value.ToString() + "";
                    } 
                }
                else
                    throw new KeyNotFoundException(SystemMessage.RefNullOrEmpty("PriKey", language));
                if (cols.Count > 0 && !string.IsNullOrEmpty(Where))
                {
                    string Sql = Table.UpdateToSQL(DataBaseType, cols)+Where;
                    iDataBase DBOper = MakeConnection();
                    if (DBOper != null)
                        return DBOper.ExecuteNonQuery(Sql) > 0;
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
        /// <returns></returns>
        [DisplayName("Delete")]
        [Description("删除记录")]
        public bool Delete(Dictionary<string, object> Column)
        {
            if (string.IsNullOrEmpty(this.Table)) return false;
            if (Column == null) return false;
            if (Column.Count <= 0) return false;
            string Where = "";
            try
            {
                Dictionary<ColumnAttribute, Type> SCOL = GetCol();
                if (SCOL.Where(c => c.Key.IsPrimaryKey).Count() > 0)
                {
                    var tmp = SCOL.FirstOrDefault(c => c.Key.IsPrimaryKey);
                    if (Column.Where(c => c.Key.ToUpper().Trim() == tmp.Key.Name.ToUpper().Trim()).Count() > 0)
                    {
                        var tmpID = Column.FirstOrDefault(c => c.Key.ToUpper().Trim() == tmp.Key.Name.ToUpper().Trim());
                        if (tmpID.Value == null)
                            throw new NullReferenceException( SystemMessage.RefNullOrEmpty(tmp.Key.Name, language));
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
                if (!string.IsNullOrEmpty(Where))
                {
                    string Sql = Table.DeleteToSQL()+ Where;
                    iDataBase DBOper = MakeConnection();
                    if (DBOper != null)
                        return DBOper.ExecuteNonQuery(Sql) > 0;
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
        /// <returns>字段及值</returns>
        [DisplayName("Find")]
        [Description("查找记录")]
        public Dictionary<string, object> Find(string WHERE)
        {
            if (string.IsNullOrEmpty(this.Table)) return null;
            if (string.IsNullOrEmpty(WHERE)) return null;
            if (!WHERE.ToUpper().Contains("WHERE")) WHERE = " WHERE " + WHERE;
            try
            {  
                iDataBase DBOper = MakeConnection();
                if (DBOper != null)
                {
                    string Sql = Table.SelectToSql(null) + WHERE;
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
            try
            {
                if (string.IsNullOrEmpty(this.Table)) return null;
                if (!string.IsNullOrEmpty(where))
                { if (!where.ToUpper().Contains("WHERE")) where = " WHERE " + where; }
                else
                { where = ""; }
                if (!string.IsNullOrEmpty(order))
                { if (!order.ToUpper().Contains("OREDR")) order = " ORDER BY " + order; }
                else
                { order = ""; }
                iDataBase DBOper = MakeConnection();
                if (DBOper != null)
                {
                    string Sql = Table.SelectToSql(null) + where+ order;
                    DataTable DT = DBOper.getDataTable(Sql);
                    DT.TableName = this.Table;
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
            if (string.IsNullOrEmpty(this.Table)) return null;
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
                    DataTable DT = DBOper.getDataTableByRam(PageIndex, PageSize, "", this.Table, where, orderby, "", out RecordCount, out PageCount);
                    DT.TableName = this.Table;
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
                res= DBOper.ExecuteNonQuery(sql);
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

    }
}
