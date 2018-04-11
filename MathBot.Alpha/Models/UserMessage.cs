using System;
using CafeT.BusinessObjects;
using CafeT.Mathematics;
using CafeT.Text;
using CafeT.Languages;

namespace MathBot.Models
{
    public enum LanguageType
    {
        English,
        Vietnamese,
        Other
    }

    public class Message
    {
        public string Text { set; get; }
        public string message { set; get; } = string.Empty;
        public string token { set; get; } = string.Empty;
        public Message(string text)
        {
            Text = text;
        }
        public void Process()
        {
            //Process EndWith()
            if(message.EndsWith("?"))
            {
                token = "?";
                message = Text.GetFromBeginTo("?");
            }

            //Process StartWith()
            if(message.StartsWith("Search"))
            {
                token = "Search";
                message = Text.GetFromEndTo("Search");
            }
        }
    }

    public class UserMessage : BaseObject
    {
        public string Text { set; get; }
        public LanguageType LangType { set; get; }
        public UserMessageType Type { set; get; }

        public UserMessage(string text) : base()
        {
            Text = text;
            DetectLang();
            DetectType();
        }

        private void DetectLang()
        {
            var lang = new Language().GetLanguage(Text);
        }

        public bool IsMathExpression(string text)
        {
            MathEngine mathEngine = new MathEngine();
            return mathEngine.IsMathExpression(text);
        }
        public void DetectType()
        {
            string text = Text.ToLower().Trim();
            if (text.IsUrl())
            {
                Type = UserMessageType.Url;
                return;
            }
            else if (text.IsEmail())
            {
                Type = UserMessageType.Email;
                return;
            }
            else if (text.EndsWith("=?"))
            {
                Text = text.GetFromBeginTo("=");
                Type = UserMessageType.MathExpression;
                return;
            }
            else if (text.IsYouTubeUrl() || text.IsYouTubeWatchUrl())
            {
                Type = UserMessageType.YouTubeUrl;
                return;
            }
            else if (text.Contains("?"))
            {
                if (text.EndsWith("?"))
                {
                    Type = UserMessageType.Question;
                    return;
                }
                else
                {
                    string _endToken = text.GetFromEndTo("?");
                    if (_endToken.IsLangCode())
                    {
                        Type = UserMessageType.Translate;
                        return;
                    }
                    else if (_endToken.IsCurrencyCode())
                    {
                        Type = UserMessageType.ChangeCurrency;
                        return;
                    }
                }
            }
            else if (text.StartsWith("search"))
            {
                Type = UserMessageType.Search;
                return;
            }
            else if (text.StartsWith("#"))
            {
                Type = UserMessageType.DatabaseCommand;
                return;
            }
            else if (text.StartsWith("image"))
            {
                Type = UserMessageType.ImageSearch;
                return;
            }
            else if (text == "hi")
            {
                Type = UserMessageType.SayHello;
                return;
            }
            else if (text == "bye")
            {
                Type = UserMessageType.SayGoodbye;
                return;
            }
            else
            {
                Type = UserMessageType.Other;
                return;
            }
        }
    }

    public enum UserMessageType
    {
        SayHello,
        SayGoodbye,
        DatabaseCommand,
        Url,
        ImageUrl,
        YouTubeUrl,
        Image,
        PdfFile,
        DocFile,
        File,
        Email,
        MathExpression,
        Question,
        Translate,
        ChangeCurrency,
        Search,
        ImageSearch,
        Other
    }
}