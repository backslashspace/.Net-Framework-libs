using System;

namespace BSS.ColoredConsole
{
    ///<summary>Writes to console with color</summary>
    public static class xConsole
    {
        ///<summary>Print to console with color.</summary>
        public static void Write(Object input, ConsoleColor foregroundColor = ConsoleColor.Gray, ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            Console.ForegroundColor = foregroundColor;

            Console.BackgroundColor = backgroundColor;

            Console.Write(input.ToString());

            Console.ResetColor();
        }

        ///<summary>Print to console with color.</summary>
        public static void WriteLine(Object input, ConsoleColor foregroundColor = ConsoleColor.Gray, ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            Console.ForegroundColor = foregroundColor;

            Console.BackgroundColor = backgroundColor;

            Console.WriteLine(input.ToString());

            Console.ResetColor();
        }
    }
}