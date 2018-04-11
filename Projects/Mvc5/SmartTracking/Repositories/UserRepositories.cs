using CafeT.Frameworks.Identity.Models;
using SmartTracking.Mappers;
using SmartTracking.Models;
using SmartTracking.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTracking.Repositories
{
    public class UserRepositories
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        public static List<ProfileUserViewModel> GetListUser()
        {
            List<ApplicationUser> users = db.Users.Where(m => m.BugNetUserId != null).ToList();
            List<ProfileUserViewModel> userProfiles = UserMappers.ProfileUserToViewModels(users);

            return userProfiles;
        }
    }
}
