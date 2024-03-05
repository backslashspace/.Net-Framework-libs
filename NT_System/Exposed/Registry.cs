using Microsoft.Win32;
using System;
using System.Security;

namespace BSS.System.Registry
{
    ///<summary>Handles Registry access.</summary>
    public static partial class xRegistry
    {
        /// <summary>Patched Registry.SetValue()</summary>
        [SecuritySafeCritical]
        public static void SetValue(String keyName, String valueName, Object value, RegistryValueKind registryValueKind)
        {
            if (keyName == null)
            {
                throw new ArgumentNullException("path");
            }

            String[] pathParts = new String[2];
            keyName.Split(new Char[] { '\\' }, 2).CopyTo(pathParts, 0);
            pathParts[1] ??= "";

            RegistryKey registryKey = OpenSubKey(ref pathParts[0], pathParts[1]);

            if (registryKey == null)
            {
                registryKey = OpenSubKey(ref pathParts[0], "");
                registryKey.CreateSubKey(pathParts[1]);
                registryKey = OpenSubKey(ref pathParts[0], pathParts[1]);
            }

            //fix Registry.SetValue not beeing able to set uints
            if (registryValueKind is RegistryValueKind.DWord && value is UInt32)
            {
                Byte[] rawUInt32 = BitConverter.GetBytes((UInt32)value);
                value = BitConverter.ToInt32(rawUInt32, 0);
            }
            else if (registryValueKind is RegistryValueKind.QWord && value is UInt64)
            {
                Byte[] rawUInt64 = BitConverter.GetBytes((UInt64)value);
                value = BitConverter.ToInt64(rawUInt64, 0);
            }

            try
            {
                registryKey.SetValue(valueName, value, registryValueKind);
                registryKey.Close();
            }
            catch
            {
                registryKey.Close();

                throw;
            }
        }

        #region Get
        /// <summary>Reads a value from the Registry.</summary>
        /// <returns>
        /// Returns data in form of specified <typeparamref name="RegistryValueKind"/>.<br/>
        /// Returns <see langword="null"/> when value is not present.<br/>
        /// Returns '<c>-1</c>' when value has the wrong type and <paramref name="deleteIfWrongType"/> is set to <see langword="false"/>.
        /// </returns>
        public static dynamic GetValue(String path, String value, RegistryValueKind expectedType, Boolean deleteIfWrongType = false)
        {
            Object regOutput = Microsoft.Win32.Registry.GetValue(path, value, null);

            if (regOutput == null)
            {
                return null;
            }

            switch (expectedType)
            {
                case RegistryValueKind.String:
                    if (regOutput is String)
                    {
                        return regOutput;
                    }

                    break;

                case RegistryValueKind.DWord:
                    if (regOutput is Int32)
                    {
                        return BitConverter.ToUInt32(BitConverter.GetBytes((Int32)regOutput), 0);
                    }

                    break;

                case RegistryValueKind.QWord:
                    if (regOutput is Int64)
                    {
                        return BitConverter.ToUInt64(BitConverter.GetBytes((Int64)regOutput), 0);
                    }

                    break;

                case RegistryValueKind.MultiString:
                    if (regOutput is String[])
                    {
                        return regOutput;
                    }

                    break;

                case RegistryValueKind.Binary:
                    if (regOutput is Byte[])
                    {
                        return regOutput;
                    }

                    break;
            }

            //fallback

            if (deleteIfWrongType)
            {
                using RegistryKey Key = PathToRegistryKey(ref path);

                try
                {
                    Key.DeleteValue(value, false);
                }
                catch (ArgumentException) { }

                return null;
            }

            return -1;
        }

        /// <summary>Returns all subkeys.</summary>
        public static String[] GetSubKeys(String path)
        {
            return PathToRegistryKey(ref path).GetSubKeyNames();
        }
        #endregion

        #region Delete
        ///<summary>Removes a value from the Registry.</summary>
        ///<remarks>Takes a value and removes them in the specified path.</remarks>
        ///<returns><see langword="true"/> when at least one error occurred.</returns>
        public static Boolean DeleteValue(String path, String value, Boolean continueOnError = false)
        {
            String[] lonlyValue = new String[] { value };

            return DeleteValues(ref path, ref lonlyValue, ref continueOnError);
        }

        ///<summary>Removes values from the Registry.</summary>
        ///<remarks>Takes an array of values and removes them in the specified path.</remarks>
        ///<returns><see langword="true"/> when at least one error occurred.</returns>
        public static Boolean DeleteValues(String path, String[] values, Boolean continueOnError = true)
        {
            return DeleteValues(ref path, ref values, ref continueOnError);
        }

        ///<summary>Removes a key from the Registry.</summary>
        ///<remarks>Takes a keys and removes it in the specified path.</remarks>
        ///<returns><see langword="true"/> when an error occurred.</returns>
        public static Boolean DeleteSubKeyTree(String keyPath, String key, Boolean continueOnError = false)
        {
            String[] lonlyKey = new String[] { key };

            return DeleteSubKeyTrees(ref keyPath, ref lonlyKey, ref continueOnError);
        }

        ///<summary>Removes keys from the Registry.</summary>
        ///<remarks>Takes an array of keys and removes them in the specified path.</remarks>
        ///<returns><see langword="true"/> when at least one error occurred.</returns>
        public static Boolean DeleteSubKeyTrees(String keyPath, String[] keys, Boolean continueOnError = true)
        {
            return DeleteSubKeyTrees(ref keyPath, ref keys, ref continueOnError);
        }
        #endregion

        ///<summary>Tests if a value exists in the Registry.</summary>
        ///<returns><see langword="false"/> if not present.</returns>
        ///<returns> if not present.</returns>
        public static Boolean TestRegValuePresense(String path, String value)
        {
            return TestRegValuePresence(ref path, ref value);
        }

        /// <summary>Humane path to <see cref="RegistryKey"/></summary>
        public static RegistryKey PathToRegistryKey(String path)
        {
            return PathToRegistryKey(ref  path);
        }
    }
}








