using System.ComponentModel;
namespace NK.ENum
{  
    /// <summary>
    /// 配置保存方式
    /// </summary>
[Description("配置保存方式")]
    public enum Save_Mode : byte
    {
        /// <summary>
        /// 无
        /// </summary>
        [Description("无")]
        None = 0,
        /// <summary>
        /// 保存文本
        /// </summary>
        [Description("保存文本")]
        JSON = 1,
        /// <summary>
        /// 保存XML
        /// </summary>
        [Description("保存XML")]
        XML = 2,
        /// <summary>
        /// 保存数据库
        /// </summary>
        [Description("保存数据库")]
        DB = 3,
    }
}
