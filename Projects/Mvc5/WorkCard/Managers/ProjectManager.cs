using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Managers
{
    public class ProjectManager:BaseManager
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ProjectManager(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {
        }

        public Project GetById(Guid id)
        {
            return db.Projects.FindAsync(id).Result;
        }
        public Project GetByName(string projectName)
        {
            var _projects = db.Projects.Where(t => t.Title.ToLower().Contains(projectName.ToLower()));
            if (_projects != null && _projects.Count() == 1)
            {
                return _projects.FirstOrDefault();
            }
            else
            {
                return null;
            }
        }
        public List<Project> GetAllOf(string userName)
        {
            var _projects = _unitOfWorkAsync.RepositoryAsync<Project>()
                .Queryable()
                .ToList();

            return _projects.Where(t=>t.IsOf(userName)).ToList();
        }
       
        public IEnumerable<WorkIssue> GetIssues(Guid projectId)
        {
            var _issues = _unitOfWorkAsync.RepositoryAsync<WorkIssue>()
                .Queryable()
                .Where(t => t.ProjectId == projectId);
            return _issues.AsEnumerable();
        }

        public IEnumerable<Question> GetQuestions(Guid projectId)
        {
            var _questions = db.Questions.Where(t => t.ProjectId == projectId);
            return _questions.AsEnumerable();
        }
        public IEnumerable<Comment> GetComments(Guid projectId)
        {
            var _comments = db.Comments.Where(t => t.ProjectId == projectId);
            return _comments.AsEnumerable();
        }
        public IEnumerable<Contact> GetContacts(Guid projectId)
        {
            var _contacts = db.Contacts.Where(t => t.ProjectId == projectId);
            return _contacts.AsEnumerable();
        }

        public async Task<bool> AddContactAsync(Guid projectId, Contact contact)
        {
            if (!contact.ProjectId.HasValue) return false;
            var _nowContacts = GetContacts(projectId).Select(t => t.Email);
            if (!_nowContacts.Contains(contact.Email))
            {
                db.Contacts.Add(contact);
                await db.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}