using System.IO.Ports;
using System.ComponentModel;
using LinqToDB.Mapping;
using NK.ENum;
namespace NK.Entity
{
    /// <summary>
    /// 传统IO参数，LPT/SERIAL
    /// </summary>
    [DisplayName("传统IO参数")]
    [Description("传统IO参数，LPT/SERIAL")]
    [Table(Name = "PortsSet")]
    public class PortsSet
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
        /// 通讯类型
        /// </summary>
        [DisplayName("通讯类型")]
        [Description("通讯类型")]
        [Column(Name = "PortMode", CanBeNull = true)]
        public Port_Mode PortType { get; set; }
        /// <summary>
        ///类型
        /// </summary>
        [DisplayName("类型")]
        [Description("类型")]
        [Column(Name = "NetMode", CanBeNull = true)]
        public Net_Mode Mode { get; set; }
        /// <summary>
        /// 端号
        /// </summary>
        [DisplayName("端号")]
        [Description("端号")]
        [Column(Name = "Port", CanBeNull = true)]
        public int Port { get; set; }
        /// <summary>
        /// 波特率
        /// </summary>
        [DisplayName("波特率")]
        [Description("波特率")]
        [Column(Name = "Rate", CanBeNull = true)]
        public int Rate { get; set; }
        /// <summary>
        /// 数据位
        /// </summary>
        [DisplayName("数据位")]
        [Description("数据位")]
        [Column(Name = "DataBit", CanBeNull = true)]
        public int DataBit { get; set; }
        /// <summary>
        /// 停止位
        /// </summary>
        [DisplayName("停止位")]
        [Description("停止位")]
        [Column(Name = "StopBit", CanBeNull = true)]
        public StopBits StopBit { get; set; }
        /// <summary>
        /// 校验
        /// </summary>
        [DisplayName("校验")]
        [Description("校验")]
        [Column(Name = "Parity", CanBeNull = true)]
        public Parity Parity { get; set; }
        /// <summary>
        /// 流控
        /// </summary>
        [DisplayName("流控")]
        [Description("流控")]
        [Column(Name = "Ctrl", CanBeNull = true)]
        public Handshake Ctrl { get; set; }
        /// <summary>
        /// 设备编址
        /// </summary>
        [DisplayName("设备编址")]
        [Description("设备编址")]
        [Column(Name = "Address", CanBeNull = true)]
        public int Address { get; set; }
        /// <summary>
        /// 启用配置
        /// </summary>
        [DisplayName("启用配置")]
        [Description("启用配置")]
        [Column(Name = "Enable", CanBeNull = false)]
        public bool Enable { get; set; }
    }
}
