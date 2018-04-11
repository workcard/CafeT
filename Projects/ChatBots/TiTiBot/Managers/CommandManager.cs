using MathBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MathBot.Managers
{
    
    public class CommandManager
    {
        private MathBotDataContext db = new MathBotDataContext();
        ContactManager _contactManager = new ContactManager();
        public string CommandText { set; get; }

        public static string AddContact = "#Add Contact";

        public void Excute()
        {
            if(CommandText.ToLower().StartsWith(AddContact.ToLower()))
            {
                string _tokens = string.Empty;
                //_tokens = CommandText.
                return;
            }
            if (CommandText.StartsWith("#View Contact".ToLower()))
            {
                return;
            }
            if (CommandText.ToLower().StartsWith("#Delete Contact".ToLower()))
            {
                return;
            }
        }

        //public bool IsBotCommand(string text)
        //{
        //    var _allBotCommands = db.BotCommands.AsEnumerable();
        //}
    }
}