using System.ComponentModel;
namespace NK.ENum
{
    /// <summary>
    /// 加密类型
    /// </summary>
    [Description("加密类型")]
    public enum Security_Mode : byte
    {
        /// <summary>
        /// 无
        /// </summary>
        [Description("无")]
        None = 0,
        /// <summary>
        ///MD5
        /// </summary>
        [Description("MD5")]
        MD5 = 1,
        /// <summary>
        /// DES
        /// </summary>
        [Description("DES")]
        DES = 2,
        /// <summary>
        /// TripeDes
        /// </summary>
        [Description("TripeDes")]
        TripeDes = 3,
    }
}
