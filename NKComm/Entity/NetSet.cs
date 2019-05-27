using System.ComponentModel;
using System.Net.Sockets;
using LinqToDB.Mapping;
using NK.ENum;
namespace NK.Entity
{
    /// <summary>
    /// 以太网参数
    /// </summary>
    [DisplayName("网络参数")]
    [Description("网络参数")]
    [Table(Name = "NetSet")]
    public class NetSet
    {
        /// <summary>
        /// 主键
        /// </summary>
        [DisplayName("主键")]
        [Description("主键")]
        [Column(Name = "ID", IsPrimaryKey = true, IsIdentity = true, CanBeNull = false)]
        public int ID { get; set; }
        /// <summary>
        /// 配置名
        /// </summary>
        [DisplayName("配置名")]
        [Description("配置名")]
        [Column(Name = "ConfigName", CanBeNull = false)]
        public string ConfigName { get; set; }
        /// <summary>
        /// 寻址
        /// </summary>
        [DisplayName("寻址")]
        [Description("寻址")]
        [Column(Name = "AddressFamily", CanBeNull = false)]
        public AddressFamily Address_Family { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        [DisplayName("类型")]
        [Description("类型")]
        [Column(Name = "SocketType", CanBeNull = false)]
        public SocketType Socket_Type { get; set; }
        /// <summary>
        /// 协议
        /// </summary>
        [DisplayName("协议")]
        [Description("协议")]
        [Column(Name = "ProtocolType", CanBeNull = false)]
        public ProtocolType Protocol_Type { get; set; }
        /// <summary>
        ///类型
        /// </summary>
        [DisplayName("类型")]
        [Description("类型")]
        [Column(Name = "NetMode", CanBeNull = false )]
        public Net_Mode Mode { get; set; }
        /// <summary>
        /// 目标IP地址
        /// </summary>
        [DisplayName("IP地址")]
        [Description("IP地址")]
        [Column(Name = "IPAddress", CanBeNull = true)]
        public string IPAddress { get; set; }
        /// <summary>
        /// 目标发送端口
        /// </summary>
        [DisplayName("端口")]
        [Description("端口")]
        [Column(Name = "Port", CanBeNull = true)]
        public int Port { get; set; }
        /// <summary>
        /// 域名
        /// </summary>
        [DisplayName("域名")]
        [Description("域名")]
        [Column(Name = "DomainName", CanBeNull = true)]
        public string DomainName { get; set; }
        /// <summary>
        /// 地址参数
        /// </summary>
        [DisplayName("地址参数")]
        [Description("地址参数")]
        [Column(Name = "AddrRef", CanBeNull = true)]
        public string AddrRef { get; set; }
        /// <summary>
        /// 设备编址
        /// </summary>
        [DisplayName("设备编址")]
        [Description("设备编址")]
        [Column(Name = "Address", CanBeNull = true)]
        public string Address { get; set; }
        /// <summary>
        /// 启用配置
        /// </summary>
        [DisplayName("启用配置")]
        [Description("启用配置")]
        [Column(Name = "Enable", CanBeNull = false)]
        public bool Enable { get; set; }
    }
}
