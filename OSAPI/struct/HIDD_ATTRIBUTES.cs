using System.Runtime.InteropServices;
namespace NK.API.Struct
{
    /// <summary>
    /// The HIDD_ATTRIBUTES structure contains vendor information about a HIDClass device.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct HIDD_ATTRIBUTES
    {
        /// <summary>
        /// Specifies the size, in bytes, of a HIDD_ATTRIBUTES structure.
        /// </summary>
        public int Size;
        /// <summary>
        /// Specifies a HID device's vendor ID.
        /// </summary>
        public ushort VendorID;
        /// <summary>
        /// Specifies a HID device's product ID.
        /// </summary>
        public ushort ProductID;
        /// <summary>
        /// Specifies the manufacturer's revision number for a HIDClass device.
        /// </summary>
        public ushort VersionNumber;
    }
}
