using CafeT.Objects;
using CafeT.Text;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web;

namespace TiTiBot.Models
{
    public class TiTiBotDataContext : DbContext
    {
        public TiTiBotDataContext()
            : base("TiTiBotDataContextConnectionString")
        {
        }
        public static TiTiBotDataContext Create()
        {
            return new TiTiBotDataContext();
        }

        public DbSet<ActivityBo> Activities { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Url> Urls { get; set; }
    }

    
    public class User:BaseObject
    {
        public string UserName { set; get; }
        public string LastName { set; get; }
        public string MidleName { set; get; }
        public string FirstName { set; get; }
        public string Password { set; get; }
        public string Email { set; get; }
        public string PhoneNumber { set; get; }

        public User() : base() { }
        public User(string userName, string password):base(){ }
        public bool HasEmail()
        {
            if (Email.IsEmail()) return true;
            return false;
        }
        public bool HasPhoneNumber()
        {
            if (PhoneNumber.IsNumeric()) return true;
            return false;
        }
        public bool HasVerified()
        {
            if (!UserName.IsNullOrEmptyOrWhiteSpace() && !Password.IsNullOrEmptyOrWhiteSpace()) return true;
            return false;
        }
    }

    public class ActivityBo:BaseObject
    {
        public string FromId { get; set; }
        public string FromName { get; set; }
        public string RecipientId { get; set; }
        public string RecipientName { get; set; }
        public string TextFormat { get; set; }
        public string TopicName { get; set; }
        public bool HistoryDisclosed { get; set; }
        public string Local { get; set; }
        public string Text { get; set; }
        public string Summary { get; set; }
        public string ChannelId { get; set; }
        public string ServiceUrl { get; set; }
        public string ReplyToId { get; set; }
        public string Action { get; set; }
        public string Type { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string ConversationId { get; set; }

        public ActivityBo():base()
        {
        }
    }
}