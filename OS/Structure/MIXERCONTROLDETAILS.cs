using System;
namespace NK.OS.Struct
{
    public struct MIXERCONTROLDETAILS
    {
        public int cbStruct;
        public int dwControlID;
        public int cChannels;
        public int item;
        public int cbDetails;
        public IntPtr paDetails;
    }
}
