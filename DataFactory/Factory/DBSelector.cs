using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.ComponentModel;
using LinqToDB.Mapping;
using NK.ENum;
using NK.Entity;
using NK.Event;
using NK.Interface;
using NK.Message;
using LinqToDB;

namespace NK.Data
{
    /// <summary>
    /// 实体转T-SQL
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DisplayName("DBSelector")]
    [Description("实体转T-SQL")]
    public class DBSelector<T> : DataHelper, IDisposable 
        where T : class, new()
    {
         
        #region 构造函数

        /// <summary>
        /// 数据库T-SQL基本操作
        /// </summary> 
        public DBSelector() : base()
        { 
            ClassName = this.GetType().ToString();
            this.language = Language.Chinese;
        }

        /// <summary>
        /// 数据库T-SQL基本操作
        /// </summary>
        /// <param name="info"></param>
        public DBSelector(DBInfo info = null) : base(info)
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
       public DBSelector(DBType ConnectionType,string ConnectionString,int Timeout=60) : base(ConnectionType, ConnectionString, Timeout)
        {
            ClassName = this.GetType().ToString();
            this.language = Language.Chinese;
        }

       /// <summary>
       /// 释放资源
       /// </summary>
       ~DBSelector()
      {
        Dispose(false);
      }
          
        #endregion
        
        #region 属性
         
        /// <summary>
        /// 实体类型
        /// </summary>
        [DisplayName("EntityType")]
        [Description("实体类型")]
        public Type EntityType {
            get {
                T org = new T();
                return org.GetType();
            }
        }

        #endregion

        #region 方法
         
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
        /// 插入实体
        /// </summary>
        /// <param name="entity">实体</param>
        [DisplayName("Insert")]
       [Description("插入实体")]
       public virtual bool Insert(T entity)
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
                Dictionary<KeyValuePair<string, DataType>, object> pram = null;
                var ColSVal = EToSqlInsertEX(entity, out TableName);
                string sql = TableName.InsertToSQLEX(DataBaseType, ColSVal,out pram);
                return Execute(sql, pram)>0;
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
       [DisplayName("Update")]
       [Description("更新实体")]
       public virtual bool Update(T entity)
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
                string where = "", TableName = "";
                Dictionary<string, object> ColSVal = EToSqlUpdate<T>(entity,out TableName,out where);
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
       [DisplayName("Delete")]
       [Description("删除实体")]
       public virtual bool Delete(T entity)
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
                string where = "", TableName = "";
                EToSqlDelete<T>(entity, out TableName, out where);
                if (string.IsNullOrEmpty(where))
                    throw new NullReferenceException(SystemMessage.RefNullOrEmpty("PriKey", language));
                else
                    where = " where " + where; 
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
        /// 查询所有结果
        /// </summary>
        /// <param name="whereLambda">查询条件</param>
        /// <returns>所有结果</returns>
        [DisplayName("List")]
        [Description("查询所有实体")]
        public virtual IList<T> GetTable(Expression<Func<T, bool>> whereLambda=null)
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
                string where = "";
                if (whereLambda != null)
                    where = " where " + whereLambda.WhereToSQL();
                string TableName = Table<T>();
                DataTable DT = getTable("select * from " + TableName, TableName);
                return DataTableToEntity<T>(DT);
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
        /// <param name="whereLambda">查询条件</param>
        /// <param name="orderLambda">排序条件</param>
        /// <param name="ASCDESC">顺序倒叙</param>
        /// <returns>所有结果</returns>
        [DisplayName("List")]
        [Description("查询所有实体")]
        public virtual List<T> Query(Expression<Func<T, bool>> whereLambda = null, Expression<Func<T, object>> orderLambda = null, bool ASCDESC = false)
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
                string where = "";
                if (whereLambda != null)
                    where = " where " + whereLambda.WhereToSQL();
                string orderby = "";
                if (orderLambda != null)
                    orderby = " order " + orderLambda.OrderbyToSql() + " " + (ASCDESC ? "DESC" : "");
                string TableName = Table<T>();
                DataTable DT  = getTable("select * from " + TableName+ where, TableName);
                return DataTableToEntity<T>(DT).ToList();
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
        /// 查询并分页
        /// </summary>
        /// <param name="PageIndex">当前页</param>
        /// <param name="PageSize">分页大小</param> 
        /// <param name="PageCount">总页数</param>
        /// <param name="RecordCount">总记录数</param>
        /// <param name="whereLambda">查询条件</param>
        /// <param name="orderLambda">排序条件</param>
        /// <param name="ASCDESC">顺序倒叙</param>
        /// <returns>结果</returns>
        [DisplayName("Page")]
        [Description("分页查询所有实体")]
        public virtual List<T> Select(int PageIndex, int PageSize, out int PageCount, out int RecordCount, Expression<Func<T, bool>> whereLambda = null, Expression<Func<T, object>> orderLambda = null, bool ASCDESC=false)
        {
            PageCount = 0;
            RecordCount = 0;
            try
            {
                string where = "";
                if (whereLambda != null)
                    where = " where " + whereLambda.WhereToSQL();
                string orderby = "";
                if (orderLambda != null)
                    orderby = " order " + orderLambda.OrderbyToSql() + " " + (ASCDESC ? "DESC" : "");
                string TableName = Table<T>();
                DataTable DT  =getTable(PageIndex, PageSize, "", TableName, where, orderby, "", out RecordCount, out PageCount);
                return DataTableToEntity<T>(DT).ToList();
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
        /// 查找
        /// </summary>
        /// <param name="whereLambda">查询条件</param>
        /// <returns>找到为实体，找不到为NULL</returns>
        [DisplayName("Find")]
        [Description("查找")]
        public virtual T Find(Expression<Func<T, bool>> whereLambda)
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
                string where = whereLambda.WhereToSQL();
                if (!string.IsNullOrEmpty(where))
                    where = " where " + where;
                string TableName = Table<T>(); 
                DataTable DT = getTable("select * from " + TableName + " " + where, TableName);
                if (DT != null)
                {
                    if (DT.Rows.Count > 0)
                        return DataRowToEntity<T>(DT.Rows[0]);
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
        /// <param name="where">查询条件</param>
        /// <returns>所有结果</returns>
        [DisplayName("GetTable")]
        [Description("查询所有实体")]
        public virtual IList<T> GetTable(string where = "")
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
                if (!string.IsNullOrEmpty(where))
                    where = " where " + where;
                string TableName = Table<T>();
                DataTable DT = getTable("select * from " + TableName + where, TableName);
                return DataTableToEntity<T>(DT);
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
        /// <param name="where">查询条件</param>
        /// <param name="order">排序条件</param>
        /// <param name="ASCDESC">顺序倒叙</param>
        /// <returns>所有结果</returns>
        [DisplayName("GetTable")]
        [Description("查询所有实体")]
        public virtual List<T> Query(string where = "", string order = "", bool ASCDESC = false)
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
                if (!string.IsNullOrEmpty(where))
                    where = " where " + where;
                if (!string.IsNullOrEmpty(order))
                    order = " order by " + order + " " + (ASCDESC ? "DESC" : "");
                string TableName = Table<T>();
                DataTable DT = getTable("select * from " + TableName + where + order, TableName);
                return DataTableToEntity<T>(DT).ToList();
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
        /// 查询并分页
        /// </summary>
        /// <param name="PageIndex">当前页</param>
        /// <param name="PageSize">分页大小</param>
        /// <param name="where">查询条件</param>
        /// <param name="orderby">排序条件</param>
        /// <param name="PageCount">总页数</param>
        /// <param name="RecordCount">总记录数</param>
        /// <param name="ASCDESC">顺序倒叙</param>
        /// <returns>结果</returns>
        [DisplayName("Page")]
        [Description("分页查询所有实体")]
        public virtual List<T> Select(int PageIndex, int PageSize, out int PageCount, out int RecordCount, string where = "", string orderby = "", bool ASCDESC = false)
        {
            PageCount = 0;
            RecordCount = 0;
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
                if (!string.IsNullOrEmpty(where))
                    where = " where " + where;
                if (!string.IsNullOrEmpty(orderby))
                    orderby = " order by " + orderby + " " + (ASCDESC ? "DESC" : "");
                string TableName = Table<T>();
                DataTable DT = getTable(PageIndex, PageSize, "", TableName, where, orderby, "", out RecordCount, out PageCount);
                return DataTableToEntity<T>(DT).ToList();
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
        /// 查找
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="order">排序条件</param>
        /// <param name="ASCDESC">顺序倒叙</param>
        /// <returns>找到为实体，找不到为NULL</returns>
        [DisplayName("Find")]
        [Description("查找")]
        public virtual T Find(string where, string order = "", bool ASCDESC = false)
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
                if (!string.IsNullOrEmpty(where))
                    where = " where " + where;
                if (!string.IsNullOrEmpty(order))
                    order = " order by " + order + (ASCDESC ? " DESC " : "");
                string TableName = Table<T>();
                DataTable DT = getTable("select * from " + TableName + " " + where+ order, TableName);
                if (DT != null)
                {
                    if (DT.Rows.Count > 0)
                        return DataRowToEntity<T>(DT.Rows[0]);
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
        
        #endregion

    }
}
