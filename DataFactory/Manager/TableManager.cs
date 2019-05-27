using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data;
using LinqToDB;
using System.Reflection;
using System.ComponentModel;
using LinqToDB.Mapping;
using NK.ENum;
using NK.Entity;
using NK.Message;
using NK.Attribut;

namespace NK.Data.Manager
{
    /// <summary>
    /// 实体表管理类
    /// </summary>
    /// <typeparam name="T">实体</typeparam>
    [DisplayName("DataBase")]
    [Description("数据库参数")]
    public   class TableManager<T> : DataHelper,IDisposable where T : class,
                  new()
    {
        
        #region 构造函数
        
        /// <summary>
        ///  Linq数据库处理
        /// </summary>
        public TableManager() : base()
        {
            ClassName = this.GetType().ToString();
            this.language = Language.Chinese;
        }

        /// <summary>
        ///  Linq数据库处理
        /// </summary>
        /// <param name="info">数据库参数</param>
        public TableManager(DBInfo info) : base(info)
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
        public TableManager(DBType ConnectionType, string ConnectionString, int Timeout = 60) : base(ConnectionType, ConnectionString, Timeout)
        {
            ClassName = this.GetType().ToString();
            this.language = Language.Chinese;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        ~TableManager()
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
                if (HasError != null)
                    HasError(ClassName, MethodName, ex);
                else
                    throw ex;
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
                if (HasError != null)
                    HasError(ClassName, MethodName, ex);
                else
                    throw ex;
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
                if (HasError != null)
                    HasError(ClassName, MethodName, ex);
                else
                    throw ex;
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
        public virtual T Find(Expression<Func<T, bool>> whereLambda, Expression<Func<T, object>> orderLambda = null, bool ASCDESC = false)
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
                if (HasError != null)
                    HasError(ClassName, MethodName, ex);
                else
                    throw ex;
                return null;
            } 
        }

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
        /// 查询符合条件的记录
        /// </summary>
        /// <param name="whereLambda">查询条件</param>
        /// <param name="orderLambda">查询条件</param>
        /// <param name="ASCDESC">查询条件</param>
        /// <returns>符合条件的记录</returns>
        [DisplayName("Query")]
        [Description("查询符合条件的记录")]
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
                if (HasError != null)
                    HasError(ClassName, MethodName, ex);
                else
                    throw ex;
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
        public virtual List<T> Select( int PageIndex, int PageSize, out int PageCount, out int RecordCount, Expression<Func<T, bool>> whereLambda = null, Expression<Func<T, object>> orderLambda = null, bool ASCDESC = false)
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
                if (HasError != null)
                    HasError(ClassName, MethodName, ex);
                else
                    throw ex;
                return null;
            }
        }
         
        #endregion
         
    }
}
