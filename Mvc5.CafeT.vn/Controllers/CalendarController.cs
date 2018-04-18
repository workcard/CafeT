/*
Copyright 2015 Google Inc

Licensed under the Apache License, Version 2.0(the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/


using CafeT.GoogleManager;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Mvc;
using Google.Apis.Drive.v2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Mvc5.CafeT.vn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Mvc5.CafeT.vn.Controllers
{

    public class AppFlowMetadata : FlowMetadata
    {
        private static string folder = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Uploads/");
        private static readonly IAuthorizationCodeFlow flow =
            new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets
                {
                    ClientId = MyClientSecrets.ClientId,
                    ClientSecret = MyClientSecrets.ClientSecret,
                },
                Scopes = MyRequestedScopes.Scopes,
                DataStore = new FileDataStore(folder, false)
            });

        public override string GetUserId(Controller controller)
        {
            // In this sample we use the session to store the user identifiers.
            // That's not the best practice, because you should have a logic to identify
            // a user. You might want to use "OpenID Connect".
            // You can read more about the protocol in the following link:
            // https://developers.google.com/accounts/docs/OAuth2Login.
            var user = controller.Session["user"];
            if (user == null)
            {
                user = Guid.NewGuid();
                controller.Session["user"] = user;
            }
            return user.ToString();

        }

        public override IAuthorizationCodeFlow Flow
        {
            get { return flow; }
        }
    }

    public class AuthCallbackController : Google.Apis.Auth.OAuth2.Mvc.Controllers.AuthCallbackController
    {
        protected override Google.Apis.Auth.OAuth2.Mvc.FlowMetadata FlowData
        {
            get { return new AppFlowMetadata(); }
        }
    }


    //[Authorize]
    public class CalendarController : Controller
    {
        public async Task<ActionResult> GetNewFilesAsync(int? n, CancellationToken cancellationToken)
        {
            var result = await new AuthorizationCodeMvcApp(this, new AppFlowMetadata()).
                AuthorizeAsync(cancellationToken);

            if (result.Credential != null)
            {
                var service = new DriveService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = result.Credential,
                    ApplicationName = "ASP.NET MVC Sample"
                });

                // YOUR CODE SHOULD BE HERE..
                // SAMPLE CODE:
                var list = await service.Files.List().ExecuteAsync();
                List<FileModel> files = new List<FileModel>();
                if (list.Items.Count > 0)
                {
                    foreach (var file in list.Items)
                    {
                        FileModel fileModel = Mappers.Mappers.GoogleFileToModel(file);
                        files.Add(fileModel);
                    }
                }

                ViewBag.Message = "FILE COUNT IS: " + list.Items.Count();
                if (Request.IsAjaxRequest())
                {
                    return (PartialView("Files/_Files", list));
                }
                return View("Files/_Files", list);
            }
            else
            {
                return new RedirectResult(result.RedirectUri);
            }
            //Uploader service = new Uploader(UserName);
            //service.ClientSecrectFile = Server.MapPath("~/App_Code/client_secrets.json");
            //var files = service.GetFilesAsync(n).Result.TakeMax(n);
            //List<FileModel> list = new List<FileModel>();
            //if (files != null && files.Count() > 0)
            //{
            //    foreach (var file in files)
            //    {
            //        FileModel fileModel = Mappers.Mappers.GoogleFileToModel(file);
            //        list.Add(fileModel);
            //    }
            //}
            //if (Request.IsAjaxRequest())
            //{
            //    return (PartialView("Files/_Files", list));
            //}
            //return View("Files/_Files", list);
        }
        public async Task<ActionResult> IndexAsync(CancellationToken cancellationToken)
        {
            var result = await new AuthorizationCodeMvcApp(this, new AppFlowMetadata()).
                AuthorizeAsync(cancellationToken);

            if (result.Credential != null)
            {
                var service = new DriveService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = result.Credential,
                    ApplicationName = "ASP.NET MVC Sample"
                });

                // YOUR CODE SHOULD BE HERE..
                // SAMPLE CODE:
                var list = await service.Files.List().ExecuteAsync();
                List<FileModel> projects = new List<FileModel>();
                if (list.Items.Count > 0)
                {
                    foreach(var item in list.Items)
                    {
                        projects.Add(new FileModel() { Id = Guid.NewGuid(), Title = item.Title });
                    }
                }
                
                ViewBag.Message = "FILE COUNT IS: " + list.Items.Count();
                return View(projects);
            }
            else
            {
                return new RedirectResult(result.RedirectUri);
            }
        }


        //private readonly IDataStore dataStore = new FileDataStore(GoogleWebAuthorizationBroker.Folder);

        //private async Task<UserCredential> GetCredentialForApiAsync()
        //{
        //    var initializer = new GoogleAuthorizationCodeFlow.Initializer
        //    {
        //        ClientSecrets = new ClientSecrets
        //        {
        //            ClientId = MyClientSecrets.ClientId,
        //            ClientSecret = MyClientSecrets.ClientSecret,
        //        },
        //        Scopes = MyRequestedScopes.Scopes,
        //    };
        //    var flow = new GoogleAuthorizationCodeFlow(initializer);

        //    var identity = await HttpContext.GetOwinContext().Authentication.GetExternalIdentityAsync(
        //        DefaultAuthenticationTypes.ApplicationCookie);
        //    var userId = identity.FindFirstValue(MyClaimTypes.GoogleUserId);

        //    var token = await dataStore.GetAsync<TokenResponse>(userId);
        //    return new UserCredential(flow, userId, token);
        //}

        //// GET: /Calendar/UpcomingEvents
        //public async Task<ActionResult> UpcomingEvents()
        //{
        //    const int MaxEventsPerCalendar = 20;
        //    const int MaxEventsOverall = 50;

        //    var model = new UpcomingEventsViewModel();

        //    var credential = await GetCredentialForApiAsync();

        //    var initializer = new BaseClientService.Initializer()
        //    {
        //        HttpClientInitializer = credential,
        //        ApplicationName = "WorkCard.vn",
        //    };
        //    var service = new CalendarService(initializer);

        //    // Fetch the list of calendars.
        //    var calendars = await service.CalendarList.List().ExecuteAsync();

        //    // Fetch some events from each calendar.
        //    var fetchTasks = new List<Task<Google.Apis.Calendar.v3.Data.Events>>(calendars.Items.Count);
        //    foreach (var calendar in calendars.Items)
        //    {
        //        var request = service.Events.List(calendar.Id);
        //        request.MaxResults = MaxEventsPerCalendar;
        //        request.SingleEvents = true;
        //        request.TimeMin = DateTime.Now;
        //        fetchTasks.Add(request.ExecuteAsync());
        //    }
        //    var fetchResults = await Task.WhenAll(fetchTasks);

        //    // Sort the events and put them in the model.
        //    var upcomingEvents = from result in fetchResults
        //                         from evt in result.Items
        //                         where evt.Start != null
        //                         let date = evt.Start.DateTime.HasValue ?
        //                             evt.Start.DateTime.Value.Date :
        //                             DateTime.ParseExact(evt.Start.Date, "yyyy-MM-dd", null)
        //                         let sortKey = evt.Start.DateTimeRaw ?? evt.Start.Date
        //                         orderby sortKey
        //                         select new { evt, date };
        //    var eventsByDate = from result in upcomingEvents.Take(MaxEventsOverall)
        //                       group result.evt by result.date into g
        //                       orderby g.Key
        //                       select g;

        //    var eventGroups = new List<CalendarEventGroup>();
        //    foreach (var grouping in eventsByDate)
        //    {
        //        eventGroups.Add(new CalendarEventGroup
        //        {
        //            GroupTitle = grouping.Key.ToLongDateString(),
        //            Events = grouping,
        //        });
        //    }

        //    model.EventGroups = eventGroups;
        //    return View(model);
        //}
    }
}