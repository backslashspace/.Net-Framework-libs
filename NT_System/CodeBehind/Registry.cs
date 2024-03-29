﻿using Microsoft.Win32;
using System;

namespace BSS.System.Registry
{
    public static partial class xRegistry
    {
        #region Delete
        private static Boolean DeleteValues(ref String path, ref String[] values, ref Boolean continueOnError)
        {
            using RegistryKey key = PathToRegistryKey(ref path);

            if (key == null)
            {
                return false;
            }

            Boolean errored = false;

            foreach (String value in values)
            {
                try
                {
                    key.DeleteValue(value, true);
                }
                catch
                {
                    if (continueOnError == false)
                    {
                        return true;
                    }

                    errored = true;
                }
            }

            return errored;
        }

        private static Boolean DeleteSubKeyTrees(ref String keyPath, ref String[] keys, ref Boolean continueOnError)
        {
            using RegistryKey path = PathToRegistryKey(ref keyPath);

            if (path == null)
            {
                return false;
            }

            Boolean errored = false;

            foreach (String key in keys)
            {
                try
                {
                    path.DeleteSubKeyTree(key);
                }
                catch
                {
                    if (continueOnError == false)
                    {
                        return true;
                    }

                    errored = true;
                }
            }

            return errored;
        }
        #endregion

        private static Boolean TestRegValuePresence(ref String path, ref String value)
        {
            try
            {
                if (Microsoft.Win32.Registry.GetValue(path, value, null).ToString() == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex) when (ex is ArgumentNullException || ex is NullReferenceException)
            {
                return false;
            }
        }

        private static RegistryKey PathToRegistryKey(ref String path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            String[] pathParts = new String[2];
            path.Split(new Char[] { '\\' }, 2).CopyTo(pathParts, 0);
            pathParts[1] ??= "";

            return OpenSubKey(ref pathParts[0], pathParts[1]);
        }

        private static RegistryKey OpenSubKey(ref String topLevel, String subKey)
        {
            return topLevel.ToUpper() switch
            {
                "HKEY_CURRENT_USER" => Microsoft.Win32.Registry.CurrentUser.OpenSubKey(subKey, true),
                "HKEY_LOCAL_MACHINE" => Microsoft.Win32.Registry.LocalMachine.OpenSubKey(subKey, true),
                "HKEY_CLASSES_ROOT" => Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(subKey, true),
                "HKEY_USERS" => Microsoft.Win32.Registry.Users.OpenSubKey(subKey, true),
                "HKEY_PERFORMANCE_DATA" => Microsoft.Win32.Registry.PerformanceData.OpenSubKey(subKey, true),
                "HKEY_CURRENT_CONFIG" => Microsoft.Win32.Registry.CurrentConfig.OpenSubKey(subKey, true),
                "HKEY_DYN_DATA" => Microsoft.Win32.Registry.DynData.OpenSubKey(subKey, true),
                _ => throw new ArgumentException("Arg_RegInvalidKeyName"),
            };
        }
    }
}