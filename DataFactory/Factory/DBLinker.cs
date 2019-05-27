using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data;
using System.Data.Common;
using System.Text;
using LinqToDB;
using System.Runtime.Serialization.Json;
using System.IO;
using System.ComponentModel;
using LinqToDB.Mapping;
using NK.ENum;
using NK.Entity;
using NK.Event;
using NK.Message;
using System.Reflection;

namespace NK.Data
{
    /// <summary>
    /// Linq数据库多表处理
    /// </summary>
    [DisplayName("DBLinker")]
    [Description("Linq数据库多表处理")]
    public class DBLinker:  DataHelper, IDisposable
    {
        
        #region 构造函数

        /// <summary>
        ///  Linq数据库处理
        /// </summary>
        public DBLinker() : base()
        {
            ClassName = this.GetType().ToString();
            this.language = Language.Chinese;
        }

        /// <summary>
        ///  Linq数据库处理
        /// </summary>
        /// <param name="info">数据库参数</param>
        public DBLinker(DBInfo info) : base(info)
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
        public DBLinker(DBType ConnectionType, string ConnectionString, int Timeout = 60) : base(ConnectionType, ConnectionString, Timeout)
        {
            DB.Mode = ConnectionType;
            this.connstr = ConnectionString;
            DB.TimeOut = Timeout;
            ClassName = this.GetType().ToString();
            this.language = Language.Chinese; 
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        ~DBLinker()
        {
            Dispose(false);
        } 

        #endregion
  
        #region 方法
        
        /// <summary>
        /// 建表
        /// </summary>
        /// <returns></returns>
        [DisplayName("CreatTable")]
        [Description("建表")]
        public virtual void CreatTable<T>() where T : class, new()
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
                T org = new T();
                TableAttribute[] TableAttributes = (TableAttribute[])org.GetType().GetCustomAttributes(typeof(TableAttribute), false);
                string TableName = (TableAttributes == null ? "" : (TableAttributes.Length > 0 ? TableAttributes.ToList().First().Name.Trim() : ""));
                if (string.IsNullOrEmpty(TableName))
                    TableName = org.GetType().Name;
                context.CreateTable<T>(TableName);
            }
            catch (Exception ex)
            {
                CatchErr(ClassName, MethodName, ex);
            }
        }

