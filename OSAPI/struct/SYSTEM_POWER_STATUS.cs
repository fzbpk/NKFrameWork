using System.Runtime.InteropServices;
namespace NK.API.Struct
{
    /// <summary>
    /// Contains information about the power status of the system.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SYSTEM_POWER_STATUS
    {
        /// <summary>
        /// The AC power status. This member can be one of the following values.
        /// </summary>
        byte ACLineStatus;
        /// <summary>
        /// The battery charge status. This member can contain one or more of the following flags.
        /// </summary>
        byte BatteryFlag;
        /// <summary>
        /// The percentage of full battery charge remaining. This member can be a value in the range 0 to 100, or 255 if status is unknown.
        /// </summary>
        byte BatteryLifePercent;
        /// <summary>
        /// The status of battery saver. To participate in energy conservation, avoid resource intensive tasks when battery saver is on. To be notified when this value changes, call the RegisterPowerSettingNotification function with the power setting GUID, GUID_POWER_SAVING_STATUS.
        /// </summary>
        byte SystemStatusFlag;
        /// <summary>
        /// The number of seconds of battery life remaining, or –1 if remaining seconds are unknown or if the device is connected to AC power.
        /// </summary>
        int BatteryLifeTime;
        /// <summary>
        /// The number of seconds of battery life when at full charge, or –1 if full battery lifetime is unknown or if the device is connected to AC power.
        /// </summary>
        int BatteryFullLifeTime;
    }
}
