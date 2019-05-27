using System;
using System.Runtime.InteropServices;
namespace NK.API.Struct
{
    /// <summary>
    /// An SP_DEVICE_INTERFACE_DATA structure defines a device interface in a device information set.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SP_DEVICE_INTERFACE_DATA
    {
        /// <summary>
        /// The size, in bytes, of the SP_DEVICE_INTERFACE_DATA structure. For more information, see the Remarks section.
        /// </summary>
        public int cbSize;
        /// <summary>
        /// The GUID for the class to which the device interface belongs.
        /// </summary>
        public Guid InterfaceClassGuid;
        /// <summary>
        /// Can be one or more of the following:
        /// SPINT_ACTIVE  The interface is active(enabled).
        /// SPINT_DEFAULT The interface is the default interface for the device class.
        /// SPINT_REMOVED The interface is removed.
        /// </summary>
        public int Flags;
        /// <summary>
        /// Reserved. Do not use.
        /// </summary>
        public IntPtr Reserved;
    }
}
