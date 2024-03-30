using System;
using System.IO;
using System.Reflection;
using System.Xml;
using BSS.ColoredConsole;
using BSS.EmbedExtractor;
using BSS.HashTools;
using BSS.Launcher;
using BSS.System.Windows;
using BSS.System.Registry;
using Microsoft.Win32;
using BSS.Encryption.Fips;

namespace Console_Test_Namespace
{
    internal class Program
    {
        private static String hKey = "HKEY_LOCAL_MACHINE";

        static void Main(String[] args)
        {

            //xFipsSecureRandomProxy proxy = new();


            byte[] test = new Byte[32];


            //proxy.NextBytes(ref test);

            


            xConsole.WriteLine(hKey, ConsoleColor.DarkBlue, ConsoleColor.DarkMagenta);
            Console.WriteLine(hKey);





            xRegistry.SetValue("HKEY_LOCAL_MACHINE\\HARDWARE\\test", "", "ff", RegistryValueKind.String);










        }



    }
}