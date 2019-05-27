using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.Serialization.Json;
using NK.Entity;
using NK.ENum;
using NK.Event;
using NK.Interface;
using NK.Message;
namespace NK.Communicate
{
    /// <summary>
    /// TCPIP通讯类
    /// </summary>
    public partial class SocketSDK : IDisposable, iNet
    {
        #region 定义
        private IPEndPoint EndPoint = new IPEndPoint(IPAddress.Any,0);
        private Socket_ASyncSend ASYWork = null;
        private Socket_ASyncRecv ASYRecv = null;
        private bool m_disposed;
        private Socket SocketRS = null;
        private string ClassName = ""; 
        #endregion

        #region 构造

       /// <summary>
       ///  TCPIP通讯类
       /// </summary>
       public SocketSDK(string connction = "")
       {
            this.Connection = connction;
            ClassName = this.GetType().ToString();
            this.language = Language.Chinese;
        }

       /// <summary>
       ///  TCPIP通讯类
       /// </summary>
       /// <param name="address_family">地址类型</param>
       /// <param name="socket_type">SOCKET类型</param>
       /// <param name="protocol_type">协议类型</param>
       public SocketSDK(Net_Mode Mode,AddressFamily address_family, SocketType socket_type, ProtocolType protocol_type,string IP,int Port)
       {
            NetSet net = new NetSet();
            net.Address_Family = address_family;
            net.Socket_Type = socket_type;
            net.Protocol_Type = protocol_type;
            net.IPAddress = IP;
            net.Port = Port;
            net.Mode = Mode;
            this.Connection = Serialize(net);
            ClassName = this.GetType().ToString();
            this.language = Language.Chinese;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
       ~SocketSDK()
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
                        if (ASYWork != null)
                        {
                            ASYWork.SendEnA = false;
                            ASYWork.SendEnB = false;
                            Thread.Sleep(1000);
                            ASYWork = null;
                        }
                        if (ASYRecv != null)
                        {
                            ASYRecv.Listen = false;
                            Socket socks = new Socket(SocketRS.AddressFamily, SocketRS.SocketType, SocketRS.ProtocolType);
                            try
                            { socks.Connect(EndPoint); }
                            catch
                            { 

                            }
                            socks.Close();
                            Thread.Sleep(1000);
                            ASYRecv = null;
                        }
                        try
                      {
                          SocketRS.Shutdown(SocketShutdown.Both);
                          System.Threading.Thread.Sleep(100);
                          SocketRS.Close();
                      }
                      catch { }
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
        /// <summary>
        /// 异步接收事件
        /// </summary>
        /// <param name="socket">Socket连接</param>
        public delegate void Socket_ASyncRecvedEven(ref Socket socket);
        /// <summary>
        /// 异步接收事件
        /// </summary>
        public event Socket_ASyncRecvedEven ASyncRecved = null;
        #endregion
         
        #region 基本属性
        /// <summary>
        /// 连接方式
        /// </summary>
        public ReferForUse IMode { get { return ReferForUse.NetSet; } }
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
        public bool IsConnected { get { return (SocketRS != null) ? SocketRS.Connected || Socket.IsBound : false; } }
        /// <summary>
        /// 显示语言
        /// </summary>
        public Language language { get; set; }
        #endregion

        #region 扩展属性

        /// <summary>
        /// PING次数
        /// </summary>
        public int PingTimes { get; set; }
 
       /// <summary>
       /// 转换Socket
       /// </summary>
       public Socket Socket
       {
           get
           { return SocketRS; }
           set
           {
               if (value != null)
               {
                   if (SocketRS != null)
                   {
                       if (SocketRS.IsBound || SocketRS.Connected)
                       { SocketRS.Close(); }
                       SocketRS = null;
                   }
                   SocketRS = value;
                   EndPoint = (IPEndPoint)SocketRS.LocalEndPoint;
                    NetSet net = new NetSet();
                    net.Address_Family = SocketRS.AddressFamily;
                    net.Socket_Type = SocketRS.SocketType;
                    net.Protocol_Type = SocketRS.ProtocolType;
                    net.IPAddress = EndPoint.Address.ToString();
                    net.Port = EndPoint.Port;
                    this.Connection = Serialize(net);
                } 
           }
       }
 
       /// <summary>
       /// 异步接收是否启用
       /// </summary>
       public bool ASyncRecvState
       {
           get
           {
               try
               {
                   if (ASYRecv == null)
                       return false;
                   return ASYRecv.Listen;
               }
               catch  
               { 
                   return false;
               }
           }
       }

       /// <summary>
       /// 异步发送队列剩余数
       /// </summary>
       public int ASyncListCount
       {
           get
           {
               try
               {
                   if (ASYWork == null)
                       return -1;
                   int res = 0;
                   if (ASYWork.SendLsA != null)
                       res += ASYWork.SendLsA.Count;
                   if (ASYWork.SendLsB != null)
                       res += ASYWork.SendLsB.Count;
                   return res;
               }
               catch  
               { 
                   return -2;
               }
           }
       }

       /// <summary>
       /// 异步发送状态
       /// </summary>
       public bool ASyncState
       {
           get
           {
               try
               {
                   if (ASYWork == null)
                       return false;
                   return ASYWork.SendEnA || ASYWork.SendEnB;
               }
               catch  
               { 
                   return false;
               }
           }
       }

       /// <summary>
       /// 异步通信错误
       /// </summary>
       public string ASyncErrMessage
       {
           get
           {
               try
               {
                   if (ASYWork == null)
                       return "未开启异步连接";
                   return ASYWork.ErrMessage;
               }
               catch(Exception ex)
               {
                   return ex.Message;
               }
           }
       }

       /// <summary>
       /// 获取可接收字节数
       /// </summary>
       public int Available
       {
           get
           {
               try
               {
                   if (SocketRS == null)
                   { return -1; }
                   else
                   { return SocketRS.Available; }
               }
               catch  
               { 
                   return -2;
               }
           }
       }

        /// <summary>
        /// 获取或设置阻塞状态
        /// </summary>
       public bool Blocking
       {
           get
           {
               try
               {
                   if (SocketRS == null)
                   { return false; }
                   else
                   { return SocketRS.Blocking; }
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
                   { SocketRS.Blocking = value; }
               }
               catch  
               { 
               }
           }
       }

       /// <summary>
       /// 获取或设置数据是否分包
       /// </summary>
       public bool DontFragment
       {
           get
           {
               try
               {
                   if (SocketRS == null)
                   { return false; }
                   else
                   { return SocketRS.DontFragment; }
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
                   { SocketRS.DontFragment = value; }
               }
               catch  
               {
                   
               }
           }
       }
             
        /// <summary>
       /// 获取或设置是否允许多播
        /// </summary>
       public bool ExclusiveAddressUse
       {
           get
           {
               try
               {
                   if (SocketRS == null)
                   { return false; }
                   else
                   { return SocketRS.ExclusiveAddressUse; }
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
                   { SocketRS.ExclusiveAddressUse = value; }
               }
               catch  
               {
                  
               }
           }
       }
         
        /// <summary>
       /// 获取或设置启用保持连接
        /// </summary>
       public bool KeepAlive
       {
           set
           {
               try
               {
                   if (SocketRS != null)
                   {
                       if (value)
                       {
                           byte[] inValue = new byte[] { 1, 0, 0, 0, 0x20, 0x4e, 0, 0, 0xd0, 0x07, 0, 0 };
                           SocketRS.SetSocketOption(System.Net.Sockets.SocketOptionLevel.Socket, System.Net.Sockets.SocketOptionName.KeepAlive, true);
                           SocketRS.IOControl(IOControlCode.KeepAliveValues, BitConverter.GetBytes(1), null);
                       }
                       else
                       {
                           byte[] inValue = new byte[] { 1, 0, 0, 0, 0x20, 0x4e, 0, 0, 0xd0, 0x07, 0, 0 };
                           SocketRS.SetSocketOption(System.Net.Sockets.SocketOptionLevel.Socket, System.Net.Sockets.SocketOptionName.KeepAlive, false);
                       }
                   }
               }
               catch  
               { 
               }
           }
           get
           {
               try
               {
                   if (SocketRS != null)
                   {
                       return (bool)SocketRS.GetSocketOption(System.Net.Sockets.SocketOptionLevel.Socket, System.Net.Sockets.SocketOptionName.KeepAlive);
                   }
                   else
                   { return false; }
               }
               catch 
               { 
                   return false;
               }
           }
       }

      /// <summary>
      /// 获取本机所有IP地址
      /// </summary>
      /// <returns></returns>
       public List<string> LocalIPAddress
       {
            get
            {
                List<string> res = new List<string>();
                try
                {
                    string strHostName = Dns.GetHostName(); //得到本机的主机名 
                    IPHostEntry ipEntry = Dns.GetHostEntry(strHostName); //取得本机IP 
                    for (int i = 0; i < ipEntry.AddressList.Length; i++)
                        res.Add(ipEntry.AddressList[i].ToString());
                }
                catch
                { }
                return res;
            } 
       }

      /// <summary>
      /// 获取远端连接
      /// </summary>
       public IPEndPoint RemoteEndPoint
       {
           get
           {
               try
               {
                   if (SocketRS != null)
                       return (IPEndPoint)SocketRS.RemoteEndPoint;
               }
               catch  
               {   }
               return null;
           }
       }

        /// <summary>
        /// 获取会话句柄
        /// </summary>
       public IntPtr Handle
        {
            get {
                try
                {
                    if (SocketRS != null)
                        return SocketRS.Handle;
                }
                catch (Exception ex)
                {   }
                return IntPtr.Zero;
            }
        } 

       #endregion

        #region 基本方法

        private T Deserialize<T>(string json)
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
       /// 打开远程连接
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
            NetSet net = null;
            try
            { net = Deserialize<NetSet>(this.Connection); }
            catch
            {
                if (this.HasError != null)
                    this.HasError(ClassName, "Open", new InvalidCastException(SystemMessage.CastError("Connection", language)));
                else
                    throw new InvalidCastException(SystemMessage.CastError("Connection", language));
                return;
            }
            if (net.Port <= 0)
            {
                if (this.HasError != null)
                    this.HasError(ClassName, "Open", new InvalidCastException(SystemMessage.CastError("Port", language)));
                else
                    throw new InvalidCastException(SystemMessage.CastError("Port", language));
                return;
            }
            try
           {
                int ConnPool=1,ConnectTimeOut=0;
                IPAddress addr = IPAddress.Any;
                if (string.IsNullOrEmpty(net.IPAddress))
                    EndPoint.Address = IPAddress.Any;
                else if (IPAddress.TryParse(net.IPAddress, out addr))
                    EndPoint.Address = addr;
                EndPoint.Port = net.Port;
                if (this.Refer_Prama != null)
                {
                    if (this.Refer_Prama.ConnPool > 0)
                        ConnPool = this.Refer_Prama.ConnPool;
                    if (this.Refer_Prama.ConnectTimeOut > 0)
                        ConnectTimeOut = this.Refer_Prama.ConnectTimeOut;
                }
                if (SocketRS == null)
                    SocketRS = new Socket(net.Address_Family, net.Socket_Type, net.Protocol_Type);
                if (net.Mode == Net_Mode.Local)
                {
                    if (!SocketRS.IsBound )
                    {
                        if (Refer_Prama != null)
                        {
                            SocketRS.ReceiveBufferSize = Refer_Prama.ReceiveBufferSize;
                            SocketRS.ReceiveTimeout = Refer_Prama.ReceiveTimeout;
                            SocketRS.SendBufferSize = Refer_Prama.SendBufferSize;
                            SocketRS.SendTimeout = Refer_Prama.SendTimeout;
                        }
                        else
                        {
                            SocketRS.ReceiveBufferSize = 1024;
                            SocketRS.ReceiveTimeout = 0;
                            SocketRS.SendBufferSize = 1024;
                            SocketRS.SendTimeout = 0;
                        } 
                        SocketRS.Bind(EndPoint);
                        SocketRS.Listen(ConnPool); 
                    }
                }
                else if (net.Mode == Net_Mode.Remote)
                {
                    if (!SocketRS.Connected)
                    { 
                        if (Refer_Prama != null)
                        {
                            SocketRS.ReceiveBufferSize  = Refer_Prama.ReceiveBufferSize;
                            SocketRS.ReceiveTimeout  = Refer_Prama.ReceiveTimeout;
                            SocketRS.SendBufferSize = Refer_Prama.SendBufferSize;
                            SocketRS.SendTimeout = Refer_Prama.SendTimeout;
                        }
                        else
                        {
                            SocketRS.ReceiveBufferSize = 1024;
                            SocketRS.ReceiveTimeout = 0;
                            SocketRS.SendBufferSize = 1024;
                            SocketRS.SendTimeout = 0;
                        }
                        ASYWork = null;
                        ASYRecv = null;
                        if (this.PingTimes > 0)
                        {
                            for (int counts = 0; counts < PingTimes; counts++)
                            {
                                System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping();
                                System.Net.NetworkInformation.PingReply Res = ping.Send(EndPoint.Address.ToString(), ConnectTimeOut);
                                if (Res.Status == System.Net.NetworkInformation.IPStatus.Success)
                                {
                                    SocketRS.Connect(EndPoint);
                                    if (this.Connect != null)
                                        this.Connect(SocketRS.LocalEndPoint.ToString(), this.Connection, ReferForUse.NetSet, SocketRS.Handle.ToInt64());
                                    break;
                                }
                            }
                        }
                        else
                        {
                            SocketRS.Connect(EndPoint);
                            if (this.Connect != null)
                                this.Connect(SocketRS.LocalEndPoint.ToString(), this.Connection, ReferForUse.NetSet, SocketRS.Handle.ToInt64());
                        }
                    }
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
       /// 关闭连接
       /// </summary>
       public void Close()
       {
            try
            {
                if (SocketRS != null)
                {
                    if (SocketRS.IsBound)
                    {
                        try
                        {
                            Socket socks = new Socket(SocketRS.AddressFamily, SocketRS.SocketType, SocketRS.ProtocolType);
                            socks.Connect(EndPoint);
                            socks.Close();
                            socks = null;
                        }
                        catch { }
                        if (this.DisConnect != null)
                            this.DisConnect(this.Connection ,"", ReferForUse.NetSet, SocketRS.Handle.ToInt64());
                    }
                    else if (SocketRS.Connected )
                    {
                        if (this.DisConnect != null)
                            this.DisConnect(SocketRS.LocalEndPoint.ToString(), this.Connection, ReferForUse.NetSet, SocketRS.Handle.ToInt64());
                    } 
                    try
                    { SocketRS.Shutdown(SocketShutdown.Both);  }
                    catch { }
                    try
                    {  SocketRS.Close();  }
                    catch { }
                    Thread.Sleep(100);
                    SocketRS = null;
                    ASYRecv = null;
                    ASYWork = null;
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
                    this.HasError(ClassName, "Write", new NullReferenceException(SystemMessage.Badsequencecommands(language)));
                else
                    throw new NullReferenceException(SystemMessage.Badsequencecommands(language));
                return pack.ToArray();
            }
            else if (!(SocketRS.IsBound || SocketRS.Connected))
            {
                if (this.HasError != null)
                    this.HasError(ClassName, "Write", new NullReferenceException(SystemMessage.Badsequencecommands(language)));
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
                        len = SocketRS.Receive(buf);
                        byte[] recv = new byte[len];
                        Array.Copy(buf, recv, len);
                        pack.AddRange(recv);
                        Datalen -= len;
                    }
                }
                else if (ReadTout > 0)
                {
                    SocketRS.ReceiveTimeout  = 100;
                    len = 0;
                    if (len <= 0)
                    {
                        for (int i = 0; i <= (ReadTout / SocketRS.ReceiveTimeout); i++)
                        {
                            try
                            { len = SocketRS.Receive(buf); }
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
                        { len = SocketRS.Receive(buf); }
                        catch { len = 0; }
                    }
                    SocketRS.ReceiveTimeout = ReadTout;
                }
                else
                {
                    len = SocketRS.Receive(buf);
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
            else if (!(SocketRS.IsBound || SocketRS.Connected) )
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
                    if (Datalen >  size)
                        len = size;
                    else
                        len =  Datalen;
                    buf = new byte[len];
                    Array.Copy(Data, index, buf, 0, len);
                    len=SocketRS.Send(buf);
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
        /// 打开目标连接
        /// </summary>
        /// <param name="IP">IP</param>
        /// <param name="Port">端口</param>
        public bool Open(string IP, int Port)
       {
            NetSet net = new NetSet();
            net.Address_Family =  AddressFamily.InterNetwork ;
            net.Socket_Type =  SocketType.Stream ;
            net.Protocol_Type = ProtocolType.Tcp;
            net.IPAddress = IP;
            net.Port = Port;
            net.Mode = Net_Mode.Remote;
            this.Connection = Serialize(net);
            Open();
            return IsConnected;
        }

        /// <summary>
        /// 绑定并监听端口
        /// </summary>
        /// <param name="IP">IP</param>
        /// <param name="Port">端口</param>
        public bool Bind(string IP, int Port)
       {
            NetSet net = new NetSet();
            net.Address_Family = AddressFamily.InterNetwork;
            net.Socket_Type = SocketType.Stream;
            net.Protocol_Type = ProtocolType.Tcp;
            net.IPAddress = IP;
            net.Port = Port;
            net.Mode = Net_Mode.Local;
            this.Connection = Serialize(net);
            Open();
            return IsConnected;
        }

        /// <summary>
        /// 接收到新连接
        /// </summary>
       /// <returns>Socket</returns>
       public Socket AcceptAs()
       { 
           try
           {
               if (SocketRS != null )
               {
                   if (SocketRS.IsBound)
                        return SocketRS.Accept();
               }
           }
           catch (Exception ex)
           {
                if (this.HasError != null)
                    this.HasError(ClassName, "AcceptAs", ex);
                else
                    throw ex;
            }
           return null;
        }

       /// <summary>
       /// 接收到新连接
       /// </summary>
       /// <returns>SocketSDK</returns>
       public SocketSDK Accept()
       {
           try
           {
               if (SocketRS != null)
               {
                   if (SocketRS.IsBound)
                   {
                        SocketSDK ss= new SocketSDK();
                        var sock= SocketRS.Accept();
                        IPEndPoint lop = (IPEndPoint)SocketRS.LocalEndPoint;
                        NetSet net = new NetSet();
                        net.Address_Family = sock.AddressFamily;
                        net.Socket_Type = sock.SocketType;
                        net.Protocol_Type = sock.ProtocolType;
                        net.IPAddress = lop.Address.ToString();
                        net.Port = lop.Port;
                        net.Mode = Net_Mode.Remote;
                        ss.Socket = sock;
                        ss.Connection = Serialize(net);
                        ss.Refer_Prama = this.Refer_Prama; 
                       return ss;
                   }
               }
           }
           catch (Exception ex)
           {
                if (this.HasError != null)
                    this.HasError(ClassName, "Accept", ex);
                else
                    throw ex;
            }
            return null;
        }
         
       
        /// <summary>
        /// 发送并接收数据
        /// </summary>
       /// <param name="Data">发送数据</param>
        /// <returns>接收数据</returns>
       public byte[] SendBytesReply(byte[] Data)
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
       public bool SendString(string Data, bool NewLine = true)
       {
           try
           {
               if (Data == null)
                   return false ;
               if (!Data.Contains("\r\n"))
                   Data += "\r\n";
                Encoding CharSet = Encoding.Default;
                if (this.Refer_Prama != null)
                {
                    if (string.IsNullOrEmpty(Refer_Prama.CharSet))
                        CharSet = Encoding.GetEncoding(Refer_Prama.CharSet);
                } 
               byte[] data = CharSet.GetBytes(Data);
               return Write(data);
           }
           catch (Exception ex)
           {
                if (this.HasError != null)
                    this.HasError(ClassName, "SendString", ex);
                else
                    throw ex;
                return false;
           }
       }

       /// <summary>
       /// 接收字符串
       /// </summary>
       /// <returns>字符串</returns>
       public string Receive( )
       {
            byte[] data = Read();
            if (data.Length <= 0)
                return "";
            Encoding CharSet = Encoding.Default;
            if (this.Refer_Prama != null)
            {
                if (string.IsNullOrEmpty(Refer_Prama.CharSet))
                    CharSet = Encoding.GetEncoding(Refer_Prama.CharSet);
            }
            return CharSet.GetString(data);
       }
         
        #endregion

        #region 高级设置

        /// <summary>
        /// 高级设置
        /// </summary>
        /// <param name="optionLevel"></param>
        /// <param name="optionName"></param>
        /// <param name="optionValue"></param>
        public void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, bool optionValue)
        {
            try
            {
                if (SocketRS != null)
                {
                    SocketRS.SetSocketOption(optionLevel, optionName, optionValue);
                }
            }
            catch(Exception ex)
            {
                if (this.HasError != null)
                    this.HasError(ClassName, "SetSocketOption", ex);
                else
                    throw ex;
            }
        }

        /// <summary>
        /// 高级设置
        /// </summary>
        /// <param name="optionLevel"></param>
        /// <param name="optionName"></param>
        /// <param name="optionValue"></param>
        public void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, byte[] optionValue)
        {
            try
            {
                if (SocketRS != null)
                {
                    SocketRS.SetSocketOption(optionLevel, optionName, optionValue);
                }
            }
            catch (Exception ex)
            {
                if (this.HasError != null)
                    this.HasError(ClassName, "SetSocketOption", ex);
                else
                    throw ex;
            }
        }

        /// <summary>
        /// 高级设置
        /// </summary>
        /// <param name="optionLevel"></param>
        /// <param name="optionName"></param>
        /// <param name="optionValue"></param>
        public void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, int optionValue)
        {
            try
            {
                if (SocketRS != null)
                {
                    SocketRS.SetSocketOption(optionLevel, optionName, optionValue);
                }
            }
            catch (Exception ex)
            {
                if (this.HasError != null)
                    this.HasError(ClassName, "SetSocketOption", ex);
                else
                    throw ex;
            }
        }

        /// <summary>
        /// 高级设置
        /// </summary>
        /// <param name="optionLevel"></param>
        /// <param name="optionName"></param>
        /// <param name="optionValue"></param>
        public void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, object optionValue)
        {
            try
            {
                if (SocketRS != null)
                {
                    SocketRS.SetSocketOption(optionLevel, optionName, optionValue);
                }
            }
            catch (Exception ex)
            {
                if (this.HasError != null)
                    this.HasError(ClassName, "SetSocketOption", ex);
                else
                    throw ex;
            }
        }

        /// <summary>
        /// IO控制
        /// </summary>
        /// <param name="optionLevel"></param>
        /// <param name="optionName"></param>
        /// <param name="optionValue"></param>
        public void IOControl(IOControlCode ioControlCode, byte[] optionInValue, byte[] optionOutValue)
        {
            try
            {
                if (SocketRS != null)
                {
                    SocketRS.IOControl(ioControlCode, optionInValue, optionOutValue);
                }
            }
            catch (Exception ex)
            {
                if (this.HasError != null)
                    this.HasError(ClassName, "IOControl", ex);
                else
                    throw ex;
            }
        }

        /// <summary>
        /// PING 
        /// </summary>
        /// <param name="PackSize">b包大小</param>
        /// <returns></returns>
        public long Ping(int PackSize = 0)
        {
            try
            {
                if (EndPoint == null)
                { return -1; }
                else
                {
                    int ConnectTOut = 1000,retry=1;
                    if (this.Refer_Prama != null)
                    {
                        if (this.Refer_Prama.ConnectTimeOut > 0)
                            ConnectTOut = this.Refer_Prama.ConnectTimeOut;
                    }
                    if (this.PingTimes > 0)
                        retry = PingTimes;
                    if (PackSize <= 0)
                    {
                        long sum = 0,n=0;
                        for (int counts = 0; counts < retry; counts++)
                        {
                            System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping();
                            System.Net.NetworkInformation.PingReply Res = ping.Send(EndPoint.Address.ToString(), ConnectTOut);
                            if (Res.Status == System.Net.NetworkInformation.IPStatus.Success)
                            {
                                sum += Res.RoundtripTime;
                                n++;
                            }
                        }
                        if (n > 0)
                            return sum / n;
                        else
                            return -1;
                    }
                    else
                    {
                        byte[] buf = new byte[(int)PackSize];
                        for (int i = 0; i < buf.Length; i++)
                        { buf[i] = 0; }
                        long sum = 0, n = 0;
                        for (int counts = 0; counts < retry; counts++)
                        {
                            System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping();
                            System.Net.NetworkInformation.PingReply Res = ping.Send(EndPoint.Address.ToString(), ConnectTOut, buf);
                            if (Res.Status == System.Net.NetworkInformation.IPStatus.Success)
                            {
                                sum += Res.RoundtripTime;
                                n++;
                            }
                        }
                        if (n > 0)
                            return sum / n;
                        else
                            return -1;
                    }
                }
            }
            catch (Exception ex)
            {
                if (this.HasError != null)
                    this.HasError(ClassName, "Ping", ex);
                else
                    throw ex;
                return -2;
            }
        }

        /// <summary>
        /// 传输性能
        /// </summary>
        /// <returns>包大小，花费时间ms</returns>
        public Dictionary<int, long> MaxPacket()
        {
            Dictionary<int, long> res = new Dictionary<int, long>();
            if (EndPoint == null)
                return res;
            try
            {
                int ConnectTOut = 1000, retry = 4;
                if (this.Refer_Prama != null)
                {
                    if (this.Refer_Prama.SendTimeout > 0)
                        ConnectTOut = this.Refer_Prama.SendTimeout;
                }
                if (this.PingTimes > 0)
                    retry = PingTimes;
                int[] Pack = new int[] { 4096, 2048, 1024, 512, 256, 128, 64, 32 };

                for (int i = 0; i < Pack.Length; i++)
                {
                    byte[] buf = new byte[Pack[i]];
                    long sum = 0, n = 0;
                    for (int counts = 0; counts < retry; counts++)
                    {
                        System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping();
                        System.Net.NetworkInformation.PingReply Res = ping.Send(EndPoint.Address.ToString(), ConnectTOut, buf);
                        if (Res.Status == System.Net.NetworkInformation.IPStatus.Success)
                        {
                            sum += Res.RoundtripTime;
                            n++;
                        }
                        if (n > 0)
                            res.Add(Pack[i], sum / n);
                    }
                }
            }
            catch (Exception ex)
            {
                if (this.HasError != null)
                    this.HasError(ClassName, "MaxPacket", ex);
                else
                    throw ex;
            }
            return res;
        }


        #endregion

        #region 异步方法

        /// <summary>
        /// 异步发送
        /// </summary>
        /// <param name="Data">发送数据</param>
        public void ASyncSendBytes(byte[] Data)
       {
           try
           {
               if (ASYWork == null && SocketRS.Connected)
               {
                   ASYWork = new Socket_ASyncSend();
                   ASYWork.SendEnA = true;
                   ASYWork.SendEnB = false; 
                   ASYWork.SocketRS = SocketRS;
                   ASYWork.Ref  = this.Refer_Prama ;  
                   Thread tw = new Thread(new ThreadStart(ASYWork.main));
                   tw.Start();
               }
               if (ASYWork.SendEnA)
                   ASYWork.SendLsA.Add(Data);
               else if (ASYWork.SendEnB)
                   ASYWork.SendLsB.Add(Data);
           }
           catch (Exception ex)
           {
                if (this.HasError != null)
                    this.HasError(ClassName, "ASyncSendBytes", ex);
                else
                    throw ex;
            }
       }

        /// <summary>
        /// 异步发送停止
        /// </summary>
       public void ASyncSendStop()
       {
           try
           {
               if (ASYWork != null)
               {
                   ASYWork.SendEnA = false;
                   ASYWork.SendEnB = false;
                   Thread.Sleep(1000);
                   ASYWork = null;
               }
           }
           catch (Exception ex)
           {
                if (this.HasError != null)
                    this.HasError(ClassName, "ASyncSendStop", ex);
                else
                    throw ex;
            }
       }

        /// <summary>
        /// 异步接收开启
        /// </summary>
       public void ASyncRecvStart()
       {

           try
           {
                int ConnPool = 1;
                if (this.Refer_Prama != null)
                {
                    if (this.Refer_Prama.ConnPool > 0)
                        ConnPool = this.Refer_Prama.ConnPool;
                }
                if (SocketRS != null && EndPoint != null && ASYRecv == null)
               {
                   if (!SocketRS.IsBound)
                   {
                       SocketRS.Bind(EndPoint);
                       SocketRS.Listen(ConnPool);
                       ASYRecv = new  Socket_ASyncRecv();
                       ASYRecv.SocketRS = SocketRS;
                       ASYRecv.Listen = true;
                       if (ASyncRecved != null)
                           ASYRecv.ASyncRecved = ASyncRecved;
                       Thread tw = new Thread(new ThreadStart(ASYRecv.ListenIng));
                       tw.Start();
                   }
               }
           }
           catch (Exception ex)
           {
                if (this.HasError != null)
                    this.HasError(ClassName, "MaxPacket", ex);
                else
                    throw ex;
            }
       }

        /// <summary>
       /// 异步接收停止
        /// </summary>
       public void ASyncRecvStop()
       {

           try
           {
               if (ASYRecv != null)
               {
                   ASYRecv.Listen = false; 
                   try
                   {
                        Socket socks = new Socket(SocketRS.AddressFamily , SocketRS.SocketType, SocketRS.ProtocolType);
                        socks.Connect(EndPoint);
                        socks.Close();
                    }
                   catch
                   {    } 
                   Thread.Sleep(1000);
                   ASYRecv = null;
               }
           }
           catch (Exception ex)
           {
                if (this.HasError != null)
                    this.HasError(ClassName, "ASyncRecvStop", ex);
                else
                    throw ex;
            }
       }

       #endregion

    }

    /// <summary>
    /// 异步处理发送类
    /// </summary>
    internal class Socket_ASyncSend
    {
        public Socket SocketRS { get; set; }
        public ReferSet Ref {get;set;}
        public string ErrMessage { get; private set; }
        public bool SendEnA { get; set; }
        public bool SendEnB { get; set; }
        public List<byte[]> SendLsA { get; set; }
        public List<byte[]> SendLsB { get; set; }
 
        public bool PingCheck { get; set; }
 
        public Socket_ASyncSend()
        {
            SendEnA = SendEnB = false;
            SendLsA = new List<byte[]>();
            SendLsB = new List<byte[]>();
            ErrMessage = ""; 
            PingCheck = false;
        }
        public void main()
        {
            if (SocketRS == null)
                return;
            if (SendLsA == null || SendLsB == null)
                return;
            if (!SocketRS.Connected)
                return;
            try
            {
                int ConnectTOut = 0,WaitTime=0;
                if (Ref != null)
                {
                    if (Ref.ConnectTimeOut > 0)
                        ConnectTOut = Ref.ConnectTimeOut;
                    if (Ref.WaitTime > 0)
                        WaitTime = Ref.WaitTime;
                }
                while (SendEnA || SendEnB)
                {
                    bool senden = true;
                    if (PingCheck)
                    {
                        System.Net.IPEndPoint ips = (System.Net.IPEndPoint)SocketRS.RemoteEndPoint;
                        System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping();
                        System.Net.NetworkInformation.PingReply Res = ping.Send(ips.Address.ToString(), ConnectTOut);
                        if (Res.Status == System.Net.NetworkInformation.IPStatus.Success)
                            senden = true;
                        else
                            senden = false;
                    }
                    if (!senden)
                        continue;

                    SendEnA = false;
                    SendEnB = true;
                    if (SendLsA.Count > 0)
                    {
                        foreach (byte[] Data in SendLsA)
                        {
                            SocketRS.Send(Data);
                            if (WaitTime > 0)
                                System.Threading.Thread.Sleep(WaitTime);
                        }
                    }
                    if (SendLsA.Count > 0)
                        SendLsA.Clear();
                    Thread.Sleep(100);

                    SendEnA = true;
                    SendEnB = false;
                    if (SendLsB.Count > 0)
                    {
                        foreach (byte[] Data in SendLsB)
                        {
                            SocketRS.Send(Data);
                            if (WaitTime > 0)
                                System.Threading.Thread.Sleep(WaitTime);
                        }
                    }
                    if (SendLsB.Count > 0)
                        SendLsB.Clear();
                    Thread.Sleep(100);

                    if (!SocketRS.Connected)
                    {
                        SendEnA = false;
                        SendEnB = false;
                        if (SendLsA.Count > 0)
                            SendLsA.Clear();
                        if (SendLsB.Count > 0)
                            SendLsB.Clear();
                    }
                }

            }
            catch (Exception ex)
            {
                ErrMessage = ex.Message;
                SendEnA = false;
                SendEnB = false;
                if (SendLsA.Count > 0)
                    SendLsA.Clear();
                if (SendLsB.Count > 0)
                    SendLsB.Clear();
            }
        }
    }

    /// <summary>
    /// 异常处理接收类
    /// </summary>
    internal class Socket_ASyncRecv
    {
        public Socket SocketRS { get; set; }
        public bool Listen { get; set; }
        public string Errmessage { get; private set; }
        public SocketSDK.Socket_ASyncRecvedEven ASyncRecved = null;

        private struct ListenDeal
        {
            public SocketSDK.Socket_ASyncRecvedEven ASyncRecved;
            public Socket sock;
            public void main()
            {
                if (sock == null)
                    return;
                if (ASyncRecved != null)
                    ASyncRecved(ref sock);

            }

        }

        public void ListenIng()
        {
            try
            {
                while (Listen)
                {
                    ListenDeal dl = new ListenDeal();
                    dl.ASyncRecved = ASyncRecved;
                    dl.sock = SocketRS.Accept();
                    Thread tw = new Thread(new ThreadStart(dl.main));
                    tw.Start();
                }
                SocketRS.Close();
                SocketRS.Dispose();
            }
            catch (Exception ex)
            { Errmessage = ex.Message; }
        }
    }

}
