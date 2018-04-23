using CafeT.Text;
using System;
using System.Threading;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Web.Helpers;
using Web.Models;
using Web.ModelViews;
using Web.ScheduledTasks;

namespace Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            System.Web.Http.GlobalConfiguration.Configure(WebApiConfig.Register);

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());

            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            log4net.Config.XmlConfigurator.Configure();

            //Database.SetInitializer<ApplicationDbContext>(ApplicationDbContext);

            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<WorkIssue, IssueView>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                    //.ForMember(dest => dest.Keywords, opt => opt.MapFrom(src => src.Title.ToWords().Select(t=>t.IsVnKeyword()))
                    .ForMember(dest => dest.Emails, opt => opt.MapFrom(src => src.Title.GetEmails()))
                    ;
                    //.ForMember(dest => dest.RecipientId, opt => opt.MapFrom(src => src.Recipient.Id))
                    //.ForMember(dest => dest.FromName, opt => opt.MapFrom(src => src.From.Name))
                    //.ForMember(dest => dest.RecipientName, opt => opt.MapFrom(src => src.Recipient.Name));
            });

            JobScheduler.StartAsync().GetAwaiter().GetResult();
        }
        protected void Application_Error(Object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            if (ex is ThreadAbortException)
                return;
            WriteLog.LogError(ex);
        }
    }
}
