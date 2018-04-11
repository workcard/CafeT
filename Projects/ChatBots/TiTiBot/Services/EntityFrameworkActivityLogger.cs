using AutoMapper;
using CafeT.Text;
using Microsoft.Bot.Builder.History;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TiTiBot.Models;

namespace TiTiBot
{
    public class EntityFrameworkActivityLogger : IActivityLogger
    {
        Task IActivityLogger.LogAsync(IActivity activity)
        {
            IMessageActivity msg = activity.AsMessageActivity();
            using (Models.TiTiBotDataContext dataContext = new Models.TiTiBotDataContext())
            {
                var newActivity = Mapper.Map<IMessageActivity, Models.ActivityBo>(msg);
                if (string.IsNullOrEmpty(newActivity.Id))
                    newActivity.Id = Guid.NewGuid().ToString();

               
                var _users = dataContext.Users.AsEnumerable();
                if (_users == null ||
                    !_users.Select(t => t.UserName)
                        .Contains(newActivity.FromName))
                {
                    User _user = new User();
                    _user.UserName = newActivity.FromName;
                    dataContext.Users.Add(_user);
                }
                if (msg.Text.IsUrl())
                {
                    dataContext.Activities.Add(newActivity);
                }

                try
                {
                    dataContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            
            
            return Task.FromResult(0);
        }
    }
}