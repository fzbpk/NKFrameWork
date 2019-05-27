using System.ComponentModel;
using System.Net.Sockets;
using LinqToDB.Mapping;
namespace NK.Entity
{
    /// <summary>
    /// IP地址信息
    /// </summary>
    [DisplayName("IP地址信息")]
    [Description("IP地址信息")]
    [Table(Name = "IPInfo")]
    public class IPInfo
    {
        /// <summary>
        /// 主键
        /// </summary>
        [DisplayName("主键")]
        [Description("主键")]
        [Column(Name = "ID", IsPrimaryKey = true, IsIdentity = true, CanBeNull = false)]
        public int ID { get; set; }
        /// <summary>
        /// 启用配置
        /// </summary>
        [DisplayName("启用配置")]
        [Description("启用配置")]
        [Column(Name = "Enable", CanBeNull = false)]
        public bool Enable { get; set; }
        /// <summary>
        ///IP类型
        /// </summary>
        [DisplayName("IP类型")]
        [Description("IP类型")]
        [Column(Name = "Address_Family", CanBeNull = false)]
        public  AddressFamily Address_Family { get; set; }
        /// <summary>
        ///DHCP
        /// </summary>
        [DisplayName("DHCP")]
        [Description("DHCP")]
        [Column(Name = "DHCP", CanBeNull = true)]
        public bool DHCP { get; set; }
        /// <summary>
        ///IPAddress
        /// </summary>
        [DisplayName("IP地址")]
        [Description("IP地址")]
        [Column(Name = "IPAddress", CanBeNull = true)]
        public string IPAddress { get; set; }
        /// <summary>
        ///SubnetMask
        /// </summary>
        [DisplayName("掩码")]
        [Description("掩码")]
        [Column(Name = "SubnetMask", CanBeNull = true)]
        public string SubnetMask { get; set; }
        /// <summary>
        ///GateWay
        /// </summary>
        [DisplayName("网关")]
        [Description("网关")]
        [Column(Name = "GateWay", CanBeNull = true)]
        public string GateWay { get; set; }
        /// <summary>
        ///DNS地址
        /// </summary>
        [DisplayName("DNS地址")]
        [Description("DNS地址")]
        [Column(Name = "DNS", CanBeNull = true)]
        public string DNS { get; set; }
        /// <summary>
        /// 配置名
        /// </summary>
        [DisplayName("配置名")]
        [Description("配置名")]
        [Column(Name = "ConfigName", CanBeNull = false)]
        public string ConfigName { get; set; }
    }
}
