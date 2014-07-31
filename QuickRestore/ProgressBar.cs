using System;

namespace QuickRestore
{
    public class ProgressBar
    {
        public const string Bar = "----------------------------------------";

        public static void IncrementProgressBar()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("****");
            Console.ResetColor();
        }

        public static void SetupProgressBar(string title)
        {
            Console.WriteLine(Bar);
            Console.WriteLine(title.PadBoth(Bar.Length));
            Console.WriteLine(Bar);
        }
    }
}
