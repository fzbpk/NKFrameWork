using System.Runtime.InteropServices;
using NK.OS.Enum;
namespace NK.OS.Struct
{
    public struct MIXERLINE
    {
        public int cbStruct;
        public int dwDestination;
        public int dwSource;
        public int dwLineID;
        public int fdwLine;
        public int dwUser;
        public int dwComponentType;
        public int cChannels;
        public int cConnections;
        public int cControls;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst =Const. MIXER_SHORT_NAME_CHARS)]
        public string szShortName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Const.MIXER_LONG_NAME_CHARS)]
        public string szName;
        public int dwType;
        public int dwDeviceID;
        public int wMid;
        public int wPid;
        public int vDriverVersion;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Const.MAXPNAMELEN)]
        public string szPname;
    }
}
