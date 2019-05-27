using System.Runtime.InteropServices;
namespace NK.OS.Struct
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct LUID_AND_ATTRIBUTES
    {
        /// <summary>
        /// Specifies an LUID value.
        /// </summary>
        public LUID pLuid;
        /// <summary>
        /// Specifies attributes of the LUID. This value contains up to 32 one-bit flags. Its meaning is dependent on the definition and use of the LUID.
        /// </summary>
        public int Attributes;
    }
}
