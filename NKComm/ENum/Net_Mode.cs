using System.ComponentModel;
namespace NK.ENum
{
    /// <summary>
    /// 网络模式
    /// </summary>
    [Description("网络模式")]
    public  enum Net_Mode
    {
        /// <summary>
        /// 无
        /// </summary>
        [Description("无")]
        None = 0,
        /// <summary>
        /// 本地
        /// </summary>
        [Description("本地")]
        Local = 1,
        /// <summary>
        /// 远程
        /// </summary>
        [Description("远程")]
        Remote = 2,
 
    }
}
