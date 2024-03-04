using System;
using System.Collections.Generic;
using System.Management;
using System.Security.Principal;

namespace BSS.System.Windows
{
    public static class xLocalUsers
    {
        ///<summary>Retrieves</summary>
        public static List<String> GetSystemUserList(Boolean hideDisabled = false, Boolean hide_StatusNotOK = false)
        {
            String wql_Query = "SELECT * FROM Win32_UserAccount";
            Boolean isFirstArg = false;

            if (hideDisabled)
            {
                AddCA("Disabled = 0");
            }

            if (hide_StatusNotOK)
            {
                AddCA("STATUS LIKE 'OK'");
            }

            AddCA("LocalAccount = 1");

            using ManagementObjectSearcher managementObjectSearcher = new(new SelectQuery(wql_Query));

            List<String> users = new();

            foreach (ManagementBaseObject managementBaseObject in managementObjectSearcher.Get())
            {
                users.Add($"{managementBaseObject["Name"]}");
            }

            return users;

            //# # # # # # # # # # # # # #

            void AddCA(String add)
            {
                if (!isFirstArg)
                {
                    wql_Query += $" WHERE {add}";

                    isFirstArg = true;
                }
                else
                {
                    wql_Query += $" AND {add}";
                }
            }
        }

        ///<summary>Gets the current UAC user</summary>
        public static String GetUACUser()
        {
            return WindowsIdentity.GetCurrent().Name.Split('\\')[1];
        }
    }
}