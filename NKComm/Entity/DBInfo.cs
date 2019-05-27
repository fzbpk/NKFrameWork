using System.ComponentModel;
using LinqToDB.Mapping;
using NK.ENum;
namespace NK.Entity
{
    /// <summary>
    /// 数据库配置
    /// </summary>
    [DisplayName("数据库配置")]
    [Description("数据库配置")]
    [Table(Name = "DBInfo")]
    public class DBInfo
    {
        /// <summary>
        /// 主键
        /// </summary>
        [DisplayName("主键")]
        [Description("主键")]
        [Column(Name = "ID", IsPrimaryKey = true,IsIdentity =true, CanBeNull = false)]
        public int ID { get; set; }
        /// <summary>
        /// 配置名
        /// </summary>
        [DisplayName("配置名")]
        [Description("配置名")]
        [Column(Name = "ConfigName", CanBeNull = false)]
        public string ConfigName { get; set; }
        /// <summary>
        /// 启用配置
        /// </summary>
        [DisplayName("启用配置")]
        [Description("启用配置")]
        [Column(Name = "Enabled", CanBeNull = false)]
        public bool Enable { get; set; }
        /// <summary>
        /// 数据库类型
        /// </summary>
        [DisplayName("数据库类型")]
        [Description("数据库类型")]
        [Column(Name = "DBType", CanBeNull = false)]
        public DBType Mode { get; set; }
        /// <summary>
        /// 数据库驱动
        /// </summary>
        [DisplayName("数据库驱动")]
        [Description("数据库驱动")]
        [Column(Name = "Driver", CanBeNull = true)]
        public string Driver { get; set; }
        /// <summary>
        /// 数据库地址
        /// </summary>
        [DisplayName("数据库地址")]
        [Description("数据库地址")]
        [Column(Name = "Url", CanBeNull = false)]
        public string Url { get; set; }
        /// <summary>
        /// 数据库端口
        /// </summary>
        [DisplayName("数据库端口")]
        [Description("数据库端口")]
        [Column(Name = "Port", CanBeNull = false)]
        public int Port { get; set; }
        /// <summary>
        /// 数据库账号
        /// </summary>
        [DisplayName("数据库账号")]
        [Description("数据库账号")]
        [Column(Name = "Account", CanBeNull = false)]
        public string User { get; set; }
        /// <summary>
        /// 数据库密码
        /// </summary>
        [DisplayName("数据库密码")]
        [Description("数据库密码")]
        [Column(Name = "Passwd", CanBeNull = false)]
        public string Password { get; set; }
        /// <summary>
        /// 数据库名称
        /// </summary>
        [DisplayName("数据库名称")]
        [Description("数据库名称")]
        [Column(Name = "DataBaseName", CanBeNull = false)]
        public string DataBaseName { get; set; }
        /// <summary>
        /// 超时时间，ms
        /// </summary>
        [DisplayName("超时时间")]
        [Description(" 超时时间，ms")]
        [Column(Name = "DBTimeOut", CanBeNull = false)]
        public int TimeOut { get; set; }
        /// <summary>
        /// 字符编码
        /// </summary>
        [DisplayName("字符编码")]
        [Description("字符编码")]
        [Column(Name = "DBCharset", CanBeNull = true)]
        public string Charset { get; set; }
        /// <summary>
        /// 连接字符串
        /// </summary>
        [DisplayName("连接字符串")]
        [Description("连接字符串")]
        [Column(Name = "ConnStrs", CanBeNull = true)]
        public string ConnStr { get; set; }
    }
}
