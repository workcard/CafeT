using CafeT.Text;
using System;

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

        public Guid? ProjectId { set; get; }
        public Guid? IssueId { set; get; }

        public string UserName { set; get; }
        public bool? IsRegistered { set; get; }

        //public virtual string FullName { set; get; }
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
        }

        public Contact(string firstName, string lastName, string email) : base()
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }

        //public void Load()
        //{
            
        //}

        public bool Contains(string userName)
        {
            string FullName = FirstName + " " + LastName;
            FullName = FullName.RemoveUnicode();
            if (FullName.ToLower().Contains(userName.ToLower())) return true;
            return false;
        }
    }
}