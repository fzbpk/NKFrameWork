using System.Runtime.InteropServices;
namespace NK.API.Struct
{
    /// <summary>
    /// Serves as a standard header for information related to a device event reported through the WM_DEVICECHANGE message.
    /// The members of the DEV_BROADCAST_HDR structure are contained in each device management structure.To determine which structure you have received through WM_DEVICECHANGE, treat the structure as a DEV_BROADCAST_HDR structure and check its dbch_devicetype member.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct DEV_BROADCAST_HDR
    {
        /// <summary>
        /// The size of this structure, in bytes.
        /// If this is a user-defined event, this member must be the size of this header, plus the size of the variable-length data in the _DEV_BROADCAST_USERDEFINED structure.
        /// </summary>
        public int dbch_size;
        /// <summary>
        /// The device type, which determines the event-specific information that follows the first three members. This member can be one of the following values.
        /// </summary>
        public int dbch_devicetype;
        /// <summary>
        /// Reserved; do not use.
        /// </summary>
        public int dbch_reserved;
    }
}
