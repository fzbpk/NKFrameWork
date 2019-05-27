using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data;
using LinqToDB;
using System.ComponentModel;
using LinqToDB.Mapping;
using NK.ENum;
using NK.Entity;
using System.Reflection;

namespace NK.Data
{
    /// <summary>
    /// Linq数据库处理
    /// </summary>
    [DisplayName("DBAdaptor")]
    [Description("Linq数据库处理")]
    public class DBAdaptor<T> : DataHelper,IDisposable where T : class,
                  new()
    {
         
        #region 构造函数

       
        /// <summary>
        ///  Linq数据库处理
        /// </summary>
        public DBAdaptor() : base()
        {
            ClassName = this.GetType().ToString();
            this.language = Language.Chinese;
        }

        /// <summary>
        ///  Linq数据库处理
        /// </summary>
        /// <param name="info">数据库参数</param>
        public DBAdaptor(DBInfo info) : base(info)
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
        public DBAdaptor(DBType ConnectionType,string ConnectionString,int Timeout=60) : base(ConnectionType, ConnectionString, Timeout)
        { 
            ClassName = this.GetType().ToString();
            this.language = Language.Chinese; 
        }

       /// <summary>
       /// 释放资源
       /// </summary>
       ~DBAdaptor()
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
        public Type EntityType
        {
            get
            {
                T org = new T();
                return org.GetType();
            }
        }

        #endregion 
 
        #region 方法
          
        /// <summary>
        /// 建表
        /// </summary>
        /// <returns></returns>
        [DisplayName("CreatTable")]
        [Description("建表")]
        public virtual void CreatTable()
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
        public virtual void ModifyTable()
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
        public virtual void DropTable()
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
        public virtual bool TableIsExist()
        {
            init();
            try
            {
                var tmp=context.GetTable<T>().FirstOrDefault();
                return true;
            }
            catch 
            {
                return false;
            }
        }

        /// <summary>
        /// 根据条件查找对象
        /// </summary>
        /// <param name="Entity">条件</param>
        /// <returns></returns>
        [DisplayName("Insert")]
       [Description("插入对象")]
       public virtual void Insert(T Entity)
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
                context.Insert(Entity);
            }
            catch (Exception ex)
            {
                CatchErr(ClassName, MethodName, ex);
            }
       }

       /// <summary>
       /// 更新实体
       /// </summary>
       /// <param name="Entity">实体</param>
       [DisplayName("Update")]
       [Description("更新实体")]
       public virtual void Update(T Entity)
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
                CatchErr(ClassName, MethodName, ex);
            }
       }

        /// <summary>
       /// 删除实体
       /// </summary>
       /// <param name="Entity"></param>
       [DisplayName("Delete")]
       [Description("删除实体")]
       public virtual void Delete(T Entity)
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
       public virtual T Find(Expression<Func<T, bool>> whereLambda,Expression<Func<T, object>> orderLambda=null,bool ASCDESC=false)
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
                if(orderLambda==null)
                  return context.GetTable<T>().FirstOrDefault(whereLambda);
                else if(ASCDESC)
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
       public virtual IQueryable<T> GetTable(Expression<Func<T, bool>> whereLambda=null)
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
        public virtual List<T> Query(Expression<Func<T, bool>> whereLambda=null, Expression<Func<T, object>> orderLambda=null,bool ASCDESC = false)
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
                if(orderLambda==null)
                   return context.GetTable<T>().Where(whereLambda).ToList();
                else if(ASCDESC)
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
        public virtual List<T> Select(int PageIndex, int PageSize, out int PageCount, out int RecordCount,Expression<Func<T, bool>> whereLambda=null, Expression<Func<T, object>> orderLambda = null, bool ASCDESC = false)
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
                if(orderLambda == null)
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
