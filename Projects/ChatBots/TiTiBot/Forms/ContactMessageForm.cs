using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TiTiBot.Forms
{
    public class ContactMessageForm
    {
    }

    [Serializable]
    public class ContactMessage
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public ContactMethod PreferredContactMethod { get; set; }
        public string Message { get; set; }

        public static IForm<ContactMessage> BuildForm()
        {
            return new FormBuilder<ContactMessage>()
                    .Message("I just need a few details to submit your message")
                    .Build();
        }
    }

    public enum ContactMethod
    {
        IGNORE,
        Telephone,
        SMS,
        Email
    }
}