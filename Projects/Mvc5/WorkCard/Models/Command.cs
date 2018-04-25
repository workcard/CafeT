using CafeT.Text;
using System;

namespace Web.Models
{
    public enum CommandType
    {
        None,
        IsQuestion,
        IsEmail,
        IsUrl,
        IsLatex
    }

    public class Command
    {
        public string Text { set; get; }
        public string Header { set; get; } = string.Empty;
        public string Body { set; get; } = string.Empty;
        public string Title { set; get; }
        public CommandType Type { set; get; } = CommandType.None;
        //public Command() { }
        public bool IsCommand() {
            if(Text.StartsWith("{") && Text.EndsWith("}"))
            {
                return true;
            }
            return false;
        }

        public Command(string command)
        {
            Text = command;
            if (IsCommand())
            {
                Text = command
                    .Replace("{",string.Empty)
                    .Replace("}",string.Empty);
                if(!Text.Contains(","))
                {
                    Text = Text.AddAfter(",");
                }
                if(Text.Contains(","))
                {
                    string[] elements = Text.Split(new string[] { "," }, StringSplitOptions.None);
                    Header = elements[0];
                    Body = elements[1];
                    Title = Body.GetFirstSentence();
                    Type = CommandType.None;
                    if (Header.Contains("?")) Type = CommandType.IsQuestion;
                    if (Header.Contains("$$")) Type = CommandType.IsLatex;
                    if (Header.IsEmail()) Type = CommandType.IsEmail;
                    if (Header.IsUrl()) Type = CommandType.IsUrl;
                }
                //else //If is one statement not include {,}
                //{
                //    Header = Text;
                //    Body = Text;
                //    Title = Body.GetFirstSentence();
                //    Type = CommandType.None;
                //    if (Header.Contains("?")) Type = CommandType.IsQuestion;
                //    if (Header.Contains("$$")) Type = CommandType.IsLatex;
                //    if (Header.IsEmail()) Type = CommandType.IsEmail;
                //    if (Header.IsUrl()) Type = CommandType.IsUrl;
                //}
            }
        }

        public string Excute()
        {
            return string.Empty;
        }
    }
}