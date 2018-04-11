using System.Collections.Generic;
using System.Linq;

namespace CafeT.Text
{
    public static class MathOnText
    {
        public static string[] ExtractAllMathText(this string text)
        {
            string[] _commands = text.ExtractAllBetween("$$", "$$");
            return _commands;
        }

        public static char[] ExtractOperators(string expression)
        {
            string operatorCharacters = "+-*/";
            List<char> operators = new List<char>();
            foreach (char c in expression)
            {
                if (operatorCharacters.Contains(c))
                {
                    operators.Add(c);
                }
            }
            return operators.ToArray();
        }
    }
}
