using CafeT.BusinessObjects;
using CafeT.Mathematics;
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
}