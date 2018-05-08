using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Repository.Pattern.DataContext;
using Repository.Pattern.Ef6;
using Repository.Pattern.Repositories;
using Repository.Pattern.UnitOfWork;
using System;

using Unity;
using Unity.AspNet.Mvc;
using Unity.Injection;
using Unity.Lifetime;
using Web.Controllers;
using Web.Managers;
using Web.Models;

namespace Web
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;
        #endregion

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            container
               .RegisterType<IDataContext, DataContext>(new PerRequestLifetimeManager())
               .RegisterType<IDataContextAsync, DataContext>(new PerRequestLifetimeManager())
               .RegisterType<IUnitOfWork, UnitOfWork>(new PerRequestLifetimeManager())
               .RegisterType<IUnitOfWorkAsync, UnitOfWork>(new PerRequestLifetimeManager())
               .RegisterType<IDataContextAsync, ApplicationDbContext>(new PerRequestLifetimeManager());

            //.RegisterType<IRepositoryAsync<ApplicationSetting>, Repository<ApplicationSetting>>()

            container
               //.RegisterType<IIsssueService, IsssueService>()
               .RegisterType<BaseManager>()
               .RegisterType<IssuesManager>()
               .RegisterType<JobManager>()
               .RegisterType<QuestionManager>()
               .RegisterType<ContactManager>()
               ;

            container
               .RegisterType<IRepositoryAsync<Document>, Repository<Document>>()
               .RegisterType<IRepositoryAsync<WorkIssue>, Repository<WorkIssue>>()
               .RegisterType<IRepositoryAsync<Project>, Repository<Project>>()
               .RegisterType<IRepositoryAsync<Question>, Repository<Question>>()
               .RegisterType<IRepositoryAsync<Answer>, Repository<Answer>>()
               .RegisterType<IRepositoryAsync<Story>, Repository<Story>>()
               .RegisterType<IRepositoryAsync<Url>, Repository<Url>>()
               .RegisterType<IRepositoryAsync<Contact>, Repository<Contact>>()

                .RegisterType<IUserStore<IdentityUser>, UserStore<IdentityUser>>()
                .RegisterType<UserManager<ApplicationUser>>(new HierarchicalLifetimeManager())
                .RegisterType<RoleManager<ApplicationRole>>(new HierarchicalLifetimeManager())
                .RegisterType<ApplicationUserManager>()
                .RegisterType<ApplicationRoleManager>()
                .RegisterType<ApplicationRoleStore>()
                .RegisterType<GroupStoreBase>()
                .RegisterType<ApplicationGroupManager>();

            container.RegisterType<AccountController>(new InjectionConstructor());
            container.RegisterType<RolesAdminController>(new InjectionConstructor());
            container.RegisterType<UsersAdminController>(new InjectionConstructor());
        }
    }
}