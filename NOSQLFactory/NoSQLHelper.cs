using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NK.Entity;
using NK.ENum;
namespace NK.Data
{
    public class NoSQLHelper : IDisposable
    {
        #region 定义 
        protected DBInfo DB = new DBInfo();
        protected string connstr = "";
        protected bool m_disposed;
        #endregion

        #region 构造

        /// <summary>
        /// 重写初始化
        /// </summary>
        protected virtual void Initialization()
        {

        }

        /// <summary>
        /// 实例化
        /// </summary>
        protected void NEW()
        {
            dispose();
            GC.SuppressFinalize(this);
            init();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        protected void init()
        {
            connstr = "";
            if (DB != null)
            {
                if (string.IsNullOrEmpty(DB.ConnStr))
                    connstr = DB.ConnectionString();
                else
                    connstr = DB.ConnStr;
            }
            if (string.IsNullOrEmpty(connstr))
                throw new NullReferenceException("Database Connection");
            Initialization();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// 释放连接处理
        /// </summary>
        /// <param name="disposing">是否释放</param>
        protected void Dispose(bool disposing)
        {
            lock (this)
            {
                if (disposing && !m_disposed)
                {
                    dispose();
                    DB = null;
                    m_disposed = true;
                }
            }
        }

        protected virtual void dispose()
        {
        }

        #endregion

        #region 属性

        /// <summary>
        /// 数据库参数
        /// </summary> 
        public DBInfo DataBase
        {
            get { return DB; }
            set
            {
                if (value == null)
                {
                    DB = value;
                    NEW();
                }
            }
        }

        /// <summary>
        /// 数据库连接类型
        /// </summary> 
        public DBType DataBaseType
        {
            get { return DB.Mode; }
            set
            {
                DB.Mode = value;
                NEW();
            }
        }

        /// <summary>
        /// 数据库连接串
        /// </summary> 
        public string Connection
        {
            get { return DB.ConnStr; }
            set
            {
                DB.ConnStr = value;
                NEW();
            }
        }
        /// <summary>
        /// 数据库操作超时时间
        /// </summary> 
        public int Timeout
        {
            get { return DB.TimeOut; }
            set
            {
                DB.TimeOut = value;
                NEW();
            }
        }

        #endregion

    }
}
