using System;
namespace NK.OS.Struct
{
    public struct MIXERLINECONTROLS
    {
        public int cbStruct;
        public int dwLineID;
        public int dwControl;
        public int cControls;
        public int cbmxctrl;
        public IntPtr pamxctrl;
    }
}
