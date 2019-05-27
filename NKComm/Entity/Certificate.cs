using System;
using System.Collections.Generic;
using System.ComponentModel;
using LinqToDB.Mapping;
namespace NK.Entity
{
    /// <summary>
    /// 证书配置
    /// </summary>
    [DisplayName("证书配置")]
    [Description("证书配置")]
    [Table(Name = "CertificateInfo")]
    public class Certificate
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
        /// 启用配置
        /// </summary>
        [DisplayName("启用配置")]
        [Description("启用配置")]
        [Column(Name = "Enable", CanBeNull = false)]
        public bool Enable { get; set; }
        /// <summary>
        /// 唯一ID
        /// </summary>
        [DisplayName("唯一ID")]
        [Description("唯一ID")]
        [Column(Name = "guid", CanBeNull = false)]
        public Guid guid { get; set; }
        /// <summary>
        /// 启用配置
        /// </summary>
        [DisplayName("系统名称")]
        [Description("系统名称")]
        [Column(Name = "SYSName", CanBeNull = false)]
        public string SYSName { get; set; }
        /// <summary>
        /// 授权单位
        /// </summary>
        [DisplayName("授权单位")]
        [Description("授权单位")]
        [Column(Name = "Company", CanBeNull = false)]
        public string Company { get; set; }
        /// <summary>
        /// 接入账号名
        /// </summary>
        [DisplayName("接入账号名")]
        [Description("接入账号名")]
        [Column(Name = "User", CanBeNull = false)]
        public string User { get; set; }
        /// <summary>
        /// 接入密码
        /// </summary>
        [DisplayName("接入密码")]
        [Description("接入密码")]
        [Column(Name = "Password", CanBeNull = false)]
        public string Password { get; set; }
        /// <summary>
        /// 接入秘钥
        /// </summary>
        [DisplayName("接入秘钥")]
        [Description("接入秘钥")]
        [Column(Name = "APPKey", CanBeNull = false)]
        public string APPKey { get; set; }
        /// <summary>
        /// 验证信息
        /// </summary>
        [DisplayName("验证信息")]
        [Description("验证信息")]
        [Column(Name = "CertificateData", CanBeNull = true )]
        public string CertificateData { get; set; }
        /// <summary>
        /// 生效时间
        /// </summary>
        [DisplayName("生效时间")]
        [Description("生效时间")]
        [Column(Name = "StartDateTime", CanBeNull = false)]
        public DateTime StartDateTime { get; set; }
        /// <summary>
        /// 失效时间
        /// </summary> 
        [DisplayName("失效时间")]
        [Description("失效时间")]
        [Column(Name = "EndDateTime", CanBeNull = false)]
        public DateTime EndDateTime { get; set; }
        /// <summary>
        /// 密匙
        /// </summary>
        [DisplayName("密匙")]
        [Description("密匙")]
        [Column(Name = "Key", CanBeNull = false)]
        public string Key { get; set; }
        /// <summary>
        /// 证书权限
        /// </summary> 
        [DisplayName("证书权限")]
        [Description("证书权限")]
        [Column(Name = "Power", CanBeNull = true )]
        public List<CertificateDetail> Power { get; set; }
    }
}
