using CafeT.Text;
using StackExchange.Profiling;
using System;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
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

            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());

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
        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            if (FormsAuthentication.CookiesSupported == true)
            {
                if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
                {
                    try
                    {
                        //let us take out the username now                
                        string username = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
                        string roles = string.Empty;

                        using (ApplicationDbContext dbContext = new  ApplicationDbContext())
                        {
                            var user = dbContext.Users.Find(username);
                            roles = user.Roles.ToString();
                        }
                        //let us extract the roles from our own custom cookie
                        //Let us set the Pricipal with our user specific details
                        HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(
                          new System.Security.Principal.GenericIdentity(username, "Forms"), roles.Split(';'));
                    }
                    catch (Exception)
                    {
                        //somehting went wrong
                    }
                }
            }
        }
        protected void Application_BeginRequest()
        {
            if (Request.IsLocal)
            {
                MiniProfiler.Start();
            }
        }
        protected void Application_EndRequest()
        {
            MiniProfiler.Stop();
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
