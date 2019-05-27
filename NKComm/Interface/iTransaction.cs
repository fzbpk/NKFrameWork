using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using NK.ENum;
using NK.Entity;
using NK.Event;
namespace NK.Interface
{
    /// <summary>
    /// 数据库事务
    /// </summary>
    public interface iTransaction
    {

        #region 属性

        /// <summary>
        /// 数据库信息
        /// </summary>
        DBInfo DataBase { get; set; }
        /// <summary>
        /// 连接参数
        /// </summary>
        string Connection { get; set; }

        /// <summary>
        /// 超时
        /// </summary>
        int Timeout { get; set; }

        /// <summary>
        /// 获取所有表
        /// </summary>
        List<string> Tables { get; }

        /// <summary>
        /// 获取所有视图
        /// </summary>
        List<string> Views { get; }

        /// <summary>
        /// 长连接/短连接
        /// </summary>
        bool KeepAlive { get; set; }

        #endregion

        #region 方法

        /// <summary>
        /// 释放
        /// </summary>
        void Dispose();

        /// <summary>
        /// 检查数据库连接是否连接成功
        /// </summary>
        /// <param name="ErrMsg">错误信息</param>
        /// <returns>是否连接成功</returns>
        bool CheckConnection(out string ErrMsg);

        /// <summary>
        /// 数据库Insert,update,delete 
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        void ExecuteNonQuery(string sql);

        /// <summary>
        /// 获取查询数据
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        DataTable getDataTable(string sql);

        /// <summary>
        /// 获取查询数据
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="RecCount">返回记录数</param>
        /// <returns></returns>
        DataTable getDataTable(string sql, out int RecCount);

        /// <summary>
        /// 获取查询数据
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="PageSize">分页大小</param>
        /// <param name="PageCount">返回总页数</param>
        /// <returns></returns>
        DataTable getDataTable(string sql, int PageSize, out int PageCount);

        /// <summary>
        /// 获取查询数据
        /// </summary>
        /// <param name="sql">查询SQL</param>
        /// <param name="PageSize">分页大小</param>
        /// <param name="RecCount">返回查询记录数</param>
        /// <param name="PageCount">返回总页数</param>
        /// <param name="TableName">表名</param>
        /// <returns>查询结果</returns>
        DataTable getDataTable(string sql, int PageSize, out int RecCount, out int PageCount, string TableName = "Query");

        /// <summary>
        /// 判断是否存在记录
        /// </summary>
        /// <param name="sql">查询数据库</param>
        /// <returns>Boolean,TRUE为存在，False为不存在</returns>
        bool IsExist(string sql);

        /// <summary>
        /// 查询数据记录
        /// </summary>
        /// <param name="sql">查询SQL</param>
        /// <returns>返回第一行查询数据</returns>
        Dictionary<string, object> Find(string sql);

        /// <summary>
        /// 查询记录
        /// </summary>
        /// <param name="sql">查询SQL</param>
        /// <returns>返回第一行第一列数据</returns>
        object ExecuteScalar(string sql);

        /// <summary>
        /// 查询表是否存在
        /// </summary>
        /// <param name="TableName">表名 </param>
        /// <returns></returns>
        bool TableIsExist(string TableName);

        /// <summary>
        /// 查询字段属性
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <param name="Field">字段名</param>
        /// <param name="FieldType">字段类型</param>
        /// <param name="CanBeNull">非空</param>
        /// <param name="IsPrimaryKey">主键</param>
        /// <returns>true 字段存在，false 字段不存在</returns>
        bool CheckField(string TableName, string Field, out System.Type FieldType, out bool CanBeNull, out bool IsPrimaryKey);

        /// <summary>
        /// 利用内存分页因此适合数据小但SQL语句复杂的查询
        /// </summary>
        /// <param name="PageIndex">当前页</param>
        /// <param name="PageSize">分页大小</param>
        /// <param name="DisplayField">字段列，每个字段用,分开</param>
        /// <param name="TableName">表名，支持（） k 视图方式</param>
        /// <param name="Where">查询条件，不带关键字WHERE</param>
        /// <param name="OrderBy">排序语句，带order by</param>
        /// <param name="GroupBy">GROUP BY 字段，不带关键字GROUP BY</param>
        /// <param name="RecodeCount">返回总记录数</param>
        /// <param name="PageCount">返回总记录数</param>
        /// <returns>查询结果</returns>
        DataTable getDataTableByRam(int PageIndex, int PageSize, string DisplayField, string TableName, string Where, string OrderBy, string GroupBy, out int RecodeCount, out int PageCount);

        /// <summary>
        /// 利用数据库分页因此适合数据小但SQL语句复杂的查询
        /// </summary>
        /// <param name="PageIndex">当前页</param>
        /// <param name="PageSize">分页大小</param>
        /// <param name="DisplayField">字段列，每个字段用,分开</param>
        /// <param name="TableName">表名，支持（） k 视图方式</param>
        /// <param name="Where">查询条件，不带关键字WHERE</param>
        /// <param name="OrderBy">排序语句，带order by</param>
        /// <param name="OrderField">排序字段</param>
        /// <param name="GroupBy">GROUP BY 字段，不带关键字GROUP BY</param>
        /// <param name="RecodeCount">返回总记录数</param>
        /// <param name="PageCount">返回总记录数</param>
        /// <returns>查询结果</returns>
        DataTable getDataTableByDB(int PageIndex, int PageSize, string DisplayField, string TableName, string Where, string OrderField, string OrderBy, string GroupBy, out int RecodeCount, out int PageCount);

        /// <summary>
        /// 列出表字段
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <returns></returns>
        List<string> Columns(string TableName);

        #endregion

        #region 事务 

        /// <summary>
        /// 启用实物
        /// </summary>
        void Transaction();

        /// <summary>
        /// 执行事务并完成事务，当出错后自动回滚。再次执行需要执行Transaction
        /// </summary>
        void SaveChange(bool Rollback = true);

        /// <summary>
        /// 回滚后完成事务。再次执行需要执行Transaction
        /// </summary>
        void Cancel();

        #endregion

        #region 事件

        /// <summary>
        /// 调试信息事件
        /// </summary>
        event CommEvent.LogEven log;
        /// <summary>
        /// 错误出现事件，性能参数内DEBUG设置为EVENT有效
        /// </summary>
        event CommEvent.HasErrorEven HasError;
        /// <summary>
        /// 连接事件
        /// </summary>
        event DBEvent.Connect Connect;
        /// <summary>
        /// 连接断开
        /// </summary>
        event DBEvent.DisConnect DisConnect;

        #endregion

    }
}
