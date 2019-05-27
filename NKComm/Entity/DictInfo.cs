using System.ComponentModel;
using LinqToDB.Mapping;
using NK.ENum;
namespace NK.Entity
{
    /// <summary>
    /// 字典信息
    /// </summary>
    [DisplayName("字典信息")]
    [Description("字典信息")]
    [Table(Name = "DictInfo")]
    public class DictInfo
    {
        /// <summary>
        /// 主键
        /// </summary>
        [DisplayName("主键")]
        [Description("主键")]
        [Column(Name = "ID", IsPrimaryKey = true, IsIdentity = true, CanBeNull = false)]
        public int ID { get; set; }
        /// <summary>
        /// 语言设置
        /// </summary>
        [DisplayName("语言设置")]
        [Description("语言设置")]
        [Column(Name = "language", CanBeNull = false)]
        public Language language { get; set; }
        /// <summary>
        /// 字典类型
        /// </summary>
        [DisplayName("字典类型")]
        [Description("字典类型")]
        [Column(Name = "DictName", CanBeNull = false)]
        public string Name { get; set; }
        /// <summary>
        /// 字典键名
        /// </summary>
        [DisplayName("字典键名")]
        [Description("字典键名")]
        [Column(Name = "DictKey", CanBeNull = false)]
        public string Key { get; set; }
        /// <summary>
        /// 字典显示值
        /// </summary>
        [DisplayName("字典显示值")]
        [Description("字典显示值")]
        [Column(Name = "DictDisp", CanBeNull = true)]
        public string Display { get; set; }
        /// <summary>
        /// 字典值
        /// </summary>
        [DisplayName("字典值")]
        [Description("字典值")]
        [Column(Name = "DictValue", CanBeNull = false)]
        public string Value { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [DisplayName("描述")]
        [Description("描述")]
        [Column(Name = "DictCaption", CanBeNull = true)]
        public string Caption { get; set; }
        /// <summary>
        /// 启用配置
        /// </summary>
        [DisplayName("启用配置")]
        [Description("启用配置")]
        [Column(Name = "DictEnable", CanBeNull = false)]
        public bool Enable { get; set; } 
        /// <summary>
        /// 默认
        /// </summary>
        [DisplayName("启用配置")]
        [Description("启用配置")]
        [Column(Name = "DicIsDefault", CanBeNull = false)]
        public bool Default { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [DisplayName("排序")]
        [Description("排序")]
        [Column(Name = "PX", CanBeNull = false)]
        public int PX { get; set; }
    }
}
