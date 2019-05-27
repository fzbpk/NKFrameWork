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
using NK.Class;
using NK.Message;
using System.Threading;
using System.Net;
using System.Reflection;

namespace NK.Communicate
{
    internal class CommTHelper 
    {

        #region 定义
    
        public Socket NetRS { get; set; }
        public SerialPort UartRS { get; set; }
        public ReferSet Refer { get; set; }
        public CommunicateSession Session = new CommunicateSession();

        public int HBTime { get; set; }
        public int RTime { get; set; }
        public int FlagCount { get; set; }
        public int RegCount { get; set; }
        public int HBCount { get; set; }
        public int DataCount { get; set; }
        public int ChkTime { get; set; }
        public Language lan { get; set; }
        public bool Run { get; set; }

        public bool IntegerPoint { get; set; }

        public bool KeepAlive { get; set; }
        
        public string flags;
        public List<string> SubFlags;
        public Dictionary<string, object> session { get; set; }

        public CommEvent.HasErrorEven error;
        public NetEvent.LogEven log;
        public NetEvent.Connect connect;
        public NetEvent.DisConnect disconnect;
        public NetEvent.RequestInitEven RequestInit;
        public NetEvent.ResponseInitEven ResponseInit;
        public NetEvent.RequestDataEven RequestData;
        public NetEvent.ResponsetDataEven ResponsetData;
        public NetEvent.RequestHeartBeatEven RequestHeartBeat;
        public NetEvent.ResponeHeartBeatEven ResponeHeartBeat;
        public NetEvent.ReceiveDataEven ReceiveData;
        public NetEvent.RequestCMDEven RequestCMD;
        public NetEvent.ResponseCMDEven ResponseCMD;
        public NetEvent.RequestFlagsEven RequestFlags;
        public NetEvent.ResponseFlagsEven ResponseFlags;

        #endregion

        #region 内部变量
        protected bool m_disposed;
        protected bool Pause;
        protected bool Exec;
        protected int WaitTime;
        protected int ConnTOut;
        protected int ReadTout;
        public  IPEndPoint EndPoint;
        protected string ClassName = "";
        protected string MethodName = "";
        #endregion

        #region 子方法
        protected   void Dispose(bool disposing)
        {
            lock (this)
            {
                if (disposing && !m_disposed)
                {
                    try
                    {
                        Session = null;
                        if (session != null) session.Clear();
                        session = null;
                        if (SubFlags != null) SubFlags.Clear();
                        SubFlags = null;
                        Refer = null;
                        if (NetRS != null)
                            NetRS.Dispose();
                        NetRS = null;
                        if (UartRS != null)
                            UartRS.Dispose();
                        UartRS = null;
                    }
                    catch { }
                    GC.SuppressFinalize(this);
                    m_disposed = true; 
                }
            }
        }
        
        protected virtual void WriteLog(string functions, Log_Type Modes, string Messages)
        {
            if (log != null) log(ClassName, functions, Session.Remote, Session.Connection, Session.LocalMode, Modes, Messages);
        }

