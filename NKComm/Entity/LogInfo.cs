using System;
using System.ComponentModel;
using LinqToDB.Mapping;
using NK.ENum;
namespace NK.Entity
{
    /// <summary>
    /// 日志信息
    /// </summary>
    [DisplayName("日志信息")]
    [Description("日志信息")]
    [Table(Name = "LogInfo")]
    public class LogInfo
    {
        /// <summary>
        /// 主键
        /// </summary>
        [DisplayName("主键")]
        [Description("主键")]
        [Column(Name = "ID", IsPrimaryKey = true, IsIdentity = true, CanBeNull = false)]
        public int ID { get; set; }
        /// <summary>
        /// 记录时间
        /// </summary>
        [DisplayName("记录时间")]
        [Description("记录时间")]
        [Column(Name = "RecTime", CanBeNull = false)]
        public DateTime RecTime { get; set; }
        /// <summary>
        /// 类名称
        /// </summary>
        [DisplayName("类名称")]
        [Description("类名称")]
        [Column(Name = "ClassName", CanBeNull = false)]
        public string ClassName { get; set; }
        /// <summary>
        /// 模块名
        /// </summary>
        [DisplayName("模块名")]
        [Description("模块名")]
        [Column(Name = "FuncName", CanBeNull = false)]
        public string FuncName { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        [DisplayName("类型")]
        [Description("类型")]
        [Column(Name = "MessID", CanBeNull = false)]
        public Log_Type MessID { get; set; }
        /// <summary>
        /// 日志内容
        /// </summary>
        [DisplayName("日志内容")]
        [Description("日志内容")]
        [Column(Name = "Message", CanBeNull = false)]
        public string Message { get; set; }
    }
}
