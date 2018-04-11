using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CafeT.Text
{
    public static class TimeOnText
    {
        public static DateTime[] GetTimes(this string str)
        {
            if (str == null || str.Length <= 0) return null;
            string RegexPattern = @"(\d+)[-.\/](\d+)[-.\/](\d+)";
            System.Text.RegularExpressions.MatchCollection matches
                = Regex.Matches(str, RegexPattern, RegexOptions.IgnoreCase);

            //if (m.Success)
            //{
            //    DateTime dt = DateTime.ParseExact(m.Value, "yyyy-MM-dd-hh-mm-ss", CultureInfo.InvariantCulture);
            //}

            string[] MatchList = new string[matches.Count];
            List<DateTime> _results = new List<DateTime>();
            //string _format = "ddd dd MMM h:mm tt yyyy";
            // add each match
            int c = 0;
            foreach (System.Text.RegularExpressions.Match match in matches)
            {
                MatchList[c] = match.ToString();
                string _dd = match.ToString().Split(new string[] { "/", "-", "." }, StringSplitOptions.None)[0];
                string _mm = match.ToString().Split(new string[] { "/", "-", "." }, StringSplitOptions.None)[1];
                string _yy = match.ToString().Split(new string[] { "/", "-", "." }, StringSplitOptions.None)[2];
                try
                {
                    int _day = int.Parse(_dd);
                    int _month = int.Parse(_mm);
                    int _year = int.Parse(_yy);

                    _results.Add(new DateTime(_year, _month, _day));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                c++;
            }

            return _results.Distinct().ToArray();
        }
    }
}