        protected virtual byte[] Read(int Index = 0, int Datalen = 0)
        { 
            int len = 0;
            int ReadTout = DLLConfig.ReceiveTimeout, size = DLLConfig.ReceiveBufferSize;
            if (Refer != null)
            {
                if (Refer.ReceiveTimeout > 0)
                    ReadTout = Refer.ReceiveTimeout;
                if (Refer.ReceiveBufferSize > 0)
                    size = Refer.ReceiveBufferSize;
            }
            byte[] buf = null;
            byte[] recv = null;
            List <byte> pack = new List<byte>();
            switch (Session.LocalMode)
            {
                case ReferForUse.UartSet:
                    if (UartRS == null)
                        return pack.ToArray();
                    try
                    {
                        buf = new byte[size];
                        if (Datalen > 0)
                        {
                            while (Datalen > 0 && Run)
                            {
                                len = UartRS.Read(buf, 0, buf.Length);
                                recv = new byte[len];
                                Array.Copy(buf, recv, len);
                                pack.AddRange(recv);
                                Datalen -= len;
                            }
                        }
                        else if (ReadTout > 0)
                        {
                            UartRS.ReadTimeout = 100;
                            len = 0;
                            if (len <= 0)
                            {
                                for (int i = 0; i <= (ReadTout / UartRS.ReadTimeout); i++)
                                {
                                    try
                                    { len = UartRS.Read(buf, 0, buf.Length); }
                                    catch
                                    { len = 0; }
                                    if (len > 0)
                                        break;
                                    else if (!Run)
                                        break;
                                }
                            }
                            while (len > 0 && Run)
                            {
                                recv = new byte[len];
                                Array.Copy(buf, recv, len);
                                pack.AddRange(recv);
                                buf = new byte[size];
                                try
                                { len = UartRS.Read(buf, 0, buf.Length); }
                                catch { len = 0; }
                            }
                            UartRS.ReadTimeout = ReadTout;
                        }
                        else
                        {
                            len = UartRS.Read(buf, 0, buf.Length);
                            recv = new byte[len];
                            Array.Copy(buf, recv, len);
                            pack.AddRange(recv);
                        }
                    }
                    catch (Exception ex)
                    {
                        Run = false;
                        if (error != null)
                            error(ClassName, "Read", ex);
                    }
                    break;
                case ReferForUse.NetSet:
                    if (NetRS == null)
                        return pack.ToArray();
                    try
                    {
                        buf = new byte[size];
                        if (Datalen > 0)
                        {
                            while (Datalen > 0 && Run)
                            {
                                len = NetRS.Receive(buf);
                                recv = new byte[len];
                                Array.Copy(buf, recv, len);
                                pack.AddRange(recv);
                                Datalen -= len;
                            }
                        }
                        else if (ReadTout > 0)
                        {
                            NetRS.ReceiveTimeout = 100;
                            len = 0;
                            if (len <= 0)
                            {
                                for (int i = 0; i <= (ReadTout / NetRS.ReceiveTimeout); i++)
                                {
                                    try
                                    { len = NetRS.Receive(buf); }
                                    catch
                                    { len = 0; }
                                    if (len > 0)
                                        break;
                                    else if (!Run)
                                        break;
                                }
                            }
                            while (len > 0 && Run)
                            {
                                recv = new byte[len];
                                Array.Copy(buf, recv, len);
                                pack.AddRange(recv);
                                buf = new byte[size];
                                try
                                { len = NetRS.Receive(buf); }
                                catch { len = 0; }
                            }
                            NetRS.ReceiveTimeout = ReadTout;
                        }
                        else
                        {
                            len = NetRS.Receive(buf);
                            recv = new byte[len];
                            Array.Copy(buf, recv, len);
                            pack.AddRange(recv);
                        }
                    }
                    catch (Exception ex)
                    {
                        Run = false;
                        if (error != null)
                            error(ClassName, "Read", ex);
                    }
                    break;
            }
            recv = null;
            buf = null;
            return pack.ToArray();
        }

