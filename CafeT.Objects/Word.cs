using CafeT.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CafeT.Objects
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
            if (!value.IsNullOrEmpty())
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
            if(!Value.IsNullOrEmptyOrWhiteSpace())
            {
                var _chars = Value.ToCharArray();
                var _resutls = _chars.Where(t => t.IsOutOfWord());
                if (_resutls.IsNullTypeOrEmpty() || _resutls.Count() <= 0) return true;
            }
            return false;
        }

        public bool IsWord()
        {
            if (!Value.IsNullOrEmptyOrWhiteSpace())
            {
                var _chars = Value.ToCharArray();
                var _resutls = _chars.Where(t => t.IsOutOfWord());
                if (_resutls.IsNullTypeOrEmpty() || _resutls.Count() > 0) return false;
            }
            return true;
        }

        public void ToClean()
        {
            if(!Value.IsNullOrEmptyOrWhiteSpace())
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
            if (!IsWord()) return WordLang.Others;
            var _langCode = string.Empty;
            string[] _vnSignals = new string[]{
                "ă", "â", "á", "à", "ạ", "ả", "ã",
                "ầ","ẩ","ấ","ậ","ẩ","ẫ",
                "ắ","ẳ", "ẵ","ằ","ặ",
                "đ",
                "è", "é", "ẻ", "ẹ","ẽ",
                "ê","ể","ế","ệ","ề","ễ",
                "ỹ", "ỳ","ý","ỷ", "ỵ",
                "ị","ì","í","ỉ","ĩ",
                "ò","ỏ","ó","õ","ọ",
                "ụ", "ù", "ú","ủ","ũ",
                "ư", "ứ", "ừ", "ự","ữ","ử",
                "ơ", "ở", "ờ", "ớ", "ợ",
                "ô","ổ","ỗ","ồ","ố","ộ",
            }
            .Distinct().ToArray();

            string[] _vnWords = new string[]{
                "anh", "khi", "giao", "phi","heo","quan","gia","cha","ra","trong","cho","nam","mang","khai",
                "cao","tuy","thao","quen","theo","minh","trang","tham","quen",
                "chia","kho", "mua", "bao", "xin", "ma",
                "sao","sau","ta","dung","khi","con","vui","em",
                "tin", "nhanh", "chung", "qua", "hay", "truy", "hy", "nay", "nghe",
                "danh", "tra", "ngay", "chi"
            }
            .Distinct().ToArray();

            string[] _enSignals = new string[]{
                "f", "z", "br", "fr", "ir", "ar", "sr", "w"
            }
            .Distinct().ToArray();

            if (Value.ToLower().ContainsAny(_vnSignals)
                || _vnWords.Contains(Value.ToLower()))
            {
                _langCode = "vi";
            }
            else if(!Value.IsNumeric() && Value.Length > 1)
            {
                if(Value.ToCharArray().Where(t=>t.IsUpper()).Count() <= 1)
                    _langCode = "en"; //default if not Vietnamese then it's English
            }
            switch(_langCode)
            {
                case "vi":
                    return WordLang.Vietnamese;
                case "en":
                    return WordLang.English;
                default:
                    return WordLang.Others;
            }
        }

        //public override string ToString()
        //{
        //    switch (Type)
        //    {
        //        case WordType.Numberic:
        //            return "<ins>" + Value + "</ins>";
        //        case WordType.Email:
        //            return "<ins>" + Value + "</ins>";
        //        case WordType.Url:
        //            return "<ins>" + Value + "</ins>";
        //        default:
        //            return Value;
        //    }
        //}

        //public string ToStringWithIndex()
        //{
        //    return "$" + Value + "_{" + Index +  "}$";
        //}

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
