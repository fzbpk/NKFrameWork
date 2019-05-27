using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Net.Sockets;
using System.IO.Ports;
using NK.Entity;
using NK.ENum;
using NK.Event;
using NK.Message;
using NK.Class;
using System.Threading;
using System.Net;
using System.Reflection;

namespace NK.Communicate
{
    internal  class ClientSession:CommTHelper
    { 
        #region 构造

        public ClientSession()
        {
         
            NetRS = null;
            UartRS = null;
            Refer = null; 
            lan = Language.Chinese; 

            KeepAlive = DLLConfig.KeepAlive;
            ChkTime = DLLConfig.ChkTime;
            RTime = 0;
            HBTime = 0;
            FlagCount = 0;
            RegCount = 0;
            HBCount = 0;

            session = new Dictionary<string, object>();
            SubFlags = new List<string>();
            flags = "";
            Pause = false;
            Exec = false;
            Run = false; 
            ConnTOut = DLLConfig.ConnTOut;
            WaitTime = DLLConfig.WaitTime;

            Session = new CommunicateSession(); 
            Session.ConnectTime = DateTime.Now;
            Session.SessionID = 0;
            Session.Remote = "";
            Session.ListNO = "";

            ClassName = this.GetType().Name;
        }

        ~ClientSession()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
         
        #endregion

        #region 私有方法