        protected virtual bool Write(byte[] Data, int Index = 0)
        {
            if (Data == null)
                return false;
            else if (Data.Length == 0)
                return false;
            int Datalen = Data.Length, len = 0, n = 0;
            int size = DLLConfig.SendBufferSize;
            int WiteTOut = DLLConfig.SendTimeout;
            if (Refer != null)
            {
                if (Refer.SendBufferSize > 0)
                    size = Refer.SendBufferSize;
                if (Refer.SendTimeout > 0)
                    WiteTOut = Refer.SendTimeout;
            }
            switch (Session.LocalMode)
            {
                case ReferForUse.UartSet:
                    if (UartRS == null)
                        return false;
                    UartRS.WriteTimeout = WiteTOut;
                    UartRS.WriteBufferSize = size;
                    try
                    {
                        byte[] buf = null;
                        n = 0;
                        int index = 0;
                        while (Datalen > 0 && Run)
                        {
                            if (Datalen > size)
                                len = size;
                            else
                                len = Datalen;
                            buf = new byte[len];
                            Array.Copy(Data, index, buf, 0, len);
                            UartRS.Write(buf, 0, buf.Length);
                            Datalen -= len;
                            index += len;
                            n++;
                        }
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Run = false;
                        if (error != null)
                            error(ClassName, "Write", ex);
                        return false;
                    }
                case ReferForUse.NetSet:
                    if (NetRS == null)
                        return false;
                    NetRS.SendBufferSize = size;
                    NetRS.SendTimeout = WiteTOut;
                    try
                    {
                        byte[] buf = null;
                        n = 0;
                        int index = 0;
                        while (Datalen > 0 && Run)
                        {
                            if (Datalen > size)
                                len = size;
                            else
                                len = Datalen;
                            buf = new byte[len];
                            Array.Copy(Data, index, buf, 0, len);
                            len = NetRS.Send(buf);
                            Datalen -= len;
                            index += len;
                            n++;
                        }
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Run = false;
                        if (error != null)
                            error(ClassName, "Write", ex);
                        return false;
                    }
                default:
                    return false;
            }
        }

        protected virtual void CloseConnect()
        {
            if (Session == null) return;
            switch (Session.LocalMode)
            {
                case ReferForUse.NetSet:
                    if (NetRS != null)
                        NetRS.Disconnect(true);
                    break;
                case ReferForUse.UartSet:
                    if (UartRS != null)
                        UartRS.Close();
                    break;
            }
        }

        protected virtual void GetRemote()
        {
            if (Session == null) return;
            switch (Session.LocalMode)
            {
                case ReferForUse.NetSet:
                    if (NetRS != null)
                    {
                        Session.SessionID = NetRS.Handle.ToInt64();
                        Session.Remote = NetRS.RemoteEndPoint.ToString();
                        Session.ConnectTime = DateTime.Now;
                    }
                    break;
                case ReferForUse.UartSet:
                    if (UartRS != null)
                    {
                        Random RListNO = new Random();
                        Session.SessionID = RListNO.Next(1, int.MaxValue);
                        Session.Remote = UartRS.PortName; ;
                        Session.ConnectTime = DateTime.Now;
                    }
                    break;
            }
        }

