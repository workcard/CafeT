using Calendar.ASP.NET.MVC5;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Mvc;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.Owin.Security.Google;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Web.Services
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

    public class GoogleService
    {
        //public async System.Threading.Tasks.Task<bool> IsAuthenticateAsync()
        //{
        //    var result = await new AuthorizationCodeMvcApp(this, new AppFlowMetadata()).
        //        AuthorizeAsync(cancellationToken);

        //    if (result.Credential != null)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
        
    }
    //public class AppFlowMetadata : FlowMetadata
    //{
    //    private static readonly IAuthorizationCodeFlow flow =
    //        new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
    //        {
    //            ClientSecrets = new ClientSecrets
    //            {
    //                ClientId = "367301950518-821q4o7bgd9c6aai8gb41itj8ergfgih.apps.googleusercontent.com",
    //                ClientSecret = "mZNCYvAWm-Y9XF4E_Q86Xut7"
    //            },
    //            Scopes = new[] { CalendarService.Scope.Calendar }
    //            //DataStore = new FileDataStore("Drive.Api.Auth.Store")
    //        });

    //    public override string GetUserId(Controller controller)
    //    {

    //        // In this sample we use the session to store the user identifiers.
    //        // That's not the best practice, because you should have a logic to identify
    //        // a user. You might want to use "OpenID Connect".
    //        // You can read more about the protocol in the following link:
    //        // https://developers.google.com/accounts/docs/OAuth2Login.
    //        var user = controller.Session["user"];
    //        if (user == null)
    //        {
    //            user = Guid.NewGuid();
    //            controller.Session["user"] = user;
    //        }
    //        return user.ToString();
    //    }

    //    public override IAuthorizationCodeFlow Flow
    //    {
    //        get { return flow; }
    //    }
    //}


    //public interface ICalendar
    //{
    //    /// <summary>
    //    /// Connects to the Calendar Provider. Needs to be called before any data can be retrieved
    //    /// </summary>
    //    /// <returns>True when the connection could be established successfuly</returns>
    //    bool ConnectCalendar();

    //    /// <summary>
    //    /// Returns the name of the Calendar provider
    //    /// </summary>
    //    /// <returns></returns>
    //    string GetProviderName();

    //    /// <summary>
    //    /// Returns a list of all calendars available for the user
    //    /// </summary>
    //    /// <returns></returns>
    //    List<CalendarId> GetCalendars();

    //    /// <summary>
    //    /// Sets the calendar this instance should use.
    //    /// (Sometimes a user can have several calendars linked to one account)
    //    /// </summary>
    //    /// <param name="calendar"></param>
    //    void SetActiveCalendar(CalendarId calendar);

    //    /// <summary>
    //    /// Returns all calendar events in a given timespan
    //    /// </summary>
    //    /// <param name="from">The DateTime of the earliest calendar event</param>
    //    /// <param name="to">The DateTime of the last calendar event</param>
    //    /// <returns>List of all matching CalendarEntries</returns>
    //    //List<CalendarEntry> GetCalendarEntriesInRange(DateTime from, DateTime to);

    //    /// <summary>
    //    /// Adds a new event to the calendar
    //    /// </summary>
    //    /// <param name="newEntry">Entry to e created</param>
    //    /// <returns>True when there were no errors while saving the event to the calendar</returns>
    //    //bool AddNewCalendarEntry(CalendarEntry newEntry);

    //    /// <summary>
    //    /// Removes a calendar event from the calendar
    //    /// </summary>
    //    /// <param name="entry">Entry to be deleted</param>
    //    /// <returns>True when there were no errors while deleting the entry</returns>
    //    //bool DeleteCalendarEntry(CalendarEntry entry);
    //}

    //public class CalendarId
    //{
    //    public string Provider { get; set; }
    //    public string DsplayName { get; set; }
    //    public string InternalId { get; set; }
    //    public string Description { get; set; }
    //}

    //public class GoogleCalendar //: ICalendar
    //{
    //    CalendarService calendarConnection;
    //    CalendarId activeCalendar;
    //    public string ApplicationName { set; get; } = "WorkCard.vn";
    //    public UserCredential Credential { set; get; }
    //    public ClientSecrets ClientSecrets { set; get; }

    //    public GoogleCalendar()
    //    {
    //        ClientSecrets = new ClientSecrets();
    //        ConnectCalendar();
    //    }

    //    public string GetProviderName()
    //    {
    //        return "Google";
    //    }

    //    //private IDataStore dataStore = new FileDataStore("~/App_Data");

    //    public bool ConnectCalendar()
    //    {
    //        ClientSecrets.ClientId = "367301950518-821q4o7bgd9c6aai8gb41itj8ergfgih.apps.googleusercontent.com";
    //        ClientSecrets.ClientSecret = "mZNCYvAWm-Y9XF4E_Q86Xut7";

    //        try
    //        {
    //            var google = new GoogleOAuth2AuthenticationOptions()
    //            {
    //                AccessType = "offline",     // Request a refresh token.
    //                ClientId = "367301950518-821q4o7bgd9c6aai8gb41itj8ergfgih.apps.googleusercontent.com",
    //                ClientSecret = "mZNCYvAWm-Y9XF4E_Q86Xut7",

    //                Provider = new GoogleOAuth2AuthenticationProvider()
    //                {
    //                    OnAuthenticated = async context =>
    //                    {
    //                        var userId = context.Id;
    //                        context.Identity.AddClaim(new Claim(MyClaimTypes.GoogleUserId, userId));

    //                        var tokenResponse = new TokenResponse()
    //                        {
    //                            AccessToken = context.AccessToken,
    //                            RefreshToken = context.RefreshToken,
    //                            ExpiresInSeconds = (long)context.ExpiresIn.Value.TotalSeconds,
    //                            Issued = DateTime.Now,
    //                        };
    //                        //await dataStore.StoreAsync(userId, tokenResponse);
    //                    }
    //                },
    //            };
    //            foreach (var scope in MyRequestedScopes.Scopes)
    //            {
    //                google.Scope.Add(scope);
    //            }

    //            Credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
    //            ClientSecrets,
    //            new string[]
    //            {
    //                    CalendarService.Scope.Calendar
    //            },
    //            "user",
    //            CancellationToken.None)
    //            .Result;

    //            var initializer = new BaseClientService.Initializer();
    //            initializer.HttpClientInitializer = Credential;
    //            initializer.ApplicationName = ApplicationName;
    //            calendarConnection = new CalendarService(initializer);
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine(ex.Message);
    //            return false;
    //        }
    //        //if (google != null)
    //        return true;
    //        //return false;
    //    }

    //    public List<CalendarId> GetCalendars()
    //    {
    //        var availableCalendars = new List<CalendarId>();

    //        if (calendarConnection == null)
    //        {
    //            //Return empty list if no connection is active
    //            //TODO: oder besser ne Exception?
    //            return availableCalendars;
    //        }

    //        try
    //        {
    //            var calendars = calendarConnection.CalendarList.List().Execute().Items;

    //            foreach (CalendarListEntry entry in calendars)
    //            {
    //                availableCalendars.Add(new CalendarId()
    //                {
    //                    Provider = "google",
    //                    DsplayName = entry.Summary,
    //                    InternalId = entry.Id,
    //                    Description = entry.Description
    //                });
    //            }
    //            return availableCalendars;
    //        }
    //        catch (Exception ex)
    //        {
    //            //TODO: Exceptions aufschlüsseln
    //            Console.WriteLine(ex.Message);
    //            return availableCalendars;
    //        }
    //    }

    //    public void SetActiveCalendar(CalendarId calendar)
    //    {
    //        this.activeCalendar = calendar;
    //    }

    //    public void AddEvent(Event item)
    //    {
    //        // . . . Authentication here

    //        // Create Calendar Service.
    //        //var service = new CalendarService(new BaseClientService.Initializer()
    //        //{
    //        //    HttpClientInitializer = Credential,
    //        //    ApplicationName = ApplicationName,
    //        //});

    //        var service = new CalendarService(new BaseClientService.Initializer()
    //        {
    //            HttpClientInitializer = Credential,
    //            ApplicationName = ApplicationName,
    //        });



    //        var list = service.CalendarList.List().Execute();
    //        var myCalendar = list.Items.SingleOrDefault(c => c.Summary == "WorkCard.vn");

    //        if (myCalendar != null)
    //        {
    //            var newEventRequest = service.Events.Insert(item, myCalendar.Id);
    //            var eventResult = newEventRequest.Execute();
    //        }
    //    }
    //}
}