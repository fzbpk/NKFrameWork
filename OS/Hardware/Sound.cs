using System;
using System.Diagnostics;
using Microsoft.VisualBasic;
using System.Management;
using System.Runtime.InteropServices;
using NK.OS.Struct;
using NK.OS.Enum;
namespace NK.OS
{
    public static partial class Sound
    {

        /// <summary>
        /// 获取音量值
        /// </summary>
        /// <param name="MxId"></param>
        /// <returns></returns>
        public static int GetVolume(int MxId = 0)
        {
            int mixer;
            MIXERCONTROL volCtrl = new MIXERCONTROL();
            int currentVol;
            APIHelper.mixerOpen(out mixer, MxId, 0, 0, 0);
            int type = Const.MIXERCONTROL_CONTROLTYPE_VOLUME;
            APIHelper.GetVolumeControl(mixer, Const.MIXERLINE_COMPONENTTYPE_DST_SPEAKERS, type, out volCtrl, out currentVol);
            APIHelper.mixerClose(mixer);
            return currentVol;
        }

        /// <summary>
        /// 设置音量
        /// </summary>
        /// <param name="vVolume"></param>
        /// <param name="MxId"></param>
        /// <returns></returns>
        public static bool SetVolume(int vVolume, int MxId = 0)
        {
            int mixer;
            MIXERCONTROL volCtrl = new MIXERCONTROL();
            int currentVol;
            APIHelper.mixerOpen(out mixer, MxId, 0, 0, 0);
            int type = Const.MIXERCONTROL_CONTROLTYPE_VOLUME;
            APIHelper.GetVolumeControl(mixer, Const.MIXERLINE_COMPONENTTYPE_DST_SPEAKERS, type, out volCtrl, out currentVol);
            if (vVolume > volCtrl.lMaximum) vVolume = volCtrl.lMaximum;
            if (vVolume < volCtrl.lMinimum) vVolume = volCtrl.lMinimum;
            APIHelper.SetVolumeControl(mixer, volCtrl, vVolume);
            APIHelper.GetVolumeControl(mixer, Const.MIXERLINE_COMPONENTTYPE_DST_SPEAKERS, type, out volCtrl, out currentVol);
            APIHelper.mixerClose(mixer);
            if (vVolume != currentVol)
                return false;
            else
                return true;
        }

        /// <summary>
        /// 提升音量
        /// </summary>
        /// <param name="hwd"></param>
        /// <returns></returns>
        public static bool VolumeUp(IntPtr hwd)
        {
            return APIHelper.SendMessage(hwd,  (int)SYSMessage.WM_APPCOMMAND,  0x30292, (int)(0x0a * 0x10000)) != IntPtr.Zero;
        }

        /// <summary>
        /// 降低音量
        /// </summary>
        /// <param name="hwd"></param>
        /// <returns></returns>
        public static bool VolumeDown(IntPtr hwd)
        {
            return APIHelper.SendMessage(hwd, (int)SYSMessage.WM_APPCOMMAND, 0x30292, (int)(0x09 * 0x10000)) != IntPtr.Zero;
        }

        /// <summary>
        /// 静音
        /// </summary>
        /// <param name="hwd"></param>
        /// <returns></returns>
        public static bool Mute(IntPtr hwd)
        {
            return APIHelper.SendMessage(hwd, (int)SYSMessage.WM_APPCOMMAND,  0x200eb0, (int)(0x08 * 0x10000)) != IntPtr.Zero;
        }
    }
}
