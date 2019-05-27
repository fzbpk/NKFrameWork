using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Ports;
using NK.Message;
using System.Runtime.Serialization.Json;
using NK.Entity;
using NK.ENum;
using NK.Event;
using NK.Interface;
namespace NK.Communicate
{
    /// <summary>
    /// 串口通讯类
    /// </summary>
    public partial class SerialPortSDK : IDisposable, iNet
    {
        #region 定义
        private bool m_disposed;
        private SerialPort SocketRS = null;
        private string ClassName = "";
        private long m_session = 0;
        #endregion

        #region 构造

        /// <summary>
        /// 串口通讯类
        /// </summary>
        public SerialPortSDK(string connction = "")
        {
            SocketRS = new SerialPort();
            this.Connection = connction;
            ClassName = this.GetType().ToString();
            this.language = Language.Chinese;
        }

        /// <summary>
        /// 串口通讯类
        /// </summary>
        /// <param name="ComPort">端口号</param>
        /// <param name="ComRate">波特率</param>
        /// <param name="Databits">数据位</param>
        /// <param name="Stopbits">停止位</param>
        /// <param name="Parity">校验</param>
        /// <param name="ctrl">流控</param>
        public SerialPortSDK(int ComPort, int ComRate, int Databits, StopBits Stopbits, Parity Parity, Handshake ctrl)
        {
            SocketRS = new SerialPort();
            PortsSet port = new PortsSet(); 
            port.Port = ComPort;
            port.Rate = ComRate;
            port.DataBit = Databits;
            port.StopBit = Stopbits;
            port.Parity = Parity;
            port.Ctrl = ctrl;
            this.Connection = Serialize(port);
            ClassName = this.GetType().ToString();
            this.language = Language.Chinese;
        }

