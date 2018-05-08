using CafeT.Text;
using System;
using System.Collections.Generic;

namespace Web.Models
{
    public struct Address
    {
        string Country { set; get; }
        string City { set; get; }
        string Town { set; get; }
        string Street { set; get; }
        string AddressNumber { set; get; }
    }

    public class Contact : BaseObject
    {
        public string FirstName { set; get; } = string.Empty;
        public string LastName { set; get; } = string.Empty;
        
        public string Email { set; get; } = string.Empty;
        public string MobilePhone { set; get; } = string.Empty;

        public string HomeAddress { set; get; } = string.Empty;
        public string HomePhone { set; get; } = string.Empty;

        public string OfficeName { set; get; }
        public string OfficePhone { set; get; }
        
        public string About { set; get; }
        public Address Address { set; get; }

        public virtual List<Project> Projects { set; get; } = new List<Project>();
        public virtual List<WorkIssue> Issues { set; get; } = new List<WorkIssue>();

        public string UserName { set; get; }
        public bool? IsRegistered { set; get; }

        public bool IsValid()
        {
            if (FirstName.IsNullOrEmptyOrWhiteSpace()) return false;
            if (LastName.IsNullOrEmptyOrWhiteSpace()) return false;
            return true;
        }
        public Contact() : base() { }

        public Contact(string email):base()
        {
            Email = email;
            UserName = email;
        }

        public Contact(string firstName, string lastName, string email) : base()
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }

        public bool Contains(string userName)
        {
            if (Email.IndexOf(userName, StringComparison.CurrentCultureIgnoreCase) >= 0) return true;
            return false;
        }
    }
}