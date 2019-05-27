using System.Runtime.InteropServices;
namespace NK.API.Struct
{
    /// <summary>
    /// Specifies a date and time, using individual members for the month, day, year, weekday, hour, minute, second, and millisecond. The time is either in coordinated universal time (UTC) or local time, depending on the function that is being called.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SYSTEMTIME
    {
        ushort wYear;
        ushort wMonth;
        ushort wDayOfWeek;
        ushort wDay;
        ushort wHour;
        ushort wMinute;
        ushort wSecond;
        ushort wMilliseconds;
    }
}
