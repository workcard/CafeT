using System.Linq;
using System.Text.RegularExpressions;

namespace CafeT.Text
{
    public static class EmailOnText
    {
        public static bool HasEmail(this string str)
        {
            if (str.IsNullOrEmptyOrWhiteSpace()) return false;
            if (str.GetEmails().Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static int CountEmails(this string str)
        {
            return str.GetEmails().Count();
        }

        public static string[] GetEmails(this string str)
        {
            if (str == null || str.Length <= 0) return null;

            string RegexPattern = @"\b[A-Z0-9._-]+@[A-Z0-9][A-Z0-9.-]{0,61}[A-Z0-9]\.[A-Z.]{2,6}\b";

            System.Text.RegularExpressions.MatchCollection matches
                = Regex.Matches(str, RegexPattern, RegexOptions.IgnoreCase);

            string[] MatchList = new string[matches.Count];

            int c = 0;
            foreach (System.Text.RegularExpressions.Match match in matches)
            {
                MatchList[c] = match.ToString();
                c++;
            }

            return MatchList;
        }
    }
}