        /// <summary>
        /// 修改表
        /// </summary>
        /// <returns></returns>
        [DisplayName("ModifyTable")]
        [Description("修改表")]
        public virtual void ModifyTable<T>() where T : class, new()
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
                try
                {
                    T org = new T();
                    TableAttribute[] TableAttributes = (TableAttribute[])org.GetType().GetCustomAttributes(typeof(TableAttribute), false);
                    TableName = (TableAttributes == null ? "" : (TableAttributes.Length > 0 ? TableAttributes.ToList().First().Name.Trim() : ""));
                    if (string.IsNullOrEmpty(TableName))
                        TableName = org.GetType().Name;
                    var tmp = context.GetTable<T>().FirstOrDefault();
                }
                catch
                {
                    context.DropTable<T>(TableName);
                    context.CreateTable<T>(TableName);
                }
            }
            catch (Exception ex)
            {
                CatchErr(ClassName, MethodName, ex);
            }
        }

        /// <summary>
        /// 删表
        /// </summary>
        /// <returns></returns>
        [DisplayName("DropTable")]
        [Description("删表")]
        public virtual void DropTable<T>() where T : class, new()
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
                T org = new T();
                TableAttribute[] TableAttributes = (TableAttribute[])org.GetType().GetCustomAttributes(typeof(TableAttribute), false);
                string TableName = (TableAttributes == null ? "" : (TableAttributes.Length > 0 ? TableAttributes.ToList().First().Name.Trim() : ""));
                if (string.IsNullOrEmpty(TableName))
                    TableName = org.GetType().Name;
                context.DropTable<T>(TableName);
            }
            catch (Exception ex)
            {
                CatchErr(ClassName, MethodName, ex);
            }
        }

        /// <summary>
        /// 表是否存在
        /// </summary>
        /// <returns>是否存在</returns>
        [DisplayName("TableIsExist")]
        [Description("表是否存在")]
        public virtual bool TableIsExist<T>() where T : class, new()
        {
            init();
            try
            {
                var tmp = context.GetTable<T>().FirstOrDefault();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 插入对象
        /// </summary>
        /// <param name="Entity">条件</param>
        /// <returns></returns>
        [DisplayName("Insert")]
        [Description("插入对象")]
        public virtual void Insert<T>(T Entity) where T :class,new()
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
            {context.Insert(Entity);}
            catch (Exception ex)
            {
                try
                {
                    string TableName = "";
                    Dictionary<KeyValuePair<string, DataType>, object> pram = null;
                    var ColSVal = EToSqlInsertEX(Entity, out TableName);
                    string sql = TableName.InsertToSQLEX(DataBaseType, ColSVal,out pram);
                    Execute(sql, pram);
                }
                catch
                { CatchErr(ClassName, MethodName, ex); } 
            }
        }
  
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="Entity">实体</param>
        [DisplayName("Update")]
        [Description("更新实体")]
        public virtual void Update<T>(T Entity) where T : class, new()
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
                context.Update(Entity);
            }
            catch (Exception ex)
            {
                string SQL = "";
                try
                {
                    Dictionary<KeyValuePair<string, DataType>, object> pram = null;
                    string where = "", TableName = "";
                    var ColSVal = EToSqlUpdateEX(Entity, out TableName, out where );
                    if (string.IsNullOrEmpty(where))
                        throw new NullReferenceException(SystemMessage.RefNullOrEmpty("PriKey", language));
                    else
                        where = " where " + where;
                    SQL = TableName.UpdateToSQLEX(DataBaseType, ColSVal,out pram) + where; 
                    Execute(SQL, pram); 
                }
                catch { CatchErr(ClassName, SQL, ex); } 
            }
        }
         
        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="Entity"></param>
        [DisplayName("Delete")]
        [Description("删除实体")]
        public virtual void Delete<T>(T Entity) where T : class, new()
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
                context.Delete(Entity);
            }
            catch (Exception ex)
            {
                CatchErr(ClassName, MethodName, ex);
            }
        }

        /// <summary>
        /// 根据条件查找对象
        /// </summary>
        /// <param name="whereLambda">条件</param>
        /// <param name="orderLambda">排序</param>
        /// <param name="ASCDESC">顺序倒叙</param>
        /// <returns></returns>
        [DisplayName("Find")]
        [Description("根据条件查找对象")]
        public virtual T Find<T>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, object>> orderLambda = null, bool ASCDESC = false) where T : class, new()
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
                if (orderLambda == null)
                    return context.GetTable<T>().FirstOrDefault(whereLambda);
                else if (ASCDESC)
                    return context.GetTable<T>().OrderByDescending(orderLambda).FirstOrDefault(whereLambda);
                else
                    return context.GetTable<T>().OrderBy(orderLambda).FirstOrDefault(whereLambda);
            }
            catch (Exception ex)
            {
                CatchErr(ClassName, MethodName, ex);
                return null;
            }
        }

        /// <summary>
        /// 查询符合条件的记录
        /// </summary>
        /// <param name="whereLambda">查询条件</param>
        /// <returns>符合条件的记录</returns>
        [DisplayName("Query")]
        [Description("查询符合条件的记录")]
        public virtual IQueryable<T> GetTable<T>(Expression<Func<T, bool>> whereLambda = null) where T : class, new()
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
                CatchErr(ClassName, MethodName, ex);
                return null;
            }

        }

        /// <summary>
        /// 查询符合条件的记录
        /// </summary>
        /// <param name="whereLambda">查询条件</param>
        /// <param name="orderLambda">查询条件</param>
        /// <param name="ASCDESC">查询条件</param>
        /// <returns>符合条件的记录</returns>
        [DisplayName("Query")]
        [Description("查询符合条件的记录")]
        public virtual List<T> Query<T>(Expression<Func<T, bool>> whereLambda = null, Expression<Func<T, object>> orderLambda = null, bool ASCDESC = false) where T : class, new()
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
                if (orderLambda == null)
                    return context.GetTable<T>().Where(whereLambda).ToList();
                else if (ASCDESC)
                    return context.GetTable<T>().Where(whereLambda).OrderByDescending(orderLambda).ToList();
                else
                    return context.GetTable<T>().Where(whereLambda).OrderBy(orderLambda).ToList();
            }
            catch (Exception ex)
            {
                CatchErr(ClassName, MethodName, ex);
                return null;
            } 
        }

        /// <summary>
        /// 分页查询数据
        /// </summary>
        /// <param name="PageIndex">当前页，从1开始</param>
        /// <param name="PageSize">页面大小</param>
        /// <param name="PageCount">总页数</param>
        /// <param name="RecordCount">记录数</param>
        /// <param name="whereLambda">查询条件</param>
        /// <param name="orderLambda">排序字段</param>
        /// <param name="ASCDESC">顺序倒叙</param>
        /// <returns>符合条件的记录</returns>
        [DisplayName("Page")]
        [Description("分页查询数据")]
        public virtual List<T> Select<T>(int PageIndex, int PageSize, out int PageCount, out int RecordCount, Expression<Func<T, bool>> whereLambda = null, Expression<Func<T, object>> orderLambda = null, bool ASCDESC = false) where T : class, new()
        {
            init();
            MethodName = "";
            try
            {
                MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            PageCount = 0;
            RecordCount = 0;
            try
            {
                if (whereLambda == null) whereLambda = c => true;
                var query = context.GetTable<T>().Where(whereLambda);
                RecordCount = query.Count();
                if (PageSize == 0)
                    PageCount = (RecordCount > 0 ? 1 : 0);
                else
                    PageCount = (RecordCount % PageSize == 0 ? RecordCount / PageSize : RecordCount / PageSize + 1);
                if (orderLambda == null)
                    return query.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
                else if (ASCDESC)
                    return query.OrderByDescending(orderLambda).Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
                else
                    return query.OrderBy(orderLambda).Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
            }
            catch (Exception ex)
            {
                RecordCount = 0;
                CatchErr(ClassName, MethodName, ex);
                return null;
            }
        }

       
        #endregion
        
    }
}
