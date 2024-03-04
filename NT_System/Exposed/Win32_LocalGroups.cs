using System;
using System.Linq;
using System.Management;

namespace BSS.System.Windows
{
    public static class xLocalGroups
    {
        ///<summary>Gets local Administrator group name</summary>
        public static String GetAdminGroupName()
        {
            SelectQuery licenseQuery = new(@"SELECT Name FROM Win32_Group 
                                             WHERE SID LIKE 'S-1-5-32-544'");

            ManagementObjectSearcher searcher = new(licenseQuery);

            ManagementObject wmiObject = searcher.Get().OfType<ManagementObject>().FirstOrDefault();

            return $"{wmiObject["Name"]}";
        }
    }
}