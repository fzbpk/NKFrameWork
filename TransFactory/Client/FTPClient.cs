using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Security.Cryptography;
using System.Runtime.Serialization.Json;
using NK.Entity;
using NK.ENum;
using NK.Event;
using NK.Message;
namespace NK.Communicate
{
    /// <summary>
    /// FTP客户端
    /// </summary>
    public  class FTPClient : IDisposable
    {
        #region 定义
 
        private bool m_disposed;
        private bool Anymous = false;
        private NetworkCredential account = new NetworkCredential();
        private FtpWebRequest listRequest = null;
        private FtpWebResponse listResponse = null;
        private FileStream file = null;
        private string ClassName = "";

        #endregion

        #region 结构体

        /// <summary>
        /// FTP客户端
        /// </summary>
        public FTPClient(string connection="")
        {
            this.Connection = connection;
            account.UserName = "";
            account.Password = "";
            Anymous = false;
            MD5 = false;
            ClassName = this.GetType().ToString();
            this.language = Language.Chinese;
        }

        /// <summary>
        /// FTP客户端
        /// </summary>
        /// <param name="FTPIP">地址</param>
        /// <param name="FTPPort">端口</param>
        /// <param name="UrlRef">地址参数</param>
        /// <param name="UserName">用户名</param>
        /// <param name="Password">密码</param>
        /// <param name="timeout">超时</param>
        public FTPClient(string IPOrDomain, int Port, string UrlParameter)
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
            net.Protocol_Type = System.Net.Sockets.ProtocolType.Udp;
            net.Socket_Type = System.Net.Sockets.SocketType.Stream;
            this.Connection = Serialize(net);
            Anymous = false; 
            MD5 = false;
            ClassName = this.GetType().ToString();
            this.language = Language.Chinese;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        ~FTPClient()
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
                    if (file != null)
                    {
                        try
                        {
                            file.Close();
                        }
                        catch { }
                        file = null;
                    }
                    if (listRequest != null)
                    {
                        try
                        {
                            listRequest.Abort();
                        }
                        catch { }
                        listRequest = null;
                    }
                    if (listResponse != null)
                    {
                        try
                        {
                            listResponse.Close();
                        }
                        catch { }
                        listResponse = null;
                    }
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
        public CommEvent.HasErrorEven HasError { get; set; }

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
        public bool IsConnected { get { return listRequest != null ? listRequest.KeepAlive : false; } }
        /// <summary>
        /// 显示语言
        /// </summary>
        public Language language { get; set; }
        #endregion

        #region 扩展属性

        /// <summary>
        /// 获取用户账户
        /// </summary>
        public string User
        {
            get
            {
                if (account != null)
                    return account.UserName;
                else
                    return "";
            }
        }

        /// <summary>
        /// 获取或设置是否使用MD5加密
        /// </summary>
        public bool MD5 { get; set; }

        /// <summary>
        /// 获取或设置密码是否已加密
        /// </summary>
        public bool HadMD5 { get; set; }

