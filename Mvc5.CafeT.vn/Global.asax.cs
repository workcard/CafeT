using Mvc5.CafeT.vn.Models;
using Mvc5.CafeT.vn.ScheduledTasks;
using StackExchange.Profiling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace Mvc5.CafeT.vn
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());

            //Application["TotalOnlineUsers"] = 0;
            JobScheduler.Start();
        }
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //filters.Add(new MvcPerformanceAttribute());
        }
        //protected void Application_End()
        //{
        //    //InstanceNameRegistry.RemovePerformanceCounterInstances();
        //    //PerformanceMetricFactory.CleanupPerformanceMetrics();
        //}
        //void Session_Start(object sender, EventArgs e)
        //{
        //    Application.Lock();
        //    Application["TotalOnlineUsers"] = (int)Application["TotalOnlineUsers"] + 1;
        //    Application.UnLock();
        //}

        //void Session_End(object sender, EventArgs e)
        //{
        //    Application.Lock();
        //    Application["TotalOnlineUsers"] = (int)Application["TotalOnlineUsers"] - 1;
        //    Application.UnLock();
        //}

        //protected void Application_EndRequest()
        //{
        //    var loggedInUsers = (Dictionary<string, DateTime>)HttpRuntime.Cache["LoggedInUsers"];

        //    if (User != null && User.Identity.IsAuthenticated)
        //    {
        //        var userName = User.Identity.Name;
        //        if (loggedInUsers != null)
        //        {
        //            if (userName != null)
        //            {
        //                loggedInUsers[userName] = DateTime.Now;
        //                HttpRuntime.Cache["LoggedInUsers"] = loggedInUsers;
        //            }
        //            else
        //            {
        //                Console.WriteLine("userName is null");
        //            }
        //        }
        //    }

        //    if (loggedInUsers != null)
        //    {
        //        foreach (var item in loggedInUsers.ToList())
        //        {
        //            if (item.Value < DateTime.Now.AddMinutes(-10))
        //            {
        //                loggedInUsers.Remove(item.Key);
        //            }
        //        }
        //        HttpRuntime.Cache["LoggedInUsers"] = loggedInUsers;
        //    }

        //}
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

                        using (ApplicationDbContext dbContext = new ApplicationDbContext())
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
    }
}
