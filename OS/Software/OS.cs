using System;
using System.Diagnostics; 
using System.Management;
using NK.OS.Enum;
using NK.OS.Struct;
namespace NK.OS
{
    /// <summary>
    /// 操作系统相关
    /// </summary>
   public static partial class OS
    {
        /// <summary>
        /// 操作系统版本
        /// </summary>
        /// <returns></returns>
        public static WinsVers CurrentPlatform()
        {
            OperatingSystem os = Environment.OSVersion;
            WinsVers Vers = WinsVers.UnKnown;
            switch (os.Platform)
            {
                case PlatformID.Win32Windows:
                    switch (os.Version.Minor)
                    {
                        case 0:
                            Vers = WinsVers.Windows95;
                            break;
                        case 10:
                            if (os.Version.Revision.ToString() == "2222A")
                                Vers = WinsVers.Windows982ndEdition;
                            else
                                Vers = WinsVers.Windows98;
                            break;
                        case 90:
                            Vers = WinsVers.WindowsME;
                            break; 
                    }
                    break;
                case PlatformID.Win32NT:
                    switch (os.Version.Major)
                    {
                        case 3:
                            Vers = WinsVers.WindowsNT351;
                            break;
                        case 4:
                            Vers = WinsVers.WindowsNT40;
                            break;
                        case 5:
                            switch (os.Version.Minor)
                            {
                                case 0:
                                    Vers = WinsVers.Windows2000;
                                    break;
                                case 1:
                                    Vers = WinsVers.WindowsXP;
                                    break;
                                case 2:
                                    Vers = WinsVers.Windows2003;
                                    break; 
                            }
                            break;
                        case 6:
                            switch (os.Version.Minor)
                            {
                                case 0:
                                    Vers = WinsVers.WindowsVista;
                                    break;
                                case 1:
                                    Vers = WinsVers.Windows7;
                                    break;
                            }
                            break;
                    }
                    break;
                case PlatformID.WinCE:
                    Vers = WinsVers.WindowsCE;
                    break;
                case PlatformID.MacOSX:
                    Vers = WinsVers.Mac;
                    break;
                case PlatformID.Unix:
                    Vers = WinsVers.Unix;
                    break;
                case PlatformID.Xbox:
                    Vers = WinsVers.Xbox;
                    break;
            }
            return Vers;
        }

        /// <summary>
        /// 64位操作系统
        /// </summary>
        /// <returns></returns>
        public static bool OS64Bit()
        {
            return Environment.Is64BitOperatingSystem;
        }

        public static string OSVersion()
        {
            OperatingSystem osInfo = Environment.OSVersion;
            return osInfo.Version.ToString();
        }

        /// <summary>
        /// .NET版本号
        /// </summary>
        /// <returns></returns>
        public static string DotNetVersion()
        {
            return Environment.Version.Major.ToString() + "." + Environment.Version.Minor.ToString() + "." + Environment.Version.Build.ToString();
        }


        /// <summary>
        /// DirectX版本
        /// </summary>
        /// <returns></returns>
        public static string DirectXVersion()
        {
            const uint HKEY_LOCAL_MACHINE = 0x80000002;
            ObjectGetOptions obj = new ObjectGetOptions();  
            ManagementClass registry =OSHelper.CreateCom("StdRegProv");  
            ManagementBaseObject inParams = registry.GetMethodParameters("GetStringValue");  
            inParams["hDefKey"] = HKEY_LOCAL_MACHINE;       
            inParams["sSubKeyName"] = "Software\\Microsoft\\DirectX";  
            inParams["sValueName"] = "Version";
            ManagementBaseObject outParams = registry.InvokeMethod("GetStringValue", inParams, null);
            if ((uint)outParams.Properties["ReturnValue"].Value == 0)
                return (string)outParams.Properties["sValue"].Value;
            else
                return "";
        }

        /// <summary>
        /// 计算机名
        /// </summary>
        /// <returns></returns>
        public static string MachineName()
        {
            return Environment.MachineName;
        }

        /// <summary>
        /// 当前工作域
        /// </summary>
        /// <returns></returns>
        public static string Domain()
        {
            return Environment.UserDomainName;
        }

        /// <summary>
        /// 写入系统日志
        /// </summary>
        /// <param name="LogSource">日志源</param>
        /// <param name="LogMessage">日志信息</param>
        /// <param name="LogType">日志类型</param>
        /// <param name="type">信息类型</param>
        public static void WriteEventLog(string LogSource,string LogMessage,string LogType="", EventLogEntryType type= EventLogEntryType.Information)
        {
            if (string.IsNullOrEmpty(LogSource) || string.IsNullOrEmpty(LogMessage))
                return;
            if (string.IsNullOrEmpty(LogType))
                LogType = "Application";
            if (!EventLog.SourceExists(LogSource))
                EventLog.CreateEventSource(LogSource, LogType);
            EventLog myLog = new EventLog();
            myLog.Source = LogSource;
            myLog.WriteEntry(LogMessage, type);
        }

        /// <summary>
        /// 读取日志
        /// </summary>
        /// <param name="LogSource">日志来源</param>
        /// <returns></returns>
        public static EventLogEntryCollection ReadEventLog(string LogSource)
        {
            if (string.IsNullOrEmpty(LogSource) )
                return null;
            EventLog myLog = new EventLog();
            myLog.Source = LogSource;
            return myLog.Entries;
        }

        /// <summary>
        /// 删除日志
        /// </summary>
        /// <param name="LogSource">日志来源</param>
        /// <returns></returns>
        public static void ClearLog(string LogSource)
        {
            if (string.IsNullOrEmpty(LogSource))
                return ;
            if (EventLog.Exists(LogSource))
                EventLog.Delete(LogSource);
        }

    }
}
