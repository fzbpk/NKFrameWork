using System;
using System.Runtime.InteropServices;
using NK.OS.Struct;
using NK.OS.Enum;
namespace NK.OS
{
    internal class APIHelper
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetSystemTime(SYSTEMTIME lpSystemTime);
        [DllImport("winmm.dll")]
        public static extern long waveOutSetVolume(uint deviceID, uint Volume);

        [DllImport("winmm.dll")]
        public static extern long waveOutGetVolume(uint deviceID, out uint Volume);

        [DllImport("winmm.dll", CharSet = CharSet.Ansi)]
        public static extern int mixerClose(int hmx);

        [DllImport("winmm.dll", CharSet = CharSet.Ansi)]
        public static extern int mixerGetControlDetailsA(int hmxobj, ref

        MIXERCONTROLDETAILS pmxcd, int fdwDetails);

        [DllImport("winmm.dll", CharSet = CharSet.Ansi)]
        public static extern int mixerGetDevCapsA(int uMxId, MIXERCAPS pmxcaps, int cbmxcaps);

        [DllImport("winmm.dll", CharSet = CharSet.Ansi)]
        public static extern int mixerGetID(int hmxobj, int pumxID, int fdwId);

        [DllImport("winmm.dll", CharSet = CharSet.Ansi)]
        public static extern int mixerGetLineControlsA(int hmxobj, ref MIXERLINECONTROLS pmxlc, int fdwControls);

        [DllImport("winmm.dll", CharSet = CharSet.Ansi)]
        public static extern int mixerGetLineInfoA(int hmxobj, ref MIXERLINE pmxl, int fdwInfo);

        [DllImport("winmm.dll", CharSet = CharSet.Ansi)]
        public static extern int mixerGetNumDevs();

        [DllImport("winmm.dll", CharSet = CharSet.Ansi)]
        public static extern int mixerMessage(int hmx, int uMsg, int dwParam1, int dwParam2);

        [DllImport("winmm.dll", CharSet = CharSet.Ansi)]
        public static extern int mixerOpen(out int phmx, int uMxId, int dwCallback, int dwInstance, int fdwOpen);

