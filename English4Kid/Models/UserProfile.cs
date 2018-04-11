using CafeT.BusinessObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MathBot.Models
{
    public class UserProfile : BaseObject
    {
        public string DisplayName { set; get; } = string.Empty;
        public string FirstName { set; get; } = string.Empty;
        public string LastName { set; get; } = string.Empty;

        [DataType(DataType.EmailAddress)]
        public string Email { set; get; } = string.Empty;
        [DataType(DataType.Password)]
        public string Password { set; get; } = string.Empty;

        public string MobilePhone { set; get; } = string.Empty;

        public string About { set; get; }

        public UserProfile() : base() { }

        public UserProfile(string email):base()
        {
            Email = email;
        }

        public UserProfile(string firstName, string lastName, string email) : base()
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }
    }
}