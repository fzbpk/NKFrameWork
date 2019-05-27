using NK.Entity;
using NK.ENum;
using NK.Class;
using System; 
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
namespace NK.Event
{
    /// <summary>
    /// 网络事件
    /// </summary>
    public class NetEvent
    {
        /// <summary>
        /// 日志
        /// </summary> 
        /// <param name="Class">类名</param>
        /// <param name="Func">方法</param> 
        /// <param name="Remote">远端连接</param>
        /// <param name="Connection">本地连接</param>
        /// <param name="LocalMode">连接模式</param>
        /// <param name="flag"></param>
        /// <param name="Message"></param>
        public delegate void LogEven(string Class, string Func,string Remote, string Connection ,ReferForUse LocalMode, Log_Type flag, string Message);
        /// <summary>
        /// 连接事件
        /// </summary>
        /// <param name="Local">本地信息</param> 
        /// <param name="Remote">远端信息</param> 
        /// <param name="Imode">连接模式</param> 
        /// <param name="SessionID">会话ID</param> 
        public delegate void Connect(string Local,string Remote,ReferForUse Imode,long SessionID);
        /// <summary>
        /// 断开连接事件，无论主动还是被动断开也会触发
        /// </summary>
        /// <param name="Local">本地信息</param> 
        /// <param name="Remote">远端信息</param> 
        /// <param name="Imode">连接模式</param> 
        /// /// <param name="SessionID">会话ID</param> 
        public delegate void DisConnect(string Local, string Remote, ReferForUse Imode, long SessionID);
        /// <summary>
        /// 请求获取FLAGS
        /// </summary>
        /// <param name="Session">通信信息</param> 
        /// <param name="Flags">标识</param>
        /// <param name="SubConnection">子连接</param>
        /// <param name="Step">步骤</param> 
        /// <returns></returns>
        public delegate byte[] RequestFlagsEven(CommunicateSession Session,ref string Flags, ref List<string> SubConnection, int Step );
        /// <summary>
        /// 返回Flags
        /// </summary>
        /// <param name="Session">通信信息</param>
        /// <param name="Flags">标识</param>
        /// <param name="SubConnection">子连接</param>
        /// <param name="Step">步骤</param> 
        /// <param name="Data">接收信息</param> 
        /// <returns>确认指令</returns>
        public delegate byte[] ResponseFlagsEven(CommunicateSession Session, ref string Flags,ref List<string> SubConnection, int Step ,ref byte[] Data );
        /// <summary>
        /// 请求注册信息事件
        /// </summary>
        /// <param name="Session">通信信息</param>
        /// <param name="Flags">标识</param>
        /// <param name="SubConnection">子连接</param>
        /// <param name="Step">步骤</param> 
        /// <returns>处理结果</returns>
        public delegate byte[] RequestInitEven(CommunicateSession Session, string Flags,string SubConnection, int Step );
        /// <summary>
        /// 返回注册信息
        /// </summary>
        /// <param name="Session">通信信息</param>
        /// <param name="Flags">标识</param>
        /// <param name="SubConnection">子连接</param>
        /// <param name="Step">步骤</param>
        /// <param name="Data">接收信息</param>
        /// <returns>标识字段</returns>
        public delegate void  ResponseInitEven(CommunicateSession Session, string Flags, string SubConnection, int Step,  byte[] Data );
        /// <summary>
        /// 请求心跳数据
        /// </summary>
        /// <param name="Session">通信信息</param>
        /// <param name="Flags">标识</param>
        /// <param name="SubConnection">子连接</param>
        /// <param name="Step">步骤</param> 
        /// <returns>心跳查询数据</returns>
        public delegate byte[] RequestHeartBeatEven(CommunicateSession Session, string Flags,string SubConnection, int Step );
        /// <summary>
        /// 返回心跳数据
        /// </summary>
        /// <param name="Session">通信信息</param>
        /// <param name="Flags">标识</param>
        /// <param name="SubConnection">子连接</param>
        /// <param name="Step">步骤</param> 
        /// <param name="Data">心跳信息</param>
        /// <returns>确认</returns>
        public delegate byte[] ResponeHeartBeatEven(CommunicateSession Session, string Flags, string SubConnection, int Step ,ref byte[] Data);
        /// <summary>
        /// 请求数据传输
        /// </summary>
        /// <param name="Session">通信信息</param>
        /// <param name="Flags">标识</param>
        /// <param name="SubConnection">子连接</param>
        /// <param name="Step">步骤</param> 
        /// <returns>发送数据</returns>
        public delegate byte[] RequestDataEven(CommunicateSession Session, string Flags, string SubConnection, int Step );
        /// <summary>
        /// 返回数据传输
        /// </summary>
        /// <param name="Session">通信信息</param>
        /// <param name="Flags">标识</param>
        /// <param name="SubConnection">子连接</param>
        /// <param name="Step">步骤</param> 
        /// <param name="Data">接收数据</param>
        /// <returns>确认</returns>
        public delegate byte[] ResponsetDataEven(CommunicateSession Session, string Flags, string SubConnection, int Step,  ref byte[] Data);
        /// <summary>
        /// 接收到数据
        /// </summary>
        /// <param name="Session">通信信息</param>
        /// <param name="Flags">标识</param> 
        /// <param name="Data">接收数据,返回包尾</param>
        /// <returns>返回数据</returns>
        public delegate byte[] ReceiveDataEven(CommunicateSession Session, string Flags, ref byte[] Data);
        /// <summary>
        /// 发送命令-
        /// </summary>
        /// <param name="Session"></param>
        /// <param name="Flags"></param>
        /// <param name="Step"></param>
        /// <param name="ReturnVal"></param>
        /// <returns></returns>
        public delegate byte[] RequestCMDEven(CommunicateSession Session, string Flags, int Step, out bool ReturnVal);
        /// <summary>
        /// 返回命令
        /// </summary>
        /// <param name="Session"></param>
        /// <param name="Flags"></param>
        /// <param name="Step"></param>
        /// <param name="Data"></param>
        /// <returns></returns>
        public delegate bool ResponseCMDEven(CommunicateSession Session, string Flags, int Step,   byte[] Data) ;
        /// <summary>
        /// 错误显示
        /// </summary>
        /// <param name="Session">通信信息</param>
        /// <param name="ex">错误信息</param> 
        public delegate bool RequestErrorEven(HttpListenerSession Session, Exception ex);
        /// <summary>
        /// GET事件
        /// </summary>
        /// <param name="Session">通信信息</param> 
        /// <param name="UrlPram">URL参数</param> 
        /// <param name="RawData">原始数据</param> 
        public delegate bool RequestGETEven(HttpListenerSession Session, Dictionary<string, object> UrlPram,string RawData );
        /// <summary>
        /// POST事件
        /// </summary>
        /// <param name="Session">通信信息</param> 
        /// <param name="UrlPram">URL参数</param>
        /// <param name="PostData">POST参数</param> 
        ///  <param name="RawData">原始数据</param> 
        public delegate bool RequestPOSTEven(HttpListenerSession Session, Dictionary<string, object> UrlPram, Dictionary<string, object> PostData, string RawData);
        /// <summary>
        /// PUT事件
        /// </summary>
        ///  <param name="Session">通信信息</param>  
        public delegate bool RequestPUTEven(HttpListenerSession Session );
        /// <summary>
        /// DELETE事件
        /// </summary>
        ///  <param name="Session">通信信息</param>  
        public delegate bool RequestDELETEEven(HttpListenerSession Session);
        /// <summary>
        /// FTP验证目录事件
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Session"></param>
        /// <param name="Curdir"></param>
        /// <param name="args"></param>
        /// <param name="file"></param>
        /// <param name="newCurdir"></param>
        /// <returns></returns>
        public delegate bool IsValidDirEven(string UserName, long Session, string Curdir, string args, out string file, out string newCurdir);
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Password"></param>
        /// <param name="Session"></param>
        /// <returns></returns>
        public delegate bool FTPAuthEven(ref string UserName, string Password, long Session);
        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Session"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public delegate string RequestMKDEven(string UserName, long Session, string file);
        /// <summary>
        /// RNTO
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Session"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public delegate string RequestRNTOEven(string UserName, long Session, string file);
        /// <summary>
        /// 续传
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Session"></param>
        /// <param name="SOCK"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public delegate string RequestAPPEEven(string UserName, long Session, Socket SOCK, string file);
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Session"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public delegate string RequsetDELEEven(string UserName, long Session, string file);
        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Session"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public delegate string RequestRMDEven(string UserName, long Session, string file);
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Session"></param>
        /// <param name="SOCK"></param>
        /// <param name="file"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public delegate string RequestSTOREven(string UserName, long Session, Socket SOCK, string file,int offset);
        /// <summary>
        /// 下载文件，带续传
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Session"></param>
        /// <param name="SOCK"></param>
        /// <param name="file"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public delegate string RequestRETREven(string UserName, long Session, Socket SOCK, string file, int offset);
        /// <summary>
        /// 获取根目录
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Session"></param>
        /// <returns></returns>
        public delegate string RequestPWDEven(string UserName, long Session);
        /// <summary>
        /// 列出文件信息
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Session"></param>
        /// <param name="cdir"></param>
        /// <returns></returns>
        public delegate string RequestLISTEven(string UserName, long Session, string cdir);
        /// <summary>
        /// 列出文件大小
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Session"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public delegate int RequestSIZEEven(string UserName, long Session, string file);
        /// <summary>
        /// 列出文件名
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Session"></param>
        /// <param name="cdir"></param>
        /// <returns></returns>
        public delegate string RequsetNLSTEven(string UserName, long Session, string cdir);
    }

}
