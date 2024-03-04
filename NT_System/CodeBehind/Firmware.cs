using System;
using System.Linq;
using System.Management.Automation;

namespace BSS.System.Windows
{
    public static partial class xFirmware
    {
        private static String PSQuery(String command)
        {
            return PowerShell.Create().AddScript(command).Invoke().FirstOrDefault().ToString().ToLower();
        }
    }
}