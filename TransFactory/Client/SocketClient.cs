using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using NK.Entity;
using NK.ENum;
using NK.Event;
using NK.Interface;
using NK.Message;
using System.Reflection;
using NK.Class;

namespace NK.Communicate
{
    /// <summary>
    /// Socket客户端
    /// </summary>
    public class SocketClient : IDisposable, iCommunicate
    {
        #region 定义
        private bool m_disposed;
        private string ClassName = "";
        private string MethodName = "";
        private List<ClientSession> lth = null;
        private int CheckAliveTime = DLLConfig.ChkTime;
        #endregion

        #region 构造

        /// <summary>
        /// Socket客户端
        /// </summary>
        public SocketClient()
        {
            ClassName = this.GetType().ToString();
            this.language = Language.Chinese;
        }

        /// <summary>
        /// Socket客户端
        /// </summary>
        public SocketClient(string connection)
        {
            this.Connection = connection;
            ClassName = this.GetType().ToString();
            this.language = Language.Chinese;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        ~SocketClient()
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
         
        #region 事件
        /// <summary>
        ///调试信息事件
        /// </summary>
        public event CommEvent.LogEven log = null;
        /// <summary>
        /// 连接调试信息
        /// </summary>
        public event NetEvent.LogEven Connectlog = null;
        /// <summary>
        /// 错误出现事件，性能参数内DEBUG设置为EVENT有效
        /// </summary>
        public event CommEvent.HasErrorEven HasError = null;
        /// <summary>
        /// 连接事件
        /// </summary>
        public event NetEvent.Connect Connect = null;
        /// <summary>
        /// 连接断开
        /// </summary>
        public event NetEvent.DisConnect DisConnect = null;
        /// <summary>
        /// 请求注册事件
        /// </summary>
        public event NetEvent.RequestInitEven RequestInit = null;
        /// <summary>
        /// 返回注册事件
        /// </summary>
        public event NetEvent.ResponseInitEven ResponseInit = null;
        /// <summary>
        /// 请求数据事件
        /// </summary>
        public event NetEvent.RequestDataEven RequestData = null;
        /// <summary>
        /// 返回数据事件
        /// </summary>
        public event NetEvent.ResponsetDataEven ResponsetData = null;
        /// <summary>
        /// 接收到数据
        /// </summary>
        public event NetEvent.ReceiveDataEven ReceiveData;
        /// <summary>
        /// 请求心跳事件
        /// </summary>
        public event NetEvent.RequestHeartBeatEven RequestHeartBeat = null;
        /// <summary>
        /// 返回心跳事件
        /// </summary>
        public event NetEvent.ResponeHeartBeatEven ResponeHeartBeat = null;
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
        public ReferSet Refer { get; set; }
        /// <summary>
        /// 是否已连接
        /// </summary>
        public bool KeepAlive { get; set; }
        /// <summary>
        /// 是否运行
        /// </summary>
        public bool IsRuning { get { return lth == null ? false : (lth.Count > 0); } }
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
        /// SOCKET检测时间，分钟
        /// </summary>
        public int CheckTime { get; set; }
        /// <summary>
        /// 整点数据
        /// </summary>
        public bool IntegralPoint { get; set; }
        #endregion

        #region 方法

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
                if (lth==null)
                {
                    lth = new List<ClientSession>();
                    try
                    { 
                        ClientSession th = new ClientSession();
                        th.Session.Connection = Connection;
                        th.Session.LocalMode = this.IMode;
                        th.Refer = this.Refer;
                        th.KeepAlive = this.KeepAlive;
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
                        if (this.Connectlog != null)
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
                        th.Run = true;
                        lock (lth)
                        {
                            Thread tw = new Thread(new ThreadStart(th.Start));
                            tw.Start();
                            lth.Add(th);
                        }
                        if (this.log != null) this.log(ClassName, MethodName, Log_Type.Infomation, SystemMessage.ExecStart(this.language));

                    }
                    catch (Exception listex)
                    {
                        if (this.log != null) this.log(ClassName, MethodName, Log_Type.Error, listex.Message);
                        if (this.HasError != null) this.HasError(ClassName, MethodName, listex);
                    }
                    if (lth.Count <= 0)
                    {
                        lth = null;
                        throw new NullReferenceException("Connection is Null");
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
                    if (this.log != null) this.log(ClassName, MethodName, Log_Type.Infomation, SystemMessage.ExecStop(this.language)); 
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
        public byte[] IO(string flags,byte[] Data)
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
        public Dictionary<CommunicateSession,string> Online
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
