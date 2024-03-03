using System;
using System.Reflection;
using BSS.ColoredConsole;
using BSS.EmbedExtractor;
using BSS.HashTools;

namespace Console_Test_Namespace
{
    internal class Program
    {


        static void Main(String[] args)
        {

            Boolean e = xHash.CompareHash(@"C:\Users\dev0\Desktop\ffff\HashTools\adhaopd.txt", "5c65cbc1fa611558b2354ce306dcb6081d491e2deff4f0c717f7303715db83fe");

            xConsole.WriteLine(e, ConsoleColor.DarkYellow);
            xConsole.WriteLine(e, ConsoleColor.DarkYellow);



           

            xExtractor.SaveResourceToDisk("C:\\Users\\dev0\\Desktop\\ffff\\EmbedExtractor\\testnew.txt", "Console_Test_Namespace.Test.txt", Assembly.GetExecutingAssembly(), true);

        }



    }
}