using CafeT.Enumerable;
using CafeT.Text;
using System.Collections.Generic;
using System.Text;

namespace CafeT.Html
{
    
    public static class HtmlText
    {
        /// <summary>
        /// Return the first n words in the html
        /// </summary>
        /// <param name="html"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static string TakeMaxWords(this string html, int n)
        {
            string words = html, n_words;
            words = html.StripHtml();
            n_words = GetNWords(words, n);
            return n_words;
        }


        /// <summary>
        /// Returns the first n words in text
        /// Assumes text is not a html string
        /// http://stackoverflow.com/questions/13368345/get-first-250-words-of-a-string
        /// http://stackoverflow.com/questions/1279859/how-to-replace-multiple-white-spaces-with-one-white-space
        /// </summary>
        /// <param name="text"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static string GetNWords(this string text, int n)
        {
            StringBuilder builder = new StringBuilder();
            string cleanedString = System.Text.RegularExpressions.Regex.Replace(text, @"\s+", " ");
            IEnumerable<string> words = cleanedString.Split().TakeMax(n + 1);
            foreach (string word in words)
                builder.Append(" " + word);

            return builder.ToString();
        }
    }
}
