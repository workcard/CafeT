using CafeT.Enumerable;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace CafeT.Text
{
    public static class StringMethodExtensions
    {
        private static string _paraBreak = "\r\n\r\n";
        private static string _link = "<a href=\"{0}\">{1}</a>";
        private static string _linkNoFollow = "<a href=\"{0}\" rel=\"nofollow\">{1}</a>";

        /// <summary>
        /// Returns a copy of this string converted to HTML markup.
        /// </summary>
        public static string ToHtml(this string s)
        {
            return ToHtml(s, false);
        }

        /// <summary>
        /// Returns a copy of this string converted to HTML markup.
        /// </summary>
        /// <param name="nofollow">If true, links are given "nofollow"
        /// attribute</param>
        public static string ToHtml(this string s, bool nofollow)
        {
            StringBuilder sb = new StringBuilder();

            int pos = 0;
            while (pos < s.Length)
            {
                // Extract next paragraph
                int start = pos;
                pos = s.IndexOf(_paraBreak, start);
                if (pos < 0)
                    pos = s.Length;
                string para = s.Substring(start, pos - start).Trim();

                // Encode non-empty paragraph
                if (para.Length > 0)
                    EncodeParagraph(para, sb, nofollow);

                // Skip over paragraph break
                pos += _paraBreak.Length;
            }
            // Return result
            return sb.ToString();
        }

        /// <summary>
        /// Encodes a single paragraph to HTML.
        /// </summary>
        /// <param name="s">Text to encode</param>
        /// <param name="sb">StringBuilder to write results</param>
        /// <param name="nofollow">If true, links are given "nofollow"
        /// attribute</param>
        private static void EncodeParagraph(string s, StringBuilder sb, bool nofollow)
        {
            // Start new paragraph
            sb.AppendLine("<p>");

            // HTML encode text
            s = HttpUtility.HtmlEncode(s);

            // Convert single newlines to <br>
            s = s.Replace(Environment.NewLine, "<br />\r\n");

            // Encode any hyperlinks
            EncodeLinks(s, sb, nofollow);

            // Close paragraph
            sb.AppendLine("\r\n</p>");
        }

        /// <summary>
        /// Encodes [[URL]] and [[Text][URL]] links to HTML.
        /// </summary>
        /// <param name="text">Text to encode</param>
        /// <param name="sb">StringBuilder to write results</param>
        /// <param name="nofollow">If true, links are given "nofollow"
        /// attribute</param>
        private static void EncodeLinks(string s, StringBuilder sb, bool nofollow)
        {
            // Parse and encode any hyperlinks
            int pos = 0;
            while (pos < s.Length)
            {
                // Look for next link
                int start = pos;
                pos = s.IndexOf("[[", pos);
                if (pos < 0)
                    pos = s.Length;
                // Copy text before link
                sb.Append(s.Substring(start, pos - start));
                if (pos < s.Length)
                {
                    string label, link;

                    start = pos + 2;
                    pos = s.IndexOf("]]", start);
                    if (pos < 0)
                        pos = s.Length;
                    label = s.Substring(start, pos - start);
                    int i = label.IndexOf("][");
                    if (i >= 0)
                    {
                        link = label.Substring(i + 2);
                        label = label.Substring(0, i);
                    }
                    else
                    {
                        link = label;
                    }
                    sb.Append(String.Format(nofollow ? _linkNoFollow : _link, link, label));
                    pos += 2;
                }
            }
        }
    }

    public static class StringHelper
    {
        // This will discard digits 
        public static char[] delimiters_inclue_command = new char[] {
            //'{', '}', '(', ')', '[', ']', '>', '<','-', '_', '=', '+',
            //'|', '\\', ':',  ' ', ',',  '/', '?', '~', '!', //'.',';',
            //'@', '#', '$', '%', '^', '&', '*',
            ' ', '\r', '\n', '\t',
            //'0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
        };
        public static char[] delimiters_clean_word = new char[] {
            '{', '}', '(', ')', '[', ']', '>', '<','-', '_', '=', '+',
            '|', '\\', ':', ';', ' ', ',', '.', '/', '?', '~', '!',
            '@', '#', '$', '%', '^', '&', '*', ' ', '\r', '\n', '\t',
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
        };
        public static char[] delimiters_inclue_digits = new char[] {
            '{', '}', '(', ')', '[', ']', '>', '<','-', '_', '=', '+',
            '|', '\\', ':', ';', ' ', ',','/', '?', '~', '!', // '.', - Nguyen thuy no co cai nay
            '@', '#', '$', '%', '^', '&', '*', ' ', '\r', '\n', '\t'
        };
        public static string[] Tokenize(this string text)
        {
            string[] tokens = text.Split(delimiters_clean_word, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < tokens.Length; i++)
            {
                string token = tokens[i];

                // Change token only when it starts and/or ends with "'" and  
                // it has at least 2 characters. 

                if (token.Length > 1)
                {
                    if (token.StartsWith("'") && token.EndsWith("'"))
                        tokens[i] = token.Substring(1, token.Length - 2); // remove the starting and ending "'" 

                    else if (token.StartsWith("'"))
                        tokens[i] = token.Substring(1); // remove the starting "'" 

                    else if (token.EndsWith("'"))
                        tokens[i] = token.Substring(0, token.Length - 1); // remove the last "'" 
                }
            }
            return tokens;
        }
        /// <summary>
        ///  Make a string-integer dictionary out of an array of words.
        /// </summary>
        /// <param name="words"> the words out of which to make the dictionary</param>
        /// <returns> a string-integer dictionary</returns>
        public static Dictionary<string, int> ToStrIntDict(string[] words)
        {
            Dictionary<string, int> dict = new Dictionary<string, int>();

            foreach (string word in words)
            {
                // if the word is in the dictionary, increment its freq. 
                if (dict.ContainsKey(word))
                {
                    dict[word]++;
                }
                // if not, add it to the dictionary and set its freq = 1 
                else
                {
                    dict.Add(word, 1);
                }
            }

            return dict;
        }

        /// <summary>
        ///  Sort a string-int dictionary by its entries' values.
        /// </summary>
        /// <param name="strIntDict"> a string-int dictionary to sort</param>
        /// <param name="sortOrder"> one of the two enumerations: Ascending and Descending</param>
        /// <returns> a string-integer dictionary sorted by integer values</returns>
        public static Dictionary<string, int> ListWordsByFreq(Dictionary<string, int> strIntDict, SortOrder sortOrder)
        {
            // Copy keys and values to two arrays 
            string[] words = new string[strIntDict.Keys.Count];
            strIntDict.Keys.CopyTo(words, 0);

            int[] freqs = new int[strIntDict.Values.Count];
            strIntDict.Values.CopyTo(freqs, 0);

            //Sort by freqs: it sorts the freqs array, but it also rearranges 
            //the words array's elements accordingly (not sorting) 
            Array.Sort(freqs, words);

            // If sort order is descending, reverse the sorted arrays. 
            if (sortOrder == SortOrder.Descending)
            {
                //reverse both arrays 
                Array.Reverse(freqs);
                Array.Reverse(words);
            }

            //Copy freqs and words to a new Dictionary<string, int> 
            Dictionary<string, int> dictByFreq = new Dictionary<string, int>();
            Dictionary<string, List<int>> dictResult = new Dictionary<string, List<int>>();

            for (int i = 0; i < freqs.Length; i++)
            {
                dictByFreq.Add(words[i], freqs[i]);
            }

            return dictByFreq;
        }

        public static Dictionary<string, int> ToWordsWithFreq(this string text)
        {
            // Split text into array of words 
            string[] words = Tokenize(text);

            if (words.Length > 0)
            {
                // Make a string-int dictionary out of the array of words  
                Dictionary<string, int> dict = ToStrIntDict(words);
                SortOrder sortOrder = SortOrder.Ascending;

                // Sort dict by values 
                dict = ListWordsByFreq(dict, sortOrder);
                return dict;
            }
            return null;
        }

        public static string[] ToTokens(this string text)
        {
            if (!text.IsNullOrEmptyOrWhiteSpace())
            {
                text = text.ToStandard();
                string[] words = text.Split(delimiters_inclue_command, StringSplitOptions.RemoveEmptyEntries);
                words = words
                    .Where(t => t.EndsWith(".")).Select(t => t.PadRight(1))
                    .Union(words)
                    .Distinct().ToArray();
                return words;
            }
            return null;
        }

        public static string[] ToWords(this string text)
        {
            if (!text.IsNullOrEmptyOrWhiteSpace())
            {
                text = text.ToStandard();
                string[] words = text.Split(delimiters_inclue_command, StringSplitOptions.RemoveEmptyEntries);
                words = words
                    .Where(t => t.EndsWith(".")).Select(t => t.PadRight(1))
                    .Union(words)
                    .Distinct().ToArray();
                return words;
            }
            return null;
        }


        public static string RemoveUnicode(this string text)
        {
            string[] arr1 = new string[] { "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ",
        "đ",
        "é","è","ẻ","ẽ","ẹ","ê","ế","ề","ể","ễ","ệ",
        "í","ì","ỉ","ĩ","ị",
        "ó","ò","ỏ","õ","ọ","ô","ố","ồ","ổ","ỗ","ộ","ơ","ớ","ờ","ở","ỡ","ợ",
        "ú","ù","ủ","ũ","ụ","ư","ứ","ừ","ử","ữ","ự",
        "ý","ỳ","ỷ","ỹ","ỵ",};
            string[] arr2 = new string[] { "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a",
        "d",
        "e","e","e","e","e","e","e","e","e","e","e",
        "i","i","i","i","i",
        "o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o",
        "u","u","u","u","u","u","u","u","u","u","u",
        "y","y","y","y","y",};
            for (int i = 0; i < arr1.Length; i++)
            {
                text = text.Replace(arr1[i], arr2[i]);
                text = text.Replace(arr1[i].ToUpper(), arr2[i].ToUpper());
            }
            return text;
        }


        public static string[] ToCleanWords(this string text)
        {
            if (!text.IsNullOrEmptyOrWhiteSpace())
            {
                text = text.ToStandard();
                string[] words = text.Split(delimiters_clean_word, StringSplitOptions.RemoveEmptyEntries);
                return words;
            }
            return null;
        }
        
        //public static bool IsWord(this string word)
        //{
        //    var _chars = word.ToCharArray();
        //    var _endOfwords = _chars.Where(t => t.IsEndOfWord());
        //    if (_chars.LastOrDefault() == ' ' || _endOfwords.Count() <= 1) return true;
        //    return false;
        //}

        public static bool IsWord(this string text)
        {
            if (!text.IsNullOrEmptyOrWhiteSpace())
            {
                var _chars = text.ToCharArray();
                var _resutls = _chars.Where(t => t.IsOutOfWord());
                if (_resutls != null || _resutls.Count() > 0) return false;
            }
            return true;
        }

        /// <summary>
        /// Must to extend another lang code
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsLangCode(this string text)
        {
            string[] langCodes = new string[] { "en", "vi", "vn" };
            if (langCodes.Contains(text.ToLower())) return true;
            return false;
        }
        public static bool IsVietnameseLangCode(this string text)
        {
            if (text == "vi" || text == "vn") return true;
            return false;
        }

        public static bool IsEnglishLangCode(this string text)
        {
            if (text == "en") return true;
            return false;
        }
        /// <summary>
        /// Must to extend another currency code
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsCurrencyCode(this string text)
        {
            string[] langCodes = new string[] { "vnd", "usd" };
            if (langCodes.Contains(text.ToLower())) return true;
            return false;
        }

        public static string ExtendRightTo(this string word, string text, string to)
        {
            int _index = text.IndexOf(word);
            int _toIndex = text.IndexAll(to).Where(t => t >= _index).FirstOrDefault();
            try
            {
                return text.Substring(_index, _toIndex - _index + 1);
            }
            catch
            {
                return string.Empty;
            }
        }
        public static string ExtendLeftTo(this string word, string text, string to)
        {
            int _index = text.IndexOf(word);
            int _toIndex = text.IndexAll(to).Where(t => t <= _index).LastOrDefault();
            try
            {
                return text.Substring(_toIndex, _index - _index + 1);
            }
            catch
            {
                return string.Empty;
            }
        }
        
        public static string WordsToString(this string[] words)
        {
            string _result = string.Empty;
            foreach (string _word in words)
            {
                _result += " " + _word;
            }
            return _result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Count of words in input string</returns>
        public static int GetCountWords(this string input)
        {
            var count = 0;
            try
            {
                var re = new Regex(@"[^\s]+");
                var matches = re.Matches(input);
                count = matches.Count;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return count;
        }

        public static string[] GetMaxWords(this string text, int? n)
        {
            if (text.IsNullOrEmptyOrWhiteSpace()) return null;
            string[] _words = text.ToWords().AsEnumerable().TakeMax(n).ToArray();
            return _words;
        }

        public static int CountOf(this string input, string word)
        {
            int _result = 0;
            int _index = input.IndexOf(word);
            while (_index >= 0)
            {
                input = input.Remove(_index, word.Length);
                _index = input.IndexOf(word);
                _result = _result + 1;
            }
            return _result;
        }

        public static bool HtmlIsJustText(HtmlNode rootNode)
        {
            return rootNode.Descendants().All(n => n.NodeType == HtmlNodeType.Text);
        }
        public static bool IsHtmlString(this string text)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(text);
            return !HtmlIsJustText(doc.DocumentNode);
        }

        

        public static string[] GetLines(string text)
        {
            string[] lines = text.Replace("\r", "").Split('\n');
            return lines;
        }
        public static string GetLine(string text, int n)
        {
            string[] lines = text.Replace("\r", "").Split('\n');
            return lines.Length >= n ? lines[n - 1] : null;
        }

        public static string DeleteBeginTo(this string text, string to)
        {
            int _firstIndex = text.IndexOf(to);
            return text.Substring(_firstIndex);
        }
        public static string DeleteAllWord(this string text, string word)
        {
            return text.Replace(word, "").Trim();
        }
        public static string DeleteEndTo(this string text, string to)
        {
            if (text.IsNullOrEmptyOrWhiteSpace()) return string.Empty;
            if(text.Contains(to))
            {
                int _lastIndex = text.LastIndexOf(to);
                return text.Substring(0, _lastIndex);
            }
            return text;
        }
        /// <summary>
        /// Exclude the token character (to)
        /// </summary>
        /// <param name="text"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static string GetFromBeginTo(this string text, string to)
        {
            if (text.IsNullOrEmptyOrWhiteSpace()) return string.Empty;
            if(text.Contains(to))
            {
                int _firstIndex = text.IndexOf(to);
                return text.Substring(0, _firstIndex);
            }
            return text;
        }
        /// <summary>
        /// Exclude the token character (to)
        /// </summary>
        /// <param name="text"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static string GetFromEndTo(this string text, string to)
        {
            if (text.IsNullOrEmptyOrWhiteSpace()) return string.Empty;
            if(text.Contains(to))
            {
                int _lastIndex = text.LastIndexOf(to);
                return text.Substring(_lastIndex + 1);
            }
            return text;
        }

        /// <summary>
        /// Not include {from} and {to}
        /// </summary>
        /// <param name="text"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static string GetFromTo(this string text, string from, string to)
        {
            if (text.IsNullOrEmptyOrWhiteSpace()) return string.Empty;
            if (text.Contains(from))
            {
                int _fromIndex = text.IndexOf(from);
                int _toIndex = text.IndexAll(to).Where(t => t > _fromIndex).FirstOrDefault();
                return text.Substring(_fromIndex + 1, _toIndex - 1);
            }
            return text;
        }
        

        //#endregion
        #region Commands
        public static string[] IntersectWords(this string text, string[] anothers)
        {
            string _tmpStr = text;
            List<string> _words = new List<string>();
            foreach (string _str in anothers)
            {
                _words.AddRange(_tmpStr.IntersectWords(_str).ToList());
            }
            return _words.Distinct().ToArray();
        }
        public static string[] UnionWords(this string text, string another)
        {
            string[] _words = text.ToWords().Select(t => t.ToStandard()).ToArray();
            string[] _words2 = another.ToWords().Select(t => t.ToStandard()).ToArray();
            return _words.Union(_words2).Distinct().ToArray();
        }

        public static string[] IntersectWords(this string text, string another)
        {
            if(!text.IsNullOrEmptyOrWhiteSpace())
            {
                string[] _words = text.ToHtmlWords().Select(t => t.ToStandard()).ToArray();
                string[] _words2 = another.ToHtmlWords().Select(t => t.ToStandard()).ToArray();
                return _words.Intersect(_words2).ToArray();
            }
            return null;
        }
        #endregion
        
        #region Basic
        
        public static string FirstWord(this string text)
        {
            string[] _words = text.ToWords();
            return _words[0];
        }

        public static void RemoveFirstWord(this string text)
        {
            text = text.Remove(0, text.FirstWord().Length);
        }

        public static string GetWord(this string text, int i)
        {
            if (text.IsNullOrEmptyOrWhiteSpace()) return null;
            else
            {
                string[] _words = text.ToWords();
                if (i <= _words.Count())
                {
                    return _words[i];
                }
                return string.Empty;
            }
        }

        
        public static string[] NextWords(this string text, string word)
        {
            
            int[] _index = text.IndexAll(word);
            List<string> _nextWords = new List<string>();
            foreach (int i in _index)
            {
                try
                {
                    _nextWords.Add(text.GetWord(i + 1));
                }
                catch
                {
                    _nextWords.Add(string.Empty);
                }
            }
            return _nextWords.ToArray();
        }

        public static string[] PrveWords(this string text, string word)
        {
            int[] _index = text.IndexAll(word);
            List<string> _nextWords = new List<string>();

            foreach (int i in _index)
            {
                try
                {
                    _nextWords.Add(text.GetWord(i- 1));
                }
                catch
                {
                    _nextWords.Add(string.Empty);
                }
            }
            return _nextWords.ToArray();
        }

        public static int[] IndexAll(this string text, string word)
        {
            if (!word.IsNullOrEmptyOrWhiteSpace())
            {
                List<int> indexes = new List<int>();
                for (int index = 0; ; index += word.Length)
                {
                    index = text.IndexOf(word, index);
                    if (index == -1)
                    {
                        return indexes.ToArray();
                    }
                    indexes.Add(index);
                }
            }
            else
            {
                return null;
            }
            
        }
        
        public static bool StartWithUpper(this string input)
        {
            return input.ToCharArray()[0].IsUpper();
        }

        public static bool DoesNotStartWith(this string input, string pattern)
        {
            return string.IsNullOrEmpty(pattern) ||
                   input.IsNullOrEmptyOrWhiteSpace() ||
                   !input.StartsWith(pattern, StringComparison.CurrentCulture);
        }

        public static bool DoesNotEndWith(this string input, string pattern)
        {
            return string.IsNullOrEmpty(pattern) ||
                     input.IsNullOrEmptyOrWhiteSpace() ||
                     !input.EndsWith(pattern, StringComparison.CurrentCulture);
        }
        
        public static string AddBefore(this string text, string before)
        {
            return before + text;
        }

        public static string AddAfter(this string text, string after)
        {
            return text + after;
        }

        public static string AddAfter(this string text, char c, string after)
        {
            if(text.Contains(c))
            {
                int firstIndex = text.IndexOf(c);
                text = text.Insert(firstIndex, after);
            }
            return text;
        }
        public static string AddBefore(this string text, char c, string after)
        {
            if (text.Contains(c))
            {
                int index = text.LastIndexOf(c);
                text = text.Insert(index-1, after);
            }
            return text;
        }
        public static string AddLineBefore(this string text, string before)
        {
            return before + "\n" + text;
        }
        public static string AddLineAfter(this string text, string after)
        {
            return text + "\n" + after;
        }

        public static string[] Select(this string[] texts, int min, int max)
        {
            return texts.Where(t => t.ToWords().Length <= max && t.ToWords().Length >= min).ToArray();
        }
        
        public static bool IsUpper(this string word)
        {
            bool result = word.Equals(word.ToUpper());
            return result;
        }

        public static bool IsLower(this string word)
        {
            bool result = word.Equals(word.ToLower());
            return result;
        }

        public static string[] ToHtmlWords(this string text)
        {
            string[] _strSplits = new string[] { " "};
            string[] _words =  text.ToStandard().Split(_strSplits, StringSplitOptions.None)
                .Where(t => !string.IsNullOrWhiteSpace(t)).ToArray();
            return _words;
        }

        public static string ToStandard(this string text)
        {
            if (text == null || string.IsNullOrWhiteSpace(text)) return string.Empty;
            text = text.Trim();
            while (text.Contains("  "))
            {
                text = text.Replace("  ", " ");
            }
            while (text.Contains("\n\n"))
            {
                text = text.Replace("\n\n", "\n");
            }
            while (text.Contains("\t\t"))
            {
                text = text.Replace("\t\t", "\t");
            }
            while (text.Contains("\r\n"))
            {
                text = text.Replace("\r\n", " ");
            }
            while (text.Contains("\r\r"))
            {
                text = text.Replace("\r\r", "\r");
            }
            while (text.Contains("\r\n\r\n"))
            {
                text = text.Replace("\r\n\r\n","\r\n");
            }
            return text.Trim();
        }

        #endregion

        #region Strip
        /// <summary>
        /// Strip unwanted characters and replace them with empty string
        /// </summary>
        /// <param name="data">the string to strip characters from.</param>
        /// <param name="textToStrip">Characters to strip. Can contain Regular expressions</param>
        /// <returns>The stripped text if there are matching string.</returns>
        /// <remarks>If error occurred, original text will be the output.</remarks>
        public static string Strip(this string data, string textToStrip)
        {
            var stripText = data;

            if (string.IsNullOrEmpty(data)) return stripText;

            try
            {
                stripText = Regex.Replace(data, textToStrip, string.Empty);
            }
            catch(Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
            return stripText;
        }

        /// <summary>
        /// Strips unwanted characters on the specified string
        /// </summary>
        /// <param name="data">the string to strip characters from.</param>
        /// <param name="textToStrip">Characters to strip. Can contain Regular expressions</param>
        /// <param name="textToReplace">the characters to replace the stripped text</param>
        /// <returns>The stripped text if there are matching string.</returns>
        /// <remarks>If error occurred, original text will be the output.</remarks>
        public static string Strip(this string data, string textToStrip, string textToReplace)
        {
            var stripText = data;

            if (string.IsNullOrEmpty(data)) return stripText;

            try
            {
                stripText = Regex.Replace(data, textToStrip, textToReplace);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
            return stripText;
        }
        public static string StripHtml(this string source)
        {
            try
            {
                string result = string.Empty;

                // Remove HTML Development formatting
                // Replace line breaks with space
                // because browsers inserts space
                result = source.Replace("\r", " ");
                // Replace line breaks with space
                // because browsers inserts space
                result = result.Replace("\n", " ");
                // Remove step-formatting
                result = result.Replace("\t", string.Empty);
                // Remove repeating spaces because browsers ignore them
                result = System.Text.RegularExpressions.Regex.Replace(result,
                                                                      @"( )+", " ");

                // Remove the header (prepare first by clearing attributes)
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*head([^>])*>", "<head>",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"(<( )*(/)( )*head( )*>)", "</head>",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         "(<head>).*(</head>)", string.Empty,
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // remove all scripts (prepare first by clearing attributes)
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*script([^>])*>", "<script>",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"(<( )*(/)( )*script( )*>)", "</script>",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                //result = System.Text.RegularExpressions.Regex.Replace(result,
                //         @"(<script>)([^(<script>\.</script>)])*(</script>)",
                //         string.Empty,
                //         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"(<script>).*(</script>)", string.Empty,
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // remove all styles (prepare first by clearing attributes)
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*style([^>])*>", "<style>",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"(<( )*(/)( )*style( )*>)", "</style>",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         "(<style>).*(</style>)", string.Empty,
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // insert tabs in spaces of <td> tags
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*td([^>])*>", "\t",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // insert line breaks in places of <BR> and <LI> tags
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*br( )*>", "\r",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*li( )*>", "\r",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // insert line paragraphs (double line breaks) in place
                // if <P>, <DIV> and <TR> tags
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*div([^>])*>", "\r\r",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*tr([^>])*>", "\r\r",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*p([^>])*>", "\r\r",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // Remove remaining tags like <a>, links, images,
                // comments etc - anything that's enclosed inside < >
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<[^>]*>", string.Empty,
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // replace special characters:
                //result = System.Text.RegularExpressions.Regex.Replace(result,
                //         @" ", " ",
                //         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                //result = System.Text.RegularExpressions.Regex.Replace(result,
                //         @"&bull;", " * ",
                //         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                //result = System.Text.RegularExpressions.Regex.Replace(result,
                //         @"&lsaquo;", "<",
                //         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                //result = System.Text.RegularExpressions.Regex.Replace(result,
                //         @"&rsaquo;", ">",
                //         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                //result = System.Text.RegularExpressions.Regex.Replace(result,
                //         @"&trade;", "(tm)",
                //         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                //result = System.Text.RegularExpressions.Regex.Replace(result,
                //         @"&frasl;", "/",
                //         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                //result = System.Text.RegularExpressions.Regex.Replace(result,
                //         @"&lt;", "<",
                //         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                //result = System.Text.RegularExpressions.Regex.Replace(result,
                //         @"&gt;", ">",
                //         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                //result = System.Text.RegularExpressions.Regex.Replace(result,
                //         @"&copy;", "(c)",
                //         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                //result = System.Text.RegularExpressions.Regex.Replace(result,
                //         @"&reg;", "(r)",
                //         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                //// Remove all others. More can be added, see
                //// http://hotwired.lycos.com/webmonkey/reference/special_characters/
                //result = System.Text.RegularExpressions.Regex.Replace(result,
                //         @"&(.{2,6});", string.Empty,
                //         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // for testing
                //System.Text.RegularExpressions.Regex.Replace(result,
                //       this.txtRegex.Text,string.Empty,
                //       System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // make line breaking consistent
                result = result.Replace("\n", "\r");

                // Remove extra line breaks and tabs:
                // replace over 2 breaks with 2 and over 4 tabs with 4.
                // Prepare first to remove any whitespaces in between
                // the escaped characters and remove redundant tabs in between line breaks
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         "(\r)( )+(\r)", "\r\r",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         "(\t)( )+(\t)", "\t\t",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         "(\t)( )+(\r)", "\t\r",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         "(\r)( )+(\t)", "\r\t",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                // Remove redundant tabs
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         "(\r)(\t)+(\r)", "\r\r",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                // Remove multiple tabs following a line break with just one tab
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         "(\r)(\t)+", "\r\t",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                // Initial replacement target string for line breaks
                string breaks = "\r\r\r";
                // Initial replacement target string for tabs
                string tabs = "\t\t\t\t\t";
                for (int index = 0; index < result.Length; index++)
                {
                    result = result.Replace(breaks, "\r\r");
                    result = result.Replace(tabs, "\t\t\t\t");
                    breaks = breaks + "\r";
                    tabs = tabs + "\t";
                }

                // That's it.
                return result;
            }
            catch
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// Truncates the string to a specified length and replace the truncated to a ...
        /// </summary>
        /// <param name="text">string that will be truncated</param>
        /// <param name="maxLength">total length of characters to maintain before the truncate happens</param>
        /// <returns>truncated string</returns>
        public static string Truncate(this string text, int maxLength)
        {
            // replaces the truncated string to a ...
            const string suffix = "...";
            string truncatedString = text;

            if (maxLength <= 0) return truncatedString;
            int strLength = maxLength - suffix.Length;

            if (strLength <= 0) return truncatedString;

            if (text == null || text.Length <= maxLength) return truncatedString;

            truncatedString = text.Substring(0, strLength);
            truncatedString = truncatedString.TrimEnd();
            truncatedString += suffix;
            return truncatedString;
        }

        #endregion
        
        #region ToHtml
        //public static string ToImageHtml(this string text)
        //{
        //    if(text.IsFullFilePath())
        //    {
        //        return string.Empty;
        //    }
        //    return string.Empty;
        //}
        #endregion

        //http://www.code-kings.com/2012/08/reverse-words-in-given-string.html
        public static string ReverseWords(this string text)
        {
            string[] arySource = text.Split(new char[] { ' ' });
            string strReverse = string.Empty;

            for (int i = arySource.Length - 1; i >= 0; i--)
            {
                strReverse = strReverse + " " + arySource[i];
            }

            Console.WriteLine("Original String: \n\n" + text);

            Console.WriteLine("\n\nReversed String: \n\n" + strReverse);

            return strReverse;
        }

        public static bool HasPair(this string text, string first, string last)
        {
            if (text.GetAfter(first).Contains(last)) return true;
            return false;
        }

        public static bool ContainsAny(this string theString, char[] characters)
        {
            foreach (char character in characters)
            {
                if (theString.Contains(character.ToString()))
                {
                    return true;
                }
            }
            return false;
        }

        private static readonly HashSet<char> DefaultNonWordCharacters
          = new HashSet<char> { ',', '.', ':', ';' };

        /// <summary>
        /// Returns a substring from the start of <paramref name="value"/> no 
        /// longer than <paramref name="length"/>.
        /// Returning only whole words is favored over returning a string that 
        /// is exactly <paramref name="length"/> long. 
        /// </summary>
        /// <param name="value">The original string from which the substring 
        /// will be returned.</param>
        /// <param name="length">The maximum length of the substring.</param>
        /// <param name="nonWordCharacters">Characters that, while not whitespace, 
        /// are not considered part of words and therefor can be removed from a 
        /// word in the end of the returned value. 
        /// Defaults to ",", ".", ":" and ";" if null.</param>
        /// <exception cref="System.ArgumentException">
        /// Thrown when <paramref name="length"/> is negative
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown when <paramref name="value"/> is null
        /// </exception>
        public static string CropWholeWords(this string value, int length,
          HashSet<char> nonWordCharacters = null)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            if (length < 0)
            {
                throw new ArgumentException("Negative values not allowed.", "length");
            }

            if (nonWordCharacters == null)
            {
                nonWordCharacters = DefaultNonWordCharacters;
            }

            if (length >= value.Length)
            {
                return value;
            }
            int end = length;

            for (int i = end; i > 0; i--)
            {
                if (value[i].IsWhitespace())
                {
                    break;
                }

                if (nonWordCharacters.Contains(value[i])
                    && (value.Length == i + 1 || value[i + 1] == ' '))
                {
                    //Removing a character that isn't whitespace but not part 
                    //of the word either (ie ".") given that the character is 
                    //followed by whitespace or the end of the string makes it
                    //possible to include the word, so we do that.
                    break;
                }
                end--;
            }

            if (end == 0)
            {
                //If the first word is longer than the length we favor 
                //returning it as cropped over returning nothing at all.
                end = length;
            }

            return value.Substring(0, end);
        }

        /// <summary>
        /// Get string value after [first] a.
        /// </summary>
        public static string GetBefore(this string value, string a)
        {
            if (value.IsNullOrEmptyOrWhiteSpace()) return string.Empty;
            else
            {
                int posA = value.IndexOf(a);
                if (posA == -1)
                {
                    return string.Empty;
                }
                return value.Substring(0, posA);
            }

        }
       
        /// <summary>
        /// Get string value after [last] a.
        /// </summary>
        public static string GetAfter(this string value, string a)
        {
            if (value.IsNullOrEmptyOrWhiteSpace()) return string.Empty;
            else
            {
                int posA = value.IndexOf(a);
                if (posA == -1)
                {
                    return string.Empty;
                }
                int adjustedPosA = posA + a.Length;
                if (adjustedPosA >= value.Length)
                {
                    return string.Empty;
                }
                return value.Substring(adjustedPosA);
            }
        }

        /// <summary>
        /// Checks string object's value to array of string values
        /// </summary>        
        /// <param name="stringValues">Array of string values to compare</param>
        /// <returns>Return true if any string value matches</returns>
        public static bool In(this string value, params string[] stringValues)
        {
            if (value.IsNullOrEmptyOrWhiteSpace()) return false;
            else
            {
                foreach (string otherValue in stringValues)
                    if (string.Compare(value, otherValue) == 0)
                        return true;
                return false;
            }
        }

        /// <summary>
        /// Converts string to enum object
        /// </summary>
        /// <typeparam name="T">Type of enum</typeparam>
        /// <param name="value">String value to convert</param>
        /// <returns>Returns enum object</returns>
        public static T ToEnum<T>(this string value)
            where T : struct
        {
            return (T)System.Enum.Parse(typeof(T), value, true);
        }

        /// <summary>
        /// Returns characters from right of specified length
        /// </summary>
        /// <param name="value">String value</param>
        /// <param name="length">Max number of charaters to return</param>
        /// <returns>Returns string from right</returns>
        public static string Right(this string value, int length)
        {
            return value != null && value.Length > length ? value.Substring(value.Length - length) : value;
        }

        /// <summary>
        /// Returns characters from left of specified length
        /// </summary>
        /// <param name="value">String value</param>
        /// <param name="length">Max number of charaters to return</param>
        /// <returns>Returns string from left</returns>
        public static string Left(this string value, int length)
        {
            return value != null && value.Length > length ? value.Substring(0, length) : value;
        }

        /// <summary>
        ///  Replaces the format item in a specified System.String with the text equivalent
        ///  of the value of a specified System.Object instance.
        /// </summary>
        /// <param name="value">A composite format string</param>
        /// <param name="arg0">An System.Object to format</param>
        /// <returns>A copy of format in which the first format item has been replaced by the
        /// System.String equivalent of arg0</returns>
        public static string Format(this string value, object arg0)
        {
            return string.Format(value, arg0);
        }

        /// <summary>
        ///  Replaces the format item in a specified System.String with the text equivalent
        ///  of the value of a specified System.Object instance.
        /// </summary>
        /// <param name="value">A composite format string</param>
        /// <param name="args">An System.Object array containing zero or more objects to format.</param>
        /// <returns>A copy of format in which the format items have been replaced by the System.String
        /// equivalent of the corresponding instances of System.Object in args.</returns>
        public static string Format(this string value, params object[] args)
        {
            return string.Format(value, args);
        }

        public static string Inject(this string source, IFormatProvider formatProvider, params object[] args)
        {
            var objectWrappers = new object[args.Length];
            for (var i = 0; i < args.Length; i++)
            {
                objectWrappers[i] = new ObjectWrapper(args[i]);
            }

            return string.Format(formatProvider, source, objectWrappers);
        }

        public static string Inject(this string source, params object[] args)
        {
            return Inject(source, CultureInfo.CurrentUICulture, args);
        }
    }
}
