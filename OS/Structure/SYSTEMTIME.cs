namespace NK.OS.Struct
{
    /// <summary>
    /// Specifies a date and time, using individual members for the month, day, year, weekday, hour, minute, second, and millisecond. 
    /// </summary>
    public class SYSTEMTIME
    {
        /// <summary>
        /// The year. The valid values for this member are 1601 through 30827.
        /// </summary>
       public  ushort wYear;
        /// <summary>
        /// The month. This member can be one of the following values.
        /// </summary>
        public ushort wMonth;
        /// <summary>
        /// The day of the week. This member can be one of the following values.
        /// </summary>
        public ushort wDayOfWeek;
        /// <summary>
        /// The day of the month. The valid values for this member are 1 through 31.
        /// </summary>
        public ushort wDay;
        /// <summary>
        /// The hour. The valid values for this member are 0 through 23.
        /// </summary>
        public ushort wHour;
        /// <summary>
        /// The minute. The valid values for this member are 0 through 59.
        /// </summary>
        public ushort wMinute;
        /// <summary>
        /// The second. The valid values for this member are 0 through 59.
        /// </summary>
        public ushort wSecond;
        /// <summary>
        /// The millisecond. The valid values for this member are 0 through 999.
        /// </summary>
        public ushort wMilliseconds;
    }
}
