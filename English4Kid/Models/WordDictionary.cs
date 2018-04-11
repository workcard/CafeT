using CafeT.BusinessObjects;

namespace MathBot.Models
{
    public class WordDictionary:BaseObject
    {
        public string English { set; get; } = string.Empty;
        public string Vietnameses { set; get; } = string.Empty;
        public bool IsRemembered { set; get; } = false;
        public string[] ViewBy { set; get; }

        public WordDictionary():base()
        {
        }
    }
}