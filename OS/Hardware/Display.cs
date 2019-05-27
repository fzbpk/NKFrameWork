using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using NK.OS.Struct;
using NK.OS.Enum;
using System.Windows.Forms;
namespace NK.OS
{
    /// <summary>
    /// 显示器
    /// </summary>
    public static partial class Display
    {
        /// <summary>
        /// 关闭显示器
        /// </summary>
        /// <param name="SW"></param>
        /// <returns></returns>
        public static bool MoniterPower(bool SW)
        {
            IntPtr HWND_BROADCAST = new IntPtr(0xFFFF);
            IntPtr res;
            if(SW)
              res=APIHelper.SendMessage(HWND_BROADCAST,(uint)SYSMessage.WM_SYSCOMMAND,Const.SC_MONITORPOWER, -1);
            else
              res = APIHelper.SendMessage(HWND_BROADCAST, (uint)SYSMessage.WM_SYSCOMMAND, Const.SC_MONITORPOWER, 2);
            if (res == IntPtr.Zero)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 设置gamma
        /// </summary>
        /// <param name="gamma"></param>
        /// <returns></returns>
        public static bool SetGamma(int gamma)
        {
            if (gamma <= 256 && gamma >= 1)
            {
                RAMP ramp = new RAMP();
                ramp.Red = new ushort[256];
                ramp.Green = new ushort[256];
                ramp.Blue = new ushort[256];
                for (int i = 1; i < 256; i++)
                {
                    int iArrayValue = i * (gamma + 128);

                    if (iArrayValue > 65535)
                        iArrayValue = 65535;
                    ramp.Red[i] = ramp.Blue[i] = ramp.Green[i] = (ushort)iArrayValue;
                }
              return   APIHelper.SetDeviceGammaRamp(APIHelper.GetDC(IntPtr.Zero), ref ramp);
            }
            return false;
        }

        public static int ChangeDirection(ScreenOrientation Angle,uint deviceID=0)
        {
            DISPLAY_DEVICE d = new DISPLAY_DEVICE();
            DEVMODE vDevMode = new DEVMODE();
            d.cb = Marshal.SizeOf(d);
            APIHelper.EnumDisplayDevices(null, deviceID, ref d, 0);
            if (APIHelper.EnumDisplaySettings(d.DeviceName, Const.ENUM_CURRENT_SETTINGS, ref vDevMode))
            {
                vDevMode.dmFields =(int) Const.DM_DISPLAYORIENTATION;
                vDevMode.dmDisplayOrientation = Angle;
                return APIHelper.ChangeDisplaySettings(ref vDevMode, 0);
            }
            return -2;   
        }

        public static int ChangeResolution(uint iWidth, uint iHeight, ushort frq, ushort ColorDeapth, uint deviceID = 0)
        {
            DISPLAY_DEVICE d = new DISPLAY_DEVICE();
            DEVMODE vDevMode = new DEVMODE();
            d.cb = Marshal.SizeOf(d);
            APIHelper.EnumDisplayDevices(null, deviceID, ref d, 0);
            if (APIHelper.EnumDisplaySettings(d.DeviceName, Const.ENUM_CURRENT_SETTINGS, ref vDevMode))
            {
                vDevMode.dmFields = (int)Const.DM_PELSWIDTH | (int)Const.DM_PELSHEIGHT | (int)Const.DM_DISPLAYFREQUENCY | (int)Const.DM_BITSPERPEL;
                vDevMode.dmPelsWidth = (int)iWidth;
                vDevMode.dmPelsHeight = (int)iHeight;
                vDevMode.dmDisplayFrequency = frq;
                vDevMode.dmBitsPerPel = ColorDeapth;
                return APIHelper.ChangeDisplaySettings(ref vDevMode, 0);
            }
            return -2;
        }

        public static bool ReadResolution(out uint iWidth, out uint iHeight, out ushort frq, out ushort ColorDeapth, uint deviceID = 0)
        {
            iWidth = 0;
            iHeight = 0;
            frq = 0;
            ColorDeapth = 0;
            DISPLAY_DEVICE d = new DISPLAY_DEVICE();
            DEVMODE vDevMode = new DEVMODE();
            d.cb = Marshal.SizeOf(d);
            APIHelper.EnumDisplayDevices(null, deviceID, ref d, 0);
            if (APIHelper.EnumDisplaySettings(d.DeviceName, Const.ENUM_CURRENT_SETTINGS, ref vDevMode))
            {
                vDevMode.dmFields = (int)Const.DM_PELSWIDTH | (int)Const.DM_PELSHEIGHT | (int)Const.DM_DISPLAYFREQUENCY | (int)Const.DM_BITSPERPEL;
                iWidth=(uint) vDevMode.dmPelsWidth  ;
                iHeight=(uint)vDevMode.dmPelsHeight ;
                frq=(ushort) vDevMode.dmDisplayFrequency ;
                ColorDeapth=(ushort)vDevMode.dmBitsPerPel ;
                return  true ;
            }
            return false ;
        }

        public static ScreenOrientation ReadDirection( uint deviceID = 0)
        {
            DISPLAY_DEVICE d = new DISPLAY_DEVICE();
            DEVMODE vDevMode = new DEVMODE();
            d.cb = Marshal.SizeOf(d);
            APIHelper.EnumDisplayDevices(null, deviceID, ref d, 0);
            if (APIHelper.EnumDisplaySettings(d.DeviceName, Const.ENUM_CURRENT_SETTINGS, ref vDevMode))
                return vDevMode.dmDisplayOrientation;
            else
                return  ScreenOrientation.Angle0;
        }

    }
}
