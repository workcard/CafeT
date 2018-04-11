
using SmartTracking.Models.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartTracking.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //Email email = new Email();
            //email.To.Add("quy.hv@bidc.com.kh");
            //email.From = "tracking@bidc.com.kh";
            //email.Subject = "test email";
            //email.Body = "huynh van quy";
            //email.ServerName = "mail.bidc.com.kh";
            //email.Port = 25;
            //email.UserName = "tracking";
            //email.Password = "Abcd@1234";
            //email.EnableSsl = true;

            //EmailHelpers.Send(email);

            return View();
        }
    }
}