          /// <summary>
        /// 释放资源
        /// </summary>
        ~SerialPortSDK()
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
               if (disposing && !m_disposed  )
              { 
                  if (SocketRS != null)
                  {
                      try
                      {
                          SocketRS.Close();
                      }
                      catch { }
                      SocketRS = null;
                  }
                  m_disposed = true; 
               }         
           }
       }


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

        #region 基本属性
        /// <summary>
        /// 连接方式
        /// </summary>
        public ReferForUse IMode { get { return ReferForUse.UartSet; } }
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
        public bool IsConnected { get { return (SocketRS != null)?SocketRS .IsOpen:false; } }
        /// <summary>
        /// 显示语言
        /// </summary>
        public Language language { get; set; }
        #endregion

        #region 扩展属性

        /// <summary>
        /// 串口
        /// </summary>
        public SerialPort Uart
        {
            get
            { return SocketRS; }
            set
            {
                if (value != null)
                {
                    if (SocketRS != null)
                    {
                        try
                        {
                            if (SocketRS.IsOpen)
                                SocketRS.Close();

                        }
                        catch {   }
                        SocketRS = null;
                    }
                    SocketRS = value;
                    int ports = 0;
                    int.TryParse(value.PortName.ToUpper().Replace("COM", ""), out ports);
                    PortsSet port = new PortsSet();
                    port.Port = ports;
                    port.Rate = value.BaudRate;
                    port.DataBit = value.DataBits;
                    port.StopBit  = value.StopBits;
                    port.Parity = value.Parity;
                    port.Ctrl = value.Handshake;
                    this.Connection = Serialize(port);
                }
            }
        }
 
        /// <summary>
        /// 获取基础对象流
        /// </summary>
        public Stream BaseStream
        {
            get
            {
                try
                {
                    if (SocketRS == null)
                    { return null; }
                    else
                    { return SocketRS.BaseStream; }
                }
                catch 
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 获取或设置中断信号
        /// </summary>
        public bool BreakState
        {
            get
            {
                try
                {
                    if (SocketRS == null)
                    { return false; }
                    else
                    { return SocketRS.BreakState; }
                }
                catch 
                { 
                    return false;
                }
            }
            set
            {
                try
                {
                    if (SocketRS != null)
                    { SocketRS.BreakState = value; }
                }
                catch (Exception ex)
                {
                    
                }
            }
        }

        /// <summary>
        /// 获取接收缓存区还有多少数据未读
        /// </summary>
        public int BytesToRead
        {
            get
            {
                try
                {
                    if (SocketRS == null)
                    { return 0; }
                    else
                    { return SocketRS.BytesToRead; }
                }
                catch  
                {  return 0;
                }
            }
        }

        /// <summary>
        /// 获取发送缓存区还有多少数据未发
        /// </summary>
        public int BytesToWrite
        {
            get
            {
                try
                {
                    if (SocketRS == null)
                    { return 0; }
                    else
                    { return SocketRS.BytesToWrite; }
                }
                catch 
                { 
                    return 0;
                }
            }
        }

        /// <summary>
        /// 获取载波检测行状态
        /// </summary>
        public bool CDHolding
        {
            get
            {
                try
                {
                    if (SocketRS == null)
                    { return false; }
                    else
                    { return SocketRS.CDHolding; }
                }
                catch  
                { 
                    return false;
                }
            }
        }

        /// <summary>
        /// 获取是否可以发送
        /// </summary>
        public bool CtsHolding
        {
            get
            {
                try
                {
                    if (SocketRS == null)
                    { return false; }
                    else
                    { return SocketRS.CtsHolding; }
                }
                catch 
                {  return false;
                }
            }
        }

        /// <summary>
        /// 获取或设置忽略NULL
        /// </summary>
        public bool DiscardNull
        {
            get
            {
                try
                {
                    if (SocketRS == null)
                    { return false; }
                    else
                    { return SocketRS.DiscardNull; }
                }
                catch  
                {    return false;
                }
            }
            set
            {
                try
                {
                    if (SocketRS != null)
                    { SocketRS.DiscardNull = value; }
                }
                catch 
                {
                   
                }
            }
        }

        /// <summary>
        /// 获取就绪信号
        /// </summary>
        public bool DsrHolding
        {
            get
            {
                try
                {
                    if (SocketRS == null)
                    { return false; }
                    else
                    { return SocketRS.DsrHolding; }
                }
                catch  
                {
                    
                    return false;
                }
            }
        }

        /// <summary>
        /// 获取或设置触发时间的数据量
        /// </summary>
        public int ReceivedBytesThreshold
        {
            get
            {
                try
                {
                    if (SocketRS == null)
                    { return -1; }
                    else
                    { return SocketRS.ReceivedBytesThreshold; }
                }
                catch  
                { 
                    return -2;
                }
            }
            set
            {
                try
                {
                    if (SocketRS != null)
                    { SocketRS.ReceivedBytesThreshold = value; }
                }
                catch  
                {
                  
                }
            }
        }

        /// <summary>
        /// 获取或设置是否启用发送请求信号
        /// </summary>
        public bool RtsEnable
        {
            get
            {
                try
                {
                    if (SocketRS == null)
                    { return false; }
                    else
                    { return SocketRS.RtsEnable; }
                }
                catch  
                { 
                    return false;
                }
            }
            set
            {
                try
                {
                    if (SocketRS != null)
                    { SocketRS.RtsEnable = value; }
                }
                catch 
                {
                
                }
            }
        }

        #endregion

        #region 基本方法

        private   T Deserialize<T>(string json)
        {
            T obj = Activator.CreateInstance<T>();
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
                return (T)serializer.ReadObject(ms);
            }
        }

        private string Serialize(object obj)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            MemoryStream stream = new MemoryStream();
            serializer.WriteObject(stream, obj);
            byte[] dataBytes = new byte[stream.Length];
            stream.Position = 0;
            stream.Read(dataBytes, 0, (int)stream.Length);
            return Encoding.UTF8.GetString(dataBytes);
        }

        /// <summary>
        /// 打开串口
        /// </summary>
        public void Open()
        {
            if (string.IsNullOrEmpty(this.Connection))
            {
                if (this.HasError != null)
                    this.HasError(ClassName, "Open", new NullReferenceException(SystemMessage.RefNullOrEmpty("Connection", language)));
                else
                    throw new NullReferenceException(SystemMessage.RefNullOrEmpty("Connection", language));
                return;
            }
            PortsSet port = null;
            try
            {   port = Deserialize<PortsSet>(this.Connection); }
            catch
            {
                if (this.HasError != null)
                    this.HasError(ClassName, "Open", new InvalidCastException(SystemMessage.CastError("Connection", language)));
                else
                    throw new InvalidCastException(SystemMessage.CastError("Connection", language));
                return;
            } 
            if (port.Port <= 0)
            {
                if (this.HasError != null)
                    this.HasError(ClassName, "Open", new NullReferenceException(SystemMessage.CastError("Port", language)));
                else
                    throw new NullReferenceException(SystemMessage.CastError("Port", language));
                return;
            }
            try
            {
                SocketRS = new SerialPort();
                if (!SocketRS.IsOpen)
                {
                    SocketRS.PortName = "Com" + port.Port.ToString();
                    SocketRS.BaudRate = port.Rate;
                    SocketRS.DataBits = port.DataBit;
                    SocketRS.StopBits = port.StopBit;
                    SocketRS.Parity = port.Parity;
                    SocketRS.Handshake = port.Ctrl;

                    if (Refer_Prama != null)
                    {
                        SocketRS.ReadBufferSize = Refer_Prama.ReceiveBufferSize;
                        SocketRS.ReadTimeout = Refer_Prama.ReceiveTimeout;
                        SocketRS.WriteBufferSize = Refer_Prama.SendBufferSize;
                        SocketRS.WriteTimeout = Refer_Prama.SendTimeout;
                    }
                    else
                    {
                        SocketRS.ReadBufferSize = 1024;
                        SocketRS.ReadTimeout = 0;
                        SocketRS.WriteBufferSize =1024;
                        SocketRS.WriteTimeout = 0;
                    }

                    Encoding Charset = Encoding.Default;
                    if (this.Refer_Prama != null)
                    {
                        if (string.IsNullOrEmpty(Refer_Prama.CharSet))
                            Charset = Encoding.GetEncoding(Refer_Prama.CharSet);
                    }
                    SocketRS.Encoding = Charset;
                    SocketRS.Open();
                    System.Random Random = new System.Random();
                    m_session = Random.Next(1, int.MaxValue);
                    if (this.Connect != null)
                        this.Connect(this.Connection , "", ReferForUse.UartSet, m_session);
                }
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
        /// 关闭串口
        /// </summary>
        public void Close()
        {
            try
            {
                if (SocketRS != null)
                {
                    if (SocketRS.IsOpen)
                        SocketRS.Close();
                    if (this.DisConnect != null)
                        this.DisConnect(this.Connection, "", ReferForUse.UartSet, m_session);
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
        /// 发送数据
        /// </summary>
        /// <param name="Data">BYTE数组</param>
        /// <returns>发送数量</returns>
        public byte[] Read(int Index = 0, int Datalen = 0)
        {
            List<byte> pack = new List<byte>();
            if (SocketRS == null)
            {
                if (this.HasError != null)
                    this.HasError(ClassName, "Read", new NullReferenceException(SystemMessage.Badsequencecommands(language)));
                else
                    throw new NullReferenceException(SystemMessage.Badsequencecommands(language));
                return pack.ToArray();
            }
            else if (!SocketRS .IsOpen)
            {
                if (this.HasError != null)
                    this.HasError(ClassName, "Read", new NullReferenceException(SystemMessage.Badsequencecommands(language)));
                else
                    throw new NullReferenceException(SystemMessage.Badsequencecommands(language));
                return pack.ToArray();
            }
            try
            {
                int len = 0, ReadTout = 0, size = 1024;
                if (this.Refer_Prama != null)
                {
                    if (this.Refer_Prama.ReceiveTimeout > 0)
                        ReadTout = this.Refer_Prama.ReceiveTimeout;
                }
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
                        len = SocketRS.Read(buf, 0, buf.Length);
                        byte[] recv = new byte[len];
                        Array.Copy(buf, recv, len);
                        pack.AddRange(recv);
                        Datalen -= len;
                    }
                }
                else if (ReadTout>0)
                {
                    SocketRS.ReadTimeout = 100;
                    len = 0;
                    if (len <= 0)
                    {
                        for (int i = 0; i <= (ReadTout / SocketRS.ReadTimeout); i++)
                        {
                            try
                            { len = SocketRS.Read(buf, 0, buf.Length); }
                            catch
                            { len = 0; }
                            if (len > 0)
                                break;
                        }
                    }
                    while (len > 0)
                    {
                        byte[] recv = new byte[len];
                        Array.Copy(buf, recv, len);
                        pack.AddRange(recv);
                        buf = new byte[size];
                        try
                        { len = SocketRS.Read(buf, 0, buf.Length); }
                        catch { len = 0; }
                    }
                    SocketRS.ReadTimeout = ReadTout;
                }
                else
                {
                    len = SocketRS.Read(buf, 0, buf.Length);
                    byte[] recv = new byte[len];
                    Array.Copy(buf, recv, len);
                    pack.AddRange(recv);
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
            if (SocketRS == null)
            {
                if (this.HasError != null)
                    this.HasError(ClassName, "Write", new NullReferenceException(SystemMessage.Badsequencecommands(language)));
                else
                    throw new NullReferenceException(SystemMessage.Badsequencecommands(language));
                return false;
            }
            else if (!SocketRS.IsOpen)
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
                n = 0;
                int index = 0;
                while (Datalen > 0)
                {
                    if (Datalen > size)
                        len = size;
                    else
                        len =  Datalen;
                    buf = new byte[len];
                    Array.Copy(Data, index, buf, 0, len);
                    SocketRS.Write(buf, 0, len);
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
        /// 打开串口
        /// </summary>
        /// <param name="ComPort">端口号</param>
        /// <param name="ComRate">波特率</param>
        /// <param name="Databits">数据位</param>
        /// <param name="Stopbits">停止位</param>
        /// <param name="Parity">校验</param>
        /// <param name="ctrl">流控</param>
        /// <returns></returns>
        public bool Open(int ComPort, int ComRate, int Databits, StopBits Stopbits, Parity Parity, Handshake ctrl)
        {
            PortsSet port = new PortsSet();
            port.Port = ComPort;
            port.Rate = ComRate;
            port.DataBit = Databits;
            port.StopBit = Stopbits;
            port.Parity = Parity;
            port.Ctrl = ctrl;
            this.Connection = Serialize(port);
            Open();
            return IsConnected;
        }

        /// <summary>
        /// 发送并回复
        /// </summary>
        /// <param name="Data">发送数据</param>
        /// <returns>回复数据</returns>
        public byte[] SendBytesReply(byte[] Data )
        {
            int WaitTime = 0;
            if (this.Refer_Prama != null)
            {
                if (this.Refer_Prama.WaitTime > 0)
                    WaitTime = this.Refer_Prama.WaitTime;
            }
            if (Data != null)
                Write(Data);
            System.Threading.Thread.Sleep(WaitTime);
            return Read();
        }

        /// <summary>
        /// 发送字符串
        /// </summary>
        /// <param name="Data">字符串</param>
        /// <param name="NewLine">是否回车换行</param>
        /// <returns>发送数量</returns>
        public bool Send(string Data, bool NewLine = true)
        {
            if (SocketRS == null)
            {
                if (this.HasError != null)
                    this.HasError(ClassName, "Send", new NullReferenceException(SystemMessage.Badsequencecommands(language)));
                else
                    throw new NullReferenceException(SystemMessage.Badsequencecommands(language));
                return false ;
            }
            else if (!SocketRS.IsOpen)
            {
                if (this.HasError != null)
                    this.HasError(ClassName, "Send", new NullReferenceException(SystemMessage.Badsequencecommands(language)));
                else
                    throw new NullReferenceException(SystemMessage.Badsequencecommands(language));
                return false;
            }
            try
            {
                if (Data == null)
                    return false;
                if (NewLine)
                    SocketRS.WriteLine(Data);
                else
                    SocketRS.Write(Data);
                return true;
            }
            catch (Exception ex)
            {
                if (this.HasError != null)
                    this.HasError(ClassName, "Send", ex);
                else
                    throw ex;
                return false;
            }
          
        }

        /// <summary>
        /// 接收字符串
        /// </summary>
        /// <returns>字符串</returns>
        public string Receive()
        {
            if (SocketRS == null)
            {
                if (this.HasError != null)
                    this.HasError(ClassName, "Receive", new NullReferenceException(SystemMessage.Badsequencecommands(language)));
                else
                    throw new NullReferenceException(SystemMessage.Badsequencecommands(language));
                return "";
            }
            else if (!SocketRS.IsOpen)
            {
                if (this.HasError != null)
                    this.HasError(ClassName, "Receive", new NullReferenceException(SystemMessage.Badsequencecommands(language)));
                else
                    throw new NullReferenceException(SystemMessage.Badsequencecommands(language));
                return "";
            }
            try
            {
                return SocketRS.ReadExisting();
            }
            catch (Exception ex)
            {
                if (this.HasError != null)
                    this.HasError(ClassName, "Receive", ex);
                else
                    throw ex;
                return "";
            } 
        }

        /// <summary>
        /// 清除缓冲区
        /// </summary>
        public void Clear()
        {
            try
            {
                SocketRS.DiscardInBuffer();
                SocketRS.DiscardOutBuffer();
            }
            catch { }

        }

        #endregion
    }
}
