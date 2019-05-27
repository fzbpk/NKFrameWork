using System; 
using System.ComponentModel; 
using NK.ENum;
namespace NK.Class
{
    /// <summary>
    /// 通信Session
    /// </summary>
    [DisplayName("通信Session")]
    [Description("承载通信信息的实体")]
    public class CommunicateSession
    {
        /// <summary>
        /// 会话ID
        /// </summary>
        [DisplayName("会话ID")]
        [Description("建立会话的编号")]
        public long SessionID { get; set; }
        /// <summary>
        /// 连接类型
        /// </summary>
        [DisplayName("连接类型")]
        [Description("连接所使用的类型")]
        public ReferForUse LocalMode { get; set; }
        /// <summary>
        /// 连接参数
        /// </summary>
        [DisplayName("连接参数")]
        [Description("根据LocalMode指向类型JSON参数")]
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
        /// 连接时间
        /// </summary>
        [DisplayName("连接时间")]
        [Description("远端设备连上的时间")]
        public DateTime ConnectTime { get; set; }

    }
}
