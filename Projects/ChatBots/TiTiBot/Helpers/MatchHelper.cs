using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TiTiBot.Helpers
{
    public class MatchHelper
    {
        public virtual Tuple<bool, int> ScoreMatch(string option, string input)
        {
            var trimmed = input.Trim();
            var text = option.ToString();
            bool occurs = text.IndexOf(trimmed, StringComparison.CurrentCultureIgnoreCase) >= 0;
            bool equals = text == trimmed;
            return occurs
                ? Tuple.Create(equals, trimmed.Length)
                : null;
        }

        public bool TryParse<T>(IEnumerable<T> options, Func<T, string> propertyFunc, string text, out T result)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                var scores = from option in options
                             let score = ScoreMatch(propertyFunc(option), text)
                             select new { score, option };

                var winner = scores.Where(s => s.score != null).OrderBy(s => s.score).First();
                if (winner.score != null)
                {
                    result = winner.option;
                    return true;
                }
            }

            result = default(T);
            return false;
        }

    }
}