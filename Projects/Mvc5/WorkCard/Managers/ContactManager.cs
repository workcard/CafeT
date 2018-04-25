using CafeT.Text;
using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Managers
{
    public class ContactManager:BaseManager
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ContactManager(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {
        }

        public async Task<bool> AddContactsAsync(List<Contact> contacts)
        {
            bool result = true;
            foreach(var contact in contacts)
            {
                result =  result && await AddContactAsync(contact);
            }
            return result;
        }


        public async Task<bool> AddContactAsync(Contact contact)
        {
            var _myContacts = db.Contacts.Where(t => t.UserName == contact.UserName).Select(t => t.Email);
            if (!_myContacts.Contains(contact.Email))
            {
                db.Contacts.Add(contact);
                await db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public Contact GetById(Guid contactId)
        {
            return db.Contacts.FindAsync(contactId).Result;
        }

        public Contact GetByEmail(string email)
        {
            if (!email.IsEmail()) return null;
            return db.Contacts.Where(t=>t.Email == email).FirstOrDefault();
        }
        public IEnumerable<WorkIssue> GetIssuesOf(string email)
        {
            var issues = _unitOfWorkAsync.RepositoryAsync<WorkIssue>()
                .Query().Select().Where(t => t.GetEmails().Contains(email))
                .AsEnumerable();

            return issues;
        }

        public IEnumerable<Contact> GetContactsOfIssue(Guid issueId)
        {
            return db.Contacts.Where(t => t.IssueId == issueId);
        }

        public IEnumerable<Contact> GetContacts(string userName)
        {
            return db.Contacts.Where(t => t.CreatedBy == userName);
        }

        
        public IEnumerable<Contact> SearchByName(string name)
        {
            return _unitOfWorkAsync.RepositoryAsync<Contact>()
                .Query().Select().Where(t => t.IsValid() && t.Contains(name))
                .AsEnumerable();
        }
        public IEnumerable<WorkIssue> GetTodayIssues(string email)
        {
            return GetIssuesOf(email)
                .Where(t => t.IsToday())
                .AsEnumerable();
        }
    }
}