using System.Runtime.InteropServices;
using NK.OS.Enum;
namespace NK.OS.Struct
{
    public struct MIXERCONTROL
    {
        public int cbStruct;
        public int dwControlID;
        public int dwControlType;
        public int fdwControl;
        public int cMultipleItems;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst =Const. MIXER_SHORT_NAME_CHARS)]
        public string szShortName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Const.MIXER_LONG_NAME_CHARS)]
        public string szName;
        public int lMinimum;
        public int lMaximum;
        [MarshalAs(UnmanagedType.U4, SizeConst = 10)]
        public int reserved;
    }
}
