using System;
using System.Collections.Generic;
using System.Linq;
using System.Data ;
using NK.Interface;
using NK.ENum;
using NK.Entity;
using NK.Event;
using NK.Message;
using System.ComponentModel;

namespace NK.Data
{
    /// <summary>
    /// T-SQL数据绑定到控件
    /// </summary>
    [DisplayName("DBHelper")]
    [Description("T-SQL数据绑定到控件")]
    public class DBHelper : DbUIControl, IDisposable
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
                        DBOper = new Access(this.Connection, this.Timeout);
                        break;
                    case DBType.MSSQL:
                        DBOper = new MSSql(this.Connection, this.Timeout);
                        break;
                    case DBType.Oracle:
                        DBOper = new Oracle(this.Connection, this.Timeout);
                        break;
                    case DBType.MYSQL:
                        DBOper = new MySql(this.Connection, this.Timeout);
                        break;
                    case DBType.SQLite:
                        DBOper = new SQLite(this.Connection, this.Timeout);
                        break;
                    case DBType.PostgreSQL:
                        DBOper = new PostgreSQL(this.Connection, this.Timeout);
                        break;
                    case DBType.OleDB:
                        DBOper = new OleDb(this.Connection, this.Timeout);
                        break;
                    case DBType.ODBC:
                        DBOper = new ODBC(this.Connection, this.Timeout);
                        break;
                    default:
                        throw new NotSupportedException(DB.Mode.ToString());
                }
            }
        }

        /// <summary>
        ///  Linq数据库处理
        /// </summary>
        public DBHelper() : base()
        {
            ClassName = this.GetType().ToString();
            this.language = Language.Chinese;
        }

        /// <summary>
        ///  Linq数据库处理
        /// </summary>
        /// <param name="info">数据库参数</param>
        public DBHelper(DBInfo info) : base(info)
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
        public DBHelper(DBType ConnectionType, string ConnectionString, int Timeout = 60) : base(ConnectionType, ConnectionString, Timeout)
        {
            ClassName = this.GetType().ToString();
            this.language = Language.Chinese;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        ~DBHelper()
        {
            Dispose(false);
        }
         
        #endregion
         
    }
}
