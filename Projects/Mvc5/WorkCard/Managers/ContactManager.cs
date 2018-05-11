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
            var _myContacts = dbContext.Contacts
                .Where(t => t.UserName == contact.UserName).Select(t => t.Email);
            if (!_myContacts.Contains(contact.Email))
            {
                dbContext.Contacts.Add(contact);
                await dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public Contact GetById(Guid contactId)
        {
            return dbContext.Contacts.FindAsync(contactId).Result;
        }

        public Contact GetByEmail(string email)
        {
            if (!email.IsEmail()) return null;
            return dbContext.Contacts.Where(t=>t.Email == email).FirstOrDefault();
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
            var contacts = _unitOfWorkAsync.RepositoryAsync<Contact>().Query().Select()
                .ToList();
            List<Contact> results = new List<Contact>();
            if(contacts != null && contacts.Any())
            {
                foreach (var contact in contacts)
                {
                    if (contact.Issues.Any())
                    {
                        foreach (var issue in contact.Issues)
                        {
                            if (issue.Id == issueId) { results.Add(contact); }
                        }
                    }
                }
            }
            
            return results;
        }

        public List<Contact> GetContacts(string userName)
        {
            var contacts = _unitOfWorkAsync.RepositoryAsync<Contact>()
                .Query().Select().Where(t => t.CreatedBy == userName)
                .ToList();

            return contacts;
        }

        public IEnumerable<Contact> GetContacts()
        {
            return _unitOfWorkAsync.RepositoryAsync<Contact>().Query().Select()
                .AsEnumerable();
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