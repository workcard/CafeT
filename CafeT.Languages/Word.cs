using CafeT.Objects;
using CafeT.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CafeT.Languages
{
    public enum WordLang
    {
        Vietnamese,
        English,
        Others
    }

    public enum WordType
    {
        Unknow,
        Knowned,
        Numberic,
        Url,
        Email,
        TextCommand,
        Correct, //Spelling
        Incorrect, //Spelling
    }

    public class Word
    {
        public string Value { set; get; }
        public int Length { get; }
        public char[] Chars { set; get; }
        public char FirstChar { set; get; }
        public char LastChar { set; get; }
        public WordType Type { get; }
        public WordLang Lang { get; }
        public Word(string value)
        {

            Value = value;
            Length = Value.Length;
            Type = IndentifyWord();
            Lang = DetectLang();
            Chars = Value.ToCharArray();
            if (!value.IsNullOrEmptyOrWhiteSpace())
            {
                FirstChar = Chars[0];
                LastChar = Chars[Length - 1];
            }
        }

        public bool CanRead()
        {
            if (!Value.IsNullOrEmptyOrWhiteSpace()) return true;
            return false;
        }
        public bool IsHtmlEncoded(string text)
        {
            return (HttpUtility.HtmlDecode(text) != text);
        }

        public bool IsCleanWord()
        {
            if (!Value.IsNullOrEmptyOrWhiteSpace())
            {
                var _chars = Value.ToCharArray();
                var _resutls = _chars.Where(t => t.IsOutOfWord());
                if (_resutls.IsNullTypeOrEmpty() || _resutls.Count() <= 0) return true;
            }
            return false;
        }

        

        public void ToClean()
        {
            if (!Value.IsNullOrEmptyOrWhiteSpace())
            {
                try
                {
                    Value = Value.ToStandard();
                    if (LastChar.IsOutOfWord())
                    {
                        Value = Value.Substring(0, Length - 1);
                    }
                }
                catch
                {
                    //Nothing to do
                }
            }
        }

        public WordType IndentifyWord()
        {
            if (Value.IsEmail()) return WordType.Email;
            if (Value.IsUrl()) return WordType.Url;
            if (Value.IsNumeric()) return WordType.Numberic;
            return WordType.Unknow;
        }

        public WordLang DetectLang()
        {
            var lang = new Language().GetLanguage(Value);
            return lang;
        }

        public bool IsNumber()
        {
            var _digits = Chars.Where(t => t.IsDigit());
            return _digits.Count() == Length ? true : false;
        }

        public bool IsIn(string text)
        {
            if (text.Contains(Value)) return true;
            return false;
        }

        public IEnumerable<string> GetSetences(string text)
        {
            if (text.Contains(Value))
            {
                var _object = text.GetSentences()
                    .Where(t => t.Contains(Value));
                return _object;
            }
            return null;
        }

        public bool IsVnKeyword()
        {
            if (Value.StartsWith("[")
                || Value.EndsWith("]")
                || Value.StartsWith("#")
                || (Type == WordType.Url)
                || (Value.ToCharArray().Count(t => t.IsUpper()) == 2)
                )
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
