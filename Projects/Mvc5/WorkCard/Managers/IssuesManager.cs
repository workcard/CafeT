using CafeT.Objects.Enums;
using CafeT.Text;
using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Web.Models;

namespace Web.Managers
{
    public class IssuesManager:BaseManager
    {
        public IssuesManager(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {
        }

        public WorkIssue GetById(Guid id)
        {
            var _object = _unitOfWorkAsync.Repository<WorkIssue>().Find(id);
            return _object;
        }

        public List<WorkIssue> GetAll()
        {
            return _unitOfWorkAsync.RepositoryAsync<WorkIssue>()
                .Queryable()
                .ToList();
        }
        public List<Question> GetQuestionsBy(Guid issueId)
        {
            return _unitOfWorkAsync.RepositoryAsync<Question>()
                .Queryable()
                .Where(t=>t.IssueId == issueId)
                .ToList();
        }
        public List<WorkIssue> GetAllOf(string userName)
        {
            return _unitOfWorkAsync.RepositoryAsync<WorkIssue>()
                .Queryable()
                .Where(t => (t.CreatedBy.ToLower() == userName) || (t.Owner.ToLower() == userName))
                .ToList();
        }
        public List<WorkIssue> GetAllOf(string userName, IssueStatus status)
        {
            return GetAllOf(userName)
                .Where(t=>t.Status == status)
                .ToList();
        }
        public List<Question> MakeQuestions(Guid issueId)
        {
            List<Question> questions = new List<Question>();
            Question qWhat = new Question() { IssueId = issueId };
            qWhat.Title = "[What] Cái gì ?";
            questions.Add(qWhat);

            Question qWhere = new Question() { IssueId = issueId };
            qWhere.Title = "[Where] Ở đâu ?";
            questions.Add(qWhere);

            Question qWhen = new Question() { IssueId = issueId };
            qWhen.Title = "[When] Khi nào hoàn thành ?";
            questions.Add(qWhen);

            Question qWhy = new Question() { IssueId = issueId };
            qWhy.Title = "[Why] Tại sao ?";
            questions.Add(qWhy);

            Question qHow = new Question() { IssueId = issueId };
            qHow.Title = "[How] Làm như thế nào ?";
            questions.Add(qHow);

            Question qWho1 = new Question() { IssueId = issueId };
            qWho1.Title = "[Who] Ai làm ?";
            questions.Add(qWho1);

            Question qWho2 = new Question() { IssueId = issueId };
            qWho2.Title = "[Who] Ai kiểm soát ?";
            questions.Add(qWho2);

            Question qResult = new Question() { IssueId = issueId };
            qResult.Title = "[Result] Kết quả cần đạt được là gì ?";
            questions.Add(qResult);

            Question qReview = new Question() { IssueId = issueId };
            qReview.Title = "[Review] Đánh giá như thế nào ?";
            questions.Add(qReview);
            return questions;
        }

        public bool Insert(WorkIssue issue)
        {
            string[] _commands = issue.GetCommands();
            if(_commands != null && _commands.Count()>0)
            {
                foreach(string _command in _commands)
                {
                    Command command = new Command(_command);
                    if(command.Type == Models.CommandType.IsQuestion)
                    {
                        Question question = new Question();
                        
                        question.Id = Guid.NewGuid();
                        question.IssueId = issue.Id;
                        question.Title = HttpUtility.HtmlDecode(command.Title);
                        question.Content = command.Body;
                        question.CreatedBy = issue.CreatedBy;
                        _unitOfWorkAsync.RepositoryAsync<Question>().Insert(question);
                        issue.Content = issue.Content
                            .Replace(_command, _command.AddAfter(',',",#" + question.Id.ToString()));

                    }
                    else if(command.Type == Models.CommandType.IsUrl)
                    {
                        var urlModel = new Url(command.Header);
                        urlModel.Load();
                        urlModel.CreatedBy = issue.CreatedBy;
                        urlModel.IssueId = issue.Id;
                        _unitOfWorkAsync.RepositoryAsync<Url>().Insert(urlModel);
                        issue.Content = issue.Content
                                .Replace(_command, command.Text.AddAfter(',', ",#" + urlModel.Id.ToString()));
                    }
                }
            }
            _unitOfWorkAsync.RepositoryAsync<WorkIssue>().Insert(issue);
            
            int _result = _unitOfWorkAsync.SaveChangesAsync().Result;
            if (_result >= 0)
            {
                Notify(issue);
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<bool> UpdateAsync(WorkIssue issue)
        {
            issue.Update();
            string[] _commands = issue.GetCommands();

            if (_commands != null && _commands.Count() > 0)
            {
                foreach (string _command in _commands)
                {
                    if (!_command.Contains("#"))
                    {
                        Command command = new Command(_command);
                        if (command.Type == Models.CommandType.IsQuestion)
                        {
                            Question question = new Question();

                            question.Id = Guid.NewGuid();
                            question.IssueId = issue.Id;
                            question.Title = HttpUtility.HtmlDecode(command.Title);
                            question.Content = command.Body;
                            question.CreatedBy = issue.CreatedBy;
                            _unitOfWorkAsync.RepositoryAsync<Question>().Insert(question);

                            issue.Content = issue.Content
                                .Replace(_command, _command.AddAfter(',', ",#" + question.Id.ToString()));

                        }
                        if (command.Type == Models.CommandType.IsUrl)
                        {
                            Url url = new Url();
                            url.Id = Guid.NewGuid();
                            url.IssueId = issue.Id;
                            url.Title = HttpUtility.HtmlDecode(command.Title);
                            url.CreatedBy = issue.CreatedBy;
                            _unitOfWorkAsync.RepositoryAsync<Url>().Insert(url);

                            issue.Content = issue.Content
                                .Replace(_command, _command.AddAfter(',', ",#" + url.Id.ToString()));

                        }
                    }
                }
            }

            _unitOfWorkAsync.Repository<WorkIssue>().Update(issue);
            int _result = _unitOfWorkAsync.SaveChangesAsync().Result;
            if (_result >= 0)
            {
                Notify(issue);
                if (issue.Status == IssueStatus.Done)
                {
                    WorkIssue _newIssue = new WorkIssue();
                    if (issue.Repeat == ScheduleType.Daily)
                    {
                        _newIssue = issue.MoveTo(issue.Start.Value.AddDays(1));
                    }
                    else if (issue.Repeat == ScheduleType.Weekly)
                    {
                        _newIssue = issue.MoveTo(issue.Start.Value.AddDays(5));
                    }
                    else if (issue.Repeat == ScheduleType.Monthly)
                    {
                        _newIssue = issue.MoveTo(issue.Start.Value.AddMonths(1));
                    }
                    else if (issue.Repeat == ScheduleType.Yearly)
                    {
                        _newIssue = issue.MoveTo(issue.Start.Value.AddYears(1));
                    }
                    _newIssue.Status = IssueStatus.New;
                    _newIssue.Update();
                    _unitOfWorkAsync.Repository<WorkIssue>().Insert(_newIssue);
                    Notify(_newIssue);
                    await _unitOfWorkAsync.SaveChangesAsync();
                }
                return true;
            }
            else
            {
                return false;
            }

            
            
        }
        public bool Verify(Guid id)
        {
            var issue = GetById(id);
            issue.Update();
            issue.IsVerified = true;
            _unitOfWorkAsync.Repository<WorkIssue>().Update(issue);
            int _result = _unitOfWorkAsync.SaveChangesAsync().Result;
            if (_result >= 0)
            {
                Notify(issue);
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool Delete(Guid id)
        {
            var workIssue = GetById(id);
            _unitOfWorkAsync.Repository<WorkIssue>().Delete(workIssue);
            int _result = _unitOfWorkAsync.SaveChangesAsync().Result;
            if (_result < 0) return false;
            return true;
        }

        public IEnumerable<WorkIssue> GetAllExpired(string userName)
        {
            var _objects = _unitOfWorkAsync.Repository<WorkIssue>().Queryable().ToList();
            _objects = _objects.Where(t => t.IsOf(userName) && !t.IsClosed && t.IsExpired()).ToList();
            return _objects;
        }

        public IEnumerable<WorkIssue> GetLastest(string userName)
        {
            var _objects = _unitOfWorkAsync.Repository<WorkIssue>()
                .Query().Select().Where(t => t.IsOf(userName))
                .OrderByDescending(t=>t.CreatedDate);
            
            return _objects;
        }
        public IEnumerable<WorkIssue> GetSubIssues(Guid id)
        {
            var _objects = _unitOfWorkAsync.Repository<WorkIssue>()
                .Query().Select()
                .Where(t => t.ParentId.HasValue && t.ParentId.Value == id)
                .OrderByDescending(t => t.UpdatedDate)
                .ThenByDescending(t=>t.CreatedDate);

            return _objects;
        }
        public void Notify(Guid id)
        {
            var _issue = GetById(id);
            if(IsNotify)
                _issue.Notify(emailService);
        }
        public bool IsNotify { set; get; } = false;
        public void Notify(WorkIssue issue)
        {
            if(IsNotify)
                issue.Notify(emailService);
        }
        
        public IEnumerable<WorkIssue> SeeList(string userName)
        {
            var _issues = GetAll();
            List<WorkIssue> _list = new List<WorkIssue>();
            foreach(var issue in _issues)
            {
                if(issue.GetEmails().Contains(userName) || issue.IsOf(userName))
                {
                    _list.Add(issue);
                }
            }
            return _list.AsEnumerable();
        }
    }
}