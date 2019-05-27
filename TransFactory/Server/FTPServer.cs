using System.Collections.Generic;
using System.Text;
using System;
using System.IO;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading; 
using System.Security.Cryptography;
using System.Runtime.Serialization.Json;
using NK.Entity;
using NK.ENum;
using NK.Event;
using NK.Message;
namespace NK.Communicate
{
    /// <summary>
    /// FTP服务
    /// </summary>
    public partial  class FTPServer : IDisposable
    {

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
        /// 数据库连接成功事件
        /// </summary>
        public event NetEvent.Connect Connect = null;
        /// <summary>
        /// 数据库关闭事件
        /// </summary>
        public event NetEvent.DisConnect DisConnect = null;
        /// <summary>
        /// 
        /// </summary>
        public NetEvent.IsValidDirEven IsValidDir = null;
        /// <summary>
        /// 
        /// </summary>
        public NetEvent.FTPAuthEven FTPAuth = null;
        /// <summary>
        /// 
        /// </summary>
        public NetEvent.RequestMKDEven RequestMKD = null;
        /// <summary>
        /// 
        /// </summary>
        public NetEvent.RequestRNTOEven RequestRNTO = null;
        /// <summary>
        /// 
        /// </summary>
        public NetEvent.RequestAPPEEven RequestAPPE = null;
        /// <summary>
        /// 
        /// </summary>
        public NetEvent.RequsetDELEEven RequsetDELE = null;
        /// <summary>
        /// 
        /// </summary>
        public NetEvent.RequestRMDEven RequestRMD = null;
        /// <summary>
        /// 
        /// </summary>
        public NetEvent.RequestSTOREven RequestSTOR = null;
        /// <summary>
        /// 
        /// </summary>
        public NetEvent.RequestRETREven RequestRETR = null;
        /// <summary>
        /// 
        /// </summary>
        public NetEvent.RequestPWDEven RequestPWD = null;
        /// <summary>
        /// 
        /// </summary>
        public NetEvent.RequestLISTEven RequestLIST = null;
        /// <summary>
        /// 
        /// </summary>
        public NetEvent.RequestSIZEEven RequestSIZE = null;
        /// <summary>
        /// 
        /// </summary>
        public NetEvent.RequsetNLSTEven RequsetNLST = null;
        #endregion

        #region 定义
        private TcpListener FTP_Listener = null;	   
        private Hashtable m_sessionList = null;	  
        private Hashtable m_sessionThreads = null;
        private Thread m_listeningThread = null;	    
        private long m_sessID = 0;
        private bool m_disposed = false;
        private bool Run = false; 
        private string ClassName = "";

        #endregion

        #region 构造函数

        /// <summary>
        ///  FTP服务
        /// </summary>
        public FTPServer(string connection)
        {
            this.Connection = connection;
            ClassName = this.GetType().ToString();
            this.language = Language.Chinese;
        }

