using System.Runtime.InteropServices;
namespace NK.API.Struct
{

    /// <summary>
    /// SP_DEVICE_INTERFACE_DETAIL_DATA structure contains the path for a device interface.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct SP_DEVICE_INTERFACE_DETAIL_DATA
    {
        /// <summary>
        /// The size, in bytes, of the SP_DEVICE_INTERFACE_DETAIL_DATA structure. For more information, see the following Remarks section.
        /// </summary>
        public int cbSize;
        /// <summary>
        /// A NULL-terminated string that contains the device interface path. This path can be passed to Win32 functions such as CreateFile.
        /// </summary>
        public short devicePath;
    }
}
