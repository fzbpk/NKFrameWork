using System.ComponentModel;
namespace NK.ENum
{
    /// <summary>
    /// 操作类型
    /// </summary>
    [Description("操作类型")]
    public enum  Operate_Type : ushort 
    {
        /// <summary>
        /// 无
        /// </summary>
        [Description("无")]
        None = 0,
        /// <summary>
        /// 插入
        /// </summary>
        [Description("插入")]
        Insert = 1,
        /// <summary>
        /// 修改
        /// </summary>
        [Description("修改")]
        Update = 10,
        /// <summary>
        /// 删除
        /// </summary>
        [Description("删除")]
        DELETE = 100,
        /// <summary>
        /// 查
        /// </summary>
        [Description("查询")]
        Find = 1000,
        /// <summary>
        /// 列出
        /// </summary>
        [Description("列出")]
        List = 10000,
    }
}
