using System.Linq;

namespace CafeT.Text
{
    public static class CharExtension
    {
        public static char[] EndOfWords = new char[] { ' ', '?', '!', ':', ';', '}', ']', ')', '>'};
        public static char[] delimiters_word = new char[] {
            '{', '}', '(', ')', '[', ']', '>', '<','-', '_', '=', '+',
            '|', '\\', ':', ';', ' ', ',', '.', '/', '?', '~', '!',
            '@', '#', '$', '%', '^', '&', '*',
            ' ', '\r', '\n', '\t',
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
            };
        public static bool IsUpper(this char c)
        {
            return char.IsUpper(c);
        }
        public static bool IsEndOfLine(this char c)
        {
            if ((c == '\n')||(c=='\t') || (c == '\r')) return true;
            return false;
        }
        public static bool IsWhitespace(this char c)
        {
            return char.IsWhiteSpace(c);
        }
        public static bool IsEndOfSentence(this char c)
        {
            if ((c == '?') || (c == '!')) return true;
            return false;
        }
        public static bool IsSpace(this char c)
        {
            if (c == ' ') return true;
            return false;
        }

        public static bool IsEndOfWord(this char c)
        {
            if (c.IsSpace()|| c.IsEndOfLine()||c.IsEndOfSentence() || c== ',' || c==':') return true;
            return false;
        }

        public static bool IsOutOfWord(this char c)
        {
            if (delimiters_word.Contains(c)) return true;
            return false;
        }

        public static bool IsDigit(this char c)
        {
            return char.IsDigit(c);
        }
    }
}
