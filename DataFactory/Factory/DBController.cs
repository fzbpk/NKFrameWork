using System;
using System.Collections.Generic;
using System.Data;
using NK.ENum;
using NK.Entity;
using System.ComponentModel;
using System.Reflection;
using System.Data.Common;

namespace NK.Data
{
    /// <summary>
    /// 数据库T-SQL基本操作
    /// </summary>
    [DisplayName("DBController")]
    [Description("数据库T-SQL基本操作")]
    public class DBController : IDataBase,IDisposable
    {

        #region 构造函数

        /// <summary>
        /// 初始化
        /// </summary>
        protected override void Init()
        {
            if (DBOper == null)
            {
                switch (DB.Mode)
                {
                    case DBType.Access:
                        DBOper = new Access(connstr, this.Timeout);
                        break;
                    case DBType.MSSQL:
                        DBOper = new MSSql(connstr, this.Timeout);
                        break;
                    case DBType.Oracle:
                        DBOper = new Oracle(connstr, this.Timeout);
                        break;
                    case DBType.MYSQL:
                        DBOper = new MySql(connstr, this.Timeout);
                        break;
                    case DBType.SQLite:
                        DBOper = new SQLite(connstr, this.Timeout);
                        break;
                    case DBType.PostgreSQL:
                        DBOper = new PostgreSQL(connstr, this.Timeout);
                        break;
                    case DBType.OleDB:
                        DBOper = new OleDb(connstr, this.Timeout);
                        break;
                    case DBType.ODBC:
                        DBOper = new ODBC(connstr, this.Timeout);
                        break;
                    default:
                        throw new NotSupportedException(DB.Mode.ToString());
                }
                DBOper.KeepAlive = this.KeepAlive; 
                if (this.log != null)
                    DBOper.log += this.log;
                if (this.HasError != null)
                    DBOper.HasError += this.HasError; 
            }
        }
         
        /// <summary>
        /// 初始化
        /// </summary>

        /// <summary>
        /// 数据库T-SQL基本操作
        /// </summary>
        public DBController():base()
        {
            ClassName = this.GetType().ToString();
            this.language = Language.Chinese;
        }

        /// <summary>
        /// 数据库T-SQL基本操作
        /// </summary>
        public DBController(DBInfo info=null) : base(info)
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
       public DBController(DBType ConnectionType,string ConnectionString,int Timeout=60) : base(ConnectionType, ConnectionString, Timeout)
        {
            ClassName = this.GetType().ToString();
            this.language = Language.Chinese;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
       ~DBController()
      {
         Dispose(false);
      } 

       #endregion
         
    }
}
