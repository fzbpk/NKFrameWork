using System.Runtime.InteropServices;
using NK.OS.Enum;
namespace NK.OS.Struct
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LUID
    {
        /// <summary>
        /// The low order part of the 64 bit value.
        /// </summary>
        public int LowPart;
        /// <summary>
        /// The high order part of the 64 bit value.
        /// </summary>
        public int HighPart;
    }
}
