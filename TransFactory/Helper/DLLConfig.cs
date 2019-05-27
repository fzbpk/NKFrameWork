using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NK.Communicate
{
    internal class DLLConfig
    {
        public  static bool KeepAlive = true;
        public static int ChkTime = 3;
        public static int ConnTOut =5000;
        public static int WaitTime = 1000;
        public static int ReceiveTimeout = 1000;
        public static int ReceiveBufferSize = 1024;
        public static int SendTimeout = 1000;
        public static int SendBufferSize = 1024;
        public static int ConnPool = 150;
        public static int ReTry = 0;
    }
}
