using System.Collections.Generic;
using System.Data.Entity;

namespace MathBot.Models
{
    public class MathBotDataContext : DbContext
    {
        public MathBotDataContext()
            : base("DefaultConnection")
        {
        }
        public static MathBotDataContext Create()
        {
            return new MathBotDataContext();
        }
        public DbSet<Question> Questions { get; set; }
        public DbSet<ActivityModel> Activities { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<UrlModel> Urls { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<UserProfile> Users { get; set; }
        public DbSet<CodeFunction> CodeFunctions { get; set; }
        public DbSet<WordDictionary> Dictionaries { get; set; }

        public DbSet<BotCommand> BotCommands { get; set; }

        public List<T> Search<T>(string keyword)
        {

            return new List<T>();
        }
    }
}