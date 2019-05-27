using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using NK.Entity;
using NK.ENum;
using NK.Event;
using NK.Interface;
using NK.Message;
namespace NK.Communicate
{
    /// <summary>
    /// 文件操作类
    /// </summary>
    public partial class FileIOSDK : IDisposable,iNet
    {

        #region 定义
        private bool m_disposed; 
        private FileStream file = null;
        private string ClassName = "";
        #endregion

        #region 事件

        /// <summary>
        /// 错误出现事件，性能参数内DEBUG设置为EVENT有效
        /// </summary>
        public CommEvent.HasErrorEven HasError { get; set; }
        /// <summary>
        /// 连接事件
        /// </summary>
        public NetEvent.Connect Connect { get; set; }
        /// <summary>
        /// 连接断开
        /// </summary>
        public NetEvent.DisConnect DisConnect { get; set; }

        #endregion

        #region 构造

        /// <summary>
        /// 文件操作类
        /// </summary>
        /// <param name="connction">文件路径</param>
        /// <param name="mode">打开方式</param>
        public FileIOSDK(string connction="", FileMode mode = FileMode.OpenOrCreate)
        {
            this.Connection = connction;
            this.Mode = mode;  
            ClassName = this.GetType().ToString();
            this.language = Language.Chinese;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        public FileIOSDK(FileInfo file)
        {
            this.Connection = file==null?"": file.FullName;
            this.Mode = FileMode.OpenOrCreate;
            ClassName = this.GetType().ToString();
            this.language = Language.Chinese;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        ~FileIOSDK()
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
        protected virtual void Dispose(bool disposing)
        {
            lock (this)
            {
                if (disposing && !m_disposed)
                {
                    if (file != null)
                    {
                        try
                        {
                            file.Close();
                        }
                        catch { }
                        file = null;
                    }
                    m_disposed = true;
                }
            }
        }

        #endregion

        #region 基本属性
        /// <summary>
        /// 连接方式
        /// </summary>
        public  ReferForUse IMode { get { return ReferForUse.File; } }
        /// <summary>
        ///  网络参数,JSON
        /// </summary>
        public string Connection { get; set; }
        /// <summary>
        /// 性能参数
        /// </summary>
        public ReferSet Refer_Prama { get; set; }
        /// <summary>
        /// 是否已连接
        /// </summary>
        public bool IsConnected { get { return file != null; } }
        /// <summary>
        /// 显示语言
        /// </summary>
        public Language language { get; set; }
        #endregion

        #region 扩展属性

        /// <summary>
        /// 连接模式
        /// </summary>
        public FileMode Mode { get; set; }

        #endregion

        #region 基本方法

        /// <summary>
        /// 打开文件
        /// </summary>
        public void Open()
        {
            if (string.IsNullOrEmpty(this.Connection))
            {
                if (this.HasError != null)
                    this.HasError(ClassName, "Open", new NullReferenceException(SystemMessage.RefNullOrEmpty("Connection",language)));
                else
                    throw new NullReferenceException(SystemMessage.RefNullOrEmpty("Connection", language));
            }
            try
            {
                file = new FileStream(this.Connection, this.Mode);
                if (this.Connect != null)
                    this.Connect(this.Connection, "", ReferForUse.File, file.Handle.ToInt64());
            }
            catch (Exception ex)
            {
                if (this.HasError != null)
                    this.HasError(ClassName, "Open", ex);
                else
                    throw ex;
            }
        }

        /// <summary>
        /// 关闭文件
        /// </summary>
        public void Close()
        {
            try
            {
                if (file != null)
                {
                    if (this.DisConnect != null)
                        this.DisConnect(this.Connection, "", ReferForUse.File, file.Handle.ToInt64());
                    file.Close();
                    file = null;
                }
            }
            catch (Exception ex)
            {
                if (this.HasError != null)
                    this.HasError(ClassName, "Close", ex);
                else
                    throw ex;
            }
        }

        /// <summary>
        /// 数据读取
        /// </summary>
        /// <param name="Index">读取起始位置</param>
        /// <param name="Datalen">数据量,0为全部读取</param>
        /// <returns></returns>
        public byte[] Read(int Index = 0, int Datalen = 0)
        {
            List<byte> pack = new List<byte>();
            if (file == null)
            {
                if (this.HasError != null)
                    this.HasError(ClassName, "Write", new NullReferenceException(SystemMessage.Badsequencecommands( language)));
                else
                    throw new NullReferenceException(SystemMessage.Badsequencecommands(language));
                return pack.ToArray();
            } 
            try
            { 
                file.Seek(Index, SeekOrigin.Begin);
                int len = 0, size = 1024;
                if (this.Refer_Prama != null)
                {
                    if (this.Refer_Prama.ReceiveBufferSize > 0)
                        size = this.Refer_Prama.ReceiveBufferSize;
                }
                byte[] buf = new byte[size];
                if (Datalen > 0)
                { 
                    while (Datalen > 0)
                    {
                        len = file.Read(buf, 0, size);
                        byte[] recv = new byte[len];
                        Array.Copy(buf, recv, len);
                        pack.AddRange(recv);
                        Datalen -= len;
                    }
                }
                else
                {
                    len = file.Read(buf, 0, size);
                    while (len > 0)
                    {
                        byte[] recv = new byte[len];
                        Array.Copy(buf, recv, len);
                        pack.AddRange(recv);
                        len = file.Read(buf, 0, size);
                    }
                } 
            }
            catch (Exception ex)
            {
                if (this.HasError != null)
                    this.HasError(ClassName, "Read", ex);
                else
                    throw ex;
            }
            return pack.ToArray();
        }

        /// <summary>
        /// 数据写入
        /// </summary>
        /// <param name="Data">数据</param>
        /// <param name="Index">写入起始位置</param>
        /// <returns></returns>
        public bool Write(byte[] Data, int Index = 0)
        {
            if (file == null)
            {
                if (this.HasError != null)
                    this.HasError(ClassName, "Write", new NullReferenceException(SystemMessage.Badsequencecommands(language)));
                else
                    throw new NullReferenceException(SystemMessage.Badsequencecommands(language));
                return false;
            }
            try
            {
                if (Data == null)
                    return false;
                else if (Data.Length == 0)
                    return false;
                int Datalen = Data.Length, size = 1024, len = 0, n;
                if (this.Refer_Prama != null)
                {
                    if (this.Refer_Prama.SendBufferSize > 0)
                        size = this.Refer_Prama.SendBufferSize;
                }
                byte[] buf = null;
                file.Seek(Index, SeekOrigin.End);
                n = 0;
                int index = 0;
                while (Datalen > 0)
                {
                    if (Datalen > size)
                        len = size;
                    else
                        len = Datalen;
                    buf = new byte[len];
                    Array.Copy(Data, index, buf, 0, len); 
                    file.Write(buf, 0, len);
                    Datalen -= len;
                    index += len;
                    n++;
                }
                return true;
            }
            catch (Exception ex)
            {
                if (this.HasError != null)
                    this.HasError(ClassName, "Write", ex);
                else
                    throw ex;
                return false;
            }
        }

        #endregion

        #region 扩展方法

        /// <summary>
        /// 打开文件
        /// </summary>
        public bool Open(string filepath,FileMode mode= FileMode.OpenOrCreate)
        {
            this.Connection = filepath;
            this.Mode = mode;
            Open();
            return IsConnected;
        } 
         
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="Index">读取位置</param>
        /// <returns></returns>
        public byte[] ReadToEnd(int Index = 0)
        {
            return Read(Index, 0);
        }

        /// <summary>
        /// 读取所有文本
        /// </summary>
        /// <returns></returns>
        public string ReadString()
        {
            try
            {
                if (file == null) return "";
                Encoding CharSet = Encoding.Default;
                if (this.Refer_Prama != null)
                {
                    if (string.IsNullOrEmpty(Refer_Prama.CharSet))
                        CharSet = Encoding.GetEncoding(Refer_Prama.CharSet);
                }
                StreamReader sw = new StreamReader(file ,CharSet);
                string res = sw.ReadToEnd();
                sw.Close();
                return res;
            }
            catch (Exception ex)
            {
                if (this.HasError != null)
                    this.HasError(ClassName, "ReadString", ex);
                else
                    throw ex;
                return "";
            }
        }

        /// <summary>
        /// 写入文本
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public bool WriteString(string Data)
        {
            try
            {
                if (file == null) return false;
                Encoding CharSet = Encoding.Default;
                if (this.Refer_Prama != null)
                {
                    if (string.IsNullOrEmpty(Refer_Prama.CharSet))
                        CharSet = Encoding.GetEncoding(Refer_Prama.CharSet);
                }
                StreamWriter sw = new StreamWriter(file,CharSet);
                sw.Write(Data);
                sw.Flush();
                sw.Close();
                return true;
            }
            catch (Exception ex)
            {
                if (this.HasError != null)
                    this.HasError(ClassName, "WriteString", ex);
                else
                    throw ex;
                return false;
            }
        }

        #endregion

    }
}
