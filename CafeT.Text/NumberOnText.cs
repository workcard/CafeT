using System;

namespace CafeT.Text
{
    public static class NumberOnText
    {
        public static string[] GetNumbers(this string str)
        {
            // Find matches
            System.Text.RegularExpressions.MatchCollection matches
                = System.Text.RegularExpressions.Regex.Matches(str, @"(\d+\.?\d*|\.\d+)");

            string[] MatchList = new string[matches.Count];

            // add each match
            int c = 0;
            foreach (System.Text.RegularExpressions.Match match in matches)
            {
                MatchList[c] = match.ToString();
                c++;
            }

            return MatchList;
        }

        public static string ToReadable(this double number)
        {
            return String.Format("{0:0,0.00}", number);
        }
        public static string ToReadable(this int number)
        {
            return String.Format("{0:0,0.00}", number);
        }
        public static string ToReadable(this float number)
        {
            return String.Format("{0:0,0.00}", number);
        }
        public static string ToReadable(this decimal number)
        {
            return String.Format("{0:0,0.00}", number);
        }
    }
}
