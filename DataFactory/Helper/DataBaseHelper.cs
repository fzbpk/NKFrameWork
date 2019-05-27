using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Common;
using LinqToDB;
using System.ComponentModel;
using LinqToDB.Mapping;
using NK.ENum;
using NK.Entity;
using NK.Message;
using NK.Event;
using NK.Interface;
using System.Reflection;

namespace NK.Data.Helper
{
    /// <summary>
    /// 数据库处理类
    /// </summary>
    [DisplayName("DataBaseHelper")]
    [Description("数据库处理类")]
    public   class DataBaseHelper :DataHelper, IDisposable
    {

        #region 构造函数
         
        /// <summary>
        ///  Linq数据库处理
        /// </summary>
        public DataBaseHelper() : base()
        {
            ClassName = this.GetType().ToString();
            this.language = Language.Chinese;
        }

        /// <summary>
        ///  Linq数据库处理
        /// </summary>
        /// <param name="info">数据库参数</param>
        public DataBaseHelper(DBInfo info) : base(info)
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
        public DataBaseHelper(DBType ConnectionType, string ConnectionString, int Timeout = 60) : base(ConnectionType, ConnectionString, Timeout)
        {
            ClassName = this.GetType().ToString();
            this.language = Language.Chinese;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        ~DataBaseHelper()
        {
            Dispose(false);
        }
        
        #endregion
       
        #region 方法
  
        /// <summary>
        /// 获取数据库名称
        /// </summary>
        [DisplayName("DataBase")]
        [Description("获取数据库名称")]
        public string DataBaseName
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
                bool conns = false;
                IDbConnection conn = null;
                string res = "";
                try
                {
                    var DataProvider = context.DataProvider;
                    conn = DataProvider.CreateConnection(context.ConnectionString);
                    res = conn.Database;
                }
                catch (Exception ex)
                {
                    if (HasError != null)
                        HasError(ClassName, MethodName, ex);
                    else
                        throw ex; 
                }
                finally
                {
                    if (conns && conn != null)
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                    conn = null;
                }
                return res;
            }
        }

