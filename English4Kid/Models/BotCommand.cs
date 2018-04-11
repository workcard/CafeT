using CafeT.BusinessObjects;
using CafeT.Text;
using System;
using System.ComponentModel.DataAnnotations;

namespace MathBot.Models
{
    public enum CommandType
    {
        Insert,
        Delete,
        Edit,
        Get,
        GetAll,
        Search
    }

    public class BotCommand:BaseObject
    {
        public string Name { set; get; } = string.Empty;
        public string Description { set; get; } = string.Empty;
        public string CsharpCode { set; get; } = string.Empty;
        public bool IsUnknow { set; get; } = false;
        public BotCommand():base()
        {
        }
    }

    public class BotMessage
    {
        [Key]
        public string Key { set; get; }
        public string Value { set; get; }
        public DateTime CreatedDate { set; get; }
        public string CreatedBy { set; get; }
        public BotMessage(){ }
    }

    public class UserMessage
    {
        [Key]
        public string Key { set; get; }
        public string Value { set; get; }
        public DateTime CreatedDate { set; get; }
        public string CreatedBy { set; get; }
        public UserMessage() { }
    }

    public class UserText:BaseObject
    {
        public string Text { set; get; }
        public UserTextType Type { set; get; }

        public UserText(string text) : base()
        {
            Text = text;
            DetectType();
        }

        public void DetectType()
        {
            string text = Text.ToLower();
            if (text.IsUrl())
            {
                Type = UserTextType.Url;
                return;
            }
            else if (text.IsEmail())
            {
                Type = UserTextType.Email;
                return;
            }
           
            else if (text.IsYouTubeUrl() || text.IsYouTubeWatchUrl())
            {
                Type = UserTextType.YouTubeUrl;
                return;
            }
            else if (text.Contains("?"))
            {
                if(text.EndsWith("?"))
                {
                    Type = UserTextType.Question;
                    return;
                }
                else
                {
                    string _endToken = text.GetFromEndTo("?");
                    if (_endToken.IsLangCode())
                    {
                        Type = UserTextType.Translate;
                        return;
                    }
                    else if (_endToken.IsCurrencyCode())
                    {
                        Type = UserTextType.ChangeCurrency;
                        return;
                    }
                }
            }
            else if(text.StartsWith("search"))
            {
                Type = UserTextType.Search;
                return;
            }
            else
            {
                Type = UserTextType.Other;
                return;
            }
        }
    }

    public enum UserTextType
    {
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
        Other
    }
}