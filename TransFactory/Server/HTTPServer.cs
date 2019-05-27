using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System;
using System.IO;
using System.Collections;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Security.Cryptography;
using System.Runtime.Serialization.Json;
using NK.Entity;
using NK.ENum;
using NK.Event;
using NK.Message;
using NK.Class;
using System.Reflection;

namespace NK.Communicate
{
    /// <summary>
    /// HTTP服务
    /// </summary>
    public partial class HTTPServer : IDisposable
    {
        
        #region 定义 
        private HttpListener listener = null;
        private Thread m_listeningThread = null;
        private Hashtable m_sessionList = null; 
        private bool m_disposed = false; 
        private string ClassName = "";
        private string MethodName="";
        private bool Run = false;
        private int m_sessID = 0;
        /// <summary>
        /// 认证策略
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public  delegate AuthenticationSchemes AuthenticationSchemeForClient(HttpListenerRequest request);

        #endregion

        #region 构造函数

        /// <summary>
        /// HTTP服务
        /// </summary>
        public HTTPServer(string connection="")
        {
            this.Connection = connection;
            listener = new HttpListener();
            ClassName = this.GetType().ToString();
            this.language = Language.Chinese;
        }

        /// <summary>
        /// HTTP服务
        /// </summary>
        public HTTPServer(string IP, int Port, string UrlParameter)
        {
            listener = new HttpListener();
            NetSet net = new NetSet();
            net.Address_Family = System.Net.Sockets.AddressFamily.InterNetwork;
            net.IPAddress = IP;
            net.Port = Port;
            net.AddrRef = (string.IsNullOrEmpty(UrlParameter) ? "" : UrlParameter);
            net.Mode = Net_Mode.Remote;
            net.Protocol_Type = System.Net.Sockets.ProtocolType.Tcp;
            net.Socket_Type = System.Net.Sockets.SocketType.Stream;
            this.Connection = net.Serialize();
            ClassName = this.GetType().ToString();
            this.language = Language.Chinese;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        ~HTTPServer()
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
                    if (listener != null)
                    {
                        try
                        {
                            listener.Stop();
                        }
                        catch { }
                        listener = null;
                    }
                    m_disposed = true;
                }
            }
        }

        #endregion

        #region 事件
        /// <summary>
        /// 错误事件
        /// </summary>
        public event CommEvent.LogEven log = null;
        /// <summary>
        /// 错误事件
        /// </summary>
        public event CommEvent.HasErrorEven HasError = null;
        /// <summary>
        /// 500事件
        /// </summary>
        public event NetEvent.RequestErrorEven ErrorEvent = null; 
        /// <summary>
        /// 错误事件
        /// </summary>
        public event NetEvent.RequestGETEven getEvent = null;
        /// <summary>
        /// 错误事件
        /// </summary>
        public event NetEvent.RequestPOSTEven postEvent = null;
        /// <summary>
        /// 错误事件
        /// </summary>
        public event NetEvent.RequestPUTEven putEvent = null;
        /// <summary>
        /// 错误事件
        /// </summary>
        public event NetEvent.RequestDELETEEven delEvent = null;
        /// <summary>
        /// 数据库连接成功事件
        /// </summary>
        public event NetEvent.Connect Connect = null;
        /// <summary>
        /// 数据库关闭事件
        /// </summary>
        public event NetEvent.DisConnect DisConnect = null;
        /// <summary>
        /// 获取或设置一个委托，调用它来确定用于客户端身份验证的协议
        /// </summary>
        public event AuthenticationSchemeForClient AuthenticationScheme = null;
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
        public bool IsConnected { get { return listener != null ? listener.IsListening : false; } }
        /// <summary>
        /// 显示语言
        /// </summary>
        public Language language { get; set; }
        /// <summary>
        /// 是否支持OPTION
        /// </summary>
        public bool OPTION { get; set; }
        /// <summary>
        /// 跨域
        /// </summary>
        public bool Access_Control_Allow { get; set; }
        /// <summary>
        /// 头信息
        /// </summary>
        public Dictionary<string, string> Header { get; set; }
        #endregion

        #region 方法

        private void Listen()
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
                bool debug = false;
                int bufsize = DLLConfig.ReceiveBufferSize;
                if (this.Refer_Prama != null)
                {
                    if(this.Refer_Prama.Debug!=  Log_Type.None)
                        debug = true;
                    if (this.Refer_Prama.ReceiveBufferSize >0)
                        bufsize = this.Refer_Prama.ReceiveBufferSize;
                }
                Run = true;
                while (Run)
                {
                    HttpListenerContext context = listener.GetContext();
                    HttpListenerRequest request = context.Request;
                    if (this.log != null)  this.log(ClassName, MethodName, Log_Type.Infomation , SystemMessage.Connect(this.Connection,language)); 
                    try
                    {
                        HttpServerSession re = new HttpServerSession();
                        re.Connection = this.Connection;
                        re.context = context;
                        re.Debug = debug;
                        re.Option=this.OPTION;
                        re.Access_Control_Allow = this.Access_Control_Allow;
                        re.Header = this.Header;
                        re.Buffsize = bufsize;
                        if (this.getEvent != null)
                            re.getEvent += this.getEvent;
                        if (this.postEvent != null)
                            re.postEvent += this.postEvent;
                        if (this.putEvent != null)
                            re.putEvent += this.putEvent;
                        if (this.delEvent != null)
                            re.delEvent += this.delEvent;
                        if (this.ErrorEvent != null)
                            re.error += this.ErrorEvent; 
                        Thread clientThread = new Thread(new ThreadStart(re.Start));
                        clientThread.Start();
                        m_sessID++;
                    }
                    catch (Exception exx)
                    {
                        if (this.HasError != null)
                            this.HasError(ClassName, MethodName, exx);
                    }
                }
            }
            catch (Exception ex)
            {
                if (this.HasError != null)
                    this.HasError(ClassName, MethodName, ex);
                else
                    throw ex;
                Run = false;
            } 
        }

        /// <summary>
        /// 启动
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
            if (!HttpListener.IsSupported)
            {
                if (this.log != null) this.log(ClassName, MethodName, Log_Type.Error, SystemMessage.NotSupported("HttpListener", language));
                if (this.HasError != null)
                    this.HasError(ClassName, MethodName, new InvalidOperationException(SystemMessage.NotSupported("HttpListener", language)));
                else
                    throw new  InvalidOperationException(SystemMessage.NotSupported("HttpListener", language));
                return; 
            }
            if (string.IsNullOrEmpty(this.Connection))
            {
                if (this.log != null) this.log(ClassName, MethodName, Log_Type.Error, SystemMessage.RefNullOrEmpty("Connection", language));
                if (this.HasError != null)
                    this.HasError(ClassName, MethodName, new NullReferenceException(SystemMessage.RefNullOrEmpty("Connection", language)));
                else
                    throw new NullReferenceException(SystemMessage.RefNullOrEmpty("Connection", language));
                return;
            }
            NetSet http = new NetSet();
            try
            { http = this.Connection. Deserialize<NetSet>(); }
            catch
            {
                if (this.log != null) this.log(ClassName, MethodName, Log_Type.Error, SystemMessage.CastError(Connection, language));
                if (this.HasError != null)
                    this.HasError(ClassName, MethodName, new NullReferenceException(SystemMessage.CastError(Connection, language)));
                else
                    throw new NullReferenceException(SystemMessage.CastError(Connection, language));
                return;
            }
            try
            { 
                if (!Run)
                {
                    m_sessionList = new Hashtable();
                    string ip = http.IPAddress;
                    if (string.IsNullOrEmpty(ip))
                        ip = "localhost";
                    string Url = String.Format("http://{0}:{1}", ip, http.Port > 0 ? http.Port.ToString() : "80");
                    if (!string.IsNullOrEmpty(http.AddrRef))
                        Url = Url + "/" + http.AddrRef;
                    Url = Url + "/";
                    if (this.log != null) this.log(ClassName, MethodName, Log_Type.Test, Url);
                    listener.Prefixes.Add(Url);
                    if(AuthenticationScheme!=null)
                        listener.AuthenticationSchemeSelectorDelegate =  new AuthenticationSchemeSelector(AuthenticationScheme);
                    else 
                        listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
                    listener.Start(); 
                    this.m_listeningThread = new Thread(new ThreadStart(Listen));
                    this.m_listeningThread.Name = string.IsNullOrEmpty(http.ConfigName) ? "HttpServer" : http.ConfigName;
                    this.m_listeningThread.Start();
                    this.Run = true;
                    if (this.log != null) this.log(ClassName, MethodName , Log_Type.Test,SystemMessage.ExecStart(language)); 
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
        ///停止
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
                if (this.Run)
                {
                    this.Run = false;
                    this.m_listeningThread.Abort();
                    listener.Stop();
                    listener = null;
                    if (this.log != null) this.log(ClassName, MethodName, Log_Type.Test, SystemMessage.ExecStop(language)); 
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
        /// 获取运行状态
        /// </summary>
        /// <returns>运行状态</returns>
        public bool State()
        { return Run; }


        #endregion

    }
}
