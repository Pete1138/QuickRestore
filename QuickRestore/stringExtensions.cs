namespace QuickRestore
{
    public static class StringExtensions
    {
        /// <summary>
        /// Centers text over a specified text length
        /// </summary>
        /// <param name="str">text to center</param>
        /// <param name="length">number of characters over which to center text</param>
        /// <returns></returns>
        public static string PadBoth(this string str, int length)
        {
            var spaces = length - str.Length;
            var padLeft = spaces / 2 + str.Length;
            return str.PadLeft(padLeft).PadRight(length);
        }
    }

}
