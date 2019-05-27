using System.Runtime.InteropServices;
using NK.OS.Enum;
namespace NK.OS.Struct
{
    public struct MIXERCAPS
    {
        public int wMid;
        public int wPid;
        public int vDriverVersion;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst =Const. MAXPNAMELEN)]
        public string szPname;
        public int fdwSupport;
        public int cDestinations;
    }
}
