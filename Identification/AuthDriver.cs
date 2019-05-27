using NK.Entity;
using NK.ENum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NK.Identification
{
    /// <summary>
    /// 认证信息基类
    /// </summary>
   public  class AuthDriver:IDisposable
   {

        #region 定义
        protected Assembly DLL = null;  
        protected Type Class = null; 
        private bool m_disposed;
        #endregion

        #region 构造函数
         
        /// <summary>
        /// 人面识别驱动
        /// </summary>
        /// <param name="FilePath">DLL路径</param>
        public AuthDriver(FileInfo FilePath)
        {
            if (FilePath == null)
                throw new NullReferenceException("File");
            if (!FilePath.Exists)
                throw new FileNotFoundException(FilePath.FullName);
            DLL = Assembly.LoadFrom(FilePath.FullName);
        }

        /// <summary>
        /// 人面识别驱动
        /// </summary>
        /// <param name="FilePath">DLL路径</param>
        public AuthDriver(string FilePath)
        {
            if (string.IsNullOrEmpty(FilePath))
                throw new NullReferenceException("FilePath");
            var fi =new FileInfo(FilePath);
            if (fi == null)
                throw new NullReferenceException("File");
            if (!fi.Exists)
                throw new FileNotFoundException(fi.FullName);
            DLL = Assembly.LoadFrom(fi.FullName);
        }

        /// <summary>
        /// 人面识别驱动
        /// </summary>
        /// <param name="Server"></param>
        public AuthDriver(Type Server)
        {
            Class = Server;
        }

        /// <summary>
        /// 驱动信息获取
        /// </summary>
        ~AuthDriver()
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

        /// <summary>
        /// 释放连接
        /// </summary>
        /// <param name="disposing">是否释放</param>
        protected  void Dispose(bool disposing)
        {
            lock (this)
            {
                if (disposing && !m_disposed)
                {
                    dispose();
                    Class = null;
                    DLL = null;
                    GC.Collect();
                    m_disposed = true;
                }
            }
        }

        /// <summary>
        /// 重载释放类
        /// </summary>
        protected virtual void dispose()
        {

        }

        #endregion
    

    }
}
