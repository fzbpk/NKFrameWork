using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Timers;
using NK.Entity;
using NK.ENum;
using NK.Event;
using NK.Interface;
using NK.Message;
using System.Collections;
using System.Reflection;
using NK.Class;

namespace NK.Communicate
{
    /// <summary>
    /// Socket服务
    /// </summary>
    public class SocketServer : IDisposable, iCommunicate
    {
        #region 定义
        private bool m_disposed;
        private string ClassName = "";
        private string MethodName = "";
        private bool Run = false;
        private System.Timers.Timer ChkTimer = null; 
        private bool ChkEn = false;
        private List<ServerSession> lth = null;
        private IPEndPoint ep = null;
        private int CheckAliveTime = DLLConfig.ChkTime;
        #endregion

        #region 构造

        /// <summary>
        /// Socket服务
        /// </summary>
        public SocketServer()
        {
            ClassName = this.GetType().ToString();
            this.language = Language.Chinese;
        }

        /// <summary>
        /// Socket服务
        /// </summary>
        public SocketServer(string connection)
        {
            this.Connection = connection;
            ClassName = this.GetType().ToString();
            this.language = Language.Chinese;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        ~SocketServer()
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
                    if (lth != null)
                        Stop();
                    m_disposed = true;
                }
            }
        }

        #endregion

        #region 属性
        /// <summary>
        /// 连接方式
        /// </summary>
        public ReferForUse IMode { get { return ReferForUse.NetSet; } }
        /// <summary>
        ///  网络参数及对应性能,JSON
        /// </summary>
        public string Connection { get; set; }
        /// <summary>
        /// 性能参数
        /// </summary>
        public ReferSet Refer { get; set; }
        /// <summary>
        /// 是否保持连接
        /// </summary>
        public bool KeepAlive { get; set; }
        /// <summary>
        /// 是否已连接
        /// </summary>
        public bool IsRuning { get { return lth == null ? false : lth.Count>0; } }
        /// <summary>
        /// 心跳时间
        /// </summary>
        public int HeartBeatTime { get; set; }
        /// <summary>
        /// 查询时间
        /// </summary>
        public int InquiryTime { get; set; }
        /// <summary>
        /// 显示语言
        /// </summary>
        public Language language { get; set; }
        /// <summary>
        /// 标识获取次数
        /// </summary>
        public int FlagCount { get; set; }
        /// <summary>
        /// 注册执行次数
        /// </summary>
        public int RegeditCount { get; set; }
        /// <summary>
        /// 心跳执行次数
        /// </summary>
        public int HeartBeatCount { get; set; }
        /// <summary>
        /// 获取数据执行次数
        /// </summary>
        public int DataCount { get; set; }
        /// <summary>
        /// 整点数据
        /// </summary>
        public bool IntegralPoint { get; set; }
        #endregion

        #region 基本事件

        /// <summary>
        /// 调试信息事件
        /// </summary>
        public event CommEvent.LogEven log;
        /// <summary>
        /// 连接调试信息
        /// </summary>
        public event NetEvent.LogEven Connectlog;
        /// <summary>
        /// 错误出现事件，性能参数内DEBUG设置为EVENT有效
        /// </summary>
        public event CommEvent.HasErrorEven HasError;
        /// <summary>
        /// 连接事件
        /// </summary>
        public event NetEvent.Connect Connect;
        /// <summary>
        /// 连接断开
        /// </summary>
        public event NetEvent.DisConnect DisConnect;
        /// <summary>
        /// 请求注册事件
        /// </summary>
        public event NetEvent.RequestInitEven RequestInit;
        /// <summary>
        /// 返回注册事件
        /// </summary>
        public event NetEvent.ResponseInitEven ResponseInit;
        /// <summary>
        /// 请求数据事件
        /// </summary>
        public event NetEvent.RequestDataEven RequestData;
        /// <summary>
        /// 返回数据事件
        /// </summary>
        public event NetEvent.ResponsetDataEven ResponsetData;
        /// <summary>
        /// 接收到数据
        /// </summary>
        public event NetEvent.ReceiveDataEven ReceiveData;
        /// <summary>
        /// 请求心跳事件
        /// </summary>
        public event NetEvent.RequestHeartBeatEven RequestHeartBeat;
        /// <summary>
        /// 返回心跳事件
        /// </summary>
        public event NetEvent.ResponeHeartBeatEven ResponeHeartBeat;
        /// <summary>
        /// 请求获取FLAGS
        /// </summary>
        public event NetEvent.RequestFlagsEven RequestFlags;
        /// <summary>
        /// 返回Flags
        /// </summary>
        public event NetEvent.ResponseFlagsEven ResponseFlags;
        /// <summary>
        /// 请求命令
        /// </summary>
        public event NetEvent.RequestCMDEven RequestCMD;
        /// <summary>
        /// 返回命令
        /// </summary>
        public event NetEvent.ResponseCMDEven ResponseCMD;

        #endregion

        #region 基本方法

        private void Listen()
        {
            MethodName = "";
            try
            {
                MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            int ConnPool = DLLConfig.ConnPool;
            int ReTry = DLLConfig.ReTry;
            CheckAliveTime = DLLConfig.ChkTime;
            Socket SocketRS = null;
            try
            {
                ReferSet  Prm = this.Refer;
                NetSet net = this.Connection. Deserialize<NetSet>();
                if (Refer != null)
                {
                    if (Refer.CheckAliveTime > 0)
                        CheckAliveTime = Refer.CheckAliveTime;
                }
                IPAddress addr = IPAddress.Any;
                if (string.IsNullOrEmpty(net.IPAddress))
                    addr = IPAddress.Any;
                else if(! IPAddress.TryParse(net.IPAddress, out addr))
                    addr = IPAddress.Any;
                ep = new IPEndPoint(addr, net.Port); 
                #region 初始化
                if (log != null) log(ClassName, MethodName, Log_Type.Infomation, SystemMessage.Init("Socket", language));
                SocketRS = new Socket(net.Address_Family, net.Socket_Type, net.Protocol_Type);
                #endregion
                #region 设置
                if (log != null) log(ClassName, MethodName, Log_Type.Infomation, SystemMessage.SET("Socket", language));
                if (Refer != null)
                {
                    SocketRS.ReceiveBufferSize = Refer.ReceiveBufferSize;
                    SocketRS.ReceiveTimeout = Refer.ReceiveTimeout;
                    SocketRS.SendBufferSize = Refer.SendBufferSize;
                    SocketRS.SendTimeout = Refer.SendTimeout;
                    ConnPool = Refer.ConnPool;
                    ReTry = Refer.ReTry;
                }
                else
                {
                    SocketRS.ReceiveBufferSize = DLLConfig.ReceiveBufferSize;
                    SocketRS.ReceiveTimeout = DLLConfig.ReceiveTimeout;
                    SocketRS.SendBufferSize = DLLConfig.SendBufferSize;
                    SocketRS.SendTimeout = DLLConfig.SendTimeout;
                }
                #endregion
                #region 服务开启
                while (Run)
                {
                    if (SocketRS.IsBound)
                    {
                        #region 重连接
                        try
                        {
                            int retry = ReTry;
                            if (ReTry > 0)
                            {
                                if (retry < 0)
                                {
                                    if (log != null) log(ClassName, MethodName, Log_Type.Infomation, SystemMessage.Init("Socket", language));
                                    SocketRS.Close();
                                    SocketRS = new Socket(net.Address_Family, net.Socket_Type, net.Protocol_Type);
                                    if (log != null) log(ClassName, MethodName, Log_Type.Infomation, SystemMessage.SET("Socket", language));
                                    if (Prm != null)
                                    {
                                        SocketRS.ReceiveBufferSize = Prm.ReceiveBufferSize;
                                        SocketRS.ReceiveTimeout = Prm.ReceiveTimeout;
                                        SocketRS.SendBufferSize = Prm.SendBufferSize;
                                        SocketRS.SendTimeout = Prm.SendTimeout;
                                    }
                                    else
                                    {
                                        SocketRS.ReceiveBufferSize = DLLConfig.ReceiveBufferSize;
                                        SocketRS.ReceiveTimeout = DLLConfig.ReceiveTimeout;
                                        SocketRS.SendBufferSize = DLLConfig.SendBufferSize;
                                        SocketRS.SendTimeout = DLLConfig.SendTimeout;
                                    }
                                    SocketRS.Bind(ep);
                                    SocketRS.Listen(ConnPool);
                                    retry = ReTry;
                                }
                                else
                                    retry--;
                            }
                        }
                        catch (Exception ex)
                        { if (log != null) log(ClassName, MethodName, Log_Type.Error, ex.Message); }
                        #endregion
                        #region 连接
                        try
                        {
                            ServerSession th = new ServerSession();
                            th.Session.Connection = Connection;
                            th.Session.LocalMode = IMode;
                            th.Refer = Refer;
                            th.session = new Dictionary<string, object>();
                            th.HBTime = this.HeartBeatTime;
                            th.RTime = this.InquiryTime;
                            th.RegCount = this.RegeditCount;
                            th.HBCount = this.HeartBeatCount;
                            th.DataCount = this.DataCount;
                            th.FlagCount = this.FlagCount;
                            th.ChkTime = CheckAliveTime;
                            th.IntegerPoint = this.IntegralPoint;
                            if (this.HasError != null)
                                th.error += this.HasError;
                            if (this.log != null)
                                th.log += this.Connectlog;
                            if (this.Connect != null)
                                th.connect += this.Connect;
                            if (this.DisConnect != null)
                                th.disconnect += this.DisConnect;
                            if (this.RequestInit != null)
                                th.RequestInit += this.RequestInit;
                            if (this.ResponseInit != null)
                                th.ResponseInit += this.ResponseInit;
                            if (this.RequestHeartBeat != null)
                                th.RequestHeartBeat += this.RequestHeartBeat;
                            if (this.ResponeHeartBeat != null)
                                th.ResponeHeartBeat += this.ResponeHeartBeat;
                            if (this.RequestData != null)
                                th.RequestData += this.RequestData;
                            if (this.ResponsetData != null)
                                th.ResponsetData += this.ResponsetData;
                            if (this.ReceiveData != null)
                                th.ReceiveData += this.ReceiveData;
                            if (this.RequestFlags != null)
                                th.RequestFlags += this.RequestFlags;
                            if (this.ResponseFlags != null)
                                th.ResponseFlags += this.ResponseFlags;
                            if (this.RequestCMD != null)
                                th.RequestCMD += this.RequestCMD;
                            if (this.ResponseCMD != null)
                                th.ResponseCMD += this.ResponseCMD;
                            th.NetRS = SocketRS.Accept();
                            th.EndPoint = ep;
                            if (Run)
                            {
                                lock (lth)
                                {
                                    Thread tw = new Thread(new ThreadStart(th.Start));
                                    tw.Start();
                                    lth.Add(th);
                                } 
                            }
                        }
                        catch
                        { }
                        #endregion
                    }
                    else
                    {
                        SocketRS.Bind(ep);
                        SocketRS.Listen(ConnPool);
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, MethodName, Log_Type.Error, ex.Message);
                if (HasError != null) HasError(ClassName, MethodName, ex);
            }
            finally
            {
                #region 关闭连接
                if (SocketRS != null)
                {
                    if (SocketRS.IsBound)
                    {
                        if (DisConnect != null) DisConnect(SocketRS.LocalEndPoint.ToString(), Connection, ReferForUse.NetSet, SocketRS.Handle.ToInt64());
                        try
                        { SocketRS.Shutdown(SocketShutdown.Both); }
                        catch { }
                        try
                        { SocketRS.Close(); }
                        catch { }
                    }
                    SocketRS = null;
                }
                #endregion
            }
        }

        /// <summary>
        /// 连接
        /// </summary>
        public void Start()
        {
            MethodName = "";
            try
            {
                MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            if (this.Connection == null)
            {
                if (this.log != null) this.log(ClassName, MethodName, Log_Type.Error, SystemMessage.RefNullOrEmpty("Connection", language));
                if (this.HasError != null)
                    this.HasError(ClassName, MethodName, new NullReferenceException(SystemMessage.RefNullOrEmpty("Connection", language)));
                else
                    throw new NullReferenceException(SystemMessage.RefNullOrEmpty("Connection", language));
                return;
            }
            try
            {
                if (Refer != null)
                {
                    if (Refer.CheckAliveTime > 0)
                        CheckAliveTime = Refer.CheckAliveTime;
                }
                if (lth == null)
                {
                    lth = new List<ServerSession>();
                    try
                    {  
                        Run = true;
                        Thread tw = new Thread(new ThreadStart(Listen));
                        tw.Start();
                        ChkEn = true;
                        ChkTimer = new System.Timers.Timer();
                        ChkTimer.Interval = CheckAliveTime * 60 * 1000;
                        ChkTimer.Elapsed += ChkTimer_Elapsed;
                        ChkTimer.Start(); 
                        if (this.log != null) this.log(ClassName, MethodName, Log_Type.Infomation, SystemMessage.ExecStart(language));
                    }
                    catch (Exception ex)
                    {
                        if (this.log != null) this.log(ClassName, MethodName, Log_Type.Error, ex.Message);
                        if (this.HasError != null) this.HasError(ClassName, MethodName, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                if (this.log != null) this.log(ClassName, MethodName, Log_Type.Error, ex.Message);
                if (this.HasError != null)
                    this.HasError(ClassName, MethodName, ex);
                else
                    throw ex;
                return;
            } 
        }

        private void ChkTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            MethodName = "";
            try
            {
                MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            if (ChkEn)
            {
                ChkEn = false;
                if (lth != null)
                {
                    int TimeO = 0;
                    if (this.Refer != null)
                        TimeO = this.Refer.ExecTime;
                    if (TimeO <= 0) TimeO = CheckAliveTime * 60;
                    try
                    {
                        if ( Monitor.TryEnter(lth, TimeO * 1000))
                        {
                            lth.Where(c => !c.Run || string.IsNullOrEmpty(c.flags)).ToList().ForEach(m => {
                                try
                                {
                                    string flags = m.flags;
                                    var i = lth.IndexOf(m);
                                    lth[i].Stop();
                                    lth[i].Dispose();
                                    if (this.HasError != null)
                                        lth[i].error -= this.HasError;
                                    if (this.log != null)
                                        lth[i].log -= this.Connectlog;
                                    if (this.Connect != null)
                                        lth[i].connect -= this.Connect;
                                    if (this.DisConnect != null)
                                        lth[i].disconnect -= this.DisConnect;
                                    if (this.RequestInit != null)
                                        lth[i].RequestInit -= this.RequestInit;
                                    if (this.ResponseInit != null)
                                        lth[i].ResponseInit -= this.ResponseInit;
                                    if (this.RequestHeartBeat != null)
                                        lth[i].RequestHeartBeat -= this.RequestHeartBeat;
                                    if (this.ResponeHeartBeat != null)
                                        lth[i].ResponeHeartBeat -= this.ResponeHeartBeat;
                                    if (this.RequestData != null)
                                        lth[i].RequestData -= this.RequestData;
                                    if (this.ResponsetData != null)
                                        lth[i].ResponsetData -= this.ResponsetData;
                                    if (this.ReceiveData != null)
                                        lth[i].ReceiveData -= this.ReceiveData;
                                    if (this.RequestFlags != null)
                                        lth[i].RequestFlags -= this.RequestFlags;
                                    if (this.ResponseFlags != null)
                                        lth[i].ResponseFlags -= this.ResponseFlags;
                                    if (this.RequestCMD != null)
                                        lth[i].RequestCMD -= this.RequestCMD;
                                    if (this.ResponseCMD != null)
                                        lth[i].ResponseCMD -= this.ResponseCMD;
                                    lth[i] = null;
                                    lth.RemoveAt(i);
                                    if (this.log != null) this.log(ClassName, "ClearConnection", Log_Type.Infomation, flags);

                                }
                                catch (Exception ex)
                                {
                                    if (this.log != null) this.log(ClassName, "ClearConnection", Log_Type.Error, ex.Message);
                                    if (this.HasError != null)
                                        this.HasError(ClassName, "ClearConnection", ex);
                                }

                            });
                        } 
                    }
                    catch (Exception exx)
                    {
                        if (this.log != null) this.log(ClassName, "ClearConnection", Log_Type.Error, exx.Message);
                        if (this.HasError != null)
                            this.HasError(ClassName, "ClearConnection", exx);
                    } 
                    GC.SuppressFinalize(this);
                }
                ChkEn = true;
            }
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public void Stop()
        {
            MethodName = "";
            try
            {
                MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            try
            {
                if (lth != null)
                {
                    ChkEn = false;
                    Run = false;
                    NetSet net = this.Connection.Deserialize<NetSet>();
                    lock (lth)
                    {
                        foreach (var th in lth)
                        {
                            try
                            {
                                th.Stop();
                                th.Dispose();
                                if (this.HasError != null)
                                    th.error -= this.HasError;
                                if (this.log != null)
                                    th.log -= this.Connectlog;
                                if (this.Connect != null)
                                    th.connect -= this.Connect;
                                if (this.DisConnect != null)
                                    th.disconnect -= this.DisConnect;
                                if (this.RequestInit != null)
                                    th.RequestInit -= this.RequestInit;
                                if (this.ResponseInit != null)
                                    th.ResponseInit -= this.ResponseInit;
                                if (this.RequestHeartBeat != null)
                                    th.RequestHeartBeat -= this.RequestHeartBeat;
                                if (this.ResponeHeartBeat != null)
                                    th.ResponeHeartBeat -= this.ResponeHeartBeat;
                                if (this.RequestData != null)
                                    th.RequestData -= this.RequestData;
                                if (this.ResponsetData != null)
                                    th.ResponsetData -= this.ResponsetData;
                                if (this.ReceiveData != null)
                                    th.ReceiveData -= this.ReceiveData;
                                if (this.RequestFlags != null)
                                    th.RequestFlags -= this.RequestFlags;
                                if (this.ResponseFlags != null)
                                    th.ResponseFlags -= this.ResponseFlags;
                                if (this.RequestCMD != null)
                                    th.RequestCMD -= this.RequestCMD;
                                if (this.ResponseCMD != null)
                                    th.ResponseCMD -= this.ResponseCMD;
                            }
                            catch { }
                        }
                        lth.Clear();
                        lth = null;
                    }
                    try
                    {
                        if (ep != null)
                        {
                            Socket sock = new Socket(net.Address_Family, net.Socket_Type, net.Protocol_Type);
                            sock.Connect("127.0.0.1", ep.Port);
                            if (sock.Connected)
                            {
                                sock.Send(new byte[] { 0x0d, 0x0a });
                                sock.Close();
                            }
                            sock.Dispose();
                            sock = null;
                        } 
                    }
                    catch { } 
                    if (this.log != null) this.log(ClassName, MethodName, Log_Type.Infomation,SystemMessage.ExecStop(language));
                    ChkTimer.Stop();
                    ChkTimer = null;
                }
            }
            catch (Exception ex)
            {
                if (this.log != null) this.log(ClassName, MethodName, Log_Type.Error, ex.Message);
                if (this.HasError != null)
                    this.HasError(ClassName, MethodName, ex);
                else
                    throw ex;
            }
        }

        /// <summary>
        /// 数据写入读取
        /// </summary>
        ///  <param name="flags">标识</param>
        /// <param name="Data">数据</param>
        /// <returns></returns>
        public byte[] IO(string flags, byte[] Data)
        {
            MethodName = "";
            try
            {
                MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            List<byte> pack = new List<byte>();
            if (string.IsNullOrEmpty(flags))
            {
                if (this.log != null) this.log(ClassName, MethodName, Log_Type.Error, SystemMessage.RefNullOrEmpty("flags", language));
                if (this.HasError != null)
                    this.HasError(ClassName, MethodName, new NullReferenceException(SystemMessage.RefNullOrEmpty("flags", language)));
                else
                    throw new NullReferenceException(SystemMessage.RefNullOrEmpty("flags", language));
                return pack.ToArray();
            }
            if (lth == null)
            {
                if (this.log != null) this.log(ClassName, MethodName, Log_Type.Error, SystemMessage.Badsequencecommands(language));
                if (this.HasError != null)
                    this.HasError(ClassName, MethodName, new InvalidOperationException(SystemMessage.Badsequencecommands(language)));
                else
                    throw new InvalidOperationException(SystemMessage.Badsequencecommands(language));
                return pack.ToArray();
            }
            try
            {
                int TimeO = 0;
                if (this.Refer != null)
                    TimeO = this.Refer.ExecTime;
                if (TimeO > 0 && Monitor.TryEnter(lth, TimeO * 1000))
                {
                    lth.Where(c => c.flags == flags && c.Run).ToList().ForEach(m =>
                    {
                        if (m.IsConnected())
                        {
                            byte[] recv = null;
                            try { recv = m.IO(Data); }
                            catch { }
                            if (recv != null) pack.AddRange(recv);
                        }
                    });
                }
                else if (TimeO <= 0)
                {
                    lock (lth)
                    {
                        lth.Where(c => c.flags == flags && c.Run).ToList().ForEach(m =>
                        {
                            if (m.IsConnected())
                            {
                                byte[] recv = null;
                                try { recv = m.IO(Data); }
                                catch { }
                                if (recv != null) pack.AddRange(recv);
                            }
                        });
                    }
                }
                else
                    throw new TimeoutException();
            }
            catch (Exception ex)
            {
                if (this.log != null) this.log(ClassName, MethodName, Log_Type.Error, ex.Message);
                if (this.HasError != null)
                    this.HasError(ClassName, MethodName, ex);
                else
                    throw ex;
            }
            return pack.ToArray();
        }

        /// <summary>
        /// 清除SESSION
        /// </summary>
        /// <param name="flags">标识</param>
        public void ClearSession(string flags)
        {
            MethodName = "";
            try
            {
                MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            if (string.IsNullOrEmpty(flags))
            {
                if (this.log != null) this.log(ClassName, MethodName, Log_Type.Error, SystemMessage.RefNullOrEmpty("flags", language));
                if (this.HasError != null)
                    this.HasError(ClassName, MethodName, new NullReferenceException(SystemMessage.RefNullOrEmpty("flags", language)));
                else
                    throw new NullReferenceException(SystemMessage.RefNullOrEmpty("flags", language));
            }
            if (lth == null)
            {
                if (this.log != null) this.log(ClassName, MethodName, Log_Type.Error, SystemMessage.Badsequencecommands(language));
                if (this.HasError != null)
                    this.HasError(ClassName, MethodName, new InvalidOperationException(SystemMessage.Badsequencecommands(language)));
                else
                    throw new InvalidOperationException(SystemMessage.Badsequencecommands(language));
            }
            try
            {
                int TimeO = 0;
                if (this.Refer != null)
                    TimeO = this.Refer.ExecTime;
                if (TimeO > 0 && Monitor.TryEnter(lth, TimeO * 1000))
                {
                    lth.Where(c => c.flags == flags && c.Run).ToList().ForEach(m => {
                        var index = lth.IndexOf(m);
                        lth[index].session.Clear();
                    });
                }
                else if (TimeO <= 0)
                {
                    lock (lth)
                    {
                        lth.Where(c => c.flags == flags && c.Run).ToList().ForEach(m => {
                            var index = lth.IndexOf(m);
                            lth[index].session.Clear();
                        });
                    }
                }
                else
                    throw new TimeoutException();
            }
            catch (Exception ex)
            {
                if (this.log != null) this.log(ClassName, MethodName, Log_Type.Error, ex.Message);
                if (this.HasError != null)
                    this.HasError(ClassName, MethodName, ex);
                else
                    throw ex;
            }
        }
        /// <summary>
        /// 获取SESSION
        /// </summary>
        /// <param name="flags">标识</param>
        /// <returns></returns>
        public Dictionary<string, object> GetSession(string flags)
        {
            MethodName = "";
            try
            {
                MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            Dictionary<string, object> res = new Dictionary<string, object>();
            if (string.IsNullOrEmpty(flags))
            {
                if (this.log != null) this.log(ClassName, MethodName, Log_Type.Error, SystemMessage.RefNullOrEmpty("flags", language));
                if (this.HasError != null)
                    this.HasError(ClassName, MethodName, new NullReferenceException(SystemMessage.RefNullOrEmpty("flags", language)));
                else
                    throw new NullReferenceException(SystemMessage.RefNullOrEmpty("flags", language));
                return res;
            }
            if (lth == null)
            {
                if (this.log != null) this.log(ClassName, MethodName, Log_Type.Error, SystemMessage.Badsequencecommands(language));
                if (this.HasError != null)
                    this.HasError(ClassName, MethodName, new InvalidOperationException(SystemMessage.Badsequencecommands(language)));
                else
                    throw new InvalidOperationException(SystemMessage.Badsequencecommands(language));
                return res;
            }
            try
            {
                int TimeO = 0;
                if (this.Refer != null)
                    TimeO = this.Refer.ExecTime;
                if (TimeO > 0 && Monitor.TryEnter(lth, TimeO * 1000))
                {
                    var th = lth.FirstOrDefault(c => c.flags == flags && c.Run);
                    if (th != null) res = th.session;
                }
                else if (TimeO <= 0)
                {
                    lock (lth)
                    {
                        var th = lth.FirstOrDefault(c => c.flags == flags && c.Run);
                        if (th != null) res = th.session;
                    }
                }
                else
                    throw new TimeoutException();
            }
            catch (Exception ex)
            {
                if (this.log != null) this.log(ClassName, MethodName, Log_Type.Error, ex.Message);
                if (this.HasError != null)
                    this.HasError(ClassName, MethodName, ex);
                else
                    throw ex;
            }
            return res;
        }
        /// <summary>
        /// 设置SESSION
        /// </summary>
        /// <param name="flags">标识</param>
        /// <param name="session">Session</param>
        /// <returns></returns>
        public void SetSession(string flags, Dictionary<string, object> session)
        {
            MethodName = "";
            try
            {
                MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            if (string.IsNullOrEmpty(flags))
            {
                if (this.log != null) this.log(ClassName, MethodName, Log_Type.Error, SystemMessage.RefNullOrEmpty("flags", language));
                if (this.HasError != null)
                    this.HasError(ClassName, MethodName, new NullReferenceException(SystemMessage.RefNullOrEmpty("flags", language)));
                else
                    throw new NullReferenceException(SystemMessage.RefNullOrEmpty("flags", language));
            }
            if (lth == null)
            {
                if (this.log != null) this.log(ClassName, MethodName, Log_Type.Error, SystemMessage.Badsequencecommands(language));
                if (this.HasError != null)
                    this.HasError(ClassName, MethodName, new InvalidOperationException(SystemMessage.Badsequencecommands(language)));
                else
                    throw new InvalidOperationException(SystemMessage.Badsequencecommands(language));
            }
            try
            {
                int TimeO = 0;
                if (this.Refer != null)
                    TimeO = this.Refer.ExecTime;
                if (TimeO > 0 && Monitor.TryEnter(lth, TimeO * 1000))
                {
                    lth.Where(c => c.flags == flags && c.Run).ToList().ForEach(m => {
                        var index = lth.IndexOf(m);
                        lth[index].session = session;
                    });
                }
                else if (TimeO <= 0)
                {
                    lock (lth)
                    {
                        lth.Where(c => c.flags == flags && c.Run).ToList().ForEach(m => {
                            var index = lth.IndexOf(m);
                            lth[index].session = session;
                        });
                    }
                }
                else
                    throw new TimeoutException();
            }
            catch (Exception ex)
            {
                if (this.log != null) this.log(ClassName, MethodName, Log_Type.Error, ex.Message);
                if (this.HasError != null)
                    this.HasError(ClassName, MethodName, ex);
                else
                    throw ex;
            }
        }

        /// <summary>
        /// 连接在线状态
        /// </summary>
        public Dictionary<CommunicateSession, string> Online
        {
            get
            {
                MethodName = "";
                try
                {
                    MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                    MethodName = method.Name;
                }
                catch { }
                Dictionary<CommunicateSession, string> res = new Dictionary<CommunicateSession, string>();
                try
                {
                    int TimeO = 0;
                    if (this.Refer != null)
                        TimeO = this.Refer.ExecTime;
                    if (lth != null)
                    {
                        if (TimeO > 0 && Monitor.TryEnter(lth, TimeO * 1000))
                        {
                            lth.Where(c => !string.IsNullOrEmpty(c.flags) && c.Run).ToArray().ToList().ForEach(m =>
                            {
                                string conn = m.flags;
                                bool online = m.IsConnected();
                                CommunicateSession Sessions = m.Session;
                                if (online && Sessions != null)
                                    res.Add(Sessions, conn);
                            });
                        }
                        else if (TimeO <= 0)
                        {
                            lock (lth)
                            {
                                lth.Where(c => !string.IsNullOrEmpty(c.flags) && c.Run).ToArray().ToList().ForEach(m =>
                                {
                                    string conn = m.flags;
                                    bool online = m.IsConnected();
                                    CommunicateSession Sessions = m.Session;
                                    if (online && Sessions != null)
                                        res.Add(Sessions, conn);
                                });
                            }
                        }
                        else
                            throw new TimeoutException();
                    }
                }
                catch (Exception ex)
                {
                    if (this.log != null) this.log(ClassName, MethodName, Log_Type.Error, ex.Message);
                }
                return res;
            }
        }

        #endregion

    }



}
