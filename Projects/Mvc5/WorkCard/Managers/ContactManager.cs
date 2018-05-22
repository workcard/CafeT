using CafeT.Text;
using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        //public async Task<bool> AddContactsAsync(List<Contact> contacts,string userName)
        //{
        //    bool result = true;
        //    foreach(var contact in contacts)
        //    {
        //        result =  result && await AddContactAsync(contact,userName);
        //    }
        //    return result;
        //}

        //public bool AddContact(Contact contact, Guid issueId)
        //{
        //    var issue = _unitOfWorkAsync.RepositoryAsync<WorkIssue>().FindAsync(issueId);
        //    if (issue == null)
        //        return false;

            
        //}
        public List<Contact> GetContactsOfIssue(Guid issueId)
        {
            var issue = _unitOfWorkAsync.RepositoryAsync<WorkIssue>().Find(issueId);
            if (issue == null)
                return null;
            //var contacts = _unitOfWorkAsync.RepositoryAsync<Contact>()
            //    .Query().Select()
            //    .Where(t=>t.)
            return issue.Contacts;
        }
        //public async Task<bool> AddContactAsync(Contact contact, string userName)
        //{
        //    var _myContacts = _unitOfWorkAsync.RepositoryAsync<Contact>()
        //        .Query().Select()
        //        .Where(t => t.CreatedBy == userName).ToList();


        //    var _myContactEmails = _myContacts.Select(t => t.Email).ToList();
        //    if (!_myContactEmails.Contains(contact.Email))
        //    {
        //        _unitOfWorkAsync.RepositoryAsync<Contact>().Insert(contact);
        //    }
        //    else
        //    {
        //        _unitOfWorkAsync.RepositoryAsync<Contact>().Update(contact);
        //    }
        //    try
        //    {
        //        await _unitOfWorkAsync.SaveChangesAsync();
        //        return true;
        //    }
        //    catch(Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        return false;
        //    }
        //}

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
            
            //var contact = _unitOfWorkAsync.RepositoryAsync<Contact>()
            //    .Query().Select()
            //    .Where(t => t.Email == email).FirstOrDefault();
            //var issues = _unitOfWorkAsync.RepositoryAsync<WorkIssue>()
            //    .Query().Select()
            //    .Where(t => t.Contacts != null && t.Contacts.Contains(contact))
            //    .AsEnumerable();

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var contact = db.Contacts.Where(t => t.Email == email).FirstOrDefault();
                var issues = db.Issues
                    .Where(t => (t.GetEmails().Contains(email))
                    ||(t.Contacts != null && t.Contacts.Count > 0 && t.Contacts.Contains(contact))
                    );
                return issues;
            }
        }
        public IEnumerable<WorkIssue> GetIssuesOf(Guid id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var contact = db.Contacts.Where(t => t.Id == id).FirstOrDefault();
                var issues = db.Issues
                    .Where(t => (t.GetEmails().Contains(contact.Email))
                    || (t.Contacts != null && t.Contacts.Count > 0 && t.Contacts.Contains(contact))
                    );
                return issues;
            }
        }

        internal void MappContactIssue(Contact contact, WorkIssue issue)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var _issue = context.Issues.Find(issue.Id);
                var _contact = context.Contacts.Find(contact.Id);

                _issue.Contacts.Add(_contact);
                _contact.Issues.Add(_issue);

                context.Entry<WorkIssue>(_issue).State = EntityState.Modified;
                context.Entry<Contact>(_contact).State = EntityState.Modified;
                try
                {
                    context.SaveChanges();
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                //_contact.Issues.Add(_issue);
            }
            //    contact.Issues.Add(issue);
            //issue.Contacts.Add(contact);
            //try
            //{
            //    //_unitOfWorkAsync.RepositoryAsync<Contact>().Update(contact);
            //    _unitOfWorkAsync.RepositoryAsync<WorkIssue>().InsertOrUpdateGraph(issue);
            //    _unitOfWorkAsync.SaveChangesAsync();
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
        }

        internal void AddContact(Contact contact)
        {
            var _myContacts = _unitOfWorkAsync.RepositoryAsync<Contact>()
                .Query().Select().Where(t => t.CreatedBy == contact.CreatedBy)
                .ToList();

            var _contactEmails = _myContacts.Select(t => t.Email).Distinct().ToList();
            if (!_contactEmails.Contains(contact.Email))
            {
                _unitOfWorkAsync.RepositoryAsync<Contact>().Insert(contact);
                _unitOfWorkAsync.SaveChangesAsync();
            }
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