using System;
using System.Diagnostics;
using Microsoft.VisualBasic;
using System.Management;
using System.Runtime.InteropServices;
using NK.OS.Struct;
using NK.OS.Enum;
namespace NK.OS
{
    /// <summary>
    /// 系统控制类
    /// </summary>
    public static partial class System32
    {
        /// <summary>
        /// 获取可用内存,MB
        /// </summary>
        /// <returns></returns>
        public static double LeftRam()
        {
            double available=0;
            ManagementClass cimobject2 = new ManagementClass("Win32_PerfFormattedData_PerfOS_Memory");
            ManagementObjectCollection moc2 = cimobject2.GetInstances();
            foreach (ManagementObject mo2 in moc2)
                available += ((Math.Round(Int64.Parse(mo2.Properties["AvailableMBytes"].Value.ToString()) / 1024.0, 1)));
            moc2.Dispose();
            cimobject2.Dispose();
            return available;
        }

        /// <summary>
        /// 获取总内存,MB
        /// </summary>
        /// <returns></returns>
        public static double TotalRam()
        {
            double capacity = 0;
            ManagementClass cimobject1 = new ManagementClass("Win32_PhysicalMemory");
            ManagementObjectCollection moc1 = cimobject1.GetInstances();
            foreach (ManagementObject mo1 in moc1)
                capacity += ((Math.Round(Int64.Parse(mo1.Properties["Capacity"].Value.ToString()) / 1024 / 1024 / 1024.0, 1)));
            moc1.Dispose();
            cimobject1.Dispose();
            return capacity;
        }

        /// <summary>
        /// CPU占用率
        /// </summary>
        /// <returns></returns>
        public static float UsedCpuPercent()
        {
            PerformanceCounter pcCpuLoad = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            pcCpuLoad.MachineName = ".";
           return  pcCpuLoad.NextValue(); 
        }

        /// <summary>
        /// 设置系统时间
        /// </summary>
        /// <param name="NewTime"></param>
        /// <returns></returns>
        public static bool SetTime(DateTime NewTime)
        {
            SYSTEMTIME time = new SYSTEMTIME();
            time.wYear = (ushort)NewTime.Year;
            time.wMonth = (ushort)NewTime.Month;
            time.wDayOfWeek = (ushort)NewTime.DayOfWeek;
            time.wDay = (ushort)NewTime.Day;
            time.wHour = (ushort)NewTime.Hour;
            time.wMinute = (ushort)NewTime.Minute;
            time.wSecond = (ushort)NewTime.Second;
            time.wMilliseconds = (ushort)NewTime.Millisecond;
            return  APIHelper.SetSystemTime(time);
        }

        /// <summary>
        /// 重启
        /// </summary>
        /// <returns></returns>
        public static bool Reboot()
        {
            return APIHelper.DoExitWin((Const.EWX_FORCE | Const.EWX_REBOOT));
        }

        /// <summary>
        /// 关机
        /// </summary>
        /// <returns></returns>
        public static bool PowerOff()
        {
            return APIHelper.DoExitWin((Const.EWX_FORCE | Const.EWX_POWEROFF));
        }

        /// <summary>
        /// 注销
        /// </summary>
        /// <returns></returns>
        public static bool LogoOff()
        {
            return APIHelper.DoExitWin((Const.EWX_FORCE | Const.EWX_LOGOFF));
        }

        /// <summary>
        /// 待机
        /// </summary>
        /// <param name="force">强制待机</param>
        public static void Suspend(bool force=false)
        {
           APIHelper.SetSuspendState(0, (int)(force? 1: 0), 0);
        }

        /// <summary>
        /// 获取电源状态
        /// </summary>
        /// <param name="BatteryLeft">电池剩余率</param>
        /// <returns></returns>
        public static int Power(out byte BatteryLeft)
        {
            BatteryLeft = 0;
            SYSTEM_POWER_STATUS status = new SYSTEM_POWER_STATUS();
            status.ACLineStatus =255;
            status.BatteryFlag = 0;
            status.BatteryFullLifeTime = 0;
            status.BatteryLifePercent = 0;
            status.BatteryLifeTime = 0;
            if (APIHelper.GetSystemPowerStatus(ref status))
                BatteryLeft = status.BatteryLifePercent;
            if (status.ACLineStatus == 255)
                return -2;
            else
                return (int)status.ACLineStatus;
        }

    }
}
