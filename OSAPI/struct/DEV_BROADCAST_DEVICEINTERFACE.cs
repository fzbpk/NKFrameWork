using System;
using System.Runtime.InteropServices;
namespace NK.API.Struct
{
    /// <summary>
    /// Contains information about a class of devices.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct DEV_BROADCAST_DEVICEINTERFACE
    {
        /// <summary>
        /// The size of this structure, in bytes. This is the size of the members plus the actual length of the dbcc_name string (the null character is accounted for by the declaration of dbcc_name as a one-character array.)
        /// </summary>
        public int dbcc_size;
        /// <summary>
        /// Set to DBT_DEVTYP_DEVICEINTERFACE.
        /// </summary>
        public int dbcc_devicetype;
        /// <summary>
        /// Reserved; do not use.
        /// </summary>
        public int dbcc_reserved;
        /// <summary>
        /// The GUID for the interface device class.
        /// </summary> 
        public Guid dbcc_classguid;
        /// <summary>
        /// A null-terminated string that specifies the name of the device.
        //When this structure is returned to a window through the WM_DEVICECHANGE message, the dbcc_name string is converted to ANSI as appropriate.Services always receive a Unicode string, whether they call RegisterDeviceNotificationW or RegisterDeviceNotificationA.
        /// </summary>
         [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0xff)]
        public char[] dbcc_name;
    }
}
