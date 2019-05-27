using System.ComponentModel;
using LinqToDB.Mapping;
using NK.ENum;
namespace NK.Entity
{
    /// <summary>
    /// 性能参数
    /// </summary>
    [DisplayName("性能参数")]
    [Description("性能参数")]
    [Table(Name = "ReferSet")]
    public class ReferSet
    {
        /// <summary>
        /// 主键
        /// </summary>
        [DisplayName("主键")]
        [Description("主键")]
        [Column(Name = "ID", IsPrimaryKey = true, IsIdentity = true, CanBeNull = false )]
        public int ID { get; set; }
        /// <summary>
        /// 配置名
        /// </summary>
        [DisplayName("配置名")]
        [Description("配置名")]
        [Column(Name = "ConfigName", CanBeNull = false)]
        public string ConfigName { get; set; }
        /// <summary>
        /// 归属
        /// </summary>
        [DisplayName("归属")]
        [Description("归属")]
        [Column(Name = "IMode", CanBeNull = false)]
        public ReferForUse IMode { get; set; }
        /// <summary>
        /// 重试次数
        /// </summary>
        [DisplayName("重试次数")]
        [Description("重试次数")]
        [Column(Name = "ReTry", CanBeNull = true)]
        public int ReTry { get; set; }
        /// <summary>
        /// 连接超时时间，毫秒
        /// </summary>
        [DisplayName("连接超时时间")]
        [Description("连接超时时间")]
        [Column(Name = "ConnectTimeOut", CanBeNull = true)]
        public int ConnectTimeOut { get; set; }
        /// <summary>
        /// 检测连接超时，分钟
        /// </summary>
        [DisplayName("检测连接超时")]
        [Description("检测连接超时")]
        [Column(Name = "CheckAliveTime", CanBeNull = true)]
        public int CheckAliveTime { get; set; }
        /// <summary>
        /// 等待时间，毫秒
        /// </summary>
        [DisplayName("等待时间")]
        [Description("等待时间")]
        [Column(Name = "WaitTime", CanBeNull = true)]
        public int WaitTime { get; set; }
        /// <summary>
        /// 执行超时，毫秒
        /// </summary>
        [DisplayName("执行超时")]
        [Description("执行超时")]
        [Column(Name = "ExecTime", CanBeNull = true)]
        public int ExecTime { get; set; }
        /// <summary>
        ///  发送超时，毫秒
        /// </summary>
        [DisplayName("发送超时")]
        [Description("发送超时")]
        [Column(Name = "SendTimeout", CanBeNull = true)]
        public int SendTimeout { get; set; }
        /// <summary>
        /// 发送缓存，B
        /// </summary>
        [DisplayName("发送缓存")]
        [Description("发送缓存")]
        [Column(Name = "SendBufferSize", CanBeNull = true)]
        public int SendBufferSize { get; set; }
        /// <summary>
        /// 接收超时，毫秒
        /// </summary>
        [DisplayName("接收超时")]
        [Description("接收超时")]
        [Column(Name = "ReceiveTimeout", CanBeNull = true)]
        public int ReceiveTimeout { get; set; }
        /// <summary>
        /// 接收缓存,B
        /// </summary>
        [DisplayName("接收缓存")]
        [Description("接收缓存")]
        [Column(Name = "ReceiveBufferSize", CanBeNull = true)]
        public int ReceiveBufferSize { get; set; }
        /// <summary>
        /// 监听连接数
        /// </summary>
        [DisplayName("监听连接数")]
        [Description("监听连接数")]
        [Column(Name = "ConnPool", CanBeNull = true)]
        public int ConnPool { get; set; }
        /// <summary>
        /// 配置名
        /// </summary>
        [DisplayName("字符编码")]
        [Description("字符编码")]
        [Column(Name = "CharSet", CanBeNull = false)]
        public string CharSet { get; set; }
        /// <summary>
        /// 调试模式
        /// </summary>
        [DisplayName("调试模式")]
        [Description("调试模式")]
        [Column(Name = "Debug", CanBeNull = true)]
        public Log_Type Debug { get; set; }
    }
}
