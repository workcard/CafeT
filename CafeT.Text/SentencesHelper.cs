using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;


namespace CafeT.Text
{
    /// <summary>
    ///  An enumeration of sorting options to be used.
    /// </summary>
    public enum SortOrder
    {
        Ascending, // from small to big numbers or alphabetically. 
        Descending // from big to small number or reversed alphabetical order 
    }

    public static class SentencesHelper
    {
        //// This will discard digits 
        //public static char[] delimiters_no_digits = new char[] {
        //    '{', '}', '(', ')', '[', ']', '>', '<','-', '_', '=', '+',
        //    '|', '\\', ':', ';', ' ', ',', '.', '/', '?', '~', '!',
        //    '@', '#', '$', '%', '^', '&', '*', ' ', '\r', '\n', '\t',
        //    '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
        //};
        //public static char[] delimiters_inclue_digits = new char[] {
        //    '{', '}', '(', ')', '[', ']', '>', '<','-', '_', '=', '+',
        //    '|', '\\', ':', ';', ' ', ',','/', '?', '~', '!', // '.', - Nguyen thuy no co cai nay
        //    '@', '#', '$', '%', '^', '&', '*', ' ', '\r', '\n', '\t'
        //};
        /// <summary>
        ///  Tokenizes a text into an array of words, using the improved
        ///  tokenizer with the discard-digit option.
        /// </summary>
        /// <param name="text"> the text to tokenize</param>
        
        #region Words
        public static char[] ExtractSeparators(this string text)
        {
            List<char> separators = new List<char>();
            foreach (char character in text)
            {
                if (!char.IsLetterOrDigit(character))
                {
                    separators.Add(character);
                }
            }
            return separators.ToArray();
        }

        public static string[] GetHasTags(this string text)
        {
            //string text = "This is a string that #contains a hashtag!";
            if (text.IsNullOrEmptyOrWhiteSpace()) return null;
            List<string> _hasTags = new List<string>();
            var regex = new Regex(@"(?<=#)\w+");
            var matches = regex.Matches(text);
            foreach (Match m in matches)
            {
                _hasTags.Add(m.Value);
            }
            return _hasTags.ToArray();
        }

        #endregion

        public static string GetFirstSentence(this string text)
        {
            return GetSentences(text)[0];
        }
        public static string[] GetSentences(this string text)
        {
            string[] sentences = Regex.Split(text, @"(?<=[\.!\?])\s+");
            string[] multiLines = sentences.Where(t => t.Contains('\n') || t.Contains('\r')).ToArray();
            return sentences;
            //string[] newSentences = multiLines
            //    .SelectMany(t=>t.Split(new char[] { '\r','\n'},StringSplitOptions.None))
            //    .ToArray();
            
            //return sentences.Except(multiLines).Union(newSentences)
            //    .Distinct().ToArray();
        }

        /// <summary>
        /// Get all setences that contains the word
        /// </summary>
        /// <param name="text"></param>
        /// <param name="word"></param>
        /// <returns></returns>
        public static string[] GetSentences(this string text, string word)
        {
            return text.GetSentences().Where(t => t.Contains(word))
                .ToArray();
        }
    }
}
