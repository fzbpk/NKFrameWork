using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NK.Exceptions
{
    /// <summary>
    /// 已存在
    /// </summary>
    public class FoundException : Exception
    {
        /// <summary>
        /// 已存在
        /// </summary>
        /// <param name="Object"></param>
        public FoundException(string Object):base (Object+" Found")
        {
        }
    }
}