        [DllImport("winmm.dll", CharSet = CharSet.Ansi)]
        public static extern int mixerSetControlDetails(int hmxobj, ref MIXERCONTROLDETAILS pmxcd, int fdwDetails);

        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, uint wParam, string lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, uint wParam, int lParam);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern bool PostMessage(IntPtr hWnd, int Msg, IntPtr wParam, string lParam);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool ExitWindowsEx(uint uFlags, int dwReason);

        [DllImport("kernel32.dll", EntryPoint = "LoadLibraryA", CharSet = CharSet.Ansi)]
        public static extern IntPtr LoadLibrary(string lpLibFileName);

        [DllImport("kernel32.dll", EntryPoint = "FreeLibrary", CharSet = CharSet.Ansi)]
        public static extern int FreeLibrary(IntPtr hLibModule);

        [DllImport("kernel32.dll", EntryPoint = "GetProcAddress", CharSet = CharSet.Ansi)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

        [DllImport("powrprof.dll", EntryPoint = "SetSuspendState", CharSet = CharSet.Ansi)]
        public static extern int SetSuspendState(int Hibernate, int ForceCritical, int DisableWakeEvent);

        [DllImport("advapi32.dll", EntryPoint = "OpenProcessToken", CharSet = CharSet.Ansi)]
        public static extern int OpenProcessToken(IntPtr ProcessHandle, int DesiredAccess, ref IntPtr TokenHandle);

        [DllImport("advapi32.dll", EntryPoint = "LookupPrivilegeValueA", CharSet = CharSet.Ansi)]
        public static extern int LookupPrivilegeValue(string lpSystemName, string lpName, ref LUID lpLuid);

        [DllImport("advapi32.dll", EntryPoint = "AdjustTokenPrivileges", CharSet = CharSet.Ansi)]
        public static extern int AdjustTokenPrivileges(IntPtr TokenHandle, int DisableAllPrivileges, ref TOKEN_PRIVILEGES NewState, int BufferLength, ref TOKEN_PRIVILEGES PreviousState, ref int ReturnLength);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetCurrentProcess();

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetSystemPowerStatus(ref SYSTEM_POWER_STATUS lpSystemPowerStatus);

        [DllImport("gdi32.dll")]
        public static extern bool GetDeviceGammaRamp(IntPtr hdc, IntPtr lpRamp);

        [DllImport("gdi32.dll")]
        public static extern bool SetDeviceGammaRamp(IntPtr hDC, ref RAMP lpRamp);

        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool EnumDisplaySettings(string lpszDeviceName, int iModeNum, ref DEVMODE lpDevMode);

        [DllImport("user32.dll")]
        public static extern bool EnumDisplayDevices(string lpDevice, uint iDevNum, ref DISPLAY_DEVICE lpDisplayDevice, uint dwFlags);

        [DllImport("user32.dll")]
        public static extern int ChangeDisplaySettings(ref DEVMODE devMode, int flags);



        public static bool GetVolumeControl(int hmixer, int componentType, int ctrlType, out MIXERCONTROL mxc, out int vCurrentVol)
        {
            MIXERLINECONTROLS mxlc = new MIXERLINECONTROLS();
            MIXERLINE mxl = new MIXERLINE();
            MIXERCONTROLDETAILS pmxcd = new MIXERCONTROLDETAILS();
            MIXERCONTROLDETAILS_UNSIGNED du = new MIXERCONTROLDETAILS_UNSIGNED();
            mxc = new MIXERCONTROL();
            int rc;
            bool retValue;
            vCurrentVol = -1;
            mxl.cbStruct = Marshal.SizeOf(mxl);
            mxl.dwComponentType = componentType;
            rc = mixerGetLineInfoA(hmixer, ref mxl,Const. MIXER_GETLINEINFOF_COMPONENTTYPE);
            if (Const.MMSYSERR_NOERROR == rc)
            {
                int sizeofMIXERCONTROL = 152;
                int ctrl = Marshal.SizeOf(typeof(MIXERCONTROL));
                mxlc.pamxctrl = Marshal.AllocCoTaskMem(sizeofMIXERCONTROL);
                mxlc.cbStruct = Marshal.SizeOf(mxlc);
                mxlc.dwLineID = mxl.dwLineID;
                mxlc.dwControl = ctrlType;
                mxlc.cControls = 1;
                mxlc.cbmxctrl = sizeofMIXERCONTROL;
                mxc.cbStruct = sizeofMIXERCONTROL; 
                rc = mixerGetLineControlsA(hmixer, ref mxlc, Const.MIXER_GETLINECONTROLSF_ONEBYTYPE);
                if (Const.MMSYSERR_NOERROR == rc)
                {
                    retValue = true;
                    mxc = (MIXERCONTROL)Marshal.PtrToStructure(mxlc.pamxctrl, typeof(MIXERCONTROL));
                }
                else
                {
                    retValue = false;
                }
                int sizeofMIXERCONTROLDETAILS = Marshal.SizeOf(typeof(MIXERCONTROLDETAILS));
                int sizeofMIXERCONTROLDETAILS_UNSIGNED = Marshal.SizeOf(typeof(MIXERCONTROLDETAILS_UNSIGNED));
                pmxcd.cbStruct = sizeofMIXERCONTROLDETAILS;
                pmxcd.dwControlID = mxc.dwControlID;
                pmxcd.paDetails = Marshal.AllocCoTaskMem(sizeofMIXERCONTROLDETAILS_UNSIGNED);
                pmxcd.cChannels = 1;
                pmxcd.item = 0;
                pmxcd.cbDetails = sizeofMIXERCONTROLDETAILS_UNSIGNED;
                rc = mixerGetControlDetailsA(hmixer, ref pmxcd, Const.MIXER_GETCONTROLDETAILSF_VALUE);
                du = (MIXERCONTROLDETAILS_UNSIGNED)Marshal.PtrToStructure(pmxcd.paDetails, typeof(MIXERCONTROLDETAILS_UNSIGNED));
                vCurrentVol = du.dwValue;
                return retValue;
            }
            retValue = false;
            return retValue;
        }

        public static bool SetVolumeControl(int hmixer, MIXERCONTROL mxc, int volume)
        {
            bool retValue;
            int rc;
            MIXERCONTROLDETAILS mxcd = new MIXERCONTROLDETAILS();
            MIXERCONTROLDETAILS_UNSIGNED vol = new MIXERCONTROLDETAILS_UNSIGNED();
            mxcd.item = 0;
            mxcd.dwControlID = mxc.dwControlID;
            mxcd.cbStruct = Marshal.SizeOf(mxcd);
            mxcd.cbDetails = Marshal.SizeOf(vol);
            mxcd.cChannels = 1;
            vol.dwValue = volume;
            mxcd.paDetails = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(MIXERCONTROLDETAILS_UNSIGNED)));
            Marshal.StructureToPtr(vol, mxcd.paDetails, false);
            rc = mixerSetControlDetails(hmixer, ref mxcd, Const.MIXER_SETCONTROLDETAILSF_VALUE);
            if (Const.MMSYSERR_NOERROR == rc)
            {
                retValue = true;
            }
            else
            {
                retValue = false;
            }
            return retValue;
        }

        public static bool DoExitWin(uint flag)
        {
            IntPtr tokenHandle = IntPtr.Zero;
            LUID privilegeLUID = new LUID();
            TOKEN_PRIVILEGES newPrivileges = new TOKEN_PRIVILEGES();
            TOKEN_PRIVILEGES tokenPrivileges;
            if (OpenProcessToken(APIHelper.GetCurrentProcess(),Const. TOKEN_ADJUST_PRIVILEGES | Const.TOKEN_QUERY, ref tokenHandle) == 0)
                return false;
            if (LookupPrivilegeValue("", "SeShutdownPrivilege", ref privilegeLUID) == 0)
                return false;
            tokenPrivileges.PrivilegeCount = 1;
            tokenPrivileges.Privileges.Attributes =Const. SE_PRIVILEGE_ENABLED;
            tokenPrivileges.Privileges.pLuid = privilegeLUID;
            int size = 4;
            if (AdjustTokenPrivileges(tokenHandle, 0, ref tokenPrivileges, 4 + (12 * tokenPrivileges.PrivilegeCount), ref newPrivileges, ref size) == 0)
                return false;
            return ExitWindowsEx(flag, 0);
        }

    }
}
