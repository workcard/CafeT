using CafeT.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeT.Languages
{
    public class Language
    {
        public WordLang GetLanguage(string text)
        {
            if (!text.IsWord()) return WordLang.Others;
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

            if (text.ToLower().ContainsAny(_vnSignals)
                || _vnWords.Contains(text.ToLower()))
            {
                _langCode = "vi";
            }
            else if (!text.IsNumeric() && text.Length > 1)
            {
                if (text.ToCharArray().Where(t => t.IsUpper()).Count() <= 1)
                    _langCode = "en"; //default if not Vietnamese then it's English
            }
            switch (_langCode)
            {
                case "vi":
                    return WordLang.Vietnamese;
                case "en":
                    return WordLang.English;
                default:
                    return WordLang.Others;
            }
        }
    }
}
