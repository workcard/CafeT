using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MathBot.Models;

namespace MathBot.Managers
{
    public class UserManager
    {
        private MathBotDataContext db = new MathBotDataContext();

        public async System.Threading.Tasks.Task<bool> AddUser(UserProfile model)
        {
            var _emails = db.Users.Select(t => t.Email);
            if (!_emails.Contains(model.Email))
            {
                db.Users.Add(model);
                var _value = await db.SaveChangesAsync();
                if (_value >= 0)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        public IEnumerable<UserProfile> GetAll()
        {
            return db.Users.AsEnumerable();
        }

    }
}