        protected virtual int Available()
        {
            try
            {
                if (Session == null) throw new NullReferenceException();
                switch (Session.LocalMode)
                {
                    case ReferForUse.NetSet:
                        if (NetRS != null)
                            return NetRS.Available;
                        break;
                    case ReferForUse.UartSet:
                        if (UartRS != null)
                            return UartRS.BytesToRead;
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }
            catch { Run = false; }
            return 0;
        }

        protected virtual void DisposeLink()
        {
            if (Session == null) return;
            switch (Session.LocalMode)
            {
                case ReferForUse.NetSet:
                    if (NetRS != null)
                    {
                        if (NetRS.Connected)
                        {
                            if (disconnect != null) disconnect(Session.Connection, Session.Remote, Session.LocalMode, Session.SessionID);
                            try
                            { NetRS.Shutdown(SocketShutdown.Both); }
                            catch { }
                            try
                            { NetRS.Close(); }
                            catch { }
                        }
                        NetRS = null;
                    }
                    break;
                case ReferForUse.UartSet:
                    if (UartRS != null)
                    {
                        if (UartRS.IsOpen)
                        {
                            if (disconnect != null) disconnect(Session.Connection, Session.Remote, Session.LocalMode, Session.SessionID);
                            try
                            { UartRS.Close(); }
                            catch { }
                        }
                        UartRS = null;
                    }
                    break;
            }
        }

        protected virtual void StopLink()
        {
            DateTime DT = DateTime.Now;
            switch (Session.LocalMode)
            {
                case ReferForUse.NetSet:
                    while (NetRS != null)
                    {
                        if (NetRS.Connected)
                        {
                            if (DT.AddSeconds(ChkTime) < DateTime.Now)
                            {
                                try
                                { NetRS.Close(); }
                                catch { }
                            }
                        }
                        else
                            break;
                    }
                    break;
                case ReferForUse.UartSet:
                    while (UartRS != null)
                    {
                        if (UartRS.IsOpen)
                        {
                            if (DT.AddSeconds(ChkTime) < DateTime.Now)
                            {
                                try
                                { UartRS.Close(); }
                                catch { }
                            }
                        }
                        else
                            break;
                    }
                    break;
            }

        }

        public virtual bool IsConnected()
        {
            if (Session == null) return false ;
            else if (!Run) return false;
            switch (Session.LocalMode)
            {
                case ReferForUse.NetSet:
                    if (NetRS == null) return false;
                    else
                    {
                        try
                        {
                            if (NetRS.Poll(ReadTout==0?-1: ReadTout, SelectMode.SelectRead))
                                return NetRS.Available != 0;
                            else
                                return true;
                        }
                        catch { }
                        return false;
                    }
                case ReferForUse.UartSet:
                    if (UartRS == null) return false;
                    else return UartRS.IsOpen;
                default:
                    return false;
            }
        }
        protected virtual bool CheckOnline()
        {
            if (Session == null) return false ;
            int ReadTout = DLLConfig.ReceiveTimeout;
            if (Refer != null)
            {
                if (Refer.ReceiveTimeout > 0)
                    ReadTout = Refer.ReceiveTimeout;
            }
            if (ReadTout == 0)
                ReadTout = -1;
            switch (Session.LocalMode)
            {
                case ReferForUse.NetSet:
                    if (NetRS == null) return false;
                    else if (NetRS.Connected)
                    {
                        if (KeepAlive)
                        {
                            try
                            {
                                NetRS.Send(new byte[] { 0x00 });
                                return true;
                            }
                            catch
                            { return false; }
                        }
                        else
                        {
                            try
                            {
                                if (NetRS.Poll(ReadTout == 0 ? -1 :ReadTout, SelectMode.SelectRead) && NetRS.Available != 0)
                                { NetRS.Send(new byte[] { 0x00 }); }
                                return true;
                            }
                            catch { return false; }
                        } 
                    }
                    else return false;
                case ReferForUse.UartSet:
                    if (UartRS == null) return false;
                    else if (UartRS.IsOpen)
                    {
                        if (KeepAlive)
                        {
                            try
                            {
                                UartRS.Write(new byte[] { 0x00},0,1);
                                return true;
                            }
                            catch
                            { return false; }
                        }
                        else return true;
                    }
                    else
                        return false;
                default:
                    return false;
            }
        }
        public virtual void Stop()
        {
            Run = false;
            MethodName = "";
            try
            {
                MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            try
            {
                StopLink();
                Session.Connection = "";
                Session.LocalMode = ReferForUse.None;
                Session.SessionID = 0;
                Session.Remote = "";
                Session.ListNO = "";
                WriteLog(MethodName, Log_Type.Infomation, SystemMessage.ExecStop());
            }
            catch (Exception ex)
            {
                WriteLog(MethodName, Log_Type.Error, ex.Message);
                if (error != null) error(ClassName, MethodName, ex);
            }
        }
        protected virtual void OpenConnect()
        {
            if (Session == null) return;
            switch (Session.LocalMode)
            {
                case ReferForUse.NetSet:
                    if (NetRS != null)
                        NetRS.Connect(EndPoint);
                    break;
                case ReferForUse.UartSet:
                    if (UartRS != null)
                        UartRS.Open();
                    break;
            }
        }

        protected virtual void init()
        {
            if (RegCount <= 0)
                RegCount = 1;
            if (HBCount <= 0)
                HBCount = 1;
            if (DataCount <= 0)
                DataCount = 1;
            if (FlagCount <= 0)
                RegCount = 1;
            flags = "";
            if (SubFlags == null) SubFlags = new List<string>();
            WaitTime = DLLConfig.WaitTime;
            ConnTOut = DLLConfig.ConnTOut;
            ReadTout = DLLConfig.ReceiveTimeout;
            Pause = false;
            Exec = false;
            Run = true;
            if (Refer != null)
            {
                if (Refer.ConnectTimeOut > 0)
                    ConnTOut = Refer.ConnectTimeOut;
                if (Refer.WaitTime > 0)
                    WaitTime = Refer.WaitTime;
                if (Refer.ReceiveTimeout > 0)
                    ReadTout = Refer.ReceiveTimeout;
            }
        }

        #endregion

        #region 业务方法
 
        protected virtual bool RecvSer(byte[] Data, ref string Flag, ref List<string> Subcon)
        {
            MethodName = "";
            try
            {
                MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            int Step = 0;
            List<byte> Pack = new List<byte>();
            if (Data != null)
                Pack.AddRange(Data);
            try
            {
                byte[] buf = null;
                byte[] send = null;
                do
                {
                    buf = Pack.ToArray();
                    Pack.Clear();
                    if(buf.Length !=0) send = ResponseFlags(Session, ref Flag, ref Subcon, Step, ref buf);
                    if (buf != null) Pack.AddRange(buf);
                    if (send != null)
                    {
                        WriteLog(MethodName, Log_Type.Test, SystemMessage.RefValDisp("Send ResponseFlags:", send.ToHex(true), lan));
                        if (Write(send))
                            WriteLog(MethodName, Log_Type.Infomation, SystemMessage.ExecOK(lan));
                        else
                            WriteLog(MethodName, Log_Type.Error, SystemMessage.ExecFail(lan));
                        send = null;
                        buf = Read();
                        if (buf.Length > 0)
                        {
                            WriteLog(MethodName, Log_Type.Test, SystemMessage.RefValDisp("Recv ResponseFlags:", buf.ToHex(true), lan));
                            Pack.AddRange(buf);
                        }
                        Step++;
                    }
                }
                while (send != null && Run);
            }
            catch (Exception ex)
            {
                if (error != null)
                    error(ClassName, MethodName, ex);
            }
            if (Subcon == null)
                Subcon = new List<string>();
            if (Subcon.Count <= 0)
                Subcon.Add("");
            Pack.Clear();
            return !string.IsNullOrEmpty(Flag);
        }

        protected virtual bool RegSer(ref string Flag, ref List<string> Subcon)
        {
            MethodName = "";
            try
            {
                MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            int Step = 0;
            List<byte> Pack = new List<byte>();
            try
            {
                for (Step = 1; Step <= FlagCount; Step++)
                {
                    if (RequestFlags != null)
                    {
                        byte[] send = RequestFlags(Session, ref Flag, ref Subcon, Step);
                        if (send != null)
                        {
                            WriteLog(MethodName, Log_Type.Test, SystemMessage.RefValDisp("RequestFlags", send.ToHex(true), lan));
                            Write(send);
                        }
                    }
                    Thread.Sleep(WaitTime);
                    if (ResponseFlags != null)
                    {
                        byte[] recv = Read();
                        if (recv != null)
                            Pack.AddRange(recv);
                        recv = Pack.ToArray();
                        byte[] send = ResponseFlags(Session, ref Flag, ref Subcon, Step, ref recv);
                        if (!string.IsNullOrEmpty(Flag))
                            WriteLog(MethodName, Log_Type.Infomation, SystemMessage.RefValDisp("ResponseFlags", flags, lan));
                        if (recv != null)
                            Pack.AddRange(recv);
                        if (send != null)
                        {
                            WriteLog(MethodName, Log_Type.Test, SystemMessage.RefValDisp("Send ResponseFlags:", send.ToHex(true), lan));
                            Write(send);
                        }
                    }
                    if (!Run) break;
                }
            }
            catch (Exception ex)
            {
                if (error != null)
                    error(ClassName, MethodName, ex);
            }
            if (Subcon == null)
                Subcon = new List<string>();
            if (Subcon.Count <= 0)
                Subcon.Add("");
            return !string.IsNullOrEmpty(Flag);
        }

        protected virtual void RecviceData(ref List<byte> Pack)
        {
            byte[] send = null;
            byte[] buf = null;
            MethodName = "";
            try
            {
                MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            try
            { 
                if (!string.IsNullOrEmpty(flags) && Run && ReceiveData != null)
                {
                    do
                    {
                        buf = Pack.ToArray();
                        Pack.Clear();
                        send = ReceiveData(Session, flags, ref buf);
                        if (buf != null)
                            Pack.AddRange(buf);
                        if (send != null)
                        {
                            WriteLog(MethodName, Log_Type.Test, SystemMessage.RefValDisp("Send Data:", send.ToHex(true), lan));
                            if (Write(send))
                                WriteLog(MethodName, Log_Type.Infomation, SystemMessage.ExecOK(lan));
                            else
                                WriteLog(MethodName, Log_Type.Error, SystemMessage.ExecFail(lan));
                            buf = Read();
                            if (buf.Length > 0)
                            {
                                WriteLog(MethodName, Log_Type.Test, SystemMessage.RefValDisp("Recv Data:", buf.ToHex(true), lan));
                                Pack.AddRange(buf);
                            }
                            send = null;
                        }
                    }
                    while (send != null && Run);
                }
            }
            catch (Exception evex)
            {
                WriteLog(MethodName, Log_Type.Error, evex.Message);
                if (error != null) error(ClassName, MethodName , evex);
            }
        }

        protected virtual void CMD()
        {
            int Step = 0;
            byte[] send = null;
            byte[] buf = null;
            bool RetVal = false;
            MethodName = "";
            try
            {
                MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            try
            { 
                if (!string.IsNullOrEmpty(flags) &&Run && RequestCMD!=null)
                {
                    do
                    { 
                        send = RequestCMD(Session, flags,Step,out RetVal); 
                        if (send != null)
                        {
                            WriteLog(MethodName, Log_Type.Test, SystemMessage.RefValDisp("Send Data:", send.ToHex(true), lan));
                            if (Write(send))
                                WriteLog(MethodName, Log_Type.Infomation, SystemMessage.ExecOK(lan));
                            else
                                WriteLog(MethodName, Log_Type.Error, SystemMessage.ExecFail(lan));
                            if (RetVal && ResponseCMD != null)
                            { 
                                buf = Read();
                                if(buf.Length>0)
                                    WriteLog(MethodName, Log_Type.Test, SystemMessage.RefValDisp("Recv Data:", buf.ToHex(true), lan));
                                if (ResponseCMD(Session, flags, Step, buf))
                                    Step = 0;
                                else
                                    Step++;
                            } 
                        }
                    }
                    while (send != null && Run);
                }
            }
            catch (Exception evex)
            {
                WriteLog(MethodName, Log_Type.Error, evex.Message);
                if (error != null) error(ClassName, MethodName, evex);
            } 
        }

        protected virtual void CallInit()
        {
            int Step = 0;
            byte[] send = null;
            byte[] recv = null;
            MethodName = "";
            try
            {
                MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            try
            {
                if (!string.IsNullOrEmpty(flags) && Run &&(RequestInit!=null || ResponseInit!=null)  )
                {
                    foreach (string subcon in SubFlags)
                    {
                        for (Step = 1; Step <= RegCount; Step++)
                        {
                            if (RequestInit != null)
                            {
                                try
                                {
                                    send = RequestInit(Session, flags, subcon, Step);
                                    if (send != null)
                                    {
                                        WriteLog(MethodName, Log_Type.Test, SystemMessage.RefValDisp("Send Data", send.ToHex(true), lan));
                                        if (Write(send))
                                            WriteLog(MethodName, Log_Type.Infomation, SystemMessage.ExecOK(lan)+"sss");
                                        else
                                            WriteLog(MethodName, Log_Type.Error, SystemMessage.ExecFail(lan)); 
                                    }
                                }
                                catch (Exception evex)
                                {
                                    WriteLog(MethodName, Log_Type.Error, evex.Message);
                                    if (error != null) error(ClassName, MethodName, evex);
                                }
                                send = null;
                            }
                            Thread.Sleep(WaitTime);
                            if (ResponseInit != null)
                            {
                                try
                                {
                                    recv = Read();
                                    WriteLog(MethodName, Log_Type.Test, SystemMessage.RefValDisp("Recv Data", recv.ToHex(true), lan));
                                    ResponseInit(Session, flags, subcon, Step, recv);
                                }
                                catch (Exception evex)
                                {
                                    WriteLog(MethodName, Log_Type.Error, evex.Message);
                                    if (error != null) error(ClassName, MethodName, evex);
                                } 
                                recv = null;
                            }
                            if (!Run) break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog(MethodName, Log_Type.Error, ex.Message);
                if (error != null) error(ClassName, MethodName, ex);
            } 
        }

        protected virtual bool  CallHeartBeat(string SubCon)
        {
            bool res = false;
            int Step = 0;
            byte[] send = null;
            byte[] recv = null;
            List<byte> Pack = new List<byte>();
            MethodName = "";
            try
            {
                MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            try
            {
                if (!string.IsNullOrEmpty(flags) && Run && (RequestHeartBeat!=null || ResponeHeartBeat!=null))
                {
                    for (Step = 1; Step <= HBCount; Step++)
                    {
                        if (RequestHeartBeat != null)
                        {
                            try
                            {
                                send = RequestHeartBeat(Session, flags, SubCon, Step);
                                WriteLog(MethodName, Log_Type.Test, SystemMessage.RefValDisp("Send Data", send.ToHex(true), lan));
                                if (send != null)
                                {
                                    if (Write(send))
                                    {
                                        WriteLog(MethodName, Log_Type.Infomation, SystemMessage.ExecOK(lan));
                                        res = true;
                                    }
                                    else
                                    { WriteLog(MethodName, Log_Type.Error, SystemMessage.ExecFail(lan)); }
                                    send = null;
                                }
                            }
                            catch (Exception evex)
                            {
                                WriteLog(MethodName, Log_Type.Error, evex.Message);
                                if (error != null) error(ClassName, MethodName, evex);
                            }
                        }
                        Thread.Sleep(WaitTime);
                        if (ResponeHeartBeat != null)
                        {
                            recv = Read();
                            if (recv.Length > 0)
                            {
                                Pack.AddRange(recv);
                                res = true;
                            }
                            WriteLog(MethodName, Log_Type.Test, SystemMessage.RefValDisp("Recv Data", recv.ToHex(true), lan));
                            try
                            {
                                send = null;
                                do
                                {
                                    recv = Pack.ToArray();
                                    Pack.Clear();
                                    send = ResponeHeartBeat(Session, flags, SubCon, Step, ref recv);
                                    if (recv != null)
                                        Pack.AddRange(recv);
                                    if (send != null)
                                    {
                                        WriteLog(MethodName, Log_Type.Test, SystemMessage.RefValDisp("SendResp Data", send.ToHex(true), lan));
                                        if (Write(send))
                                            WriteLog(MethodName, Log_Type.Infomation, SystemMessage.ExecOK(lan));
                                        else
                                            WriteLog(MethodName, Log_Type.Error, SystemMessage.ExecFail(lan));
                                        recv = Read();
                                        if (recv.Length > 0)
                                        {
                                            Pack.AddRange(recv);
                                            WriteLog(MethodName, Log_Type.Test, SystemMessage.RefValDisp("RecvResp Data", recv.ToHex(true), lan));
                                            res = true;
                                        }
                                    }
                                }
                                while (send != null && Run);
                            }
                            catch (Exception evex)
                            {
                                WriteLog(MethodName, Log_Type.Error, evex.Message);
                                if (error != null) error(ClassName, MethodName, evex);
                            }
                        } 
                        if (!Run) break;
                    }
                }
              
            }
            catch (Exception evex)
            {
                WriteLog(MethodName, Log_Type.Error, evex.Message);
                if (error != null) error(ClassName, MethodName, evex);
            }
            return res;
        }

        protected virtual bool CallInvt(string SubCon)
        {
            bool res = false;
            int Step = 0;
            byte[] send = null;
            byte[] recv = null;
            List<byte> Pack = new List<byte>();
            MethodName = "";
            try
            {
                MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            try
            {
                if (!string.IsNullOrEmpty(flags) && Run && (RequestData!=null || ResponsetData!=null) )
                { 
                    for (Step = 1; Step <= DataCount; Step++)
                    {
                        if (RequestData != null)
                        {
                            try
                            {
                               send = RequestData(Session, flags, SubCon, Step);
                                WriteLog(MethodName, Log_Type.Test, SystemMessage.RefValDisp("Send Data", send.ToHex(true), lan));
                                if (send != null)
                                {
                                    if (Write(send))
                                    {
                                        WriteLog(MethodName, Log_Type.Infomation, SystemMessage.ExecOK(lan));
                                        res = true;
                                    }
                                    else
                                    { WriteLog(MethodName, Log_Type.Error, SystemMessage.ExecFail(lan)); }
                                }
                            }
                            catch (Exception evex)
                            {
                                WriteLog(MethodName, Log_Type.Error, evex.Message);
                                if (error != null) error(ClassName, MethodName, evex);
                            }
                        }
                        Thread.Sleep(WaitTime);
                        if (ResponsetData != null)
                        {
                            recv = Read();
                            if (recv.Length != 0)
                            {
                                Pack.AddRange(recv);
                                res = true;
                            }
                            WriteLog(MethodName, Log_Type.Test, SystemMessage.RefValDisp("Recv Data", recv.ToHex(true), lan)); 
                            try
                            {
                                send = null;
                                do
                                {
                                    recv = Pack.ToArray();
                                    Pack.Clear();
                                    send = ResponsetData(Session, flags, SubCon, Step, ref recv);
                                    if (recv != null)
                                        Pack.AddRange(recv);
                                    if (send != null)
                                    {
                                        WriteLog(MethodName, Log_Type.Test, SystemMessage.RefValDisp("SendResp Data", send.ToHex(true), lan));
                                        if (Write(send))
                                            WriteLog(MethodName, Log_Type.Infomation, SystemMessage.ExecOK(lan));
                                        else
                                            WriteLog(MethodName, Log_Type.Error, SystemMessage.ExecFail(lan));
                                        recv = Read();
                                        if (recv.Length > 0)
                                        {
                                            Pack.AddRange(recv);
                                            WriteLog(MethodName, Log_Type.Test, SystemMessage.RefValDisp("RecvResp Data", recv.ToHex(true), lan));
                                            res = true;
                                        }
                                    }
                                }
                                while (send != null && Run);
                            }
                            catch (Exception evex)
                            {
                                WriteLog(MethodName, Log_Type.Error, evex.Message);
                                if (error != null) error(ClassName, MethodName, evex);
                            }
                        }
                        if (!Run) break;
                    }
                }

            }
            catch (Exception evex)
            {
                WriteLog(MethodName, Log_Type.Error, evex.Message);
                if (error != null) error(ClassName, MethodName, evex);
            } 
            return res;
        }

        #endregion

    }
}
