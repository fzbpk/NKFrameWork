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
    /// 实体视图管理类
    /// </summary>
    /// <typeparam name="T">实体</typeparam>
    [DisplayName("ViewManager")]
    [Description("实体视图管理类")]
    public   class   ViewManager<T> : DataHelper, IDisposable where T : class,
                  new()
    {
        #region 定义
        private string tsql = "";
        #endregion

        #region 构造函数

        /// <summary>
        ///  Linq数据库处理
        /// </summary>
        public ViewManager() : base()
        {
            ClassName = this.GetType().ToString();
            this.language = Language.Chinese;
        }

        /// <summary>
        ///  Linq数据库处理
        /// </summary>
        /// <param name="info">数据库参数</param>
        public ViewManager(DBInfo info) : base(info)
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
        public ViewManager(DBType ConnectionType, string ConnectionString, int Timeout = 60) : base(ConnectionType, ConnectionString, Timeout)
        {
            ClassName = this.GetType().ToString();
            this.language = Language.Chinese;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        ~ViewManager()
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
        /// 获取视图名
        /// </summary>
        [DisplayName("View")]
        [Description("获取视图名")]
        public string View
        {
            get
            {
                T org = new T();
                TableAttribute[] TableAttributes = (TableAttribute[])org.GetType().GetCustomAttributes(typeof(TableAttribute), false);
                string ViewName = (TableAttributes == null ? "" : (TableAttributes.Length > 0 ? TableAttributes.ToList().First().Name.Trim() : ""));
                if (string.IsNullOrEmpty(ViewName))
                    ViewName = org.GetType().Name;
                return ViewName;
            }
        }

        /// <summary>
        /// 显示所有字段
        /// </summary>
        [DisplayName("Column")]
        [Description("显示所有字段")]
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
        /// 字段描述
        /// </summary>
        [DisplayName("DispColumn")]
        [Description("字段描述")]
        public Dictionary<ColumnAttribute, DisplayColumnAttribute> DispColumn
        {
            get
            {
                Dictionary<ColumnAttribute, DisplayColumnAttribute> ls = new Dictionary<ColumnAttribute, DisplayColumnAttribute>();
                T org = new T();
                TableAttribute[] TableAttributes = (TableAttribute[])org.GetType().GetCustomAttributes(typeof(TableAttribute), false);
                string ViewName = (TableAttributes == null ? "" : (TableAttributes.Length > 0 ? TableAttributes.ToList().First().Name.Trim() : ""));
                if (string.IsNullOrEmpty(ViewName))
                    ViewName = org.GetType().Name;
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
                        disp.Table = ViewName;
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
        /// 获取或设置视图SQL
        /// </summary>
        [DisplayName("TSQL")]
        [Description("获取或设置视图SQL")]
        public virtual string TSQL
        {
            get
            {
                init();
                MethodName = "";
                try
                {
                    MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                    MethodName = method.Name;
                }
                catch { }
                if (string.IsNullOrEmpty(this.View)) return null;
                try
                {
                    string sql = "";
                    DbConnection conn = context.DataProvider.CreateConnection(context.ConnectionString) as DbConnection;
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();
                    DbCommand cmd = conn.CreateCommand();
                    DataTable dt = conn.GetSchema("Views");
                    int m = dt.Columns.IndexOf("TABLE_NAME");
                    int n = dt.Columns.IndexOf("VIEW_DEFINITION");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        if (dr.ItemArray.GetValue(m).ToString().ToUpper().Trim() == this.View.ToUpper().Trim())
                        {
                            sql = dr.ItemArray.GetValue(n).ToString();
                            break;
                        }
                    }
                    conn.Close(); 
                    return sql;
                }
                catch (Exception ex)
                {
                    if (HasError != null)
                        HasError(ClassName, MethodName, ex);
                    else
                        throw ex;
                    return "";
                } 
            }
            set
            {
                init();
                tsql = value; 
            }
        }

        /// <summary>
        /// 建视图
        /// </summary>
        /// <returns></returns>
        [DisplayName("CreatView")]
        [Description("建视图")]
        public virtual bool CreatView()
        {
            init();
            MethodName = "";
            try
            {
                MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            if (string.IsNullOrEmpty(tsql)) return false;
            try
            {
                T org = new T();
                TableAttribute[] TableAttributes = (TableAttribute[])org.GetType().GetCustomAttributes(typeof(TableAttribute), false);
                string ViewName = (TableAttributes == null ? "" : (TableAttributes.Length > 0 ? TableAttributes.ToList().First().Name.Trim() : ""));
                if (string.IsNullOrEmpty(ViewName))
                    ViewName = org.GetType().Name; 
                string sql= ViewName.CreatViewToSql(tsql);
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
        /// 删视图
        /// </summary>
        /// <returns></returns>
        [DisplayName("DropView")]
        [Description("删视图")]
        public virtual bool DropView()
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
                string ViewName = (TableAttributes == null ? "" : (TableAttributes.Length > 0 ? TableAttributes.ToList().First().Name.Trim() : ""));
                if (string.IsNullOrEmpty(ViewName))
                    ViewName = org.GetType().Name;
                string sql = ViewName.DropViewToSql();
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
        /// 修改视图
        /// </summary>
        /// <returns></returns>
        [DisplayName("ModifyView")]
        [Description("修改视图")]
        public virtual bool ModifyView()
        {
            if (DropView())
                return CreatView();
            else
                return false;
        }

        /// <summary>
        /// 视图是否存在
        /// </summary>
        /// <returns>是否存在</returns>
        [DisplayName("ViewIsExist")]
        [Description("视图是否存在")]
        public virtual bool ViewIsExist()
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
                string ViewName = (TableAttributes == null ? "" : (TableAttributes.Length > 0 ? TableAttributes.ToList().First().Name.Trim() : ""));
                if (string.IsNullOrEmpty(ViewName))
                    ViewName = org.GetType().Name;
                DbConnection conn = context.DataProvider.CreateConnection(context.ConnectionString) as DbConnection;
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                List<string> views = new List<string>();
                DataTable dt = conn.GetSchema("Views");
                int m = dt.Columns.IndexOf("TABLE_NAME");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    views.Add(dr.ItemArray.GetValue(m).ToString());
                } 
                conn.Close();
                return views.Where(c => c.ToUpper().Trim() == ViewName.ToUpper().Trim()).Count() > 0;
            }
            catch(Exception ex)
            {
                if (HasError != null)
                    HasError(ClassName, MethodName, ex);
                else
                    throw ex;
                return false;
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
        [DisplayName("GetView")]
        [Description("查询符合条件的记录")]
        public virtual IQueryable<T> GetView(Expression<Func<T, bool>> whereLambda = null)
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