        /// <summary>
        /// FTP服务
        /// </summary>
        /// <param name="IP"></param>
        /// <param name="Port"></param>
        /// <param name="rootpath"></param>
        public FTPServer(string IP, int Port, string rootpath)
        {
            NetSet net = new NetSet();
            net.Address_Family = System.Net.Sockets.AddressFamily.InterNetwork;
            net.IPAddress = IP;
            net.Port = Port;
            net.AddrRef = "";
            net.Mode = Net_Mode.Remote;
            net.Protocol_Type = System.Net.Sockets.ProtocolType.Udp;
            net.Socket_Type = System.Net.Sockets.SocketType.Stream;
            this.Connection = Serialize(net);
            this.RootPath = rootpath;
            ClassName = this.GetType().ToString();
            this.language = Language.Chinese;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        ~FTPServer()
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
                    if (FTP_Listener != null)
                    {
                        try
                        {
                            FTP_Listener.Stop();
                        }
                        catch { }
                        FTP_Listener = null;
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
        public bool IsConnected { get { return Run; } }
        /// <summary>
        /// 显示语言
        /// </summary>
        public Language language { get; set; }

        #endregion

        #region 扩展属性

        /// <summary>
        /// 秘钥
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 是否加密
        /// </summary>
        public bool MD5 { get; set; }

        /// <summary>
        /// 是否密码验证
        /// </summary>
        public bool Check { get; set; }

        /// <summary>
        /// 匿名主目录
        /// </summary>
        public string RootPath { get; set; }

        #endregion

        #region 方法
 
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
         
        private void Listen()
        {
            NetSet ftp = new NetSet();
            try
            { ftp = Deserialize<NetSet>(this.Connection); }
            catch
            {
                return;
            }
            try
            {
                int ConnPool = 1;
                if (this.Refer_Prama != null)
                {
                    if (this.Refer_Prama.ConnPool > 0)
                        ConnPool = this.Refer_Prama.ConnPool;
                }
                if (string.IsNullOrEmpty(ftp.IPAddress))
                    FTP_Listener = new TcpListener(System.Net.IPAddress.Any, ftp.Port);
                else
                    FTP_Listener = new TcpListener(System.Net.IPAddress.Parse(ftp.IPAddress), ftp.Port);
                FTP_Listener.Start();
                Run = true;
                while (Run)
                { 
                    if (m_sessionList.Count <  ConnPool)
                    { 
                        Socket clientSocket = FTP_Listener.AcceptSocket();
                        if (this.Connect != null)
                            this.Connect(clientSocket.LocalEndPoint.ToString(), clientSocket.RemoteEndPoint.ToString(), ReferForUse.NetSet, m_sessID); 
                        FtpSession session = new FtpSession(this.Connection ,clientSocket, this, this.m_sessID);
                        if (log != null) session.log += this.log;
                        if (HasError  != null) session.err += this.HasError;
                        if (Connect != null) session.connect += this.Connect;
                        if (DisConnect != null) session.disconnect += this.DisConnect;
                        if (IsValidDir != null)
                            session.IsValidDir += this.IsValidDir;
                        if (FTPAuth != null) session.FTPAuth += this.FTPAuth;
                        if (RequestMKD != null) session.RequestMKD += this.RequestMKD;
                        if (RequestRNTO != null) session.RequestRNTO += this.RequestRNTO;
                        if (RequestAPPE != null) session.RequestAPPE += this.RequestAPPE;
                        if (RequsetDELE != null) session.RequsetDELE += this.RequsetDELE;
                        if (RequestRMD != null) session.RequestRMD += this.RequestRMD;
                        if (RequestSTOR != null) session.RequestSTOR += this.RequestSTOR;
                        if (RequestRETR != null) session.RequestRETR += this.RequestRETR;
                        if (RequestPWD != null) session.RequestPWD += this.RequestPWD;
                        if (RequestLIST != null) session.RequestLIST += this.RequestLIST;
                        if (RequestSIZE != null) session.RequestSIZE += this.RequestSIZE;
                        if (RequsetNLST != null) session.RequsetNLST += this.RequsetNLST;
                        Thread clientThread = new Thread(new ThreadStart(session.StartProcessing));
                        this.m_sessionList.Add(m_sessID, session);
                        clientThread.Start();
                        this.m_sessionThreads.Add(m_sessID, clientThread);
                        m_sessID++;
                    }
                    else
                    { Thread.Sleep(100); }
                }
            }
            catch (Exception ex)
            {
                if (this.log != null)
                    this.log(ClassName, "Listen", Log_Type.Error, ex.Message);
                if (this.HasError != null)
                    this.HasError(ClassName, "Listen", ex);
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
            if (string.IsNullOrEmpty(this.Connection))
            {
                if (this.log != null)
                    this.log(ClassName, "Start", Log_Type.Error, SystemMessage.RefNullOrEmpty("Connection", this.language));
                if (this.HasError != null)
                    this.HasError(ClassName, "Start", new NullReferenceException(SystemMessage.RefNullOrEmpty("Connection", this.language)));
                else
                    throw new NullReferenceException(SystemMessage.RefNullOrEmpty("Connection", this.language));
                return;
            }
            NetSet ftp = new NetSet();
            try
            { ftp = Deserialize<NetSet>(this.Connection); }
            catch
            {
                if (this.log != null)
                    this.log(ClassName, "Listen", Log_Type.Error, SystemMessage.CastError(this.Connection, this.language));
                if (this.HasError != null)
                    this.HasError(ClassName, "Listen", new NullReferenceException(SystemMessage.CastError(this.Connection, this.language)));
                else
                    throw new NullReferenceException(SystemMessage.CastError(this.Connection, this.language));
                return;
            }
            if (this.IsValidDir ==null)
            {
                if (this.log != null)
                    this.log(ClassName, "Start", Log_Type.Error, SystemMessage.RefNullOrEmpty("IsValidDir", this.language));
                if (this.HasError != null)
                    this.HasError(ClassName, "Start", new NullReferenceException(SystemMessage.RefNullOrEmpty("IsValidDir", this.language)));
                else
                    throw new NullReferenceException(SystemMessage.RefNullOrEmpty("IsValidDir", this.language));
                return;
            }
            try
            {
                
                if (!Run)
                {
                    this.m_sessionList = new Hashtable();
                    this.m_sessionThreads = new Hashtable();
                    this.m_listeningThread = new Thread(new ThreadStart(Listen));
                    this.m_listeningThread.Start();
                    if (this.log != null) this.log(ClassName, "Start", Log_Type.Infomation, SystemMessage.ExecStart(language));

                }
            }
            catch (Exception ex)
            {
                if (this.log != null)
                    this.log(ClassName, "Start", Log_Type.Error, ex.Message);
                if (this.HasError != null)
                    this.HasError(ClassName, "Start", ex);
                else
                    throw ex;
            }
        }

        /// <summary>
        ///停止
        /// </summary>
        public void Stop()
        {
            try
            {
                if (this.Run)
                {
                    this.m_listeningThread.Abort();
                    this.Run = false;
                    FTP_Listener.Stop();
                    FTP_Listener = null;
                    for (int i = 0; i < m_sessID; i++)
                    {
                        this.RemoveSession(i);
                    }
                    if (this.log != null) this.log(ClassName, "Stop", Log_Type.Infomation, SystemMessage.ExecStop(language));

                }
            }
            catch (Exception ex)
            {
                if (this.log != null)
                    this.log(ClassName, "Stop", Log_Type.Error, ex.Message);
                if (this.HasError != null)
                    this.HasError(ClassName, "Stop", ex);
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

        /// <summary>
        ///用户退出
        /// </summary>
        /// <param name="session">Session ?supprimer</param>
        public void RemoveSession(long sessID)
        {
            try
            {
                var session = ((FtpSession)this.m_sessionList[sessID]);
                if (this.DisConnect!=null)
                    this.DisConnect(session.LocalEndPoint.ToString(), session.RemoteEndPoint.ToString(), ReferForUse.NetSet, sessID);
                ((Thread)this.m_sessionThreads[sessID]).Abort();
                this.m_sessionThreads.Remove(sessID);
                ((FtpSession)this.m_sessionList[sessID]).Close();
                this.m_sessionList[sessID] = null;
                this.m_sessionList.Remove(sessID);
            }
            catch (Exception ex)
            {
                if (this.log != null)
                    this.log(ClassName, "RemoveSession", Log_Type.Error,ex.Message);
                if (this.HasError != null)
                    this.HasError(ClassName, "RemoveSession", ex);
                else
                    throw ex;
            }
        }
 

        #endregion
    }

    #region 实现所需
 
    internal class FtpSession
    {
        #region 定义
        private Socket m_pClientSocket = null;  // Socket du client
        private FTPServer m_pServer = null;  // Server parent
        private long m_SessionID;				// Identificateur de la session
        private string m_UserName = "";    // Nom de l'utilisateur
        private string m_CurrentDir = "/";	// R閜ertoire courrant
        private string m_renameFrom = "";
        private bool m_PassiveMode = false;	// Flag mode passif
        private TcpListener m_pPassiveListener = null;	// Listener en cas de mode passif
        private IPEndPoint m_pDataConEndPoint = null;	// "EndPoint" pour la connection de donn閑s
        private bool m_Authenticated = false; // Flag utilisateur authentifi?
        private int m_BadCmdCount = 0;     // Nombre de commandes erron閑s
        private DateTime m_SessionStartTime;			// Date de d閎ut de session
        private EndPoint m_rmtEndPoint;
        private EndPoint m_lmtEndPoint;	 
        private int m_offset = 0;		// offset utile en cas d'usage de l'option "resume"
        private Hashtable m_vPaths;					// Liste des r閜ertoire virtuels appliqu閟 ?l'utilisateur
        private string m_ftproot = ""; 
        private bool IsMD5 = false;
        private bool needMD5 = false;
        private bool IsChk = false;
        private bool supportanonymous = false;
        private string anonymousftproot = "";

        #endregion

        #region 属性
        public CommEvent.LogEven log = null;
        public CommEvent.HasErrorEven err = null;
        public NetEvent.Connect connect = null;
        public NetEvent.DisConnect disconnect = null;
        public NetEvent.IsValidDirEven IsValidDir = null;
        public NetEvent.FTPAuthEven FTPAuth = null;
        public NetEvent.RequestMKDEven RequestMKD = null;
        public NetEvent.RequestRNTOEven RequestRNTO = null;
        public NetEvent.RequestAPPEEven RequestAPPE = null;
        public NetEvent.RequsetDELEEven RequsetDELE = null;
        public NetEvent.RequestRMDEven RequestRMD = null;
        public NetEvent.RequestSTOREven RequestSTOR = null;
        public NetEvent.RequestRETREven RequestRETR = null;
        public NetEvent.RequestPWDEven RequestPWD = null;
        public NetEvent.RequestLISTEven RequestLIST = null;
        public NetEvent.RequestSIZEEven RequestSIZE = null;
        public NetEvent.RequsetNLSTEven RequsetNLST = null;
        public Language lan = Language.Chinese; 
        public long SessionID
        {
            get { return m_SessionID; }
        }

        public EndPoint RemoteEndPoint
        {
            get { return this.m_rmtEndPoint; }
        }

        public EndPoint LocalEndPoint
        {
            get { return this.m_lmtEndPoint; }
        }

        public Socket ClientSocket
        {
            get { return this.m_pClientSocket; }
        }

        public int SessionTout { get; set; }

        public string SafeKey { get; set; }
        public string Connection { get; set; }
        #endregion

        #region 主函数


        /// <summary>
        ///构造函数
        /// </summary>
        /// <param name="clientSocket">传入Socket/param>
        /// <param name="server">传递类地址</param>
        /// <param name="sessionID">会话ID</param>
        public FtpSession(string connection,Socket clientSocket, FTPServer server, long sessionID)
        {
            this.Connection = connection;
            this.m_pClientSocket = clientSocket;
            this.m_rmtEndPoint = clientSocket.RemoteEndPoint;
            this.m_lmtEndPoint = clientSocket.LocalEndPoint;
            this.m_pServer = server;
            this.m_SessionID = sessionID;
            this.m_SessionStartTime = System.DateTime.Now;
            if (connect != null)
                connect(Connection, m_rmtEndPoint.ToString(), ReferForUse.NetSet, m_SessionID);
        }

        /// <summary>
        ///启动函数
        /// </summary>
        public void StartProcessing()
        {
            this.SendData(FTPMessage.MessReady(lan));
            long lastCmdTime = DateTime.Now.Ticks;
            string lastCmd = "";
            int ConnectTimeOut = 1000;
            int ConnPool = 1;
            if (this.m_pServer.Refer_Prama != null)
            {
                if (this.m_pServer.Refer_Prama.ConnectTimeOut > 0)
                    ConnectTimeOut = this.m_pServer.Refer_Prama.ConnectTimeOut;
            }
            if (this.m_pServer.Refer_Prama != null)
            {
                if (this.m_pServer.Refer_Prama.ConnPool > 0)
                    ConnPool = this.m_pServer.Refer_Prama.ConnPool;
            }
            try
            {
                while (true)
                {
                    if (this.m_pClientSocket.Available > 0)
                    {
                        try
                        {
                            lastCmd = this.ReadLine(m_pClientSocket, ConnectTimeOut);
                            if (log != null) log("FtpSession", "StartProcessing", Log_Type.Test, lastCmd);
                            if (!SwitchCommand(lastCmd))
                            {
                                break;
                            }
                        }
                        catch (Exception  rX)
                        {   
                            if (m_BadCmdCount > ConnPool - 1)
                            {
                                if (log != null) log("FtpSession", "StartProcessing", Log_Type.Error, FTPMessage.MessTooManyBadCmds(lan));
                                SendData(FTPMessage.MessTooManyBadCmds(lan)); 
                                break; 
                            }
                            m_BadCmdCount++;
                            SendData(FTPMessage.CmdTimeOut(lan));
                            if (log != null) log("FtpSession", "StartProcessing", Log_Type.Error, FTPMessage.CmdTimeOut(lan)); 
                        }
                        catch
                        {
                            if (!this.m_pClientSocket.Connected) break;
                            SendData(FTPMessage.UnknownError(lan));
                            if (log != null) log("FtpSession", "StartProcessing", Log_Type.Error, FTPMessage.UnknownError(lan));
                        }
                        lastCmdTime = DateTime.Now.Ticks;
                    } 
                    else
                    {
                        if (DateTime.Now.Ticks > lastCmdTime + ((long)ConnectTimeOut) * 10000000)
                        {
                            SendData(FTPMessage.UnknownError(lan));
                            if (log != null) log("FtpSession", "StartProcessing", Log_Type.Error, FTPMessage.UnknownError(lan));
                            break;
                        }
                        Thread.Sleep(100);
                    }
                }
                if (m_pClientSocket.Connected)
                {
                    m_pClientSocket.Close();
                }
                this.m_pServer.RemoveSession(this.m_SessionID);
            }
            catch
            {
                this.m_pServer.RemoveSession(this.m_SessionID);
            }
        }

        /// <summary>
        /// 关闭函数
        /// </summary>
        public void Close()
        {
            try { this.m_pClientSocket.Close(); }
            catch { }
            try { this.m_pPassiveListener.Stop(); }
            catch { }
            if (disconnect != null)
                disconnect(Connection, m_rmtEndPoint.ToString(), ReferForUse.NetSet, m_SessionID);
        }

        #endregion

        #region 事件

        private bool CheckAction(string argsText, out string file, out string newCurdir)
        {
            file = "";
            newCurdir = "";
            if (!m_Authenticated)
            {
                SendData(FTPMessage.AuthReq(lan));
                return false ;
            }
            if (!IsValidDir(this.m_UserName,this.m_SessionID,this.m_CurrentDir,argsText,out  file ,out newCurdir) || !(System.IO.File.Exists(file) || System.IO.Directory.Exists(file)))
            {
                SendData(FTPMessage.AccesDenied(lan));
                return false;
            }
            return true;
        }

        private void NOOP()
        {
            SendData(FTPMessage.NOOPOK(lan));
        }

        private void MKD(string argsText)
        {
            string file;
            string newCurdir;
            if (!CheckAction(argsText,out file,out newCurdir)) return;
            try
            {
                string res = "";
                if (RequestMKD != null)
                    res= RequestMKD(this.m_UserName, this.m_SessionID, file);
                if (string.IsNullOrEmpty(res))
                    res = FTPMessage.CmdTimeOut(lan);
                else
                   SendData(res);
            }
            catch
            {
                SendData(FTPMessage.UnknownError(lan));
                return;
            }
        }

        private void RNFR(string argsText)
        {
            string file;
            string newCurdir;
            if (!CheckAction(argsText, out file, out newCurdir)) return; 
            this.m_renameFrom = file;
            SendData(FTPMessage.RNFRFaile(lan)); 
        }

        private void RNTO(string argsText)
        {
            try
            {
                string file;
                string newCurdir;
                if (!CheckAction(argsText, out file, out newCurdir)) return;

                if (this.m_renameFrom.Length < 1)
                {
                    SendData(FTPMessage.Badsequencecommands(lan));
                    return;
                }
                string res = "";
                if (RequestRNTO != null)
                    res = RequestRNTO(this.m_UserName, this.m_SessionID, this.m_renameFrom);
                if (string.IsNullOrEmpty(res))
                    res = FTPMessage.CmdTimeOut(lan);
                else
                    SendData(res);
            }
            catch
            { SendData(FTPMessage.Errorrenameing(lan)); }
            this.m_renameFrom = ""; 
        }

        private void REST(string argsText)
        {
            if (!m_Authenticated)
            {
                SendData(FTPMessage.AuthReq(lan));
                return;
            }
            try
            {
                this.m_offset = Convert.ToInt32(argsText);
                SendData(FTPMessage.argumentREST(this.m_offset));
            }
            catch
            {
                SendData(FTPMessage.badargumentREST(lan));
            }
        }

        private void APPE(string argsText)
        {
            string file;
            string newCurdir;
            if (!CheckAction(argsText, out file, out newCurdir)) return;
            Socket socket = this.GetDataConnection();
            try
            {
                if (socket == null )
                    throw new Exception();
                string res = "";
                if (RequestAPPE != null)
                    res = RequestAPPE(this.m_UserName, this.m_SessionID, socket, file);
                if (string.IsNullOrEmpty(res))
                    res = FTPMessage.CmdTimeOut(lan);
                else
                    SendData(res);
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();            }
            catch
            {
                SendData(FTPMessage.TrFailed(lan));
            }
            finally
            {
                try
                {
                    if (socket != null)
                        socket.Close();
                }
                catch { }
            }
        }

        private void DELE(string argsText)
        {
            string file;
            string newCurdir;
            if (!CheckAction(argsText, out file, out newCurdir)) return;

            try
            { 
                string res = "";
                if (RequsetDELE != null)
                    res = RequsetDELE(this.m_UserName, this.m_SessionID, file);
                if (string.IsNullOrEmpty(res))
                    res = FTPMessage.CmdTimeOut(lan);
                else
                    SendData(res);
            }
            catch
            {
                SendData(FTPMessage.AccesDenied(lan));
            }
        }

        private void RMD(string argsText)
        {
            string file;
            string newCurdir;
            if (!CheckAction(argsText, out file, out newCurdir)) return;
            try
            {
                string res = "";
                if (RequestRMD != null)
                    res = RequestRMD(this.m_UserName, this.m_SessionID, file);
                if (string.IsNullOrEmpty(res))
                    res = FTPMessage.CmdTimeOut(lan);
                else
                    SendData(res);
            }
            catch
            {
                SendData(FTPMessage.AccesDenied(lan));
            }
        }

        private void STOR(string argsText)
        {
            string file;
            string newCurdir;
            if (!CheckAction(argsText, out file, out newCurdir)) return;
            Socket socket = this.GetDataConnection();
            try
            {
                if (socket == null)
                    throw new Exception();
                string res = "";
                if (RequestSTOR != null)
                    res = RequestSTOR(this.m_UserName, this.m_SessionID, socket, file, this.m_offset);
                if (string.IsNullOrEmpty(res))
                    res = FTPMessage.CmdTimeOut(lan);
                else
                    SendData(res);
                socket.Shutdown(SocketShutdown.Both);
            }
            catch
            {
                SendData(FTPMessage.TrFailed(lan));
            }
            finally
            {
                try
                {
                    if (socket != null)
                        socket.Close();
                }
                catch { }
            }

        }

        private void RETR(string argsText)
        {
            string file;
            string newCurdir;
            if (!CheckAction(argsText, out file, out newCurdir)) return;
            Socket socket = this.GetDataConnection();
            try
            { 
                if (socket == null )
                    throw new Exception();
                string res = "";
                if (RequestRETR != null)
                    res = RequestRETR(this.m_UserName, this.m_SessionID, socket, file, this.m_offset);
                if (string.IsNullOrEmpty(res))
                    res = FTPMessage.CmdTimeOut(lan);
                else
                    SendData(res);
                socket.Shutdown(SocketShutdown.Both);
            }
            catch
            {
                SendData(FTPMessage.TrFailed(lan));
            }
            finally
            {
                try
                {
                    if (socket != null)
                        socket.Close();
                }
                catch { }
            }
        }

        private void QUIT()
        {
            SendData(FTPMessage.SignOff(lan));
        }

        private void USER(string argsText)
        {
            if (m_Authenticated)
            {
                SendData(FTPMessage.AlreadyAuth(lan));
                return;
            }
            if (m_UserName.Length > 0)
            {
                SendData(FTPMessage.UserButNotPass(lan));
                return;
            }
            string userName = argsText;
            SendData(FTPMessage.PassReq(userName,lan));
            m_UserName = userName;
        }

        private void PASS(string argsText)
        {
            if (m_Authenticated)
            {
                SendData(FTPMessage.AlreadyAuth(lan));
                return;
            }
            if (m_UserName.Length == 0)
            {
                SendData(FTPMessage.EnterUser(lan));
                return;
            }
            if (FTPAuth == null)
            {
                m_Authenticated = true;
                SendData(FTPMessage.PassOk(lan));
            }
            else
            {
                try
                {
                    m_Authenticated = FTPAuth(ref m_UserName, argsText, this.m_SessionID);
                    if (m_Authenticated)
                    {
                        SendData(FTPMessage.PassOk(lan));
                    }
                    else
                    {
                        SendData(FTPMessage.AuthFailed(lan));
                        m_UserName = "";
                        this.m_ftproot = "";
                    }
                }
                catch
                {
                    m_Authenticated = false;
                    SendData(FTPMessage.AuthFailed(lan));
                    m_UserName = "";
                    this.m_ftproot = "";
                } 
            }
        }

        private void PWD()
        { 
            if (!m_Authenticated)
            {
                SendData(FTPMessage.AuthReq(lan));
                return;
            }
            try
            {
                string res = "";
                if (RequestPWD != null)
                    res = RequestPWD(this.m_UserName, this.m_SessionID);
                if (string.IsNullOrEmpty(res))
                    res = FTPMessage.CmdTimeOut(lan);
                else
                    SendData(res);
            }
            catch
            { SendData(FTPMessage.PwdFal(m_CurrentDir,lan)); }
        }

        private void SYST()
        {

            if (!m_Authenticated)
            {
                SendData(FTPMessage.AuthReq(lan));
                return;
            } 
            SendData("215 " + Environment.OSVersion.VersionString + "\r\n");
        }

        private void TYPE(string argsText)
        { 
            if (!m_Authenticated)
            {
                SendData(FTPMessage.AuthReq(lan));
                return;
            }
            if (argsText.Trim().ToUpper() == "A" || argsText.Trim().ToUpper() == "I")
            {
                SendData(FTPMessage.TypeSet(argsText,lan));
            }
            else
            {
                SendData(FTPMessage.InvalidType(argsText,lan));
            }
        }

         private void PORT(string argsText)
        {
            if (!m_Authenticated)
            {
                SendData(FTPMessage.AuthReq(lan));
                return;
            }
            string[] parts = argsText.Split(',');
            if (parts.Length != 6)
            {
                SendData(FTPMessage.SyntaxError(lan));
                return;
            }
            string ip = parts[0] + "." + parts[1] + "." + parts[2] + "." + parts[3];
            int port = (Convert.ToInt32(parts[4]) << 8) | Convert.ToInt32(parts[5]);
            m_pDataConEndPoint = new IPEndPoint(System.Net.Dns.GetHostByAddress(ip).AddressList[0], port);
            this.m_PassiveMode = false;
            SendData(FTPMessage.PortCmdSuccess(lan));
        }
 
        private void PASV()
        {
            if (!m_Authenticated)
            {
                SendData(FTPMessage.AuthReq(lan));
                return;
            }
            int port = 1025;
            while (true)
            {
                try
                {
                    m_pPassiveListener = new TcpListener(IPAddress.Any, port);
                    m_pPassiveListener.Start();
                    break;
                }
                catch
                {
                    port++;
                }
            }
            string ip = m_pClientSocket.LocalEndPoint.ToString();
            ip = ip.Substring(0, ip.IndexOf(":"));
            ip = ip.Replace(".", ",");
            ip += "," + (port >> 8) + "," + (port & 255);
            SendData(FTPMessage.PasvCmdSuccess(ip,lan));
            m_PassiveMode = true;

        }

        private void LIST(string args)
        {
            if (!this.m_Authenticated)
            {
                SendData(FTPMessage.AuthReq(lan));
                return;
            }
            try
            {
                string cdir;
                string newCurdir;
                if (!IsValidDir(this.m_UserName, this.m_SessionID, this.m_CurrentDir, args, out cdir, out newCurdir) || !(System.IO.File.Exists(cdir) ))
                {
                    SendData(FTPMessage.AccesDenied(lan));
                    return;
                } 
                Socket socket = this.GetDataConnection();
                string msg = "";
                if (RequestLIST != null)
                    msg = RequestLIST(this.m_UserName, this.m_SessionID, cdir);
                byte[] toSend = System.Text.Encoding.Default.GetBytes(msg); 
                socket.Send(toSend);
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
                SendData(FTPMessage.TrComplete(lan));
            }
            catch
            {
                SendData(FTPMessage.TrFailed(lan));
            }
        }

        private void SIZE(string args)
        {
            string file;
            string newCurdir;
            if (!CheckAction(args, out file, out newCurdir)) return;
            try
            {
                int size = 0;
                if (RequestSIZE != null)
                    size = RequestSIZE(this.m_UserName, this.m_SessionID, file);
                SendData("213 " + size.ToString() + "\r\n");
            }
            catch
            {
                SendData("213 0\r\n");
            }
        }

        private void NLST(string args)
        { 
            try
            { 
                string cdir;
                string newCurdir;
                if (!CheckAction(args, out cdir, out newCurdir)) return; 
                Socket socket = this.GetDataConnection();
                string msg = "";
                if (RequsetNLST != null)
                    msg=RequsetNLST(this.m_UserName, this.m_SessionID, cdir);
                byte[] toSend = System.Text.Encoding.Default.GetBytes(msg);
                socket.Send(toSend);
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
                SendData(FTPMessage.TrComplete(lan));
            }
            catch
            {
                SendData(FTPMessage.TrFailed(lan));
            }

        }

        private void CWD(string args)
        {
            try
            {
                string file;
                string newCurdir;
                if (!CheckAction(args, out file, out newCurdir)) return; 
                this.m_CurrentDir = newCurdir;
                SendData(FTPMessage.CwdOk(lan));
            }
            catch
            {
                SendData(FTPMessage.AccesDenied(lan));
            }
        }

        #endregion

        #region 子函数

        private Socket GetDataConnection()
        {
            Socket socket = null;
            try
            {
                if (m_PassiveMode)
                {
                    long startTime = DateTime.Now.Ticks;
                    while (!m_pPassiveListener.Pending())
                    {
                        System.Threading.Thread.Sleep(50);
                        if ((DateTime.Now.Ticks - startTime) / 10000 > SessionTout * 1000)
                        {
                            throw new Exception("Ftp server didn't respond !");
                        }
                    }
                    socket = m_pPassiveListener.AcceptSocket();
                    SendData(FTPMessage.DataOpen(lan));
                }
                else
                {
                    SendData(FTPMessage.DataOpening(lan));
                    socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    socket.Connect(m_pDataConEndPoint);
                }
            }
            catch
            {
                SendData(FTPMessage.opendatafailed(lan));
                return null;
            }
            m_PassiveMode = false;
            return socket;
        }
 
        private void SendData(string data)
        {
            Byte[] byte_data = System.Text.Encoding.Default.GetBytes(data.ToCharArray());
            m_pClientSocket.Send(byte_data, byte_data.Length, 0);
        }

        private string ReadLine(Socket clientSocket, int timeOut)
        {
            long lastDataTime = DateTime.Now.Ticks; 
            ArrayList lineBuf = new ArrayList(); 
            byte prevByte = 0; 
            while (true)
            {
                if (clientSocket.Available > 0)
                {
                    byte[] currByte = new byte[1];
                    int countRecieved = clientSocket.Receive(currByte, 1, SocketFlags.None);
                    if (countRecieved == 1)
                    {
                        lineBuf.Add(currByte[0]);
                        if ((prevByte == (byte)'\r' && currByte[0] == (byte)'\n'))
                        {
                            byte[] retVal = new byte[lineBuf.Count - 2]; 
                            lineBuf.CopyTo(0, retVal, 0, lineBuf.Count - 2);
                            return System.Text.Encoding.Default.GetString(retVal).Trim();
                        }
                        prevByte = currByte[0];
                        lastDataTime = DateTime.Now.Ticks;
                    }
                }
                else
                {
                    if (DateTime.Now.Ticks > lastDataTime + ((long)(SessionTout)) * 100000)
                    {
                        throw new Exception("TimeOut");
                    }
                    System.Threading.Thread.Sleep(100);	
                }
            }
        }

        private bool SwitchCommand(string commandTxt)
        {
            string[] cmdParts = commandTxt.TrimStart().Split(new char[] { ' ' });
            string command = cmdParts[0].ToUpper().Trim();

            switch (command)
            {
                case "QUIT":
                    QUIT();
                    return false;
                case "USER":
                    if (cmdParts.Length < 2) SendData(FTPMessage.UnknownError(lan));
                    else
                    {
                        if (cmdParts.Length > 2)
                        {
                            string args = "";
                            for (int i = 1; i < cmdParts.Length; i++)
                            {
                                args += cmdParts[i] + " ";
                            }
                            USER(args);
                        }
                        else
                        {
                            USER(cmdParts[1]);
                        }
                    }
                    break;
                case "TYPE":
                    if (cmdParts.Length != 2) SendData(FTPMessage.UnknownError(lan));
                    else TYPE(cmdParts[1]);
                    break;
                case "PASS":
                    if (cmdParts.Length < 2) SendData(FTPMessage.UnknownError(lan));
                    else
                    {
                        if (cmdParts.Length > 2)
                        {
                            string args = "";
                            for (int i = 1; i < cmdParts.Length; i++)
                            {
                                args += cmdParts[i] + " ";
                            }
                            PASS(args);
                        }
                        else
                        {
                            PASS(cmdParts[1]);
                        }
                    }
                    break;
                case "PWD":
                    PWD();
                    break;

                case "XPWD":
                    PWD();
                    break;

                case "SYST":
                    SYST();
                    break;
                case "PORT":
                    if (cmdParts.Length != 2) SendData(FTPMessage.UnknownError(lan));
                    else PORT(cmdParts[1]);
                    break;
                case "PASV":
                    PASV();
                    break;
                case "LIST":
                    if (cmdParts.Length > 1)
                    {
                        if (cmdParts[1].Trim().ToLower() == "-a" || cmdParts[1].Trim().ToLower() == "-l" || cmdParts[1].Trim().ToLower() == "-al")
                        {
                            string[] parts = new string[cmdParts.Length - 1];
                            parts[0] = cmdParts[0];
                            for (int i = 2; i < cmdParts.Length; i++)
                                parts[i - 1] = cmdParts[i];
                            cmdParts = parts;
                        }
                    }
                    if (cmdParts.Length > 2)
                    {
                        string args = "";
                        for (int i = 1; i < cmdParts.Length; i++)
                        {
                            args += cmdParts[i] + " ";
                        }
                        LIST(args);
                    }
                    else if (cmdParts.Length == 2) LIST(cmdParts[1]);
                    else LIST(String.Empty);
                    break;
                case "NLST":
                    if (cmdParts.Length > 1)
                    {
                        if (cmdParts[1].Trim().ToLower() == "-a" || cmdParts[1].Trim().ToLower() == "-l" || cmdParts[1].Trim().ToLower() == "-al")
                        {
                            string[] parts = new string[cmdParts.Length - 1];
                            parts[0] = cmdParts[0];
                            for (int i = 2; i < cmdParts.Length; i++)
                                parts[i - 1] = cmdParts[i];
                            cmdParts = parts;
                        }
                    }
                    if (cmdParts.Length > 2)
                    {
                        string args = "";
                        for (int i = 1; i < cmdParts.Length; i++)
                        {
                            args += cmdParts[i] + " ";
                        }
                        LIST(args);
                    }
                    else if (cmdParts.Length == 2) NLST(cmdParts[1]);
                    else NLST(String.Empty);
                    break;

                case "CWD":
                    if (cmdParts.Length < 2) SendData(FTPMessage.UnknownError(lan));
                    else
                    {
                        if (cmdParts.Length > 2)
                        {
                            string args = "";
                            for (int i = 1; i < cmdParts.Length; i++)
                            {
                                args += cmdParts[i] + " ";
                            }
                            CWD(args);
                        }
                        else
                        {
                            CWD(cmdParts[1]);
                        }
                    }
                    break;
                case "RETR":
                    if (cmdParts.Length < 2) SendData(FTPMessage.UnknownError(lan));
                    else
                    {
                        if (cmdParts.Length > 2)
                        {
                            string args = "";
                            for (int i = 1; i < cmdParts.Length; i++)
                            {
                                args += cmdParts[i] + " ";
                            }
                            RETR(args);
                        }
                        else
                        {
                            RETR(cmdParts[1]);
                        }
                    }
                    break;
                case "STOR":
                    if (cmdParts.Length < 2) SendData(FTPMessage.UnknownError(lan));
                    else
                    {
                        if (cmdParts.Length > 2)
                        {
                            string args = "";
                            for (int i = 1; i < cmdParts.Length; i++)
                            {
                                args += cmdParts[i] + " ";
                            }
                            STOR(args);
                        }
                        else
                        {
                            STOR(cmdParts[1]);
                        }
                    }
                    break;

                case "DELE":
                    if (cmdParts.Length < 2) SendData(FTPMessage.UnknownError(lan));
                    else
                    {
                        if (cmdParts.Length > 2)
                        {
                            string args = "";
                            for (int i = 1; i < cmdParts.Length; i++)
                            {
                                args += cmdParts[i] + " ";
                            }
                            DELE(args);
                        }
                        else
                        {
                            DELE(cmdParts[1]);
                        }
                    }
                    break;

                case "RMD":
                    if (cmdParts.Length < 2) SendData(FTPMessage.UnknownError(lan));
                    else
                    {
                        if (cmdParts.Length > 2)
                        {
                            string args = "";
                            for (int i = 1; i < cmdParts.Length; i++)
                            {
                                args += cmdParts[i] + " ";
                            }
                            RMD(args);
                        }
                        else
                        {
                            RMD(cmdParts[1]);
                        }
                    }
                    break;

                case "APPE":
                    if (cmdParts.Length < 2) SendData(FTPMessage.UnknownError(lan));
                    else
                    {
                        if (cmdParts.Length > 2)
                        {
                            string args = "";
                            for (int i = 1; i < cmdParts.Length; i++)
                            {
                                args += cmdParts[i] + " ";
                            }
                            APPE(args);
                        }
                        else
                        {
                            APPE(cmdParts[1]);
                        }
                    }
                    break;

                case "RNFR":
                    if (cmdParts.Length < 2) SendData(FTPMessage.UnknownError(lan));
                    else
                    {
                        if (cmdParts.Length > 2)
                        {
                            string args = "";
                            for (int i = 1; i < cmdParts.Length; i++)
                            {
                                args += cmdParts[i] + " ";
                            }
                            RNFR(args);
                        }
                        else
                        {
                            RNFR(cmdParts[1]);
                        }
                    }
                    break;

                case "RNTO":
                    if (cmdParts.Length < 2) SendData(FTPMessage.UnknownError(lan));
                    else
                    {
                        if (cmdParts.Length > 2)
                        {
                            string args = "";
                            for (int i = 1; i < cmdParts.Length; i++)
                            {
                                args += cmdParts[i] + " ";
                            }
                            RNTO(args);
                        }
                        else
                        {
                            RNTO(cmdParts[1]);
                        }
                    }
                    break;

                case "MKD":
                    if (cmdParts.Length < 2) SendData(FTPMessage.UnknownError(lan));
                    else
                    {
                        if (cmdParts.Length > 2)
                        {
                            string args = "";
                            for (int i = 1; i < cmdParts.Length; i++)
                            {
                                args += cmdParts[i] + " ";
                            }
                            MKD(args);
                        }
                        else
                        {
                            MKD(cmdParts[1]);
                        }
                    }
                    break;

                case "REST":
                    if (cmdParts.Length < 2) SendData(FTPMessage.UnknownError(lan));
                    else
                    {
                        if (cmdParts.Length > 2)
                        {
                            string args = "";
                            for (int i = 1; i < cmdParts.Length; i++)
                            {
                                args += cmdParts[i] + " ";
                            }
                            REST(args);
                        }
                        else
                        {
                            REST(cmdParts[1]);
                        }
                    }
                    break;

                case "SIZE":
                    if (cmdParts.Length < 2) SendData(FTPMessage.UnknownError(lan));
                    else
                    {
                        if (cmdParts.Length > 2)
                        {
                            string args = "";
                            for (int i = 1; i < cmdParts.Length; i++)
                            {
                                args += cmdParts[i] + " ";
                            }
                            SIZE(args);
                        }
                        else
                        {
                            SIZE(cmdParts[1]);
                        }
                    }
                    break;

                case "CDUP":
                    CWD("..");
                    break;
                case "NOOP":
                    NOOP();
                    break;

                case "CLNT":
                    SendData(FTPMessage.Noted(lan));
                    break;
                case "OPTS":
                    SendData(FTPMessage.CMDOK(lan));
                    break;

                default:
                    SendData(FTPMessage.Invalidcommand(lan));
                    break;

            }
            return true;
        }
  
        #endregion
    }

    #endregion

}
