using System;
using System.ComponentModel;
using NK.ENum;
using System.Net;
using System.Net.Sockets;
namespace NK.Class
{
    /// <summary>
    /// HTTP服务Session
    /// </summary>
    [DisplayName("HTTP服务Session")]
    [Description("承载通信信息的实体")]
    public class HttpListenerSession
    {
        /// <summary>
        /// 会话ID
        /// </summary>
        [DisplayName("会话ID")]
        [Description("建立会话的编号")]
        public long SessionID { get; set; } 
        /// <summary>
        /// 连接参数
        /// </summary>
        [DisplayName("连接参数")]
        [Description("JSON参数")]
        public string Connection { get; set; }
        /// <summary>
        /// 远端连接参数
        /// </summary>
        [DisplayName("远端连接参数")]
        [Description("远端设备连接的连接参数")]
        public string Remote { get; set; }
        /// <summary>
        /// ListNO
        /// </summary>
        [DisplayName("流水号")]
        [Description("通信流水记录")]
        public string ListNO { get; set; }
        /// <summary>
        /// URL相对地址
        /// </summary>
        [DisplayName("URL相对地址")]
        [Description("请求的URL相对地址")]
        public string UrlRef { get; set; }
        /// <summary>
        /// 请求信息
        /// </summary>
        [DisplayName("请求信息")]
        [Description("请求信息")]
        public HttpListenerRequest Request { get; set; }
        /// <summary>
        /// 返回信息
        /// </summary>
        [DisplayName("返回信息")]
        [Description("返回信息")] 
        public HttpListenerResponse Response { get; set; }

    }

}
