//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace CafeT.GoogleServices
//{
//    class GoogleCalendar
//    {
//    }
//}

using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using SyncMyCal.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SyncMyCal.Calendars
{
    public class GoogleCalendar : ICalendar
    {
        CalendarService calendarConnection;
        CalendarId activeCalendar;
        public string ApplicationName { set; get; } = "WorkCard.vn";
        public UserCredential Credential { set; get; }
        public ClientSecrets ClientSecrets { set; get; }

        public GoogleCalendar()
        {
            ClientSecrets = new ClientSecrets();
            ConnectCalendar();
        }

        public string GetProviderName()
        {
            return "Google";
        }

        public bool ConnectCalendar()
        {
            //ClientSecrets secrets = new ClientSecrets
            //{
            //    ClientId = "723833707174-6ckfm32qv6vfdv66jtq0u5nsvdpq15gh.apps.googleusercontent.com",
            //    ClientSecret = "cmqb8LAJcvLNdvCy-sVToGwh"
            //};

            ClientSecrets.ClientId = "367301950518-821q4o7bgd9c6aai8gb41itj8ergfgih.apps.googleusercontent.com";
            ClientSecrets.ClientSecret = "mZNCYvAWm-Y9XF4E_Q86Xut7";
            try
            {
                    Credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    ClientSecrets,
                    new string[]
                    {
                        CalendarService.Scope.Calendar
                    },
                    "user",
                    CancellationToken.None)
                .Result;

                var initializer = new BaseClientService.Initializer();
                initializer.HttpClientInitializer = Credential;
                initializer.ApplicationName = ApplicationName;
                calendarConnection = new CalendarService(initializer);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

            return true;
        }

        public List<CalendarId> GetCalendars()
        {
            var availableCalendars = new List<CalendarId>();

            if (calendarConnection == null)
            {
                //Return empty list if no connection is active
                //TODO: oder besser ne Exception?
                return availableCalendars;
            }

            try
            {
                var calendars = calendarConnection.CalendarList.List().Execute().Items;

                foreach (CalendarListEntry entry in calendars)
                {
                    availableCalendars.Add(new CalendarId()
                    {
                        Provider = "google",
                        DsplayName = entry.Summary,
                        InternalId = entry.Id,
                        Description = entry.Description
                    });
                }
                return availableCalendars;
            }
            catch (Exception ex)
            {
                //TODO: Exceptions aufschlüsseln
                Console.WriteLine(ex.Message);
                return availableCalendars;
            }
        }

        public void SetActiveCalendar(CalendarId calendar)
        {
            this.activeCalendar = calendar;
        }

        public void AddEvent(Event item)
        {
            // . . . Authentication here

            // Create Calendar Service.
            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = Credential,
                ApplicationName = ApplicationName,
            });


            var list = service.CalendarList.List().Execute();
            var myCalendar = list.Items.SingleOrDefault(c => c.Summary == "WorkCard.vn");

            if (myCalendar != null)
            {
                var newEventRequest = service.Events.Insert(item, myCalendar.Id);
                var eventResult = newEventRequest.Execute();
            }
        }
    }
}