        private void InitRS()
        {
            MethodName = "";
            try
            {
                MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            switch (Session.LocalMode)
            {
                case ReferForUse.NetSet:
                    if (NetRS == null)
                    {
                        Session.SessionID = 0;
                        Session.Remote = ""; 
                        NetSet NetInfo = Session.Connection.Deserialize<NetSet>();
                        #region 初始化  
                        WriteLog(MethodName, Log_Type.Infomation, SystemMessage.Init("Socket", lan));
                        NetRS = new Socket(NetInfo.Address_Family, NetInfo.Socket_Type, NetInfo.Protocol_Type);
                        IPAddress addr = IPAddress.Any;
                        if (string.IsNullOrEmpty(NetInfo.IPAddress))
                            addr = IPAddress.Any;
                        else
                            IPAddress.TryParse(NetInfo.IPAddress, out addr);
                        EndPoint = new IPEndPoint(addr, NetInfo.Port);
                        #endregion
                        #region 设置
                        WriteLog(MethodName, Log_Type.Infomation, SystemMessage.SET("Socket", lan));
                        if (Refer != null)
                        {
                            NetRS.ReceiveBufferSize = Refer.ReceiveBufferSize;
                            NetRS.ReceiveTimeout = Refer.ReceiveTimeout;
                            NetRS.SendBufferSize = Refer.SendBufferSize;
                            NetRS.SendTimeout = Refer.SendTimeout;
                        }
                        else
                        {
                            NetRS.ReceiveBufferSize = DLLConfig.ReceiveBufferSize;
                            NetRS.ReceiveTimeout = DLLConfig.ReceiveTimeout;
                            NetRS.SendBufferSize = DLLConfig.SendBufferSize;
                            NetRS.SendTimeout = DLLConfig.SendTimeout;
                        }
                        #endregion
                    } 
                    break;
                case ReferForUse.UartSet:
                    if (UartRS == null)
                    {
                        Session.SessionID = 0;
                        Session.Remote = "";
                        PortsSet PortInfo = Session.Connection.Deserialize<PortsSet>();
                        #region 初始化
                        WriteLog(MethodName, Log_Type.Infomation, SystemMessage.Init("SerialPort", lan));
                        UartRS = new SerialPort();
                        UartRS.PortName = "Com" + PortInfo.Port.ToString();
                        UartRS.BaudRate = PortInfo.Rate;
                        UartRS.DataBits = PortInfo.DataBit;
                        UartRS.StopBits = PortInfo.StopBit;
                        UartRS.Parity = PortInfo.Parity;
                        UartRS.Handshake = PortInfo.Ctrl;
                        #endregion
                        #region 设置
                        WriteLog(MethodName, Log_Type.Infomation, SystemMessage.SET("SerialPort", lan));
                        if (Refer != null)
                        {
                            UartRS.ReadBufferSize = Refer.ReceiveBufferSize;
                            UartRS.ReadTimeout = Refer.ReceiveTimeout;
                            UartRS.WriteBufferSize = Refer.SendBufferSize;
                            UartRS.WriteTimeout = Refer.SendTimeout;
                        }
                        else
                        {
                            UartRS.ReadBufferSize = DLLConfig.ReceiveBufferSize;
                            UartRS.ReadTimeout = DLLConfig.ReceiveTimeout;
                            UartRS.WriteBufferSize = DLLConfig.SendBufferSize;
                            UartRS.WriteTimeout = DLLConfig.SendTimeout;
                        }
                        #endregion
                    } 
                    break;
            }
        }
        
        #endregion

        #region 公共方法
  
        public void Start()
        {
            MethodName = "";
            try
            {
                MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            if (string.IsNullOrEmpty(Session.Connection) || Session.LocalMode == ReferForUse.None) return;
            Random RListNO = new Random();
            List<byte> Pack = new List<byte>(); 
            List<string> SubFlags = new List<string>();
            DateTime HBTimer = DateTime.Now;
            DateTime RunTimer = DateTime.Now;
            DateTime ChkTimer = DateTime.Now;
            int index = 0;
            string SubCon = "";
            byte[] buf = null;
            init();
            try
            {
                InitRS();
                #region RegEVENT
                WriteLog(MethodName, Log_Type.Infomation, SystemMessage.Check("Socket", lan));
                while (Run)
                {
                    try
                    {
                        OpenConnect(); 
                        if (IsConnected())
                        {
                            GetRemote();
                            Session.ListNO = DateTime.Now.ToString("yyyyMMddHHmmssms") + RListNO.Next(1000, 9999).ToString(); 
                            buf = Read();
                            if (buf.Length > 0)
                            {
                                WriteLog("RegEVENT Recv", Log_Type.Test, SystemMessage.RefValDisp("RegEVENT", buf.ToHex(true), lan));
                                Pack.AddRange(buf);
                                RecvSer(buf, ref flags, ref SubFlags);
                                if (!string.IsNullOrEmpty(flags))
                                    WriteLog("RegSer", Log_Type.Infomation, SystemMessage.RefValDisp("Flags", flags, lan));
                                if (SubFlags == null) SubFlags = new List<string>();
                                if (SubFlags.Count <= 0) SubFlags.Add("");
                                RecviceData(ref Pack);
                                CMD();
                            }
                            if (string.IsNullOrEmpty(flags) && Run)
                                RegSer(ref flags, ref SubFlags); 
                            CallInit(); 
                            CloseConnect();
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteLog(MethodName , Log_Type.Error, "Reg:" + ex.Message);
                        Thread.Sleep(ConnTOut);
                        Session.SessionID = 0;
                        Session.Remote = "";
                        flags = "";
                    }
                }
                #endregion
                #region 主循环
                HBTimer = DateTime.Now;
                RunTimer = DateTime.Now;
                Pack.Clear();
                if (SubFlags.Count <= 0)
                    SubFlags.Add("");
                WriteLog("Main", Log_Type.Infomation, SystemMessage.ExecStart(lan));
                while (Run)
                {
                    if (Pause) continue;
                    Exec = true;
                    Session.ListNO = DateTime.Now.ToString("yyyyMMddHHmmssms") + RListNO.Next(1000, 9999).ToString();
                    SubCon = SubFlags[index];
                    #region 链接
                    if (KeepAlive || HBTime > 0 && HBTimer.AddSeconds(HBTime) < DateTime.Now || RTime > 0 && RunTimer.AddSeconds(RTime) < DateTime.Now)
                    {
                        if (!IsConnected())
                        {
                            try
                            {
                                OpenConnect();
                                if (IsConnected())
                                {
                                    GetRemote();
                                    if (connect != null)
                                        connect(Session.Connection, Session.Remote, Session.LocalMode, Session.SessionID);
                                    WriteLog(  "Connect", Log_Type.Infomation, Session.Remote);
                                }
                            }
                            catch { }
                        }
                    }
                    #endregion
                    if (IsConnected())
                    {  
                        #region 接收
                        if (Available() > 0 && Run)
                        {
                            buf = Read();
                            if (buf.Length > 0)
                            {
                                WriteLog("ReceiveData", Log_Type.Test, SystemMessage.RefValDisp("Available Data:", buf.ToHex(true), lan));
                                Pack.AddRange(buf);
                                buf = Pack.ToArray();
                                if (string.IsNullOrEmpty(flags))
                                {
                                    RecvSer(buf, ref flags, ref SubFlags);
                                    if (!string.IsNullOrEmpty(flags))
                                        WriteLog("RegSer", Log_Type.Infomation, SystemMessage.RefValDisp("Flags", flags, lan));
                                    else
                                        RegSer(ref flags, ref SubFlags);
                                    if (SubFlags == null) SubFlags = new List<string>();
                                    if (SubFlags.Count <= 0) SubFlags.Add("");
                                }
                                RecviceData(ref Pack);
                                CMD();
                                ChkTimer = new DateTime();
                            } 
                        }
                        #endregion
                        if (string.IsNullOrEmpty(flags) && Run)
                            RegSer(ref flags, ref SubFlags);
                        #region 心跳
                        if (HBTime > 0 && Run)
                        {
                            if (IntegerPoint)
                            {
                                if (DateTime.Now.Second % HBTime == 0)
                                {
                                    if (CallHeartBeat(SubCon)) ChkTimer = DateTime.Now;
                                    HBTimer = DateTime.Now;
                                }
                            }
                            else
                            {
                                if (HBTimer.AddSeconds(HBTime) < DateTime.Now)
                                {
                                    if (CallHeartBeat(SubCon)) ChkTimer = DateTime.Now;
                                    HBTimer = DateTime.Now;
                                }
                            } 
                        }
                        #endregion
                        #region 查询
                        if (RTime > 0 && Run)
                        {
                            if (IntegerPoint)
                            {
                                if (DateTime.Now.Second % RTime==0)
                                {
                                    if (CallInvt(SubCon)) ChkTimer = DateTime.Now;
                                    RunTimer = DateTime.Now;
                                }
                            }
                            else
                            {
                                if (RunTimer.AddSeconds(RTime) < DateTime.Now)
                                {
                                    if (CallInvt(SubCon)) ChkTimer = DateTime.Now;
                                    RunTimer = DateTime.Now;
                                }
                            } 
                        }
                        #endregion
                        #region 断开 或检测
                        if (!KeepAlive || string.IsNullOrEmpty(flags))
                        {
                            CloseConnect();
                            if (disconnect != null) disconnect(Session.Remote, Session.Connection, Session.LocalMode, Session.SessionID);
                            WriteLog("DisConnect", Log_Type.Infomation, Session.Remote);
                            Session.SessionID = 0;
                            Session.Remote = "";
                            Session.ListNO = "";
                        }
                        else if(ChkTimer.AddMinutes(3) < DateTime.Now && Run)
                        {
                            Pack.Clear();
                            CheckOnline();
                            ChkTimer = DateTime.Now;
                        }
                        #endregion
                    }
                    if (index + 1 < SubFlags.Count)
                        index++;
                    else
                        index = 0;
                    Exec = false;
                    Thread.Sleep(500);
                }
                #endregion
            }
            catch (Exception ex)
            {
                WriteLog(MethodName, Log_Type.Error, ex.Message);
                if (error != null) error(ClassName, MethodName, ex);
            }
            finally
            {
                DisposeLink();
            }
            buf = null;
            Pack.Clear();
            Run = false;
        }
         
        public byte[] IO(byte[] Data)
        {
            MethodName = "";
            try
            {
                MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            Random RListNO = new Random();
            List<byte> pack = new List<byte>();
            int SendTout = 0;
            if (Refer != null)
                SendTout = Refer.SendTimeout;
            int retry = 0;
            try
            {
                InitRS();
                Pause = true;
                retry = SendTout / 1000;
                while (Exec && Run)
                {
                    if (SendTout > 0)
                    {
                        if (retry < 0)
                            retry--;
                        else
                            Thread.Sleep(1000);
                    }
                }
                bool conned = false;
                if (!IsConnected())
                {
                    try
                    {
                        OpenConnect();
                        GetRemote();
                        if (connect != null)   connect(Session.Remote, Session.Connection, Session.LocalMode, Session.SessionID);
                    }
                    catch { }
                    conned = true;
                }
                if (CheckOnline())
                { 
                    if (Data != null)
                    {
                        WriteLog(MethodName, Log_Type.Test, SystemMessage.RefValDisp("send:", Data.ToHex( true), lan));
                        if (Write(Data))
                            WriteLog(MethodName, Log_Type.Infomation, SystemMessage.ExecOK(lan)); 
                        else
                            WriteLog(MethodName, Log_Type.Error, SystemMessage.ExecFail(lan)); 
                    }
                    byte[] recv = Read();
                    WriteLog(MethodName, Log_Type.Test, SystemMessage.RefValDisp("recv", recv.ToHex( true), lan));
                    pack.AddRange(recv);
                }
                if (conned)
                {
                    try
                    {
                        CloseConnect();
                        if (disconnect != null)   disconnect(Session.Remote, Session.Connection, Session.LocalMode, Session.SessionID); 
                    }
                    catch { }
                }
                Pause = false;
            }
            catch (Exception ex)
            {
                WriteLog(MethodName, Log_Type.Error, ex.Message);
                if (error != null) error(ClassName, MethodName, ex);
            }
            return pack.ToArray();
        }
         
        #endregion

    }
}
