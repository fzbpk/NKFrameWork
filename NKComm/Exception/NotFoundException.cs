using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NK.Exceptions
{
    /// <summary>
    /// 不存在
    /// </summary>
    public class NotFoundException : Exception
    {
        /// <summary>
        /// 不存在
        /// </summary>
        /// <param name="Object"></param>
        public NotFoundException(string Object):base (Object+" Not Found")
        {
        }
    }
}
