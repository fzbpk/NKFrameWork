using System.ComponentModel;
namespace NK.ENum
{
    /// <summary>
    /// 传统IO类型
    /// </summary>
   [Description("传统IO类型")]
    public enum Port_Mode
    {
        /// <summary>
        /// 无
        /// </summary>
        [Description("无")]
        None = 0,
        /// <summary>
        /// RS232
        /// </summary>
        [Description("RS232")]
        RS232 = 1,
        /// <summary>
        /// RS485
        /// </summary>
        [Description("RS485")]
        RS485 = 2,
        /// <summary>
        /// RS422
        /// </summary>
        [Description("RS422")]
        RS422 = 3,
        /// <summary>
        /// 并口
        /// </summary>
        [Description("LPT")]
        LPT = 4,
    }
}