        /// <summary>
        /// 列出所有表
        /// </summary>
        [DisplayName("Tables")]
        [Description("列出所有表")]
        public List<string> Tables
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
                try
                {
                    List<string> res = new List<string>();
                    res = Structure("Tables");
                    return res;
                }
                catch(Exception ex)
                {
                    if (HasError != null)
                        HasError(ClassName, MethodName, ex);
                    else
                        throw ex;
                    return new List<string>();
                }
            }
        }

        /// <summary>
        /// 列出所有视图
        /// </summary>
        [DisplayName("Views")]
        [Description("列出所有表")]
        public List<string> Views
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
                try
                {
                    List<string> res = new List<string>();
                    res = Structure("Views");
                    return res;
                }
                catch (Exception ex)
                {
                    if (HasError != null)
                        HasError(ClassName, MethodName, ex);
                    else
                        throw ex;
                    return new List<string>();
                }
            }
        }

        /// <summary>
        /// 调用存储过程
        /// </summary>
        /// <param name="Name">存储过程名</param>
        /// <param name="Input">输入变量</param>
        /// <param name="OutPut">输出变量</param>
        ///  <param name="REF">传入传出变量</param>
        /// <param name="ReturnParameter">返回函数</param>
        /// <returns></returns>
        [DisplayName("StoredProcedure")]
        [Description("调用存储过程")]
        public DbParameter StoredProcedure(string Name, Dictionary<string, object> Input=null, Dictionary<string, object> OutPut =null, Dictionary<string, object> REF = null, object ReturnParameter = null)
        {
            init();
            MethodName = "";
            try
            {
                MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            DbParameter parReturn = null;
            bool conns = false;
            IDbConnection conn = null;
            try
            {
               
                if ( !string.IsNullOrEmpty(Name))
                {
                    var DataProvider = context.DataProvider;
                    conn = DataProvider.CreateConnection(context.ConnectionString);
                    if (conn.State != ConnectionState.Open)
                    {
                        conns = true;
                        conn.Open();
                    }
                    DbCommand cmd = conn.CreateCommand() as DbCommand;  
                    cmd.CommandText = Name; 
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = this.Timeout;
                    if (Input != null)
                    {
                        foreach (var par in Input)
                        {
                            DbParameter para = cmd.CreateParameter();
                            para.ParameterName = "@" + par.Key;
                            para.Value = par.Value;
                            para.Direction = ParameterDirection.Input ;
                            cmd.Parameters.Add(para);
                        }
                    }
                    if (OutPut != null)
                    {
                        foreach (var par in OutPut)
                        {
                            DbParameter para = cmd.CreateParameter();
                            para.ParameterName = "@" + par.Key;
                            para.Value = par.Value;
                            para.Direction = ParameterDirection.Output;
                            cmd.Parameters.Add(para);
                        }
                    }
                    if (REF != null)
                    {
                        foreach (var par in REF)
                        {
                            DbParameter para = cmd.CreateParameter();
                            para.ParameterName = "@"+ par.Key;
                            para.Value = par.Value;
                            para.Direction = ParameterDirection.InputOutput;
                            cmd.Parameters.Add(para);
                        }
                    }
                    if (ReturnParameter != null)
                    {
                        parReturn = cmd.CreateParameter();
                        parReturn.ParameterName = "@return";
                        parReturn.Value = ReturnParameter;
                        parReturn.Direction = ParameterDirection.ReturnValue;
                        cmd.Parameters.Add(parReturn);
                    }
                    cmd.ExecuteNonQuery(); 
                }
                return parReturn;
            }
            catch (Exception ex)
            {
                if (HasError != null)
                    HasError(ClassName, MethodName, ex);
                else
                    throw ex; 
            }
            finally
            {
                if (conns && conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
                conn = null;
            }
            return parReturn;
        }
 
        /// <summary>
        /// 建表
        /// </summary>
        /// <param name="EntityList"></param>
        [DisplayName("CreatTable")]
        [Description("建表")]
        public void CreatTable(List<Type> EntityList)
        {
            init();
            MethodName = "";
            try
            {
                MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            if (EntityList == null) EntityList = new List<Type>();
            string TableName = "";
            try
            { 
                List<ColumnAttribute> Columns = null;
                List<string> sqlbat = null;
                foreach (var tmp in EntityList)
                {
                    TableName = "";
                    Columns = OToSqlCreat(tmp, out TableName);
                    if (!TableIsExist(TableName))
                    {
                        if (Columns.Where(c => c.IsPrimaryKey).Count() <= 0)
                            throw new NullReferenceException(SystemMessage.RefNullOrEmpty("PrimaryKey", language));
                        sqlbat = TableName.CreatToSql(DB.Mode, Columns);
                        foreach (var tsql in sqlbat)
                        {
                            try {
                                if (Execute(tsql) <= 0)
                                    throw new Exception(tsql);
                            }
                            catch (Exception ex){
                                throw new Exception(TableName+"\t"+ex.Message);
                            }
                        }
                    }
                }
                Columns = null;
                sqlbat = null;
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
        /// 删表
        /// </summary>
        /// <param name="EntityList"></param>
        [DisplayName("DorpTable")]
        [Description("删表")]
        public void DorpTable(List<Type> EntityList)
        {
            init();
            MethodName = "";
            try
            {
                MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            if (EntityList == null) EntityList = new List<Type>();
            try
            {
                string TableName = "";
                List<string> sqlbat = null;
                foreach (var tmp in EntityList)
                {
                    TableName = Table(tmp);
                    sqlbat = TableName.DropToSql();
                    foreach (var tsql in sqlbat)
                    {
                        if (Execute(tsql) <= 0)
                            throw new Exception(tsql);
                    } 
                }
                sqlbat = null;
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
        /// 建表
        /// </summary>
        /// <param name="DllPath"></param>
        [DisplayName("CreatTable")]
        [Description("建表")]
        public void CreatTable(string DllPath)
        {
            init();
            MethodName = "";
            try
            {
                MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            if (string.IsNullOrEmpty(DllPath))
            {
                if (HasError != null)
                    HasError(ClassName, MethodName, new NullReferenceException(SystemMessage.RefNullOrEmpty("DllPath", language)));
                else
                    throw new NullReferenceException(SystemMessage.RefNullOrEmpty("DllPath", language));
            }
            else if (!System.IO.File.Exists(DllPath))
            {
                if (HasError != null)
                    HasError(ClassName, MethodName, new NullReferenceException(SystemMessage.RefNullOrEmpty("DllPath", language)));
                else
                    throw new NullReferenceException(SystemMessage.RefNullOrEmpty("DllPath", language));
            }
            try
            {
                string TableName = "";
                List<ColumnAttribute> Columns = null;
                List<string> sqlbat = null;
                foreach (var tmp in Assembly.LoadFrom(DllPath).GetTypes())
                {
                    TableAttribute[] TableAttributes = (TableAttribute[])tmp.GetCustomAttributes(typeof(TableAttribute), false);
                    if (TableAttributes != null)
                    {
                        if (TableAttributes.Length > 0)
                        {
                            TableName = "";
                            Columns = OToSqlCreat(tmp, out TableName);
                            if (!TableIsExist(TableName))
                            {
                                if (Columns.Where(c => c.IsPrimaryKey).Count() <= 0)
                                    throw new NullReferenceException(SystemMessage.RefNullOrEmpty("PrimaryKey", language));
                                sqlbat = TableName.CreatToSql(DB.Mode, Columns);
                                foreach (var tsql in sqlbat)
                                {
                                    if (Execute(tsql) <= 0)
                                        throw new Exception(tsql);
                                }
                            }
                        }
                    }
                }
                Columns = null;
                sqlbat = null;
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
        /// 删表
        /// </summary>
        /// <param name="DllPath"></param>
        [DisplayName("DorpTable")]
        [Description("删表")]
        public void DorpTable(string DllPath)
        {
            init();
            MethodName = "";
            try
            {
                MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            if (string.IsNullOrEmpty(DllPath))
            {
                if (HasError != null)
                    HasError(ClassName, MethodName, new NullReferenceException(SystemMessage.RefNullOrEmpty("DllPath", language)));
                else
                    throw new NullReferenceException(SystemMessage.RefNullOrEmpty("DllPath", language));
            }
            else if (!System.IO.File.Exists(DllPath))
            {
                if (HasError != null)
                    HasError(ClassName, MethodName, new NullReferenceException(SystemMessage.RefNullOrEmpty("DllPath", language)));
                else
                    throw new NullReferenceException(SystemMessage.RefNullOrEmpty("DllPath", language));
            }
            try
            {
                string TableName = "";
                List<string> sqlbat = null;
                foreach (var tmp in Assembly.LoadFrom(DllPath).GetTypes())
                {
                    TableAttribute[] TableAttributes = (TableAttribute[])tmp.GetCustomAttributes(typeof(TableAttribute), false);
                    if (TableAttributes != null)
                    {
                        if (TableAttributes.Length > 0)
                        {
                            TableName = Table(tmp);
                            sqlbat = TableName.DropToSql();
                            foreach (var tsql in sqlbat)
                            {
                                if (Execute(tsql) <= 0)
                                    throw new Exception(tsql);
                            }
                        }
                    }
                } 
                sqlbat = null;
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
        /// 数据库执行查询
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        [DisplayName("Query")]
        [Description("数据库执行查询")]
        public virtual DataTable Query(string sql)
        {
            init();
            MethodName = "";
            try
            {
                MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            DataTable res = null;
            try
            {
                res = getTable(sql);
            }
            catch (Exception ex)
            {
                CatchErr(ClassName, MethodName, ex);
            }
            return res;
        }

        /// <summary>
        /// 数据库执行查询
        /// </summary>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="sql"></param>
        /// <param name="RecodeCount"></param>
        /// <param name="PageCount"></param>
        /// <returns></returns>
        [DisplayName("Query")]
        [Description("数据库执行查询")]
        public virtual DataTable Query(int PageIndex, int PageSize, string sql, out int RecodeCount, out int PageCount)
        {
            init();
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
                res = getTable(PageIndex, PageSize, sql, out RecodeCount, out PageCount);
            }
            catch (Exception ex)
            {
                CatchErr(ClassName, MethodName, ex);
            } 
            return res;
        }
         
        #endregion
         
    }
}
