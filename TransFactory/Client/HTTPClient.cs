using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Net;
using System.Runtime.Serialization.Json;
using NK.Entity;
using NK.ENum;
using NK.Event;
using NK.Message;
using System.Security.Cryptography.X509Certificates;

namespace NK.Communicate
{
    /// <summary>
    /// HTTP客户端
    /// </summary>
    public  class HTTPClient : IDisposable
    {

        #region 定义 
        private bool m_disposed;
        private string ClassName = "";
        private CookieContainer session = null;
        private HttpWebRequest myHttpWebRequest = null;
        private HttpWebResponse myHttpWebResponse = null; 
        #endregion

        #region 构造

        /// <summary>
        /// HTTP客户端
        /// </summary>
        public HTTPClient(string connection="")
        {
            this.Connection = connection;
            ClassName = this.GetType().ToString();
            this.language = Language.Chinese;
        }

        /// <summary>
        /// HTTP客户端
        /// </summary>
        /// <param name="ComPort">端口号</param>
        /// <param name="ComRate">波特率</param>
        public HTTPClient(string IPOrDomain,int Port,string UrlParameter)
        {
            IPAddress IPS = IPAddress.Any;
            NetSet net = new NetSet();
            net.Address_Family = System.Net.Sockets.AddressFamily.InterNetwork;
            if (IPAddress.TryParse(IPOrDomain, out IPS))
                net.IPAddress = IPOrDomain;
            else
                net.DomainName = IPOrDomain;
            net.Port = Port;
            net.AddrRef = (string.IsNullOrEmpty(UrlParameter) ? "" : UrlParameter);
            net.Mode = Net_Mode.Remote;
            net.Protocol_Type = System.Net.Sockets.ProtocolType.Tcp;
            net.Socket_Type = System.Net.Sockets.SocketType.Stream;
            this.Connection = Serialize(net);
            ClassName = this.GetType().ToString();
            this.language = Language.Chinese;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        ~HTTPClient()
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
                   
                    m_disposed = true;
                }
            }
        }


        #endregion

        #region 事件

        /// <summary>
        /// 错误出现事件，性能参数内DEBUG设置为EVENT有效
        /// </summary>
        public CommEvent.LogEven log = null;
        /// <summary>
        /// 错误出现事件，性能参数内DEBUG设置为EVENT有效
        /// </summary>
        public CommEvent.HasErrorEven HasError = null;

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
        public bool IsConnected { get { return myHttpWebRequest != null ? myHttpWebRequest.KeepAlive : false; } }
        /// <summary>
        /// 显示语言
        /// </summary>
        public Language language { get; set; }
        #endregion
         
        #region 扩展属性
         
        /// <summary>
        /// HTTPS用证书路径
        /// </summary>
        public string CertFile { get; set; }
        /// <summary>
        /// 代理地址
        /// </summary>
        public string ProxyUrl { get; set; }

        /// <summary>
        /// HTTP头
        /// </summary>
        public Dictionary<string, string> Header { get; set; }

        /// <summary>
        /// 主机
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 连接类型
        /// </summary>
        public string ConnectType { get; set; }

        /// <summary>
        /// 引用
        /// </summary>
        public string Referer { get; set; }

        /// <summary>
        /// 用户参数
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// 保持连接
        /// </summary>
        public bool KeepAlive { get; set; }

        /// <summary>
        /// Chunked编码
        /// </summary>
        public string TransferEncoding { get; set; }

        /// <summary>
        /// 是否使用session
        /// </summary>
        public bool Session { get; set; }

        #endregion

        #region 响应属性

        /// <summary>
        /// 服务器响应代码
        /// </summary>
        public int StatCode
        {
            get
            {
                try
                {
                    if (myHttpWebResponse == null)
                        return -1;
                    else
                        return  (int)myHttpWebResponse.StatusCode; 
                }
                catch
                {
                    return -2;
                }
            }
        }

        /// <summary>
        /// 响应页面类型
        /// </summary>
        public string ContentType
        {
            get
            {
                try
                {
                    if (myHttpWebResponse == null)
                        return "";
                    else
                        return myHttpWebResponse.ContentType;
                }
                catch 
                { return "";  }
            }
        }

        /// <summary>
        /// 响应页面字符集
        /// </summary>
        public string CharacterSet
        {
            get
            {
                try
                {
                    if (myHttpWebResponse == null)
                        return "";
                    else
                        return myHttpWebResponse.CharacterSet;
                }
                catch 
                {  return "";  }
            }
        }

        /// <summary>
        /// 响应页面编码
        /// </summary>
        public string ContentEncoding
        {
            get
            {
                try
                {
                    if (myHttpWebResponse == null)
                        return "";
                    else
                        return myHttpWebResponse.ContentEncoding;
                }
                catch
                {  return ""; }
            }
        }


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
        
        private void InitReQuest(string url,string ConnType="")
        {
            int TimeOut = 60;
            if (this.Refer_Prama != null)
            {
                if (this.Refer_Prama.ConnectTimeOut > 0)
                    TimeOut = this.Refer_Prama.ConnectTimeOut;
            }
            myHttpWebResponse = null;
            myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            if (url.ToUpper().Contains("HTTPS"))
            {
                X509Certificate Cert = X509Certificate.CreateFromCertFile(CertFile);
                myHttpWebRequest.ClientCertificates.Add(Cert);
            }
            if (string.IsNullOrEmpty(this.ProxyUrl))
                myHttpWebRequest.Proxy = null;
            else
                myHttpWebRequest.Proxy = new WebProxy(this.ProxyUrl,true);
            myHttpWebRequest.Headers.Clear();
            if (this.Header != null)
            {
                foreach (var hed in this.Header)
                    myHttpWebRequest.Headers.Add(hed.Key, hed.Value);
            }
            myHttpWebRequest.ContentType = string.IsNullOrEmpty(ConnType) ?(string.IsNullOrEmpty(this.ConnectType)? "text/html": this.ConnectType) : ConnType;
            myHttpWebRequest.KeepAlive = this.KeepAlive;
            myHttpWebRequest.Timeout = TimeOut *1000;
            if (!string.IsNullOrEmpty(Host))
                myHttpWebRequest.Host = this.Host;
            if (!string.IsNullOrEmpty(UserAgent))
                myHttpWebRequest.UserAgent = this.UserAgent;
            if (!string.IsNullOrEmpty(Referer))
                myHttpWebRequest.Referer = this.Referer;
            if (!string.IsNullOrEmpty(TransferEncoding))
            {
                myHttpWebRequest.SendChunked = true;
                myHttpWebRequest.TransferEncoding = TransferEncoding;
            }
            else
                myHttpWebRequest.SendChunked = false;
            myHttpWebRequest.CookieContainer = new CookieContainer();
            if (  session != null)
                myHttpWebRequest.CookieContainer = session;
        }

        private string UrlMake(string UrlPath)
        {
            if (string.IsNullOrEmpty(this.Connection)) return "";
            NetSet http = new NetSet();
            try
            { http = Deserialize<NetSet>(this.Connection); }
            catch
            { return ""; }
            if (string.IsNullOrEmpty(http.IPAddress) && string.IsNullOrEmpty(http.DomainName)) return "";
            string IP = "";
            if (!string.IsNullOrEmpty(http.DomainName))
                IP = http.DomainName;
            else if (!string.IsNullOrEmpty(http.IPAddress))
                IP = http.IPAddress;
            string Url = "";
            if(string.IsNullOrEmpty(this.CertFile))
              Url= String.Format("http://{0}:{1}", IP, http.Port > 0 ? http.Port.ToString() : "80");
            else
              Url = String.Format("https://{0}:{1}", IP, http.Port > 0 ? http.Port.ToString() : "80");
            if (!string.IsNullOrEmpty(http.AddrRef))
                Url = Url + "/" + http.AddrRef;
            if (!string.IsNullOrEmpty(UrlPath))
                Url = Url + "/" + UrlPath;
            return Url;
        }

        /// <summary>
        /// POST数据并获取响应的HTML
        /// </summary>
        /// <param name="UrlPath">url相对目录,去除HTTP/HTTPS://XX:YY/?/后的部分</param>
        /// <param name="Postdata">POST数据</param>
        /// <returns></returns>
        public string PostHtml(string UrlPath="",string Postdata="")
        {
            string Html = "";
            string Url = UrlMake(UrlPath);
            if (string.IsNullOrEmpty(Url))
            {
                if (log != null) log(ClassName, "PostHtml", Log_Type.Error,SystemMessage.RefNullOrEmpty("Connecton",language) );
                if (this.HasError != null)
                    this.HasError(ClassName, "PostHtml", new NullReferenceException(SystemMessage.RefNullOrEmpty("Connecton", language)));
                else
                    throw new NullReferenceException(SystemMessage.RefNullOrEmpty("Connecton", language));
                return "";
            } 
            try
            {
                string  CharSet = "";
                if (this.Refer_Prama != null)
                {
                    if (string.IsNullOrEmpty(Refer_Prama.CharSet))
                        CharSet = Refer_Prama.CharSet;
                }
                if (log != null) log(ClassName, "PostHtml", Log_Type.Test, SystemMessage.RefValDisp("url", Url,language));
                InitReQuest(Url);
                if (log != null) log(ClassName, "PostHtml", Log_Type.Test, SystemMessage.RefValDisp("post", Postdata, language));
                myHttpWebRequest.Method = "Post";
                if (string.IsNullOrEmpty(Postdata))
                    myHttpWebRequest.ContentLength = 0;
                else  if (string.IsNullOrEmpty(CharSet))
                {
                    byte[] data = Encoding.ASCII.GetBytes(Postdata);
                    myHttpWebRequest.ContentLength = data.Length;
                    Stream newStream = myHttpWebRequest.GetRequestStream();
                    newStream.Write(data, 0, data.Length);
                    newStream.Close();
                }
                else
                {
                    byte[] data = Encoding.GetEncoding(CharSet.Trim()).GetBytes(Postdata);
                    myHttpWebRequest.ContentLength = data.Length;
                    Stream newStream = myHttpWebRequest.GetRequestStream();
                    newStream.Write(data, 0, data.Length);
                    newStream.Close();
                }
                myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                int iStatCode = (int)myHttpWebResponse.StatusCode;
                if (log != null) log(ClassName, "PostHtml", Log_Type.Infomation, SystemMessage.RefValDisp("resp" , iStatCode.ToString(),language));
                if (iStatCode == 200)
                {
                    try
                    {
                        session = myHttpWebRequest.CookieContainer;
                        Stream streamReceive = myHttpWebResponse.GetResponseStream();
                        MemoryStream stmMemory = new MemoryStream();
                        byte[] buffer = new byte[64 * 1024];
                        int i;
                        while ((i = streamReceive.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            stmMemory.Write(buffer, 0, i);
                        }
                        byte[] arraryByte = stmMemory.ToArray();
                        Html = System.Text.Encoding.Default.GetString(arraryByte);
                        stmMemory.Close(); 
                    }
                    catch (WebException ex)
                    {
                        HttpWebResponse response = (HttpWebResponse)ex.Response;
                        if (log != null) log(ClassName, "PostHtml", Log_Type.Error, SystemMessage.ExecFail(language));
                        if (this.HasError != null)
                            this.HasError(ClassName, "PostHtml", new WebException(SystemMessage.ExecFail(language)));
                         if (response.StatusCode == HttpStatusCode.BadRequest)
                        {
                            using (Stream data = response.GetResponseStream())
                            {
                                using (StreamReader readers = new StreamReader(data))
                                {
                                    Html = readers.ReadToEnd();

                                }
                            }
                        }
                    }
                }
                else
                {
                    if (log != null) log(ClassName, "PostHtml", Log_Type.Error, SystemMessage.ExecFail(language));
                    if (this.HasError != null)
                        this.HasError(ClassName, "PostHtml", new WebException(SystemMessage.ExecFail(language)));
                }
                myHttpWebResponse.Close();
                myHttpWebRequest.Abort();
                if (log != null) log(ClassName, "PostHtml", Log_Type.Infomation,SystemMessage.RefValDisp("HTML", Html,language));
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, "PostHtml", Log_Type.Error, ex.Message);
                if (this.HasError != null)
                    this.HasError(ClassName, "PostHtml", ex);
                else
                    throw ex;
            }
            return Html;
        }

        /// <summary>
        /// Get方式获取HTML
        /// </summary>
        /// <param name="UrlPath">url相对目录,去除HTTP/HTTPS://XX:YY/?/后的部分</param>
        /// <param name="UrlRefData">URL参数</param>
        /// <returns></returns>
        public string GetHtml(string UrlPath="",string UrlRefData="")
        {
            string Html = "";
            string Url = UrlMake(UrlPath);
            if (string.IsNullOrEmpty(Url))
            {
                if (log != null) log(ClassName, "GetHtml", Log_Type.Error, SystemMessage.RefNullOrEmpty("Connecton", language));
                if (this.HasError != null)
                    this.HasError(ClassName, "GetHtml", new NullReferenceException(SystemMessage.RefNullOrEmpty("Connecton", language)));
                else
                    throw new NullReferenceException(SystemMessage.RefNullOrEmpty("Connecton", language));
                return "";
            }
            try
            {
                if (log != null) log(ClassName, "GetHtml", Log_Type.Test, SystemMessage.RefValDisp("url", Url, language));
                if (!string.IsNullOrEmpty(UrlRefData))
                {
                    if (Url.Contains("?"))
                        Url = Url + "&" + UrlRefData;
                    else
                        Url = Url + "?" + UrlRefData;
                } 
                if (log != null) log(ClassName, "GetHtml", Log_Type.Test, SystemMessage.RefValDisp("get", UrlRefData, language));
                InitReQuest(Url);
                myHttpWebRequest.Method = "GET";
                myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                int iStatCode = (int)myHttpWebResponse.StatusCode;
                if (log != null) log(ClassName, "GetHtml", Log_Type.Infomation, SystemMessage.RefValDisp("resp", iStatCode.ToString(), language));
                if (iStatCode == 200)
                {
                    try
                    { 
                        Stream streamReceive = myHttpWebResponse.GetResponseStream();
                        MemoryStream stmMemory = new MemoryStream();
                        byte[] buffer = new byte[64 * 1024];
                        int i;
                        while ((i = streamReceive.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            stmMemory.Write(buffer, 0, i);
                        }
                        byte[] arraryByte = stmMemory.ToArray();
                        Html = System.Text.Encoding.Default.GetString(arraryByte);
                        stmMemory.Close();
                    }
                    catch (WebException ex)
                    {
                        HttpWebResponse response = (HttpWebResponse)ex.Response;
                        if (log != null) log(ClassName, "GetHtml", Log_Type.Error, SystemMessage.ExecFail(language));
                        if (this.HasError != null)
                            this.HasError(ClassName, "GetHtml", new WebException(SystemMessage.ExecFail(language)));
                        if (response.StatusCode == HttpStatusCode.BadRequest)
                        {
                            using (Stream data = response.GetResponseStream())
                            {
                                using (StreamReader readers = new StreamReader(data))
                                {
                                    Html = readers.ReadToEnd();
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (log != null) log(ClassName, "GetHtml", Log_Type.Error,SystemMessage.ExecFail(language));
                    if (this.HasError != null)
                        this.HasError(ClassName, "GetHtml", new WebException( SystemMessage.ExecFail(language)));
                }
                myHttpWebResponse.Close();
                myHttpWebRequest.Abort();
                if (log != null) log(ClassName, "GetHtml", Log_Type.Infomation, SystemMessage.RefValDisp("HTML", Html, language));
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, "GetHtml", Log_Type.Error, ex.Message);
                if (this.HasError != null)
                    this.HasError(ClassName, "GetHtml", ex);
                else
                    throw ex;
            }
            return Html;
        }

        /// <summary>
        /// 下载
        /// </summary>
        /// <param name="UrlPath">url相对目录,去除HTTP/HTTPS://XX:YY/?/后的部分</param>
        /// <param name="SavePath">本地路径</param>
        /// <param name="FileName">文件名</param>
        /// <param name="BreakPoint">断点下载</param>
        /// <returns></returns>
        public string DownLoad(string UrlPath, string SavePath, string FileName = "",bool BreakPoint=true)
        {
            string Html = "";
            string Url = UrlMake(UrlPath);
            if (string.IsNullOrEmpty(Url))
            {
                if (log != null) log(ClassName, "DownLoad", Log_Type.Error, SystemMessage.RefNullOrEmpty("Connecton", language));
                if (this.HasError != null)
                    this.HasError(ClassName, "DownLoad", new NullReferenceException(SystemMessage.RefNullOrEmpty("Connecton", language)));
                else
                    throw new NullReferenceException(SystemMessage.RefNullOrEmpty("Connecton", language));
                return "";
            }
            if (string.IsNullOrEmpty(SavePath))
            {
                if (log != null) log(ClassName, "DownLoad", Log_Type.Error, SystemMessage.RefNullOrEmpty("SavePath", language));
                if (this.HasError != null)
                    this.HasError(ClassName, "DownLoad", new NullReferenceException(SystemMessage.RefNullOrEmpty("SavePath", language)));
                else
                    throw new NullReferenceException(SystemMessage.RefNullOrEmpty("SavePath", language));
                return "";
            }
            try
            {
                int BufferSize = 1024;
                if (this.Refer_Prama != null)
                {
                    if (this.Refer_Prama.ReceiveBufferSize > 0)
                        BufferSize = this.Refer_Prama.ReceiveBufferSize;
                }
                if (log != null) log(ClassName, "DownLoad", Log_Type.Test, SystemMessage.RefValDisp("url", Url, language));
                InitReQuest(Url);
                DirectoryInfo di = new DirectoryInfo(SavePath);
                if (!di.Exists)
                { di.Create(); }
                FileInfo fi = null;
                FileStream fs = null;
                if (log != null) log(ClassName, "DownLoad", Log_Type.Test, SystemMessage.RefValDisp("FileName", SavePath + "\\" + FileName, language));
                if (!string.IsNullOrEmpty (FileName))
                   FileName = UrlPath.Substring(UrlPath.LastIndexOf("/") + 1);
                if (BreakPoint)
                {
                    fi = new FileInfo(SavePath + "\\" + FileName + ".tmp");
                    if (fi.Exists)
                    {
                        long index = fi.Length;
                        fs = File.OpenWrite(fi.FullName);
                        fs.Seek(index, SeekOrigin.Begin);
                        myHttpWebRequest.AddRange((int)index);
                    }
                    else
                        fs = new FileStream(fi.FullName, FileMode.Create);
                }
                else
                {
                    fi = new FileInfo(SavePath + "\\" + FileName);
                    FileName = fi.Name.Replace(fi.Extension,"") + "_副本" + fi.Extension;
                    if (fi.Exists)
                        fi = new FileInfo(SavePath + "\\" + FileName);
                    fs = new System.IO.FileStream(fi.FullName, FileMode.Create); 
                } 
                myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                int iStatCode = (int)myHttpWebResponse.StatusCode;
                if (log != null) log(ClassName, "DownLoad", Log_Type.Infomation, SystemMessage.RefValDisp("resp", iStatCode.ToString(), language));
                if (iStatCode == 200 || iStatCode == 206)
                {
                    session = myHttpWebRequest.CookieContainer;
                    Stream myStream = myHttpWebResponse.GetResponseStream();
                    byte[] btContent = new byte[BufferSize];
                    int intSize = 0;
                    intSize = myStream.Read(btContent, 0, btContent.Length);
                    while (intSize > 0)
                    {
                        fs.Write(btContent, 0, intSize);
                        intSize = myStream.Read(btContent, 0, btContent.Length);
                    }
                    myStream.Close();
                    if (BreakPoint)
                    {
                        fi.CopyTo(SavePath + "\\" + FileName, true);
                        try { File.Delete(SavePath + "\\" + FileName + ".tmp"); }
                        catch { }
                    }
                }
                else
                {
                    if (log != null) log(ClassName, "DownLoad", Log_Type.Error, SystemMessage.ExecFail(language));
                    if (this.HasError != null)
                        this.HasError(ClassName, "DownLoad", new WebException( SystemMessage.ExecFail(language)));
                }
                myHttpWebResponse.Close();
                myHttpWebRequest.Abort();
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, "DownLoad", Log_Type.Error, ex.Message);
                if (this.HasError != null)
                    this.HasError(ClassName, "DownLoad", ex);
                else
                    throw ex;
            }
            return Html;
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="UrlPath">url相对目录,去除HTTP/HTTPS://XX:YY/?/后的部分</param>
        /// <param name="FileFullName">完整路径名</param>
        /// <param name="ServerName">服务器保存文件名</param>
        /// <returns></returns>
        public bool UpLoad(string UrlPath, string FileFullName, string ServerName = "")
        {
            string Url = UrlMake(UrlPath);
            if (string.IsNullOrEmpty(Url))
            {
                if (log != null) log(ClassName, "UpLoad", Log_Type.Error, SystemMessage.RefNullOrEmpty("Connecton", language));
                if (this.HasError != null)
                    this.HasError(ClassName, "UpLoad", new NullReferenceException(SystemMessage.RefNullOrEmpty("Connecton", language)));
                else
                    throw new NullReferenceException(SystemMessage.RefNullOrEmpty("Connecton", language));
                return false ;
            }
            if (string.IsNullOrEmpty(FileFullName))
            {
                if (log != null) log(ClassName, "UpLoad", Log_Type.Error, SystemMessage.RefNullOrEmpty("FileFullName", language));
                if (this.HasError != null)
                    this.HasError(ClassName, "UpLoad", new NullReferenceException(SystemMessage.RefNullOrEmpty("FileFullName", language)));
                else
                    throw new NullReferenceException(SystemMessage.RefNullOrEmpty("FileFullName", language));
                return false;
            }
            try
            {
                int BufferSize = 1024;
                if (this.Refer_Prama != null)
                {
                    if (this.Refer_Prama.ReceiveBufferSize > 0)
                        BufferSize = this.Refer_Prama.ReceiveBufferSize;
                }
                string CharSet = "";
                if (this.Refer_Prama != null)
                {
                    if (string.IsNullOrEmpty(Refer_Prama.CharSet))
                        CharSet = Refer_Prama.CharSet;
                }
                int TimeOut = 60;
                if (this.Refer_Prama != null)
                {
                    if (this.Refer_Prama.ConnectTimeOut > 0)
                        TimeOut = this.Refer_Prama.ConnectTimeOut;
                } 
                FileInfo fi = new FileInfo(FileFullName.Trim());
                if (log != null) log(ClassName, "UpLoad", Log_Type.Test, SystemMessage.RefValDisp("FileFullName", fi.FullName,language));
                if (!fi.Exists)
                    throw new FileNotFoundException(fi.FullName + " is not Exist");
                string strBoundary = "----------NK" + DateTime.Now.Ticks.ToString("x");
                byte[] boundaryBytes = Encoding.ASCII.GetBytes("\r\n--" + strBoundary + "\r\n");
                StringBuilder sb = new StringBuilder();
                sb.Append(strBoundary);
                sb.Append("\r\n");
                sb.Append("Content-Disposition: form-data; name=\"");
                sb.Append("file");
                sb.Append("\"; filename=\"");
                if (string.IsNullOrEmpty(ServerName))
                { ServerName =    fi.Name; }
                sb.Append(System.Web.HttpUtility.HtmlEncode (ServerName));
                sb.Append("\"");
                sb.Append("\r\n");
                sb.Append("Content-Type: application/octet-stream");
                sb.Append("\r\n");
                sb.Append("\r\n");
                string strPostHeader = sb.ToString();
                byte[] end_data ;
                byte[] postHeaderBytes;
                if (!string.IsNullOrEmpty(CharSet))
                {
                    postHeaderBytes = Encoding.GetEncoding(CharSet).GetBytes(strPostHeader);
                    end_data = Encoding.GetEncoding(CharSet).GetBytes("\r\n" + strBoundary + "--\r\n") ;
                }
                else
                {
                    postHeaderBytes = Encoding.Default.GetBytes(strPostHeader);
                    end_data = Encoding.Default.GetBytes("\r\n" + strBoundary + "--\r\n");
                }
                if (log != null) log(ClassName, "UpLoad", Log_Type.Test, SystemMessage.RefValDisp("HEAD" , strPostHeader,language));
                if (log != null) log(ClassName, "PostHtml", Log_Type.Test, SystemMessage.RefValDisp("url", Url, language));
                myHttpWebRequest = (HttpWebRequest)WebRequest.Create(Url);
                myHttpWebRequest.Headers.Clear();
                if (this.Header != null)
                {
                    foreach (var hed in this.Header)
                        myHttpWebRequest.Headers.Add(hed.Key, hed.Value);
                }
                myHttpWebRequest.Method = "POST";
                myHttpWebRequest.ContentType =   "multipart/form-data; boundary=" + strBoundary; 
                myHttpWebRequest.KeepAlive = this.KeepAlive;
                myHttpWebRequest.Timeout = TimeOut;
                if (!string.IsNullOrEmpty(Host))
                    myHttpWebRequest.Host = this.Host;
                if (!string.IsNullOrEmpty(UserAgent))
                    myHttpWebRequest.UserAgent = this.UserAgent;
                if (!string.IsNullOrEmpty(Referer))
                    myHttpWebRequest.Referer = this.Referer;
                if (!string.IsNullOrEmpty(TransferEncoding))
                {
                    myHttpWebRequest.SendChunked = true;
                    myHttpWebRequest.TransferEncoding = TransferEncoding;
                }
                else
                    myHttpWebRequest.SendChunked = false;
                if (Session)
                    myHttpWebRequest.CookieContainer = session;
                myHttpWebRequest.Method = "POST";
                myHttpWebRequest.AllowWriteStreamBuffering = false;
                long length = fi.Length + postHeaderBytes.Length + end_data.Length;
                myHttpWebRequest.ContentLength = length;
                byte[] buffer = new byte[BufferSize];
                int dataRead = 0;
                using (FileStream fs = fi.OpenRead())
                {
                    try
                    {
                        using (Stream rs = myHttpWebRequest.GetRequestStream())
                        {
                            rs.Write(postHeaderBytes, 0, postHeaderBytes.Length);
                            do
                            {
                                dataRead = fs.Read(buffer, 0, BufferSize);
                                rs.Write(buffer, 0, dataRead);
                            }
                            while (dataRead >0);
                            rs.Write(end_data, 0, end_data.Length);
                        }
                    }
                    catch
                    { }
                    finally
                    { fs.Close(); }
                }
                myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                int iStatCode = (int)myHttpWebResponse.StatusCode;
                bool res = true;
                if (log != null) log(ClassName, "PostHtml", Log_Type.Infomation, SystemMessage.RefValDisp("resp", iStatCode.ToString(), language));
                if (iStatCode != 200)
                {
                    if (log != null) log(ClassName, "UpLoad", Log_Type.Error , SystemMessage.ExecFail(language));
                    if (this.HasError != null)
                        this.HasError(ClassName, "UpLoad", new WebException(SystemMessage.ExecFail(language)));

                    res = false;
                }
                myHttpWebResponse.Close();
                myHttpWebRequest.Abort();
                return res;
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, "UpLoad", Log_Type.Error, ex.Message);
                if (this.HasError != null)
                    this.HasError(ClassName, "UpLoad", ex);
                else
                    throw ex;
                return false;
            }
        }
         
        /// <summary>
        /// 清除SESSION
        /// </summary>
        public void ClearSession()
        {
            session = null;
        }

        #endregion

    }
}
