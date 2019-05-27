using System;
namespace NK.API.Struct
{
    /// <summary>
    /// SP_DEVINFO_DATA structure defines a device instance that is a member of a device information set.
    /// </summary>
    public struct SP_DEVINFO_DATA
    {
        /// <summary>
        /// The size, in bytes, of the SP_DEVINFO_DATA structure. For more information, see the following Remarks section.
        /// </summary>
        public int cbSize;
        /// <summary>
        /// The GUID of the device's setup class.
        /// </summary>
        public Guid ClassGuid;
        /// <summary>
        /// An opaque handle to the device instance (also known as a handle to the devnode).
        /// </summary>
        public int DevInst;
        /// <summary>
        /// Reserved. For internal use only.
        /// </summary>
        public int Reserved;
    }

}
