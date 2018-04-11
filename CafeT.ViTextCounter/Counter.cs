using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeT.ViTextCounter
{
    public interface ICounter
    {
        int CountOfWord(string word);
        int GetAllWord();
    }

    public class Counter:ICounter
    {
        public List<string> Texts { set; get; } = new List<string>();
        public Counter(string text)
        {
            Texts.Add(text);
        }
        public Counter(string[] texts)
        {
            if(texts.Length > 0)
            {
                foreach(string text in texts)
                {
                    Texts.Add(text);
                }
            }
        }
        public Counter(List<string> texts)
        {
            if(texts != null && texts.Count > 0)
            {
                Texts.AddRange(texts);
            }
        }

        public int CountOfWord(string word)
        {
            throw new NotImplementedException();
        }

        public int GetAllWord()
        {
            throw new NotImplementedException();
        }
    }
}
