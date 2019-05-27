using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data;
using System.Data.Common;
using LinqToDB;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using LinqToDB.Mapping;
using NK.ENum;
using NK.Entity;
using NK.Event;
using NK.Attribut;
using NK.Message;
using NK.Interface;

namespace NK.Data.Manager
{
    /// <summary>
    /// 历史表管理类
    /// </summary>
    /// <typeparam name="T">实体</typeparam>
    [DisplayName("HistoryManager")]
    [Description("历史表管理类")]
    public   class HistoryManager<T> : DataHelper, IDisposable where T : class,
                  new()
    {

        #region 构造函数

        /// <summary>
        ///  Linq数据库处理
        /// </summary>
        public HistoryManager() : base()
        {
            ClassName = this.GetType().ToString();
            this.language = Language.Chinese;
        }

        /// <summary>
        ///  Linq数据库处理
        /// </summary>
        /// <param name="info">数据库参数</param>
        public HistoryManager(DBInfo info) : base(info)
        {
            ClassName = this.GetType().ToString();
            this.language = Language.Chinese;
        }

        /// <summary>
        /// Linq数据库处理
        /// </summary>
        /// <param name="ConnectionType">数据库类型</param>
        /// <param name="ConnectionString">连接串</param>
        /// <param name="Timeout">超时时间，毫秒</param>
        public HistoryManager(DBType ConnectionType, string ConnectionString, int Timeout = 60) : base(ConnectionType, ConnectionString, Timeout)
        {
            ClassName = this.GetType().ToString();
            this.language = Language.Chinese;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        ~HistoryManager()
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

        #endregion

        #region 属性

        /// <summary>
        /// 实体类型
        /// </summary>
        [DisplayName("EntityType")]
        [Description("实体类型")]
        public Type EntityType
        {
            get
            {
                T org = new T();
                return org.GetType();
            }
        }

        /// <summary>
        /// 获取表名
        /// </summary>
        [DisplayName("TableName")]
        [Description("获取表名")]
        public string TableName
        {
            get
            {
                T org = new T();
                TableAttribute[] TableAttributes = (TableAttribute[])org.GetType().GetCustomAttributes(typeof(TableAttribute), false);
                string tablename = (TableAttributes == null ? "" : (TableAttributes.Length > 0 ? TableAttributes.ToList().First().Name.Trim() : ""));
                if (string.IsNullOrEmpty(tablename))
                    tablename = org.GetType().Name;
                return tablename;
            }
        }

        /// <summary>
        /// 显示表所有字段
        /// </summary>
        [DisplayName("Column")]
        [Description("显示表所有字段")]
        public Dictionary<ColumnAttribute, Type> Column
        {
            get
            {
                Dictionary<ColumnAttribute, Type> ls = new Dictionary<ColumnAttribute, Type>();
                T org = new T();
                PropertyInfo[] properties = org.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo p in properties)
                {
                    if (p != null)
                    {
                        Type t = p.PropertyType;
                        ColumnAttribute[] ColumnAttributes = (ColumnAttribute[])p.GetCustomAttributes(typeof(ColumnAttribute), false);
                        ColumnAttribute tmp = new ColumnAttribute();
                        tmp.CanBeNull = (ColumnAttributes == null ? false : (ColumnAttributes.Length > 0 ? ColumnAttributes.ToList().First().CanBeNull : false));
                        if (!tmp.CanBeNull)
                            tmp.CanBeNull = t.Name.ToUpper().Contains("NULL") ? true : false;
                        tmp.Name = (ColumnAttributes == null ? p.Name : (ColumnAttributes.Length > 0 ? ColumnAttributes.ToList().First().Name.Trim() : p.Name));
                        tmp.IsPrimaryKey = (ColumnAttributes == null ? false : (ColumnAttributes.Length > 0 ? ColumnAttributes.ToList().First().IsPrimaryKey : false));
                        tmp.DataType = (ColumnAttributes == null ? LinqToDB.DataType.Undefined : (ColumnAttributes.Length > 0 ? ColumnAttributes.ToList().First().DataType : LinqToDB.DataType.Undefined));
                        if (tmp.DataType == LinqToDB.DataType.Undefined)
                            tmp.DataType = t.ToDataType();
                        ls.Add(tmp, t);
                    }
                }
                if (ls.Where(c => c.Key.IsPrimaryKey).Count() <= 0)
                {
                    for (int i = 0; i < ls.Count(); i++)
                    {
                        if (ls.ElementAt(i).Key.Name.ToUpper().Contains("ID") || ls.ElementAt(i).Key.Name.ToUpper().Contains("INDEX"))
                            ls.ElementAt(i).Key.IsPrimaryKey = true;
                    }
                }
                return ls;
            }
        }

        /// <summary>
        /// 表字段描述
        /// </summary>
        [DisplayName("DispColumn")]
        [Description("表字段描述")]
        public Dictionary<ColumnAttribute, DisplayColumnAttribute> DispColumn
        {
            get
            {
                Dictionary<ColumnAttribute, DisplayColumnAttribute> ls = new Dictionary<ColumnAttribute, DisplayColumnAttribute>();
                T org = new T();
                TableAttribute[] TableAttributes = (TableAttribute[])org.GetType().GetCustomAttributes(typeof(TableAttribute), false);
                string TableName = (TableAttributes == null ? "" : (TableAttributes.Length > 0 ? TableAttributes.ToList().First().Name.Trim() : ""));
                if (string.IsNullOrEmpty(TableName))
                    TableName = org.GetType().Name;
                PropertyInfo[] properties = org.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                int n = 0;
                foreach (PropertyInfo p in properties)
                {
                    if (p != null)
                    {
                        Type t = p.PropertyType;
                        ColumnAttribute[] ColumnAttributes = (ColumnAttribute[])p.GetCustomAttributes(typeof(ColumnAttribute), false);
                        DisplayColumnAttribute[] EnumAttributes = (DisplayColumnAttribute[])p.GetCustomAttributes(typeof(DisplayColumnAttribute), false);
                        ColumnAttribute tmp = new ColumnAttribute();
                        tmp.CanBeNull = (ColumnAttributes == null ? false : (ColumnAttributes.Length > 0 ? ColumnAttributes.ToList().First().CanBeNull : false));
                        if (!tmp.CanBeNull)
                            tmp.CanBeNull = t.Name.ToUpper().Contains("NULL") ? true : false;
                        tmp.Name = (ColumnAttributes == null ? p.Name : (ColumnAttributes.Length > 0 ? ColumnAttributes.ToList().First().Name.Trim() : p.Name));
                        tmp.IsPrimaryKey = (ColumnAttributes == null ? false : (ColumnAttributes.Length > 0 ? ColumnAttributes.ToList().First().IsPrimaryKey : false));
                        tmp.DataType = (ColumnAttributes == null ? LinqToDB.DataType.Undefined : (ColumnAttributes.Length > 0 ? ColumnAttributes.ToList().First().DataType : LinqToDB.DataType.Undefined));
                        if (tmp.DataType == LinqToDB.DataType.Undefined)
                            tmp.DataType = t.ToDataType();
                        DisplayColumnAttribute disp = new DisplayColumnAttribute();
                        disp.Table = TableName;
                        disp.Column = tmp.Name;
                        disp.Name = tmp.Name;
                        disp.JS = "";
                        disp.CSS = "";
                        disp.Format = "";
                        disp.Unit = "";
                        disp.index = n;
                        disp.Seqencing = n;
                        disp.CanCount = false;
                        disp.CanHead = true;
                        disp.CanSearch = true;
                        disp.CanImpExp = false;
                        disp.IsUnique = false;
                        if (EnumAttributes.Length > 0)
                        {
                            var ens = EnumAttributes[0];
                            if (string.IsNullOrEmpty(ens.Name))
                            {
                                DescriptionAttribute[] Attributes = (DescriptionAttribute[])p.GetCustomAttributes(typeof(DescriptionAttribute), false);
                                disp.Name = (Attributes.Count() > 0 ? (string.IsNullOrEmpty(Attributes[0].Description) ? tmp.Name : Attributes[0].Description) : tmp.Name);
                            }
                            else
                                disp.Name = ens.Name;
                            disp.JS = (string.IsNullOrEmpty(ens.JS) ? "" : ens.JS);
                            disp.CSS = (string.IsNullOrEmpty(ens.CSS) ? "" : ens.CSS);
                            disp.Format = (string.IsNullOrEmpty(ens.Format) ? "" : ens.Format);
                            disp.Unit = (string.IsNullOrEmpty(ens.Unit) ? "" : ens.Unit);
                            disp.index = ens.index;
                            disp.Seqencing = ens.Seqencing;
                            disp.CanCount = ens.CanCount;
                            disp.CanHead = ens.CanHead;
                            disp.CanSearch = ens.CanSearch;
                            disp.CanImpExp = ens.CanImpExp;
                            disp.IsUnique = ens.IsUnique;
                        }
                        n++;
                        ls.Add(tmp, disp);
                    }
                }
                if (ls.Where(c => c.Key.IsPrimaryKey).Count() <= 0)
                {
                    for (int i = 0; i < ls.Count(); i++)
                    {
                        if (ls.ElementAt(i).Key.Name.ToUpper().Contains("ID") || ls.ElementAt(i).Key.Name.ToUpper().Contains("INDEX"))
                            ls.ElementAt(i).Key.IsPrimaryKey = true;
                    }
                }
                return ls;
            }
        }
 
        #endregion
  
        #region 私有方法
         
        private bool IsExist(ref DbConnection DB, string Tablename)
        {
            List<string> views = new List<string>();
            DataTable dt = DB.GetSchema("Tables");
            int m = dt.Columns.IndexOf("TABLE_NAME");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                views.Add(dr.ItemArray.GetValue(m).ToString());
            }
            return views.Where(c => c.ToUpper().Trim() == Tablename.ToUpper().Trim()).Count() > 0;
        }

