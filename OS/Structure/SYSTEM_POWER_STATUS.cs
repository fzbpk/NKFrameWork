using System.Runtime.InteropServices;
using NK.OS.Enum;
namespace NK.OS.Struct
{
   public struct SYSTEM_POWER_STATUS
    {
        public byte ACLineStatus;
        public byte BatteryFlag;
        public byte BatteryLifePercent;
        public byte SystemStatusFlag;
        public int  BatteryLifeTime;
        public int BatteryFullLifeTime;
    }
}
