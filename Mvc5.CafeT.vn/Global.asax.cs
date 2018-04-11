using Mvc5.CafeT.vn.Mappers;
using Mvc5.CafeT.vn.Models;
using Mvc5.CafeT.vn.ModelViews;
using Mvc5.CafeT.vn.ScheduledTasks;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

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

            //AutoMapper.Mapper.Initialize(cfg =>
            //{
            //    cfg.CreateMap<QuestionModel, QuestionView>()
            //        .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            //        .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
            //        //.ForMember(dest => dest.Keywords, opt => opt.MapFrom(src => src.Title.GetVnKeywords()))
            //        //.ForMember(dest => dest.Emails, opt => opt.MapFrom(src => src.Title.GetEmails()))
            //        ;
            //    //.ForMember(dest => dest.RecipientId, opt => opt.MapFrom(src => src.Recipient.Id))
            //    //.ForMember(dest => dest.FromName, opt => opt.MapFrom(src => src.From.Name))
            //    //.ForMember(dest => dest.RecipientName, opt => opt.MapFrom(src => src.Recipient.Name));
            //});
            //AutoMapperConfiguration.Configure();
            //Application["TotalOnlineUsers"] = 0;
            JobScheduler.Start();
        }
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //filters.Add(new MvcPerformanceAttribute());
        }
        protected void Application_End()
        {
            //InstanceNameRegistry.RemovePerformanceCounterInstances();
            //PerformanceMetricFactory.CleanupPerformanceMetrics();
        }
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
        //            if(userName != null)
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
    }
}
