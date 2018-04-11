using CafeT.Text;

namespace CafeT.Objects
{
    public class WordObject:Word
    {
        public int Index { set; get; } = 0;
        public int[] Indexs { set; get; }
        public WordObject():base(string.Empty)
        { }
        public WordObject(string value) : base(value)
        {
        }
        public WordObject(string value, int index):base(value)
        {
            Index = index;
        }
        public string ToLatexString()
        {
            return "$" + Value + "_{" + Index + "}$" + "[" + Indexs.Length + "]";
        }

        public bool IsStandard()
        {
            if (Value != null && !Value.Trim().IsNullOrEmptyOrWhiteSpace()) return true;
            return false;
        }

        public bool StartWith(string key)
        {
            return Value.StartsWith(key);
        }
        public bool Contains(string key)
        {
            return Value.Contains(key);
        }

        public bool IsClean()
        {
            return IsCleanWord();
        }
    }
}
