using System; 
using System.Runtime.InteropServices;
using NK.API.Struct;
using NK.API.Enum;
namespace NK.API
{
    public static partial class User32
    {

        /// <summary>
        /// Registers the device or type of device for which a window will receive notifications.
        /// </summary>
        /// <param name="hRecipient">A handle to the window or service that will receive device events for the devices specified in the NotificationFilter parameter. The same window handle can be used in multiple calls to RegisterDeviceNotification.</param>
        /// <param name="NotificationFilter">A pointer to a block of data that specifies the type of device for which notifications should be sent. This block always begins with the DEV_BROADCAST_HDR structure. The data following this header is dependent on the value of the dbch_devicetype member, which can be DBT_DEVTYP_DEVICEINTERFACE or DBT_DEVTYP_HANDLE. For more information,</param>
        /// <param name="Flags">This parameter can be one of the following values.</param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr RegisterDeviceNotification(IntPtr hRecipient, IntPtr NotificationFilter, int Flags);

        /// <summary>
        /// Closes the specified device notification handle.
        /// </summary>
        /// <param name="Handle">Device notification handle returned by the RegisterDeviceNotification function.</param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool UnregisterDeviceNotification(IntPtr Handle);

        /// <summary>
        /// Calls the ExitWindowsEx function to log off the interactive user. Applications should call ExitWindowsEx directly.
        /// </summary>
        /// <param name="dwReserved">This parameter must be zero.</param>
        /// <param name="uReserved">This parameter must be zero.</param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool ExitWindows(int dwReserved, uint uReserved);

        /// <summary>
        /// Logs off the interactive user, shuts down the system, or shuts down and restarts the system. It sends the WM_QUERYENDSESSION message to all applications to determine if they can be terminated.
        /// </summary>
        /// <param name="uFlags">The shutdown type. This parameter must include one of the following values.</param>
        /// <param name="dwReason">The reason for initiating the shutdown. This parameter must be one of the system shutdown reason codes.</param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool ExitWindowsEx(uint uFlags,   int dwReason);

        /// <summary>
        /// Places (posts) a message in the message queue associated with the thread that created the specified window and returns without waiting for the thread to process the message.
        /// </summary>
        /// <param name="hWnd">A handle to the window whose window procedure is to receive the message. The following values have special meanings.</param>
        /// <param name="Msg">The message to be posted.</param>
        /// <param name="wParam">Additional message-specific information.</param>
        /// <param name="lParam">Additional message-specific information.</param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern bool PostMessage(IntPtr hWnd, int Msg, IntPtr wParam, string lParam);

        /// <summary>
        /// Sends the specified message to a window or windows. The SendMessage function calls the window procedure for the specified window and does not return until the window procedure has processed the message.
        /// </summary>
        /// <param name="hWnd">A handle to the window whose window procedure will receive the message. If this parameter is HWND_BROADCAST ((HWND)0xffff), the message is sent to all top-level windows in the system, including disabled or invisible unowned windows, overlapped windows, and pop-up windows; but the message is not sent to child windows.</param>
        /// <param name="Msg">The message to be sent.</param>
        /// <param name="wParam">Additional message-specific information.</param>
        /// <param name="lParam">Additional message-specific information</param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, string lParam);

        /// <summary>
        /// The ChangeDisplaySettings function changes the settings of the default display device to the specified graphics mode.To change the settings of a specified display device, use the ChangeDisplaySettingsEx function.  
        /// </summary>
        /// <param name="lpDevMode">A pointer to a DEVMODE structure that describes the new graphics mode. If lpDevMode is NULL, all the values currently in the registry will be used for the display setting. Passing NULL for the lpDevMode parameter and 0 for the dwFlags parameter is the easiest way to return to the default mode after a dynamic mode change.</param>
        /// <param name="dwflags">Indicates how the graphics mode should be changed. This parameter can be one of the following values.</param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        public static extern long ChangeDisplaySettings(ref  DEVMODE lpDevMode, ChangeDisplaySettingsFlags dwflags);

        /// <summary>
        /// The EnumDisplaySettings function retrieves information about one of the graphics modes for a display device. To retrieve information for all the graphics modes of a display device, make a series of calls to this function.
        /// </summary>
        /// <param name="lpszDeviceName">A pointer to a null-terminated string that specifies the display device about whose graphics mode the function will obtain information.</param>
        /// <param name="iModeNum">The type of information to be retrieved. This value can be a graphics mode index or one of the following values.</param>
        /// <param name="lpDevMode">A pointer to a DEVMODE structure into which the function stores information about the specified graphics mode. Before calling EnumDisplaySettings, set the dmSize member to sizeof(DEVMODE), and set the dmDriverExtra member to indicate the size, in bytes, of the additional space available to receive private driver data.</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool EnumDisplaySettings(string lpszDeviceName, int iModeNum, ref DEVMODE lpDevMode);
    }
}
