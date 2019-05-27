using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NK.Exceptions
{
    /// <summary>
    /// 连接错误
    /// </summary>
    public class ConnectFailException:Exception
    {
        /// <summary>
        /// 连接错误
        /// </summary>
        /// <param name="message"></param>
        public ConnectFailException(string message) : base(message)
        {
           
        }
    }
}
