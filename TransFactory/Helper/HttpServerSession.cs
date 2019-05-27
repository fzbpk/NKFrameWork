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
    internal  class HttpServerSession
    {
        #region 定义
        public HttpListenerContext context { get; set; }
        private HttpListenerSession Session;
        private string ClassName;
        private string MethodName;
        private bool m_disposed;
        #endregion

        #region 构造
        public HttpServerSession()
        {
            ClassName = this.GetType().Name;
            Session = new HttpListenerSession();
        }

        ~HttpServerSession()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            lock (this)
            {
                if (disposing && !m_disposed)
                {
                    try
                    {
                        Session = null;
                        if (context != null) 
                          context = null;
                    }
                    catch { }
                    GC.SuppressFinalize(this);
                    m_disposed = true;
                }
            }
        }


        #endregion
          
        #region 属性
        public string Connection { get; set; }
        public string RemoteConnect { get; set; }
        public bool Debug { get; set; }
        public bool Option { get; set; }
        public int Buffsize { get; set; }
        public Language lan { get; set; } 
        public bool Access_Control_Allow {get;set;}
        public Dictionary<string,string> Header { get; set; }

        #endregion

        #region 事件

        public event CommEvent.HasErrorEven haserr;
        public event NetEvent.LogEven log;
        public event NetEvent.Connect connect;
        public event NetEvent.DisConnect disconnect;
        public event NetEvent.RequestGETEven getEvent;
        public event NetEvent.RequestPOSTEven postEvent;
        public event NetEvent.RequestPUTEven putEvent;
        public event NetEvent.RequestDELETEEven delEvent;
        public event NetEvent.RequestErrorEven error;

        #endregion

        #region 子方法

        private int CompareBytes(byte[] source, byte[] comparison, int sourceindex = 0)
        {
            int index = sourceindex;
            int count = 0;
            if (source.Length <= 0)
                return -1;
            if (source.Length < comparison.Length)
                return -1;
            if (0 >= comparison.Length)
                return -1;
            if (sourceindex >= comparison.Length)
                return -1;
            List<byte> pack = new List<byte>();
            pack.AddRange(source);
            byte key = source[0];
            do
            {
                index = pack.IndexOf(key, index);
                if (index != -1)
                {
                    count = 0;
                    for (int i = 0; i < comparison.Length; i++)
                    {
                        if (pack[index + i] != comparison[i])
                            break;
                        else
                            count++;
                    }
                    if (count > 0 && count == comparison.Length)
                        return index;
                    else if (index + 1 >= pack.Count)
                        return -1;
                    else
                        index++;
                }
            }
            while (index != -1);
            return -1;
        }
        private void WriteLog(string functions, Log_Type Modes, string Messages)
        {
            if (log != null) log(ClassName, functions, RemoteConnect, Connection, ReferForUse.NetSet, Modes, Messages);
        }
         
        #endregion

        public void Start()
        {
            ClassName = this.GetType().Name;
            MethodName = "";
            try
            {
                MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            try
            {
                System.Random Random = new System.Random();
                Session = new HttpListenerSession();
                if (context != null)
                {
                    bool res = false;
                    HttpListenerRequest request = context.Request;
                    HttpListenerResponse response = context.Response;
                    RemoteConnect = request == null ? "" : request.RemoteEndPoint.ToString();
                    Session.SessionID = Random.Next(1, int.MaxValue);
                    Session.Connection = Connection;
                    Session.Request = request;
                    Session.Response = response;
                    Session.Remote = RemoteConnect;
                    Session.ListNO = DateTime.Now.ToString("yyyyMMddHHmmssms") + Random.Next(1000, 9999).ToString();
                    if (connect != null) connect(Connection, RemoteConnect, ReferForUse.NetSet, Session.SessionID);
                    WriteLog(MethodName, Log_Type.Infomation, SystemMessage.Connect(Session.Remote, lan));
                    try
                    {
                        Dictionary<string, object> UrlPram = new Dictionary<string, object>();
                        string uri = System.Web.HttpUtility.UrlDecode(request.RawUrl);
                        int paramStartIndex = uri.IndexOf('?');
                        if (paramStartIndex > 0)
                            uri = uri.Substring(0, paramStartIndex);
                        WriteLog(MethodName, Log_Type.Test, SystemMessage.RefValDisp(uri, request.HttpMethod.ToUpper(), lan));
                        Session.UrlRef = uri;
                        var QS = request.QueryString;
                        foreach (var key in QS)
                            UrlPram.Add(key.ToString(), request.QueryString[key.ToString()]);
                        if (Header != null)
                        {
                            foreach (var dic in Header)
                                context.Response.Headers[dic.Key] = dic.Value;
                        } 
                        if (Access_Control_Allow)
                        {
                            context.Response.Headers["Access-Control-Allow-Origin"] = "*";
                            context.Response.Headers["Access-Control-Allow-Headers"]= "Origin, X-Requested-With, Content-Type, Accept";
                            context.Response.Headers["Access-Control-Allow-Methods"]= "GET, POST, PUT, DELETE, OPTIONS";
                        }
                        if (request.HttpMethod.ToUpper() == "GET" && getEvent != null)
                            res = getEvent(Session, UrlPram, QS.ToString());
                        else if (request.HttpMethod.ToUpper() == "POST")
                        {
                            Dictionary<string, object> Data = new Dictionary<string, object>();
                            string RawData = "";
                            if (request.ContentType.Length > 20 && string.Compare(request.ContentType.Substring(0, 20), "multipart/form-data;", true) == 0)
                            {
                                List<byte> pack = new List<byte>();
                                List<byte[]> datapack = new List<byte[]>();
                                Encoding Encoding = request.ContentEncoding;
                                string[] values = request.ContentType.Split(';').Skip(1).ToArray();
                                string boundary = string.Join(";", values).Replace("boundary=", "").Trim();
                                byte[] Boundary = Encoding.GetBytes(boundary + "\r\n");
                                byte[] EndBoundary = Encoding.GetBytes(boundary + "--\r\n");
                                Stream SourceStream = request.InputStream;
                                int data = -1;
                                do
                                {
                                    if (data != -1) pack.Add((byte)data);
                                    try
                                    {
                                        data = SourceStream.ReadByte();
                                    }
                                    catch { data = -1; }
                                }
                                while (data != -1);
                                RawData = Convert.ToBase64String(pack.ToArray());
                                int index = CompareBytes(pack.ToArray(), Boundary, 0);
                                do
                                {
                                    int lastindex = CompareBytes(pack.ToArray(), Boundary, index + Boundary.Length);
                                    if (lastindex == -1)
                                    {
                                        lastindex = CompareBytes(pack.ToArray(), EndBoundary, index + Boundary.Length);
                                        if (lastindex != -1)
                                        {
                                            byte[] buffer = new byte[lastindex - (index + EndBoundary.Length)];
                                            pack.CopyTo((index + Boundary.Length), buffer, 0, buffer.Length);
                                            datapack.Add(buffer);
                                        }
                                        index = -1;
                                    }
                                    else
                                    {
                                        byte[] buffer = new byte[lastindex - (index + Boundary.Length)];
                                        pack.CopyTo((index + Boundary.Length), buffer, 0, buffer.Length);
                                        datapack.Add(buffer);
                                        index = lastindex;
                                    }
                                }
                                while (index != -1); 
                                foreach (var buf in datapack)
                                {
                                    string tmp = "";
                                    if (buf.Length > Buffsize)
                                        tmp = Encoding.GetString(buf, 0, Buffsize);
                                    else
                                        tmp = Encoding.GetString(buf);
                                    if (tmp.Contains("\r\n"))
                                    {
                                        if (tmp.ToUpper().Contains("filename".ToUpper()))
                                        {
                                            string filename = tmp.Substring(0, tmp.IndexOf("\r\n") + 2);
                                            tmp = tmp.Substring(tmp.IndexOf("\r\n") + 2);
                                            string conntype = tmp.Substring(0, tmp.IndexOf("\r\n") + 2);
                                            if (buf.Length > (filename.Length + conntype.Length + 2))
                                            {
                                                byte[] pak = new byte[buf.Length - (filename.Length + conntype.Length + 2)];
                                                Array.Copy(buf, filename.Length + conntype.Length + 2, pak, 0, pak.Length);
                                                Data.Add(filename + conntype, pak);
                                            }

                                        }
                                        else
                                        {
                                            string key = tmp.Substring(0, tmp.IndexOf("\r\n") + 2);
                                            tmp = tmp.Substring(tmp.IndexOf("\r\n") + 2);
                                            if (tmp.StartsWith("\r\n"))
                                                tmp = tmp.Substring(2);
                                            Data.Add(key, tmp);
                                        }
                                    }
                                }
                            }
                            else if (request.HasEntityBody)
                            {
                                string postdat = "";
                                using (System.IO.Stream body = request.InputStream)
                                {
                                    using (System.IO.StreamReader reader = new System.IO.StreamReader(body, request.ContentEncoding))
                                    {
                                        postdat = reader.ReadToEnd();
                                    }
                                }
                                RawData = postdat;
                                List<string> postref = postdat.Split('&').ToList();
                                foreach (var key in postref)
                                {
                                    if (!string.IsNullOrEmpty(key))
                                    {
                                        string[] tmp = key.Split('=');
                                        if (tmp.Length == 2)
                                            Data.Add(tmp[0], tmp[1]);
                                    }
                                }
                            }
                            res = postEvent(Session, UrlPram, Data, RawData);
                        }
                        else if (request.HttpMethod.ToUpper() == "PUT" && getEvent != null)
                            res = putEvent(Session);
                        else if (request.HttpMethod.ToUpper() == "DELETE" && getEvent != null)
                            res = delEvent(Session);
                        else if (request.HttpMethod.ToUpper() == "OPTIONS")
                            res = true;
                        if (res)
                            response.StatusCode = 200;
                        else
                            response.StatusCode = 404;
                    }
                    catch (Exception exx)
                    {
                        if (haserr != null) haserr(ClassName, MethodName, exx);
                        WriteLog(MethodName, Log_Type.Error, exx.Message);
                        if (error != null)
                            error(Session, exx);
                        else
                        {
                            response.StatusCode = 500;
                            var sb = new StringBuilder("<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n<body>\r\n<font>\r\n");
                            if (Debug)
                                sb.AppendLine(exx.Message);
                            else
                                sb.AppendLine("出现错误");
                            sb.AppendLine("</font>\r\n</body>\r\n</html>");
                            response.ContentType = "text/html;charset=UTF-8";
                            var result = Encoding.UTF8.GetBytes(sb.ToString());
                            using (var stream = response.OutputStream)
                            {
                                stream.Write(result, 0, result.Length);
                                stream.Close();
                            }
                        }
                    }
                    response.Close();
                }
            }
            catch (Exception ex)
            {
                WriteLog(MethodName, Log_Type.Error, ex.Message);
                if (haserr != null) haserr(ClassName, MethodName, ex);
            }
        }
    }
}
