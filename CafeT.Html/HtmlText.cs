using CafeT.Enumerable;
using CafeT.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CafeT.Html
{
    
    public static class HtmlText
    {
        public static string ResizeImages(this string html)
        {
            var oldImages = html.GetImages().ToList();
            string copy = html;
            foreach (string img in oldImages)
            {
                string newImg = img.Replace(">", "");
                string _width = @"width=" + "100%";
                string _heigh = @"height=" + "auto";
                if (!img.Contains("width"))
                {
                    newImg = newImg + _width;
                }
                if (!img.Contains("height"))
                {
                    newImg = newImg + _heigh;
                }
                newImg = newImg + ">";
                copy = copy.Replace(img, newImg);
            }
            return copy;
        }
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
