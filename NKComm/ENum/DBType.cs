using System.ComponentModel;
namespace NK.ENum
{
    /// <summary>
    /// 支持的数据库类型
    /// </summary>
    [Description("支持的数据库类型")]
    public enum DBType : short
    {
        /// <summary>
        /// 无
        /// </summary>
        [Description("无")]
        None = 0,
        /// <summary>
        /// MSSQL,支持从2000,2005,2008,2012
        /// </summary>
        [Description("微软SQL Server，支持从2000,2005,2008,2012")]
        MSSQL=1,
        /// <summary>
        /// Access
        /// </summary>
        [Description("微软Access，支持mdb和accdb")]
        Access=2,
        /// <summary>
        /// MYSQL,支持5.0及以上版本
        /// </summary>
        [Description("属于Oracle公司的一个关系型数据库,支持5.0及以上版本")]
        MYSQL=3,
        /// <summary>
        /// Oracle,支持10g及以上版本
        /// </summary>
        [Description("属于Oracle公司的一个关系型数据库,支持10g及以上版本")]
        Oracle=4,
        /// <summary>
        /// SQLite
        /// </summary>
        [Description("轻型数据库系统")]
        SQLite=5,
        /// <summary>
        /// PostgreSQL
        /// </summary>
        [Description("高效率数据库系统")]
        PostgreSQL=6,
        /// <summary>
        /// OleDB
        /// </summary>
        [Description("OleDB")]
        OleDB=7,
        /// <summary>
        /// ODBC驱动
        /// </summary>
        [Description("ODBC驱动")]
        ODBC=8,
        /// <summary>
        /// MongoDB
        /// </summary>
        [Description("MongoDB")]
        MongoDB = 9,
    }
}
