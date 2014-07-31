using System;

namespace QuickRestore
{
    public class ProgressBar
    {
        public const string Bar = "******************************";

        public static void IncrementProgressBar()
        {
            Console.Write("***");
        }

        public static void SetupProgressBar(string title)
        {
            Console.WriteLine(Bar);
            Console.WriteLine(title.PadBoth(Bar.Length));
            Console.WriteLine(Bar);
        }
    }
}
