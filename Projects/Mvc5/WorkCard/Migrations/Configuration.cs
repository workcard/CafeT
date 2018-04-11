namespace Web.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Data.Entity.Migrations;
    using Web.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            
            InitializeIdentityForEF(context);
            base.Seed(context);

        }

        public static void InitializeIdentityForEF(ApplicationDbContext db)
        {
            //var userManager = HttpContext.Current
            //    .GetOwinContext().GetUserManager<ApplicationUserManager>();
            //var roleManager = HttpContext.Current
            //    .GetOwinContext().Get<ApplicationRoleManager>();

            var roleStore = new RoleStore<ApplicationRole, string, ApplicationUserRole>(db);
            var roleManager = new RoleManager<ApplicationRole, string>(roleStore);
            var userStore = new UserStore<ApplicationUser, ApplicationRole, string, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>(db);
            var userManager = new UserManager<ApplicationUser, string>(userStore);


            const string name = "admin@workcard.vn";
            const string email = "admin@workcard.vn";
            const string password = "P@$$w0rd";
            const string roleName = "Admin";

            //Create Role Admin if it does not exist
            var role = roleManager.FindByName(roleName);
            if (role == null)
            {
                role = new ApplicationRole(roleName);
                var roleresult = roleManager.Create(role);
            }

            var user = userManager.FindByName(name);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = name,
                    FirstName = "Admin",
                    LastName = "WorkCard.vn",
                    Email = email,
                    EmailConfirmed = true
                };
                var result = userManager.Create(user, password);
                result = userManager.SetLockoutEnabled(user.Id, false);
                userManager.AddToRole(user.Id, roleName);
            }

            var groupManager = new ApplicationGroupManager();
            var newGroup = new ApplicationGroup("SuperAdmins", "Full Access to All");

            groupManager.CreateGroup(newGroup);
            groupManager.SetUserGroups(user.Id, new string[] { newGroup.Id });
            groupManager.SetGroupRoles(newGroup.Id, new string[] { role.Name });
        }
    }
}
