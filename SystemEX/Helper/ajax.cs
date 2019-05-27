using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
namespace NK
{
    /// <summary>
    /// 页面HTTP请求
    /// </summary>
    public static class Ajax
    {
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="data">数据</param>
        /// <param name="type">类型</param>
        /// <param name="Done">成功</param>
        /// <param name="Fail">失败</param>
        /// <param name="ContentType">连接类型</param>
        public static void ajax(string url,string data,string type="POST", Action<string> Done=null, Action<int,string> Fail = null,string ContentType="")
        {
            if (type.ToLower().Trim() == "get")            
            {
                if (url.Contains("?"))
                    url += "&" + data;  
                else
                    url += "?" + data;
            }
            HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            if (!string.IsNullOrEmpty(ContentType)) myHttpWebRequest.ContentType = ContentType;
            if (type.ToLower().Trim() == "post")
            {
                if (string.IsNullOrEmpty(data))
                    myHttpWebRequest.ContentLength = 0;
                else  
                {
                    byte[] tmp = Encoding.UTF8.GetBytes(data);
                    myHttpWebRequest.ContentLength = data.Length;
                    Stream newStream = myHttpWebRequest.GetRequestStream();
                    newStream.Write(tmp, 0, tmp.Length);
                    newStream.Close();
                } 
            }
            try
            {
                HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                int iStatCode = (int)myHttpWebResponse.StatusCode;
                Stream streamReceive = myHttpWebResponse.GetResponseStream();
                MemoryStream stmMemory = new MemoryStream();
                byte[] buffer = new byte[64 * 1024];
                int i;
                while ((i = streamReceive.Read(buffer, 0, buffer.Length)) > 0)
                {
                    stmMemory.Write(buffer, 0, i);
                }
                byte[] arraryByte = stmMemory.ToArray();
                stmMemory.Close();
                if (iStatCode == 200)
                { 
                    if (Done != null) Done(System.Text.Encoding.Default.GetString(arraryByte));
                }
                else if (Fail != null)
                {
                    Fail(iStatCode, System.Text.Encoding.Default.GetString(arraryByte));
                }
            }
            catch (WebException ex)
            {
                if (Fail != null)
                {
                    HttpWebResponse response = (HttpWebResponse)ex.Response; 
                    StreamReader readers = new StreamReader(response.GetResponseStream());
                    string Html = readers.ReadToEnd();
                    readers.Close();
                    Fail((int)response.StatusCode, Html); 
                }
            } 
        }

        /// <summary>
        /// POST数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postdata"></param>
        /// <returns></returns>
        public static byte[] Post(string url, byte[] postdata)
        {
            using (WebClient webClient = new WebClient())
            { 
               return webClient.UploadData(url, "POST", postdata);
            }
        }

        /// <summary>
        /// POST数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postdata"></param>
        /// <returns></returns>
        public static string Post(string url, string postdata)
        {
            using (WebClient webClient = new WebClient())
            {
                byte[] postData = Encoding.UTF8.GetBytes(postdata);
                byte[] responseData = webClient.UploadData(url, "POST", postData);
                return Encoding.UTF8.GetString(responseData);
            }
        }

        /// <summary>
        /// POST数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postdata"></param>
        /// <returns></returns>
        public static string Post(string url, Dictionary<string,string> postdata)
        {
            using (WebClient webClient = new WebClient())
            {
                NameValueCollection myParameters = new NameValueCollection();
                foreach (var dic in postdata) myParameters.Add(dic.Key,dic.Value);
                webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                webClient.Encoding = Encoding.UTF8;
                return Encoding.UTF8.GetString(webClient.UploadValues(url, myParameters));
            }
        }

        /// <summary>
        /// GET数据
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static byte[] Get(string url)
        {
            using (WebClient webClient = new WebClient())
            {
                return webClient.DownloadData(url);
            }
        }

        /// <summary>
        /// GET数据
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetString(string url )
        {
            using (WebClient webClient = new WebClient())
            {
                return Encoding.UTF8.GetString(webClient.DownloadData(url));
            }
        }


    }
}
