using System;
using NK.ENum;
namespace NK.Event
{
    /// <summary>
    /// 通用事件
    /// </summary>
   public  class CommEvent
    {
        /// <summary>
        /// 出现错误事件
        /// </summary>
        /// <param name="Class">所在类</param>
        /// <param name="Func">所在函数</param>
        /// <param name="ex">错误信息</param>
        public delegate void HasErrorEven(string Class,string Func, Exception ex);

        /// <summary>
        /// 日志调试事件
        /// </summary>
        /// <param name="Class">所在类</param>
        /// <param name="Func">所在函数</param>
        /// <param name="flag">日志类型</param>
        /// <param name="Message">信息</param>
        public delegate void LogEven(string Class, string Func,Log_Type flag, string Message);
    }
}