        /// <summary>
        /// 获取或设置密匙
        /// </summary>
        public string Key { get; set; }

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
                    if (listResponse == null)
                        return -1;
                    else
                        return (int)listResponse.StatusCode;
                }
                catch
                {
                    return -2;
                }
            }
        }

        /// <summary>
        /// 获取响应的欢迎信息
        /// </summary>
        public string WelcomeMessage
        {
            get
            {
                try
                {
                    if (listResponse == null)
                        return "";
                    else
                        return listResponse.WelcomeMessage;
                }
                catch
                { return ""; }
            }
        }

        /// <summary>
        /// 响应描述
        /// </summary>
        public string StatusDescription
        {
            get
            {
                try
                {
                    if (listResponse == null)
                        return "";
                    else
                        return listResponse.StatusDescription;
                }
                catch
                { return ""; }
            }
        }

        /// <summary>
        /// 连接信息
        /// </summary>
        public string BannerMessage
        {
            get
            {
                try
                {
                    if (listResponse == null)
                        return "";
                    else
                        return listResponse.BannerMessage;
                }
                catch
                { return ""; }
            }
        }

        /// <summary>
        /// 退出信息
        /// </summary>
        public string ExitMessage
        {
            get
            {
                try
                {
                    if (listResponse == null)
                        return "";
                    else
                        return listResponse.ExitMessage;
                }
                catch
                { return ""; }
            }
        }

        /// <summary>
        /// 最后操作时间
        /// </summary>
        public DateTime LastModified
        {
            get
            {
                try
                {
                    if (listResponse == null)
                        return DateTime .Now ;
                    else
                        return listResponse.LastModified;
                }
                catch
                { return DateTime.Now; }
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
 
        private void InitFtp(string Url, string Method)
        {
            int TimeOut = 0;
            if (this.Refer_Prama != null)
            {
                if (this.Refer_Prama.ConnectTimeOut > 0)
                    TimeOut = this.Refer_Prama.ConnectTimeOut;
            }
            Uri uri = new Uri(Url);
            listRequest = (FtpWebRequest)FtpWebRequest.Create(uri);
            listRequest.Method = Method;
            listRequest.Timeout = TimeOut;
            if (!Anymous)
             listRequest.Credentials = account; 
        }

        private string  UrlMake()
        {
            if (string.IsNullOrEmpty(this.Connection)) return "";
            NetSet ftp = new NetSet();
            try
            { ftp = Deserialize<NetSet>(this.Connection); }
            catch
            { return ""; }
            if (string.IsNullOrEmpty(ftp.IPAddress) && string.IsNullOrEmpty(ftp.DomainName)) return "";
            string IP = "";
            if (!string.IsNullOrEmpty(ftp.DomainName))
                IP = ftp.DomainName;
            else if (!string.IsNullOrEmpty(ftp.IPAddress))
                IP = ftp.IPAddress;
            string Url = String.Format("ftp://{0}:{1}", IP, ftp.Port>0? ftp.Port.ToString():"21");
            if (!string.IsNullOrEmpty(ftp.AddrRef ))
                Url = Url + "/" + ftp.AddrRef;
            return Url;
        }

        /// <summary>
        /// 设置认证信息
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <param name="PassWord">密码</param>
        /// <returns>错误代码，0成功，-1参数不正，-2密码不能为空</returns>
        public bool SetAuthor(string UserName, string PassWord)
        {
            if (string.IsNullOrEmpty(UserName))
                UserName = "";
            if (string.IsNullOrEmpty(PassWord))
                PassWord = "";
            if (MD5 && string.IsNullOrEmpty(PassWord))
            {
                if (log != null) log(ClassName, "SetAuthor", Log_Type.Error, SystemMessage.RefNullOrEmpty("PassWord",language));
                if (this.HasError != null)
                    this.HasError(ClassName, "SetAuthor", new NullReferenceException(SystemMessage.RefNullOrEmpty("PassWord", language)));
                else
                    throw new NullReferenceException(SystemMessage.RefNullOrEmpty("PassWord", language));
                return false;
            }
            try
            {

                if (string.IsNullOrEmpty(UserName))
                {
                    if (log != null) log(ClassName, "SetAuthor", Log_Type.Test, SystemMessage.RefValDisp("UserName", "Anymous", language));
                    Anymous = true;
                }
                else if (MD5 && !string.IsNullOrEmpty(UserName))
                {
                    if (log != null) log(ClassName, "SetAuthor", Log_Type.Test, SystemMessage.RefValDisp(UserName , PassWord,language));
                    MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                    PassWord = BitConverter.ToString(hashmd5.ComputeHash(Encoding.Default.GetBytes(PassWord.ToLower().Trim() + this.Key))).Replace("-", "");
                    account.UserName = UserName;
                    account.Password = PassWord;
                }
                else
                {
                    if (log != null) log(ClassName, "SetAuthor", Log_Type.Test, SystemMessage.RefValDisp(UserName ,PassWord,language));
                    account.UserName = UserName;
                    account.Password = PassWord;
                }
                account.Domain = "";
                return true;
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, "SetAuthor", Log_Type.Error, ex.Message);
                if (this.HasError != null)
                    this.HasError(ClassName, "SetAuthor", ex);
                else
                    throw ex;
                return false;
            } 
        }

        /// <summary>
        /// 重输入密码
        /// </summary>
        /// <param name="PassWord">密码</param>
        /// <returns>错误代码，0成功，-1参数不正，-2密码不能为空</returns>
        public bool EditPassWord(string PassWord)
        {
            if (string.IsNullOrEmpty(PassWord))
                PassWord = "";
            if (MD5 && string.IsNullOrEmpty(PassWord))
            {
                if (log != null) log(ClassName, "EditPassWord", Log_Type.Error,SystemMessage.RefNullOrEmpty("PassWord", language) );
                if (this.HasError != null)
                    this.HasError(ClassName, "EditPassWord", new NullReferenceException(SystemMessage.RefNullOrEmpty("PassWord", language)));
                else
                    throw new NullReferenceException(SystemMessage.RefNullOrEmpty("PassWord", language));
                return false;
            }
            try
            {
                if (log != null) log(ClassName, "EditPassWord", Log_Type.Test,    PassWord);
                if (MD5)
                {
                    MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                    PassWord = BitConverter.ToString(hashmd5.ComputeHash(Encoding.Default.GetBytes(PassWord.ToLower().Trim() + this.Key))).Replace("-", "");
                    account.Password = PassWord;
                }
                else
                    account.Password = PassWord;
                return true;
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, "EditPassWord", Log_Type.Error, ex.Message);
                if (this.HasError != null)
                    this.HasError(ClassName, "EditPassWord", ex);
                else
                    throw ex;
                return false;
            } 
        }
 
        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="FolderName">目录名</param>
        /// <returns>执行结果，FTP返回码</returns>
        public int CreatDirectory(string FolderName)
        {
            string Url = UrlMake();
            if (string.IsNullOrEmpty(Url))
            {
                if (log != null) log(ClassName, "CreatDirectory", Log_Type.Error,SystemMessage.RefNullOrEmpty("Connection",language) );
                if (this.HasError != null)
                    this.HasError(ClassName, "CreatDirectory", new NullReferenceException(SystemMessage.RefNullOrEmpty("Connection", language)));
                else
                    throw new NullReferenceException(SystemMessage.RefNullOrEmpty("Connection", language));
                return (int)FtpStatusCode.Undefined; 
            }
            if (string.IsNullOrEmpty(FolderName))
            {
                if (log != null) log(ClassName, "CreatDirectory", Log_Type.Error, SystemMessage.RefNullOrEmpty("FolderName", language));
                if (this.HasError != null)
                    this.HasError(ClassName, "CreatDirectory", new NullReferenceException(SystemMessage.RefNullOrEmpty("FolderName", language)));
                else
                    throw new NullReferenceException(SystemMessage.RefNullOrEmpty("FolderName", language));
                return (int)FtpStatusCode.Undefined;
            }
            try
            {
                string str = Url + "/" + FolderName.Trim();
                if (log != null) log(ClassName, "CreatDirectory", Log_Type.Test, SystemMessage.RefValDisp("URL", str,language));
                InitFtp(str, WebRequestMethods.Ftp.MakeDirectory);
                listResponse = (FtpWebResponse)listRequest.GetResponse();
                FtpStatusCode res = listResponse.StatusCode;
                if (log != null) log(ClassName, "CreatDirectory", Log_Type.Infomation, SystemMessage.RefValDisp("RESP" , res.ToString(),language));
                listResponse.Close();
                listRequest = null;
                return (int)res; 
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, "CreatDirectory", Log_Type.Error, ex.Message);
                if (this.HasError != null)
                    this.HasError(ClassName, "CreatDirectory", ex);
                else
                    throw ex;
                return (int)FtpStatusCode.Undefined;
            }
        }

        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="FolderName">目录名</param>
        /// <returns>执行结果，FTP返回码</returns>
        public int DeleteDirectory(string FolderName)
        {
            string Url = UrlMake();
            if (string.IsNullOrEmpty(Url))
            {
                if (log != null) log(ClassName, "DeleteDirectory", Log_Type.Error, SystemMessage.RefNullOrEmpty("Connection", language));
                if (this.HasError != null)
                    this.HasError(ClassName, "DeleteDirectory", new NullReferenceException(SystemMessage.RefNullOrEmpty("Connection", language)));
                else
                    throw new NullReferenceException(SystemMessage.RefNullOrEmpty("Connection", language));
                return (int)FtpStatusCode.Undefined;
            }
            if (string.IsNullOrEmpty(FolderName))
            {
                if (log != null) log(ClassName, "DeleteDirectory", Log_Type.Error, SystemMessage.RefNullOrEmpty("FolderName", language));
                if (this.HasError != null)
                    this.HasError(ClassName, "DeleteDirectory", new NullReferenceException(SystemMessage.RefNullOrEmpty("FolderName", language)));
                else
                    throw new NullReferenceException(SystemMessage.RefNullOrEmpty("FolderName", language));
                return (int)FtpStatusCode.Undefined;
            }
            try
            {
                string str = Url + "/" + FolderName.Trim();
                if (log != null) log(ClassName, "DeleteDirectory", Log_Type.Test, SystemMessage.RefValDisp("URL", str, language));
                InitFtp(str, WebRequestMethods.Ftp.RemoveDirectory);
                listResponse = (FtpWebResponse)listRequest.GetResponse();
                FtpStatusCode res = listResponse.StatusCode;
                if (log != null) log(ClassName, "DeleteDirectory", Log_Type.Infomation, SystemMessage.RefValDisp("RESP", res.ToString(), language));
                listResponse.Close();
                listRequest = null;
                return (int)res;
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, "DeleteDirectory", Log_Type.Error, ex.Message);
                if (this.HasError != null)
                    this.HasError(ClassName, "DeleteDirectory", ex);
                else
                    throw ex;
                return (int)FtpStatusCode.Undefined;
            }
        }

        /// <summary>
        /// 重命名目录
        /// </summary>
        /// <param name="FolderName">目录名</param>
        /// <param name="NewFolderName">新文件名</param>
        /// <returns>执行结果，FTP返回码</returns>
        public int RenameDirectory(string FolderName, string NewFolderName)
        {
            string Url = UrlMake();
            if (string.IsNullOrEmpty(Url))
            {
                if (log != null) log(ClassName, "RenameDirectory", Log_Type.Error, SystemMessage.RefNullOrEmpty("Connection", language));
                if (this.HasError != null)
                    this.HasError(ClassName, "RenameDirectory", new NullReferenceException(SystemMessage.RefNullOrEmpty("Connection", language)));
                else
                    throw new NullReferenceException(SystemMessage.RefNullOrEmpty("Connection", language));
                return (int)FtpStatusCode.Undefined;
            }
            if (string.IsNullOrEmpty(FolderName))
            {
                if (log != null) log(ClassName, "RenameDirectory", Log_Type.Error, SystemMessage.RefNullOrEmpty("FolderName", language));
                if (this.HasError != null)
                    this.HasError(ClassName, "RenameDirectory", new NullReferenceException(SystemMessage.RefNullOrEmpty("FolderName", language)));
                else
                    throw new NullReferenceException(SystemMessage.RefNullOrEmpty("FolderName", language));
                return (int)FtpStatusCode.Undefined;
            }
            try
            {
                string str = Url + "/" + FolderName.Trim();
                if (log != null) log(ClassName, "RenameDirectory", Log_Type.Test, SystemMessage.RefValDisp("URL", str, language));
                InitFtp(str, WebRequestMethods.Ftp.RemoveDirectory);
                listResponse = (FtpWebResponse)listRequest.GetResponse();
                FtpStatusCode res = listResponse.StatusCode;
                if (log != null) log(ClassName, "RenameDirectory", Log_Type.Infomation, SystemMessage.RefValDisp("RESP", res.ToString(), language));
                listResponse.Close();
                listRequest = null;
                return (int)res;
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, "RenameDirectory", Log_Type.Error, ex.Message);
                if (this.HasError != null)
                    this.HasError(ClassName, "RenameDirectory", ex);
                else
                    throw ex;
                return (int)FtpStatusCode.Undefined;
            }
        }

        /// <summary>
        /// 列出文件目录
        /// </summary>
        /// <param name="FolderName">目录名</param>
        /// <returns>文件名列表</returns>
        public List<string> ListDirectory(string FolderName)
        {
            List<string> res = new List<string>();
            string Url = UrlMake();
            if (string.IsNullOrEmpty(Url))
            {
                if (log != null) log(ClassName, "ListDirectory", Log_Type.Error, SystemMessage.RefNullOrEmpty("Connection", language));
                if (this.HasError != null)
                    this.HasError(ClassName, "ListDirectory", new NullReferenceException(SystemMessage.RefNullOrEmpty("Connection", language)));
                else
                    throw new NullReferenceException(SystemMessage.RefNullOrEmpty("Connection", language));
                return res;
            }
            try
            { 
                string str = Url;
                if(!string.IsNullOrEmpty (FolderName))
                    str = Url + "/" + FolderName;
                if (log != null) log(ClassName, "ListDirectory", Log_Type.Test, SystemMessage.RefValDisp("URL", str, language));
                InitFtp(str, WebRequestMethods.Ftp.ListDirectory); 
                listResponse = (FtpWebResponse)listRequest.GetResponse();
                if (log != null) log(ClassName, "ListDirectory", Log_Type.Infomation, SystemMessage.RefValDisp("RESP", res.ToString(), language));
                Stream responseStream = listResponse.GetResponseStream();
                StreamReader readStream = new StreamReader(responseStream, System.Text.Encoding.Default);
                while (readStream.Peek() >= 0)
                {
                    str = readStream.ReadLine();
                    res.Add(str);
                }
                listResponse.Close(); 
                listRequest = null; 
                 
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, "ListDirectory", Log_Type.Error, ex.Message);
                if (this.HasError != null)
                    this.HasError(ClassName, "ListDirectory", ex);
                else
                    throw ex;
            }
            return res;
        }

        /// <summary>
        /// 获取远程文件大小
        /// </summary>
        /// <param name="FileName">文件名</param>
        /// <param name="FolderName">目录名</param>
        /// <returns>文件大小，字节</returns>
        public long FileSize(string FileName,string FolderName="")
        {
            string Url = UrlMake();
            if (string.IsNullOrEmpty(Url))
            {
                if (log != null) log(ClassName, "FileSize", Log_Type.Error, SystemMessage.RefNullOrEmpty("Connection", language));
                if (this.HasError != null)
                    this.HasError(ClassName, "FileSize", new NullReferenceException(SystemMessage.RefNullOrEmpty("Connection", language)));
                else
                    throw new NullReferenceException(SystemMessage.RefNullOrEmpty("Connection", language));
                return (int)FtpStatusCode.Undefined;
            }
            if (string.IsNullOrEmpty(FileName))
            {
                if (log != null) log(ClassName, "FileSize", Log_Type.Error, SystemMessage.RefNullOrEmpty("FileName", language));
                if (this.HasError != null)
                    this.HasError(ClassName, "FileSize", new NullReferenceException(SystemMessage.RefNullOrEmpty("FileName", language)));
                else
                    throw new NullReferenceException(SystemMessage.RefNullOrEmpty("FileName", language));
                return (int)FtpStatusCode.Undefined;
            }
            try
            {
                if (string.IsNullOrEmpty(FolderName))
                    FolderName = "";
                string str = Url;
                if (!string.IsNullOrEmpty(FolderName))
                     str = Url + "/" + FolderName;
                str = str + "/" + FileName;
                if (log != null) log(ClassName, "FileSize", Log_Type.Test, SystemMessage.RefValDisp("URL", str, language));
                InitFtp(str, WebRequestMethods.Ftp.GetFileSize);
                listResponse = (FtpWebResponse)listRequest.GetResponse();
                if (log != null) log(ClassName, "FileSize", Log_Type.Infomation, SystemMessage.RefValDisp("RESP", listResponse.StatusCode.ToString(), language));
                long upsize = 0;
                try
                {
                    string[] res = listResponse.StatusDescription.Replace("\r\n", "").Split(' ');
                    upsize = long.Parse(res[1]);
                }
                catch { }
                listResponse.Close();
                listRequest = null;
                if (log != null) log(ClassName, "FileSize", Log_Type.Test, SystemMessage.RefValDisp("FileSize", upsize.ToString(), language)  );
                return upsize;
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, "FileSize", Log_Type.Error, ex.Message);
                if (this.HasError != null)
                    this.HasError(ClassName, "FileSize", ex);
                else
                    throw ex;
                return 0;
            }
        }

        /// <summary>
        /// 删除远程文件
        /// </summary>
        ///<param name="FileName">文件名</param>
        /// <param name="FolderName">目录名</param>
        /// <returns>执行结果，FTP返回码</returns>
        public int DeleteFile(string FileName, string FolderName="")
        {
            string Url = UrlMake();
            if (string.IsNullOrEmpty(Url))
            {
                if (log != null) log(ClassName, "DeleteFile", Log_Type.Error, SystemMessage.RefNullOrEmpty("Connection", language));
                if (this.HasError != null)
                    this.HasError(ClassName, "DeleteFile", new NullReferenceException(SystemMessage.RefNullOrEmpty("Connection", language)));
                else
                    throw new NullReferenceException(SystemMessage.RefNullOrEmpty("Connection", language));
                return (int)FtpStatusCode.Undefined;
            }
            if (string.IsNullOrEmpty(FileName))
            {
                if (log != null) log(ClassName, "DeleteFile", Log_Type.Error, SystemMessage.RefNullOrEmpty("FileName", language));
                if (this.HasError != null)
                    this.HasError(ClassName, "DeleteFile", new NullReferenceException(SystemMessage.RefNullOrEmpty("FileName", language)));
                else
                    throw new NullReferenceException(SystemMessage.RefNullOrEmpty("FileName", language));
                return (int)FtpStatusCode.Undefined;
            }
            try
            {
                if (string.IsNullOrEmpty(FolderName))
                    FolderName = "";
                string str = Url;
                if (!string.IsNullOrEmpty(FolderName))
                    str = Url + "/" + FolderName;
                str = str + "/" + FileName;
                if (log != null) log(ClassName, "DeleteFile", Log_Type.Test, SystemMessage.RefValDisp("URL", str, language));
                InitFtp(str, WebRequestMethods.Ftp.DeleteFile);
                listResponse = (FtpWebResponse)listRequest.GetResponse();
                FtpStatusCode res = listResponse.StatusCode;
                if (log != null) log(ClassName, "DeleteFile", Log_Type.Infomation, SystemMessage.RefValDisp("RESP", res.ToString(), language));
                listResponse.Close();
                listRequest = null;
                return (int)res;
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, "DeleteFile", Log_Type.Error, ex.Message);
                if (this.HasError != null)
                    this.HasError(ClassName, "DeleteFile", ex);
                else
                    throw ex;
                return (int)FtpStatusCode.Undefined;
            }
        }

        /// <summary>
        /// 上传文件，带断点续传
        /// </summary>
        /// <param name="FolderName">目录名</param>
        /// <param name="FileName">文件名</param>
        /// <param name="LocalPath">本地路径</param>
        /// <param name="LocalFile">本地文件名，为空则用FileName</param>
        /// <returns>执行结果</returns>
        public bool UpLoadFile(string FileName, string LocalPath, string FolderName="", string LocalFile = "")
        {
            string Url = UrlMake();
            if (string.IsNullOrEmpty(Url))
            {
                if (log != null) log(ClassName, "UpLoadFile", Log_Type.Error, SystemMessage.RefNullOrEmpty("Connection", language));
                if (this.HasError != null)
                    this.HasError(ClassName, "UpLoadFile", new NullReferenceException(SystemMessage.RefNullOrEmpty("Connection", language)));
                else
                    throw new NullReferenceException(SystemMessage.RefNullOrEmpty("Connection", language));
                return false;
            }
            if (string.IsNullOrEmpty(FileName))
            {
                if (log != null) log(ClassName, "UpLoadFile", Log_Type.Error, SystemMessage.RefNullOrEmpty("FileName", language));
                if (this.HasError != null)
                    this.HasError(ClassName, "UpLoadFile", new NullReferenceException(SystemMessage.RefNullOrEmpty("FileName", language)));
                else
                    throw new NullReferenceException(SystemMessage.RefNullOrEmpty("FileName", language));
                return false;
            }
            try
            {
                int BufferSize = 1024;
                if (this.Refer_Prama != null)
                {
                    if (this.Refer_Prama.SendBufferSize > 0)
                        BufferSize = this.Refer_Prama.SendBufferSize;
                }
                if (string.IsNullOrEmpty(FolderName))
                    FolderName = "";
                if (string.IsNullOrEmpty(LocalFile))
                    LocalFile = FileName;
                FileInfo fi = new FileInfo(LocalPath + "\\" + LocalFile);
                if (log != null) log(ClassName, "UpLoadFile", Log_Type.Test, SystemMessage.RefValDisp("FileName", fi.FullName, language));
                if (!fi.Exists)
                     throw new FileLoadException("FILE NOT EXISTS");
                string str = Url;
                if (!string.IsNullOrEmpty(FolderName))
                    str = Url + "/" + FolderName;
                str = str + "/" + FileName;
                if (log != null) log(ClassName, "UpLoadFile", Log_Type.Test, SystemMessage.RefValDisp("URL", str, language));
                InitFtp(str, WebRequestMethods.Ftp.GetFileSize);
                listResponse = (FtpWebResponse)listRequest.GetResponse();
                long upsize = 0;
                try
                {
                    string[] res = listResponse.StatusDescription.Replace("\r\n", "").Split(' ');
                    upsize = long.Parse(res[1]);
                }catch  { }
                listResponse.Close();
                listRequest.Abort();
                if (log != null) log(ClassName, "UpLoadFile", Log_Type.Test, SystemMessage.RefValDisp("FileSize", upsize.ToString(), language) );
                if (upsize > fi.Length)
                {
                    InitFtp(str, WebRequestMethods.Ftp.DeleteFile);
                    listResponse = (FtpWebResponse)listRequest.GetResponse();
                    listResponse.Close();
                    listResponse = null;
                    listRequest = null;
                }
                if (log != null) log(ClassName, "UpLoadFile", Log_Type.Infomation, SystemMessage.ExecStart(language));
                InitFtp(str, WebRequestMethods.Ftp.AppendFile);
                listRequest.ContentLength = fi.Length;
                listRequest.KeepAlive = true;
                listRequest.UseBinary = true;
                listRequest.UsePassive = true;
                byte[] content = new byte[BufferSize];
                int dataRead = 0;
                using (FileStream fs = fi.OpenRead())
                {
                    try
                    {
                        using (Stream rs = listRequest.GetRequestStream())
                        {
                            do
                            {
                                fs.Seek(upsize, SeekOrigin.Begin);
                                dataRead = fs.Read(content, 0, BufferSize);
                                rs.Write(content, 0, dataRead);
                                upsize += dataRead;
                            }
                            while (!(dataRead < BufferSize));
                        }
                    }
                    catch
                    { }
                    finally
                    { fs.Close(); }
                }
                if (log != null) log(ClassName, "UpLoadFile", Log_Type.Infomation, SystemMessage.ExecOK(language));
                listRequest.Abort();
                listRequest = null;
                return true; 
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, "UpLoadFile", Log_Type.Error, ex.Message);
                if (this.HasError != null)
                    this.HasError(ClassName, "UpLoadFile", ex);
                else
                    throw ex;
                return false;
            }
        }

        /// <summary>
        /// 下载文件，带断点下载
        /// </summary>
        /// <param name="FolderName">目录名</param>
        /// <param name="FileName">文件名</param>
        /// <param name="LocalPath">本地路径</param>
        /// <param name="LocalFile">本地文件名，为空则用FileName</param>
        /// <param name="BreakPoint">重新下载</param>
        /// <param name="Checked">是否对比大小</param>
        /// <param name="BufferSize">缓存大小</param>
        /// <returns>执行结果</returns>
        public bool DownLoadFile(string FileName, string LocalPath, string FolderName = "", string LocalFile = "", bool BreakPoint = true)
        {
            string Url = UrlMake();
            if (string.IsNullOrEmpty(Url))
            {
                if (log != null) log(ClassName, "DownLoadFile", Log_Type.Error, SystemMessage.RefNullOrEmpty("Connection", language));
                if (this.HasError != null)
                    this.HasError(ClassName, "DownLoadFile", new NullReferenceException(SystemMessage.RefNullOrEmpty("Connection", language)));
                else
                    throw new NullReferenceException(SystemMessage.RefNullOrEmpty("Connection", language));
                return false;
            }
            if (string.IsNullOrEmpty(FileName))
            {
                if (log != null) log(ClassName, "DownLoadFile", Log_Type.Error, SystemMessage.RefNullOrEmpty("FileName", language));
                if (this.HasError != null)
                    this.HasError(ClassName, "DownLoadFile", new NullReferenceException(SystemMessage.RefNullOrEmpty("FileName", language)));
                else
                    throw new NullReferenceException(SystemMessage.RefNullOrEmpty("FileName", language));
                return false;
            }
            if (string.IsNullOrEmpty(LocalPath))
            {
                if (log != null) log(ClassName, "DownLoadFile", Log_Type.Error, SystemMessage.RefNullOrEmpty("LocalPath", language));
                if (this.HasError != null)
                    this.HasError(ClassName, "DownLoadFile", new NullReferenceException(SystemMessage.RefNullOrEmpty("LocalPath", language)));
                else
                    throw new NullReferenceException(SystemMessage.RefNullOrEmpty("LocalPath", language));
                return false;
            }
            try
            {
                int BufferSize = 1024;
                if (this.Refer_Prama != null)
                {
                    if (this.Refer_Prama.SendBufferSize > 0)
                        BufferSize = this.Refer_Prama.SendBufferSize;
                }
                if (string.IsNullOrEmpty(FolderName))
                        FolderName = "";
                if (string.IsNullOrEmpty(LocalFile))
                        LocalFile = FileName;
                string str = Url;
                if (!string.IsNullOrEmpty(FolderName))
                    str = Url + "/" + FolderName;
                str = str + "/" + FileName;
                DirectoryInfo di = new DirectoryInfo(LocalPath);
                if (!di.Exists)
                    di.Create();
                if (log != null) log(ClassName, "DownLoadFile", Log_Type.Test, SystemMessage.RefValDisp("URL", str, language));
                InitFtp(str, WebRequestMethods.Ftp.GetFileSize);
                listResponse = (FtpWebResponse)listRequest.GetResponse();
                long upsize = 0;
                try
                {
                    string[] res = listResponse.StatusDescription.Replace("\r\n", "").Split(' ');
                    upsize = long.Parse(res[1]);
                }
                catch{ }
                listResponse.Close(); 
                listRequest.Abort();
                if (log != null) log(ClassName, "DownLoadFile", Log_Type.Test, SystemMessage.RefValDisp("FileSize", upsize.ToString(), language));
                InitFtp(str, WebRequestMethods.Ftp.DownloadFile);
                listRequest.KeepAlive = true;
                listRequest.UseBinary = true;
                listRequest.UsePassive = true;
                FileInfo fi = null;
                FileStream fs= null;
                if (BreakPoint)
                {
                    fi = new FileInfo(LocalPath + "\\" + LocalFile + ".tmp");
                    if (fi.Exists)
                    {
                        listRequest.ContentOffset = fi.Length;
                        fs = new FileStream(fi.FullName, FileMode.Append, FileAccess.Write);
                    }
                    else
                        fs = new FileStream(fi.FullName, FileMode.Create, FileAccess.Write);
                }
                else
                {
                    fi = new FileInfo(LocalPath + "\\" + LocalFile);
                    FileName = fi.Name.Replace(fi.Extension, "") + "_副本" + fi.Extension;
                    if (fi.Exists)
                        fi = new FileInfo(LocalPath + "\\" + FileName);
                    fs = new FileStream(fi.FullName, FileMode.Create , FileAccess.Write);
                }
                if (log != null) log(ClassName, "DownLoadFile", Log_Type.Test, fi.FullName);
                bool resdo = false;
                try
                {
                    listResponse = (FtpWebResponse)listRequest.GetResponse();
                    Stream resStrm = listResponse.GetResponseStream();
                    if (log != null) log(ClassName, "DownLoadFile", Log_Type.Infomation, SystemMessage.ExecStart(language));
                    byte[] buffer = new byte[BufferSize];
                    while (true)
                    {
                        int readSize = resStrm.Read(buffer, 0, buffer.Length);
                        if (readSize == 0)break;
                        fs.Write(buffer, 0, readSize);
                    }
                    resStrm.Close();
                    listResponse.Close();
                    listResponse = null;
                    if (BreakPoint)
                    {
                        fi.CopyTo(LocalPath + "\\" + LocalFile, true);
                        try { File.Delete(LocalPath + "\\" + LocalFile + ".tmp"); }
                        catch { }
                    }
                    if (log != null) log(ClassName, "DownLoadFile", Log_Type.Test, SystemMessage.ExecOK(language));
                    resdo = true;
                }
                catch (Exception exx)
                {
                    if (this.HasError != null)
                        this.HasError(ClassName, "DownLoadFile", exx);
                    resdo = false;
                }
                finally
                {
                    fs.Close();
                    listRequest = null;
                } 
                return resdo;
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, "DownLoadFile", Log_Type.Error, ex.Message);
                if (this.HasError != null)
                    this.HasError(ClassName, "DownLoadFile", ex);
                else
                    throw ex;
                return false;
            }
        }

        #endregion
    }
}
