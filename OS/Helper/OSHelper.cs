using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Management;
using Microsoft.VisualBasic;
namespace NK.OS
{
     internal class OSHelper
    {
        public static ManagementObjectCollection SelectHardWare(string Name,string scorp="")
        {
            ManagementObjectSearcher hardware = null;
            if(string.IsNullOrEmpty(scorp))
                hardware = new ManagementObjectSearcher("SELECT * FROM " + Name);
            else
                hardware = new ManagementObjectSearcher(scorp,"SELECT * FROM " + Name);
            return hardware.Get();
        }

        public static ManagementClass CreateCom(string Com, string scorp = "")
        {

            ObjectGetOptions obj = new ObjectGetOptions(null, System.TimeSpan.MaxValue, true);
            ManagementClass registry = null;
            if (string.IsNullOrEmpty(scorp))
                registry = new ManagementClass(new ManagementPath(Com), obj);
            else
                registry = new ManagementClass(new ManagementScope(scorp), new ManagementPath(Com), obj);
            return registry;
        }

        public int Shell(string Cmd, AppWinStyle Style = AppWinStyle.MinimizedFocus, bool Wait = false, int Timeout = -1)
        {
            return Interaction.Shell(Cmd, Style, Wait, Timeout);
        }

    }
}
