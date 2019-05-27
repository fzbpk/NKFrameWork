using System.ComponentModel;
namespace NK.ENum
{
    /// <summary>
    /// 日志类型
    /// </summary>
    [Description("日志类型")]
    public enum Log_Type : short
    {
        /// <summary>
        /// 无
        /// </summary>
        [Description("无")]
        None = 0,
        /// <summary>
        /// 信息
        /// </summary>
        [Description("信息")]
        Infomation = 1,
        /// <summary>
        /// 错误
        /// </summary>
        [Description("错误")]
        Error = 2,
        /// <summary>
        /// 测试
        /// </summary>
        [Description("测试")]
        Test = 3,
        /// <summary>
        /// 告警
        /// </summary>
        [Description("告警")]
        Alert = 4,
        /// <summary>
        /// 其他
        /// </summary>
        [Description("全部")]
        ALL = 5,
    }
}
