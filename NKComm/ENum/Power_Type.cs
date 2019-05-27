using System.ComponentModel;
namespace NK.ENum
{
    /// <summary>
    /// 权限类型
    /// </summary>
    [Description("权限类型")]
    public enum Power_Type : byte
    {
        /// <summary>
        /// 无
        /// </summary>
        [Description("无")]
        None = 0,
        /// <summary>
        /// 无
        /// </summary>
        [Description("写入")]
        Write =1,
        /// <summary>
        /// 无
        /// </summary>
        [Description("读取")]
        Read = 2,
        /// <summary>
        /// 无
        /// </summary>
        [Description("完全控制")]
        ALL =3,
    }
}
