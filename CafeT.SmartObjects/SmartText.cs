using CafeT.Objects;
using CafeT.Text;
using CafeT.Writers;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace CafeT.SmartObjects
{
    public class SmartText
    {
        public bool HasError { set; get; }
        public string Input { set; get; }
        public string Text { set; get; }
        public string OutputText { set; get; }

        public Dictionary<int, string> Lines { set; get; } = new Dictionary<int, string>();

        public Word FirstWord { set; get; }
        public Word LastWord { set; get; }
        public string[] KeyWords { set; get; }
        public string[] Emails { set; get; }
        public string[] Urls { set; get; }
        public string[] Latexs { set; get; }

        public string[] YouTubeUrls { set; get; }
        public string[] GoogleDriveUrls { set; get; }

        public string[] Images { set; get; }
        public string[] Numbers { set; get; }
        public string[] Sentences { set; get; }
        public string[] Paragraphs { set; get; }
        public char[] Separators { set; get; }
        public Dictionary<Word, int[]> Words { set; get; } = new Dictionary<Word,int[]>();
        public int CountOfWords { set; get; }

        public SmartText(string text)
        {
            Input = text;
            if(Input.IsHtmlString())
            {
                Text = new HtmlToText().Convert(Input);
            }
            else
            {
                Text = Input;
            }
            Text = HttpUtility.HtmlDecode(Text);
            Text = Text.StripHtml();
            Text = Text.ToStandard();
            if (!Text.IsNullOrEmptyOrWhiteSpace())
            {
                try
                {
                    GetWords();
                    CountOfWords = Words.Count;
                    GetEmails();
                    GetUrls();
                    GetLines();
                    KeyWords = GetVnKeywords();
                    Separators = Text.ExtractSeparators();
                    Sentences = Text.GetSentences();
                    Numbers = Text.GetNumbers();
                    YouTubeUrls = Text.GetYouTubeUrls();
                    GoogleDriveUrls = Text.GetGoogleDriveUrls();

                    //if (Words != null && Words.Count() > 0)
                    //{
                    //    FirstWord = Words[0];
                    //    LastWord = Words[Words.Count - 1];
                    //}
                }
                catch (Exception ex)
                {
                    HasError = true;
                    throw new Exception("Can't convert this text to SmartText" + ex.Message);
                }
            }
        }
        
        public void GetWords()
        {
            var _words = Text.ToWordsWithFreq()
                .Select(t=>t.Key).ToArray();

            int _n = _words.Length;
            for(int i = 0; i<_n; i++)
            {
                Word _word = new Word(_words[i], i);
                _word.Freq = Text.CountOf(_words[i]);
                int[] _indexs = Text.IndexAll(_words[i]);
                Words.Add(_word, _indexs);
            }
        }

        public void GetEmails()
        {
            Emails = Text.GetEmails();
        }
        public void GetUrls()
        {
            Urls = Text.GetUrls();
        }

        public bool IsValid()
        {
            if (Words == null || Words.Count() <= 0) return false;
            return true;
        }

        public void AddBefore(string text)
        {
            Text.AddBefore(text);
        }

        public void AddAfter(string text)
        {
            Text.AddAfter(text);
        }

        //public Word GetNext(Word word)
        //{
        //    if(word.IsIn(Text))
        //    {
        //        var _current = Words.IndexOf(word);
        //        if (_current < CountOfWords)
        //            return Words[_current + 1];
        //        else
        //            return null;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
        //public Word GetPrevious(Word word)
        //{
        //    if (word.IsIn(Text))
        //    {
        //        var _current = Words.IndexOf(word);
        //        if (_current > 0)
        //            return Words[_current - 1];
        //        else
        //            return null;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        public void GetLines()
        {
            using (StringReader reader = new StringReader(Text))
            {
                int lineNo = 0;
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Lines.Add(++lineNo, line);
                }
            }
        }
       
        public string[] GetSentences()
        {
            return Text.GetSentences(Text);
        }

        public string[] GetVnKeywords()
        {
            List<Word> _wordObjects = new List<Word>();
            string[] _words = Text.ToWords();
            foreach (string _word in _words)
            {
                _wordObjects.Add(new Word(_word, Array.IndexOf(_words,_word)));
            }
            var _keywords = _wordObjects
                .Where(t => t.IsVnKeyword())
                .Select(t=>t.Value)
                .Distinct()
                .ToArray();
            return _keywords;
        }

        public override string ToString()
        {
            return this.PrintAllProperties();
        }
    }
}