        private DataTable GetDT(ref DbConnection DB, string TSQL, string Tablname)
        {
            DbCommand cmd = DB.CreateCommand();
            cmd.CommandText = TSQL;
            DbDataReader da = cmd.ExecuteReader();
            DataTable schemaTable = da.GetSchemaTable();
            DataTable dataTable = new DataTable();
            dataTable.TableName = Tablname;
            if (schemaTable != null)
            {
                for (int i = 0; i < schemaTable.Rows.Count; i++)
                {
                    DataRow dataRow = schemaTable.Rows[i];
                    string columnName = (string)dataRow["ColumnName"];
                    DataColumn column = new DataColumn(columnName, (Type)dataRow["DataType"]);
                    dataTable.Columns.Add(column);
                }
                while (da.Read())
                {
                    DataRow dataRow = dataTable.NewRow();
                    for (int i = 0; i < da.FieldCount; i++)
                    {
                        dataRow[i] = da.GetValue(i);
                    }
                    dataTable.Rows.Add(dataRow);
                }
                return dataTable;
            }
            return null;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 查询符合条件的记录
        /// </summary>
        /// <param name="whereLambda">查询条件</param>
        /// <returns>符合条件的记录</returns>
        [DisplayName("GetTable")]
        [Description("查询符合条件的记录")]
        public virtual IQueryable<T> GetTable(Expression<Func<T, bool>> whereLambda = null)
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
                if (whereLambda == null) whereLambda = c => true;
                return context.GetTable<T>().Where(whereLambda);
            }
            catch (Exception ex)
            {
                if (HasError != null)
                    HasError(ClassName, MethodName, ex);
                else
                    throw ex;
                return null;
            }
        }
         
        /// <summary>
        /// 创建表
        /// </summary>
        /// <returns>执行结果</returns>
        [DisplayName("CreatTable")]
        [Description("创建表")]
        public virtual bool CreatTable()
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
                string TableName = "";
                List<ColumnAttribute> Columns = EToSqlCreat<T>(out TableName);
                if (Columns.Where(c => c.IsPrimaryKey).Count() <= 0)
                    throw new NullReferenceException(SystemMessage.RefNullOrEmpty("PrimaryKey", language));
                List<string> sqlbat = TableName.CreatToSql(DB.Mode, Columns);
                foreach (var tsql in sqlbat)
                {
                    if (Execute(tsql) <= 0)
                        throw new Exception(tsql);
                }
                return TableIsExist(TableName);
            }
            catch (Exception ex)
            {
                if (HasError != null)
                    HasError(ClassName, MethodName, ex);
                else
                    throw ex;
            }
            return false;
        }

        /// <summary>
        /// 修改表格
        /// </summary>
        /// <returns></returns>
        [DisplayName("ModifyTable")]
        [Description("修改表格")]
        public virtual bool ModifyTable()
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
                int n = 0;
                string TableName = "";
                List<ColumnAttribute> Cols = null;
                List<ColumnAttribute> orgcol = null;
                ETosqlModify<T>(out TableName, out Cols, out orgcol);
                List<string> sqlbat = TableName.ModifyToSql(DB.Mode, Cols, orgcol);
                foreach (var tmp in sqlbat)
                {
                    if (Execute(tmp) > 0)
                        n++;
                }
                return n > 0;
            }
            catch (Exception ex)
            {
                if (HasError != null)
                    HasError(ClassName, MethodName, ex);
                else
                    throw ex;
                return false;
            }
        }

        /// <summary>
        /// 删除表格
        /// </summary>
        /// <returns></returns>
        [DisplayName("DropTable")]
        [Description("删除表格")]
        public virtual bool DropTable()
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
                string TableName = Table<T>();
                List<string> sqlbat = TableName.DropToSql();
                foreach (var tsql in sqlbat)
                {
                    if (Execute(tsql) <= 0)
                        throw new Exception(tsql);
                }
                return !TableIsExist(TableName);
            }
            catch (Exception ex)
            {
                if (HasError != null)
                    HasError(ClassName, MethodName, ex);
                else
                    throw ex;
                return false;
            }
        }

        /// <summary>
        /// 表是否存在
        /// </summary>
        /// <returns>是否存在</returns>
        [DisplayName("TableIsExist")]
        [Description("表是否存在")]
        public virtual bool TableIsExist()
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
                string TableName = Table<T>();
                return TableIsExist(TableName);
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
        public virtual bool HistoryIsExist(int Year, int Month)
        {
            init();
            DateTime DT = DateTime.Now;
            if (!DateTime.TryParse(Year.ToString() + "-" + (Month > 9 ? Month.ToString() : "0" + Month.ToString()), out DT))
            { DT = DateTime.Now; }
            try
            {
                string TableName= Table<T>();
                TableName = TableName + DT.Year.ToString() + (DT.Month > 9 ? DT.Month.ToString() : "0" + DT.Month.ToString());
                return TableIsExist(TableName);
            }
            catch (Exception ex)
            { 
                return false;
            }
        }

        /// <summary>
        /// 建表
        /// </summary>
        /// <param name="Year">年</param>
        /// <param name="Month">月</param>
        /// <returns></returns>
        [DisplayName("CreatTable")]
        [Description("建表")]
        public virtual bool CreatTable(int Year, int Month)
        {
            init();
            DateTime DT = DateTime.Now;
            if (!DateTime.TryParse(Year.ToString() + "-" + (Month > 9 ? Month.ToString() : "0" + Month.ToString()), out DT))
            { DT = DateTime.Now; }
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
                string TableName = "";
                List<ColumnAttribute> Columns = EToSqlCreat<T>(out TableName);
                if (Columns.Where(c => c.IsPrimaryKey).Count() <= 0)
                    throw new NullReferenceException(SystemMessage.RefNullOrEmpty("PrimaryKey", language));
                TableName = TableName + DT.Year.ToString() + (DT.Month > 9 ? DT.Month.ToString() : "0" + DT.Month.ToString());
                List<string> sqlbat = TableName.CreatToSql(DB.Mode, Columns);
                foreach (var tsql in sqlbat)
                {
                    if (Execute(tsql) <= 0)
                        throw new Exception(tsql);
                }

                return TableIsExist(TableName);
            }
            catch (Exception ex)
            {
                if (HasError != null)
                    HasError(ClassName, MethodName, ex);
                else
                    throw ex;
            }
            return false;
        }

        /// <summary>
        /// 删表
        /// </summary>
        /// <param name="Year">年</param>
        /// <param name="Month">月</param>
        /// <returns></returns>
        [DisplayName("DropTable")]
        [Description("删表")]
        public virtual bool DropTable(int Year, int Month)
        {
            init();
            DateTime DT = DateTime.Now;
            if (!DateTime.TryParse(Year.ToString() + "-" + (Month > 9 ? Month.ToString() : "0" + Month.ToString()), out DT))
            { DT = DateTime.Now; }
            try
            {
                string TableName = Table<T>();
                TableName = TableName + DT.Year.ToString() + (DT.Month > 9 ? DT.Month.ToString() : "0" + DT.Month.ToString());
                List<string> sqlbat = TableName.DropToSql();
                foreach (var tsql in sqlbat)
                {
                    if (Execute(tsql) <= 0)
                        throw new Exception(tsql);
                }
                return !TableIsExist(TableName);
            }
            catch (Exception ex)
            {
                if (HasError != null)
                    HasError(ClassName, MethodName, ex);
                else
                    throw ex;
                return false;
            }
        }
 
        /// <summary>
        /// 插入实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="Year">年</param>
        /// <param name="Month">月</param>
        [DisplayName("Insert")]
        [Description("插入实体")]
        public virtual bool Insert(T entity, int Year, int Month)
        {
            init();
            DateTime DT = DateTime.Now;
            if (!DateTime.TryParse(Year.ToString() + "-" + (Month > 9 ? Month.ToString() : "0" + Month.ToString()), out DT))
            { DT = DateTime.Now; }
            MethodName = "";
            try
            {
                MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            try
            {
                string TableName = "";
                Dictionary<string, object> ColSVal = EToSqlInsert<T>(entity, out TableName);
                TableName = TableName + DT.Year.ToString() + (DT.Month > 9 ? DT.Month.ToString() : "0" + DT.Month.ToString());
                string sql = TableName.InsertToSQL(DataBaseType, ColSVal);
                return Execute(sql) > 0;
            }
            catch (Exception ex)
            {
                if (HasError != null)
                    HasError(ClassName, MethodName, ex);
                else
                    throw ex;
                return false;
            }
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="Year">年</param>
        /// <param name="Month">月</param>
        [DisplayName("Update")]
        [Description("更新实体")]
        public virtual bool Update(T entity, int Year, int Month)
        {
            init();
            DateTime DT = DateTime.Now;
            if (!DateTime.TryParse(Year.ToString() + "-" + (Month > 9 ? Month.ToString() : "0" + Month.ToString()), out DT))
            { DT = DateTime.Now; }
            MethodName = "";
            try
            {
                MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            try
            {
                string TableName = "", where="";
                Dictionary<string, object> ColSVal = EToSqlUpdate<T>(entity, out TableName, out where);
                TableName = TableName + DT.Year.ToString() + (DT.Month > 9 ? DT.Month.ToString() : "0" + DT.Month.ToString());
                if (string.IsNullOrEmpty(where))
                    throw new NullReferenceException(SystemMessage.RefNullOrEmpty("PriKey", language));
                else
                    where = " where " + where;
                string SQL = TableName.UpdateToSQL(DataBaseType, ColSVal) + where; 
                return Execute(SQL) > 0;
            }
            catch (Exception ex)
            {
                if (HasError != null)
                    HasError(ClassName, MethodName, ex);
                else
                    throw ex;
                return false;
            }
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="Year">年</param>
        /// <param name="Month">月</param>
        [DisplayName("Delete")]
        [Description("删除实体")]
        public virtual bool Delete(T entity, int Year, int Month)
        {
            init();
            DateTime DT = DateTime.Now;
            if (!DateTime.TryParse(Year.ToString() + "-" + (Month > 9 ? Month.ToString() : "0" + Month.ToString()), out DT))
            { DT = DateTime.Now; }
            MethodName = "";
            try
            {
                MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            try
            {
                string where = "", TableName = "";
                EToSqlDelete<T>(entity, out TableName, out where);
                if (string.IsNullOrEmpty(where))
                    throw new NullReferenceException(SystemMessage.RefNullOrEmpty("PriKey", language));
                else
                    where = " where " + where;
                TableName = TableName + DT.Year.ToString() + (DT.Month > 9 ? DT.Month.ToString() : "0" + DT.Month.ToString());
                string SQL = TableName.DeleteToSQL() + where;
                return Execute(SQL) > 0;
            }
            catch (Exception ex)
            {
                if (HasError != null)
                    HasError(ClassName, MethodName, ex);
                else
                    throw ex;
                return false;
            }
        }
        
        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="whereLambda">查询条件</param>
        /// <param name="Year">年</param>
        /// <param name="Month">月</param>
        /// <returns>找到为实体，找不到为NULL</returns>
        [DisplayName("Find")]
        [Description("查找")]
        public virtual T Find(Expression<Func<T, bool>> whereLambda, int Year, int Month)
        {
            init();
            DateTime DT = DateTime.Now;
            if (!DateTime.TryParse(Year.ToString() + "-" + (Month > 9 ? Month.ToString() : "0" + Month.ToString()), out DT))
            { DT = DateTime.Now; }
            MethodName = "";
            try
            {
                MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            try
            {
                string where = whereLambda.WhereToSQL();
                if (!string.IsNullOrEmpty(where))
                    where = " where " + where;
                string TableName = Table<T>();
                TableName = TableName + DT.Year.ToString() + (DT.Month > 9 ? DT.Month.ToString() : "0" + DT.Month.ToString());
                DataTable DTS = getTable("select * from " + TableName + " " + where, TableName);
                if (DTS != null)
                {
                    if (DTS.Rows.Count > 0)
                        return DataRowToEntity<T>(DTS.Rows[0]);
                }
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

        /// <summary>
        /// 查询所有结果
        /// </summary>
        /// <param name="SDT">开始日期</param>
        /// <param name="EDT">结束日期</param>
        /// <param name="whereLambda">查询条件</param>
        /// <param name="orderLambda">排序</param>
        /// <param name="ASCDESC">升序或降序</param>
        /// <param name="IsUnique">唯一</param>
        /// <returns>所有结果</returns>
        [DisplayName("List")]
        [Description("查询所有实体")]
        public virtual List<T> Query( DateTime SDT, DateTime EDT , Expression<Func<T, bool>> whereLambda = null, Expression<Func<T, object>> orderLambda = null,bool ASCDESC=false , bool IsUnique = false)
        {
            init();
            DateTime DT = DateTime.Now;
            MethodName = "";
            try
            {
                MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            try
            {
                string where = "";
                if (whereLambda != null)
                    where = " where " + whereLambda.WhereToSQL();
                string orderby = "";
                if (orderLambda != null)
                    orderby = " order " + orderLambda.OrderbyToSql() + " " + (ASCDESC ? "DESC" : "");
                string TableName = Table<T>();
                List<string> Mothtable = new List<string>();
                if (TableIsExist(TableName))
                    Mothtable.Add(TableName);
                int i = 0;
                while ((DT = SDT.AddMonths(i)) < EDT)
                {
                   string tablename = TableName + DT.Year.ToString() + (DT.Month > 9 ? DT.Month.ToString() : "0" + DT.Month.ToString());
                    if (TableIsExist(tablename))
                        Mothtable.Add(tablename);
                    i++;
                }
                string Sql = Mothtable.UnionToSql(null, IsUnique) + where+ orderby;
                DataTable DTS = getTable(Sql);
                return DataTableToEntity<T>(DTS).ToList(); 
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

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="PageIndex">当前页</param>
        /// <param name="PageSize">分页大小</param>
        /// <param name="SDT">开始日期</param>
        /// <param name="EDT">结束日期</param>
        /// <param name="PageCount">页数</param>
        /// <param name="RecordCount">记录数</param>
        /// <param name="whereLambda">查询条件</param>
        /// <param name="orderLambda">排序</param>
        /// <param name="ASCDESC">升序或降序</param>
        /// <param name="IsUnique">唯一</param>
        /// <returns></returns>
        [DisplayName("Select")]
        [Description("查询分页")]
        public virtual List<T> Select(int PageIndex, int PageSize, DateTime SDT, DateTime EDT, out int PageCount, out int RecordCount, Expression<Func<T, bool>> whereLambda = null, Expression<Func<T, object>> orderLambda = null, bool ASCDESC = true, bool IsUnique = false)
        {
            init();
            PageCount = 0;
            RecordCount = 0;
            DateTime DT = DateTime.Now;
            MethodName = "";
            try
            {
                MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            try
            {
                string where = "";
                if (whereLambda != null)
                    where = " where " + whereLambda.WhereToSQL();
                string orderby = "";
                if (orderLambda != null)
                    orderby = " order " + orderLambda.OrderbyToSql() + " " + (ASCDESC ? "DESC" : "");
                List<string> Mothtable = new List<string>();
                if (TableIsExist(TableName))
                    Mothtable.Add(TableName);
                int i = 0;
                while ((DT = SDT.AddMonths(i)) < EDT)
                {
                    string tablename = TableName + DT.Year.ToString() + (DT.Month > 9 ? DT.Month.ToString() : "0" + DT.Month.ToString());
                    if (TableIsExist(tablename))
                        Mothtable.Add(tablename);
                    i++;
                }
                string Sql = Mothtable.UnionToSql(null, IsUnique) + where + orderby;
                DataTable DTS = getTable(PageIndex, PageSize, "", TableName, where, orderby, "", out RecordCount, out PageCount);
                return DataTableToEntity<T>(DTS).ToList();
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

        #endregion

    }
}
