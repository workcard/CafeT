using CafeT.Objects;
using CafeT.Text;
using System.Collections.Generic;
using System.Linq;

namespace CafeT.SmartObjects
{
    public class VnTextCrawler : ITextCrawler
    {
        public string Text { set; get; } = string.Empty;
        public TextProcessor Processor { set; get; }
        public List<Word> EnglishdWords { set; get; } = new List<Word>();
        public List<Word> VietnameseWords { set; get; } = new List<Word>();
        public List<Word> OtherTypeWords { set; get; } = new List<Word>();
        public List<Word> ErrorWords { set; get; } = new List<Word>();
        public string NewText { set; get; } = string.Empty;
        public VnTextCrawler() { }

        public void Run(string text)
        {
            Text = text;
            if (text.IsHtmlString())
            {
                Text = new HtmlToText().Convert(text);
            }
            else
            {
                Text = text;
            }

            Text = Text.ToStandard();
            Processor = new TextProcessor(Text);
            var _words = Processor.CleanWordObjects
                            .Select(t=>t.Value).ToList();
           
            if (_words != null && _words.Count > 0)
            {
                int _n = _words.Count;
                for (int i = 0; i < _n; i++)
                {
                    Word _model = new Word(_words[i]);
                    if(_model != null)
                    {
                        ProcessWord(_model);
                    }
                }
            }
        }

        public void ProcessWord(Word word)
        {
            if(word.Lang == WordLang.English)
            {
                EnglishdWords.Add(word);
            }
            else if (word.Lang == WordLang.Vietnamese)
            {
                VietnameseWords.Add(word);
            }
            else
            {
                OtherTypeWords.Add(word);
            }
        }
    }
}
