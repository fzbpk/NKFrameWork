using System;
using System.Collections.Generic;
using System.ComponentModel;
using LinqToDB.Mapping;
using NK.ENum;
namespace NK.Entity
{
    /// <summary>
    /// 证书配置详情
    /// </summary>
    [DisplayName("证书配置")]
    [Description("证书配置")]
    [Table(Name = "CertificateDetail")]
    public class CertificateDetail
    {
        /// <summary>
        /// 模块名称
        /// </summary>
        [DisplayName("模块名称")]
        [Description("模块名称")]
        [Column(Name = "ModuleName", CanBeNull = false)]
        public string ModuleName { get; set; }
        /// <summary>
        /// 功能名称
        /// </summary>
        [DisplayName("功能名称")]
        [Description("功能名称")]
        [Column(Name = "FuncName", CanBeNull = false)]
        public string FuncName { get; set; }
        /// <summary>
        /// 功能权限
        /// </summary>
        [DisplayName("功能权限")]
        [Description("功能权限")]
        [Column(Name = "FuncPower", CanBeNull = false)]
        public ushort FuncPower { get; set; }
        /// <summary>
        /// 可用子功能,分割
        /// </summary>
        [DisplayName("可用子功能")]
        [Description("可用子功能")]
        [Column(Name = "CanUse", CanBeNull = false)]
        public string CanUse { get; set; }
        /// <summary>
        /// 不可用子功能,分割
        /// </summary>
        [DisplayName("不可用子功能")]
        [Description("不可用子功能")]
        [Column(Name = "CanNotUse", CanBeNull = false)]
        public string CanNotUse { get; set; }
    }
}
