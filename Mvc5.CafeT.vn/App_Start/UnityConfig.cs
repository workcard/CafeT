using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Mvc5.CafeT.vn.Controllers;
using Mvc5.CafeT.vn.Models;
using Repository.Pattern.DataContext;
using Repository.Pattern.Ef6;
using Repository.Pattern.Repositories;
using Repository.Pattern.UnitOfWork;
using System;
using Unity;
using Unity.AspNet.Mvc;
using Unity.Injection;
using Unity.Lifetime;

namespace Mvc5.CafeT.vn
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
            // NOTE: To load from web.config uncomment the line below.
            // Make sure to add a Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your type's mappings here.
            // container.RegisterType<IProductRepository, ProductRepository>();
            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your types here
            // container.RegisterType<IProductRepository, ProductRepository>();

            container
               .RegisterType<IDataContext, DataContext>(new PerRequestLifetimeManager())
               .RegisterType<IDataContextAsync, DataContext>(new PerRequestLifetimeManager())
               .RegisterType<IUnitOfWork, UnitOfWork>(new PerRequestLifetimeManager())
               .RegisterType<IUnitOfWorkAsync, UnitOfWork>(new PerRequestLifetimeManager())
               .RegisterType<IDataContextAsync, ApplicationDbContext>(new PerRequestLifetimeManager());

            //.RegisterType<IRepositoryAsync<ApplicationSetting>, Repository<ApplicationSetting>>()

            container
               //.RegisterType<IArticleService, ArticleService>()
               .RegisterType<IRepositoryAsync<ArticleModel>, Repository<ArticleModel>>()
               .RegisterType<IRepositoryAsync<ArticleCategory>, Repository<ArticleCategory>>()
               .RegisterType<IRepositoryAsync<FileModel>, Repository<FileModel>>()
               //.RegisterType<IRepositoryAsync<CourseModel>, Repository<CourseModel>>()
               //.RegisterType<IRepositoryAsync<JobModel>, Repository<JobModel>>()
               .RegisterType<IRepositoryAsync<ProjectModel>, Repository<ProjectModel>>()
               .RegisterType<IRepositoryAsync<QuestionModel>, Repository<QuestionModel>>()
               .RegisterType<IRepositoryAsync<WordModel>, Repository<WordModel>>()
               .RegisterType<IRepositoryAsync<UrlModel>, Repository<UrlModel>>()
               .RegisterType<IRepositoryAsync<CommentModel>, Repository<CommentModel>>()

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