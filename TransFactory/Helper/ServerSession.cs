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
    internal class ServerSession:CommTHelper,IDisposable
    {

        #region 构造

        public ServerSession()
        {

            NetRS = null;
            UartRS = null;
            Refer = null;
            lan = Language.Chinese;

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

        ~ServerSession()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        #endregion
        
        #region 公共

        public void Start()
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
                    if (NetRS == null) return;
                    break;
                case ReferForUse.UartSet:
                    if (UartRS == null) return;
                    break;
                default:
                    return;
            }
            DateTime HBTimer = DateTime.Now;
            DateTime RunTimer = DateTime.Now;
            DateTime ChkTimer = DateTime.Now;
            System.Random RListNO = new System.Random();
            int index = 0;
            string SubCon = ""; 
            List<byte> Pack = new List<byte>();  
            byte[] buf = null;
            init();
            try
            {
               
                GetRemote();
                WriteLog( "Connect", Log_Type.Infomation, Session.Remote );
                if (connect != null)  connect(Session.Connection, Session.Remote , Session.LocalMode, Session.SessionID);
                Session.ListNO = DateTime.Now.ToString("yyyyMMddHHmmssms") + RListNO.Next(1000, 9999).ToString();
                #region RegEVENT
                WriteLog ( "RegEVENT", Log_Type.Infomation, SystemMessage.Check("Socket", lan)); 
                try
                { 
                    //Thread.Sleep(ConnTOut);

                    buf = Read(); ;
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
                    Pack.Clear();
                    if (string.IsNullOrEmpty(flags) && Run)
                       RegSer(ref flags, ref SubFlags);
                    CallInit();
                }
                catch (Exception ex)
                {
                    WriteLog( MethodName, Log_Type.Error, "Reg:" + ex.Message);
                    Thread.Sleep(ConnTOut);
                    Session.SessionID = 0;
                    Session.Remote = "";
                    flags = "";
                }
                #endregion
                #region 主循环
                HBTimer = DateTime.Now;
                RunTimer = DateTime.Now;
                if (SubFlags.Count <= 0)
                    SubFlags.Add("");
                Pack.Clear();
                WriteLog("Main", Log_Type.Infomation, SystemMessage.ExecStart(lan));
                while (Run)
                {
                    if (Pause) continue;
                    Exec = true;
                    Session.ListNO = DateTime.Now.ToString("yyyyMMddHHmmssms") + RListNO.Next(1000, 9999).ToString();
                    SubCon = SubFlags[index];
                    if (string.IsNullOrEmpty(flags) && Run)
                    {
                        RegSer(ref flags, ref SubFlags);
                        if (!string.IsNullOrEmpty(flags) && Run)
                        { CallInit(); }
                    }
                    #region 接收
                    if (Available() > 0 && Run)
                    {
                        byte[] recv = Read();
                        if (recv.Length > 0)
                        {
                            WriteLog("ReceiveData", Log_Type.Test, SystemMessage.RefValDisp("Available Data:", recv.ToHex(true), lan));
                            Pack.AddRange(recv); 
                            recv = Pack.ToArray();
                            if (string.IsNullOrEmpty(flags))
                            {
                                RecvSer(recv, ref flags, ref SubFlags);
                                if (!string.IsNullOrEmpty(flags))
                                    WriteLog("RegSer", Log_Type.Infomation, SystemMessage.RefValDisp("Flags", flags, lan));
                                else
                                    RegSer(ref flags, ref SubFlags);
                                if (SubFlags == null) SubFlags = new List<string>();
                                if (SubFlags.Count <= 0) SubFlags.Add("");
                                if (!string.IsNullOrEmpty(flags))
                                {
                                    RecviceData(ref Pack);
                                    CMD();
                                    CallInit();
                                }
                            }
                            else
                            {
                                RecviceData(ref Pack);
                                CMD();
                            }
                            ChkTimer = new DateTime();
                        }
                    }
                    #endregion
                    DateTime DT = DateTime.Now;
                    #region 心跳
                    if (HBTime > 0 && Run)
                    {
                        if (IntegerPoint)
                        {
                            if (RTime <= 60 && (DT.Second % RTime) == 0)
                            {
                                if (CallHeartBeat(SubCon)) ChkTimer = DateTime.Now;
                                HBTimer = DateTime.Now;
                            }
                            else if (RTime > 60 && (RTime <= 60 * 60) && ((DT.Minute * 60 + DT.Second) % RTime) == 0)
                            {
                                if (CallHeartBeat(SubCon)) ChkTimer = DateTime.Now;
                                HBTimer = DateTime.Now;

                            }
                            else if (RTime > 60 * 60 && ((DT.Hour * 60 + DT.Minute * 60 + DT.Second) % RTime) == 0)
                            {
                                if (CallHeartBeat(SubCon)) ChkTimer = DateTime.Now;
                                HBTimer = DateTime.Now;
                            }
                            
                        }
                        else
                        {
                            if (HBTimer.AddSeconds(HBTime) < DateTime.Now)
                            {
                                if (CallHeartBeat(SubCon))
                                    ChkTimer = DateTime.Now;
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
                            if (RTime <= 60 && (DT.Second % RTime) == 0)
                            {
                                if (CallInvt(SubCon)) ChkTimer = DateTime.Now;
                                RunTimer = DateTime.Now;
                            }
                            else if (RTime > 60 && (RTime <= 60 * 60) && ((DT.Minute * 60 + DT.Second) % RTime) == 0  )
                            {
                                if (CallInvt(SubCon)) ChkTimer = DateTime.Now;
                                RunTimer = DateTime.Now;

                            }
                            else if (RTime > 60 * 60 && ((DT.Hour * 60+ DT.Minute * 60 + DT.Second) % RTime) == 0   )
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
                    #region 检查 
                    if (ChkTimer.AddMinutes(ChkTime) < DateTime.Now && Run)
                    {
                        Pack.Clear();
                        if (string.IsNullOrEmpty(flags))
                            Run = false;
                        else
                            CheckOnline();
                        ChkTimer = DateTime.Now;
                    }
                    #endregion
                    if (index + 1 < SubFlags.Count)
                        index++;
                    else
                        index = 0;
                    Exec = false;
                    Thread.Sleep(500);
                }
                #endregion
                WriteLog("DisConnect", Log_Type.Infomation, Session.Remote);
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
            List<byte> pack = new List<byte>();
            switch (Session.LocalMode)
            {
                case ReferForUse.NetSet:
                    if (NetRS == null) return pack.ToArray();
                    break;
                case ReferForUse.UartSet:
                    if (UartRS == null) return pack.ToArray();
                    break;
                default:
                    return pack.ToArray();
            }
            int SendTout = 0;
            if (Refer != null)
                SendTout = Refer.SendTimeout;
            int retry = 0;
            try
            {
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
                if (CheckOnline())
                {
                    if (Data != null)
                    {
                        WriteLog(MethodName, Log_Type.Test, SystemMessage.RefValDisp("send:", Data.ToHex(true), lan));
                        if (Write(Data))
                            WriteLog(MethodName, Log_Type.Infomation, SystemMessage.ExecOK(lan));
                        else
                            WriteLog(MethodName, Log_Type.Error, SystemMessage.ExecFail(lan));
                    }
                    byte[] recv = Read();
                    WriteLog(MethodName, Log_Type.Test, SystemMessage.RefValDisp("recv", recv.ToHex(true), lan));
                    pack.AddRange(recv);
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
