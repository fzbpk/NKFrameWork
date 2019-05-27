using System.ComponentModel;
namespace NK.ENum
{
    /// <summary>
    /// 性能参数归属
    /// </summary>
    [Description("性能参数归属")]
    public enum ReferForUse : byte
    {
        /// <summary>
        /// 无
        /// </summary>
        [Description("无")]
        None = 0,
        /// <summary>
        /// TCPSET
        /// </summary>
        [Description("NetSet")]
        NetSet ,
        /// <summary>
        /// UartSet
        /// </summary>
        [Description("UartSet")]
        UartSet ,
        /// <summary>
        /// UartSet
        /// </summary>
        [Description("USBSet")]
        USBSet,
        /// <summary>
        /// File
        /// </summary>
        [Description("File")]
        File ,
        /// <summary>
        /// API
        /// </summary>
        [Description("API")]
        API,
    }

}
