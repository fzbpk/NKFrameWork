using System.ComponentModel;
namespace NK.ENum
{ 
    /// <summary>
   /// 调试模式
   /// </summary>
    [Description("调试模式")]
    public enum Debug_Mode : byte
    {
        /// <summary>
        /// 无调试
        /// </summary>
        [Description("无调试")]
        None = 0,
        /// <summary>
        /// 保存文本
        /// </summary>
        [Description("保存文本")]
        TXT = 1,
        /// <summary>
        /// 触发事件
        /// </summary>
        [Description("触发事件")]
        Event = 2,
        /// <summary>
        /// 系统事件
        /// </summary>
        [Description("系统事件")]
        OS = 3,
    }
}
