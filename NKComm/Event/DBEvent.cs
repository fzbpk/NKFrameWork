using System.Data.Common;
namespace NK.Event
{  
    /// <summary>
    /// 数据库事件
    /// </summary>
    public class DBEvent
    {
        /// <summary>
        /// 连接事件
        /// </summary>
        /// <param name="Cfg">配置信息</param> 
        public delegate void Connect(DbConnection Cfg );
        /// <summary>
        /// 断开连接事件
        /// </summary>
        /// <param name="Cfg">配置信息</param> 
        public delegate void DisConnect(DbConnection Cfg );
    }
}
