using System.ComponentModel;
using LinqToDB.Mapping;
using NK.ENum;
namespace NK.Entity
{
    /// <summary>
    /// 以太网参数
    /// </summary>
    [DisplayName("USB参数")]
    [Description("USB参数")]
    [Table(Name = "USBSet")]
    public  class USBSet
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
        /// 设备路径
        /// </summary>
        [DisplayName("设备路径")]
        [Description("设备路径")]
        [Column(Name = "devPath", CanBeNull = true)]
        public string devPath { get; set; }
        /// <summary>
        /// 供应商ID,hex
        /// </summary>
        [DisplayName("供应商ID")]
        [Description("供应商ID")]
        [Column(Name = "VID", CanBeNull = true)]
        public string VID { get; set; }
        /// <summary>
        /// 产品识别码,hex
        /// </summary>
        [DisplayName("产品识别码")]
        [Description("产品识别码")]
        [Column(Name = "PID", CanBeNull = true)]
        public string PID { get; set; }
        /// <summary>
        ///类型
        /// </summary>
        [DisplayName("类型")]
        [Description("类型")]
        [Column(Name = "NetMode", CanBeNull = false)]
        public Net_Mode Mode { get; set; }
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
