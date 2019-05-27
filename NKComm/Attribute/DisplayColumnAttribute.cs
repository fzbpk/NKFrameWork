using System;
using System.ComponentModel;
using LinqToDB.Mapping;
using NK.ENum;
namespace NK.Attribut
{
    /// <summary>
    /// 显示字段属性
    /// </summary> 
    [DisplayName("显示字段属性")]
    [Description("显示字段属性")]
    [Table(Name = "DisplayColumnInfo")]
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public partial class DisplayColumnAttribute : Attribute
    {
        /// <summary>
        /// 索引
        /// </summary>
        [DisplayName("主键")]
        [Description("主键")]
        [Column(Name = "ID", IsPrimaryKey = true, IsIdentity = true, CanBeNull = false)]
        public int index { get; set; }
        /// <summary>
        /// 表/实体
        /// </summary>
        [Column(Name = "TableName", CanBeNull = false)]
        public string Table { get; set; }
        /// <summary>
        /// 字段
        /// </summary>
        [Column(Name = "ColumnName", CanBeNull = false)]
        public string Column { get; set; }
        /// <summary>
        /// 显示名
        /// </summary>
        [Column(Name = "DispName", CanBeNull = false)]
        public string Name { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [Column(Name = "DispCaption", CanBeNull = false)]
        public string Caption { get; set; }
        /// <summary>
        /// 显示格式,TOSTRING表达式或正则表达式
        /// </summary>
        [Column(Name = "DispFormat", CanBeNull = true)]
        public string Format { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        [Column(Name = "DispUnit", CanBeNull = true)]
        public string Unit { get; set; }
        /// <summary>
        /// 样式
        /// </summary>
        [Column(Name = "CSS", CanBeNull = true)]
        public string CSS { get; set; }
        /// <summary>
        /// JS函数
        /// </summary>
        [Column(Name = "JS", CanBeNull = true)]
        public string JS { get; set; }
        /// <summary>
        /// 是否用于搜索
        /// </summary>
        [Column(Name = "CanSearch", CanBeNull = true)]
        public bool CanSearch { get; set; }
        /// <summary>
        /// 是否用于表头显示
        /// </summary>
        [Column(Name = "CanHead", CanBeNull = true)]
        public bool CanHead { get; set; }
        /// <summary>
        /// 是否用于增删改查页显示
        /// </summary>
        [Column(Name = "CanDeitail", CanBeNull = true)]
        public bool CanDeitail { get; set; }
        /// <summary>
        /// 是否用于统计
        /// </summary>
        [Column(Name = "CanCount", CanBeNull = true)]
        public bool CanCount { get; set; }
        /// <summary>
        /// 是否可用于导入导出
        /// </summary>
        [Column(Name = "CanImpExp", CanBeNull = true)]
        public bool CanImpExp { get; set; }
        /// <summary>
        /// 是否唯一
        /// </summary>
        [Column(Name = "IsUnique", CanBeNull = true)]
        public bool IsUnique { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [Column(Name = "Seqencing", CanBeNull = true)]
        public int Seqencing { get; set; }
        /// <summary>
        /// 显示语言
        /// </summary>
        [Column(Name = "Displaylanguage", CanBeNull = true)]
        public Language Displaylanguage { get; set; }
    }
}
