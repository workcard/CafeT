using CafeT.Mathematics;
using CafeT.Objects;
using CafeT.Text;
using CafeT.Translators;
using System.Collections.Generic;
using System.Linq;

namespace CafeT.SmartObjects
{
    public class TextCommand
    {
        public string Text { set; get; }
        private static string[] Tokens = new string[]{"[>","(",")",";"};
        public List<string> Parameters = new List<string>();
        public string Command { set; get; } = string.Empty;
        public string Result { set; get; } = string.Empty;

        public TextCommand(string textCommand)
        {
            Text = textCommand;
            if(IsCommand())
            {
                var elements = Text.Split(Tokens, System.StringSplitOptions.None).AsEnumerable();
                Command = elements.Where(t => !t.IsNullOrEmptyOrWhiteSpace() && !t.Contains("\""))
                    .ToArray()[0];
                Parameters.AddRange(elements.Where(t => !t.IsNullOrEmptyOrWhiteSpace() && t.Contains("\""))
                    .ToArray());
                Parameters = Parameters
                    .Where(t => t.Contains("\""))
                    .Select(t => t.Substring(1, t.Length - 2)).Distinct().ToList();
            }
        }

        public void Excute()
        {
            switch(Command)
            {
                case "Trans":
                    Translator translator = new Translator();
                    if(Parameters.Count > 0)
                        Result = translator.Trans(Parameters.ToArray()[0]);
                    break;
                case "Calc":
                    MathEngine mathEngine = new MathEngine();
                    if (Parameters.Count > 0)
                        Result = mathEngine.Calc(Parameters.ToArray()[0]);
                    break;
                default:
                    Result = null;
                    break;
            };
        }
        public bool IsCommand()
        {
            if (Text.StartsWith("[>") && Text.Contains(";")) return true;
            return false;
        }
        public override string ToString()
        {
            if(Parameters != null && Parameters.Count > 0)
            {
                return Parameters[0] + " : " + Result;
            }
            return base.ToString();
        }
    }
}
