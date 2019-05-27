using System.ComponentModel;
namespace NK.ENum
{
    /// <summary>
    /// 校验类型
    /// </summary>
    [Description("校验类型")]
    public enum CheckSum_Mode : byte
    {
        /// <summary>
        /// 无
        /// </summary>
        [Description("无")]
        None = 0,
        /// <summary>
        /// CRC8
        /// </summary>
        [Description("CRC8")]
        CRC8 = 1,
        /// <summary>
        /// CRC16
        /// </summary>
        [Description("CRC16")]
        CRC16 = 2,
        /// <summary>
        /// CRC32
        /// </summary>
        [Description("CRC32")]
        CRC32 = 3,
        /// <summary>
        /// XOR
        /// </summary>
        [Description("XOR")]
        XOR = 4
    }
}
