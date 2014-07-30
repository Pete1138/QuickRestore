using System;

namespace QuickRestore
{
    public class ProgressBar
    {
        public static void IncrementProgressBar()
        {
            Console.Write("*");
        }

        public static void SetupProgressBar()
        {
            Console.WriteLine("**********");
        }
    }
}
