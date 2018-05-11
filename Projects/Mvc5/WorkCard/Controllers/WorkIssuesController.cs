using AutoMapper;
using CafeT.Objects.Enums;
using CafeT.Text;
using Google.Apis.Auth.OAuth2.Mvc;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using PagedList;
using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using Web.Managers;
using Web.Mappers;
using Web.Models;
using Web.ModelViews;
using Web.Services;

namespace Web.Controllers
{
    [Authorize]
    public class WorkIssuesController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public WorkIssuesController() : base() { }
        public WorkIssuesController(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {
        }

        #region Search
        public async Task<ActionResult> SearchBy(string keyWord, int? page, FormCollection collection)
        {
            if(!keyWord.IsNullOrEmptyOrWhiteSpace())
            {
                keyWord = collection["Keyword"];
            }
            else
            {
                keyWord = string.Empty;
            }

            ViewBag.Keyword = keyWord;

            var _objects = _unitOfWorkAsync.RepositoryAsync<WorkIssue>()
                                .Query().Select().Where(t => t.Contains(keyWord))
                                .OrderByDescending(t=>t.CreatedDate)
                                .ThenByDescending(t=>t.UpdatedDate)
                                .ToList();

            var _views = IssueMappers.IssuesToViews(_objects);

            if (Request.IsAjaxRequest())
            {
                return PartialView("Issues/_RelatedIssues",
                    _views.ToPagedList(pageNumber: page ?? 1, pageSize: PageSize));
            }

            return View("_SearchResults", _views.ToPagedList(pageNumber: page ?? 1, pageSize: PageSize));
        }
        #endregion


        public async Task<ActionResult> Index()
        {
            var _objects = IssueManager.GetAllOf(User.Identity.Name);
            var _views = IssueMappers.IssuesToViews(_objects);

            if (User.Identity.IsAuthenticated)
            {
                return View("Index", _views.Where(t => t.CreatedBy == User.Identity.Name).ToList());
            }
            
            return View("Index", _views);
        }
       
        [HttpGet]
        public async Task<ActionResult> GetCompletedIssues(int? page)
        {
            if (page == null) page = 1;
            var _objects = IssueManager.GetAllOf(User.Identity.Name);

            _objects = _objects.Where(t => t.IsVerified && t.IsCompleted())
                .Where(t=>t.IsInDay(DateTime.Today))
                .OrderByDescending(t=>t.UpdatedDate)
                .ThenByDescending(t => t.CreatedDate)
                .ToList();

            var _views = IssueMappers.IssuesToViews(_objects);
            ViewBag.CompletedIssues = _views;
            if (Request.IsAjaxRequest())
            {
                return PartialView("Issues/_IssuesCompleted", 
                    _views.ToPagedList(pageNumber: page??1, pageSize: PageSize));
            }
            return View("Index", _views);
        }

        [HttpGet]
        public async Task<ActionResult> GetTodayIssues(int? page)
        {
            if (page == null) page = 1;
            var _objects = IssueManager.GetAllOf(User.Identity.Name);
            _objects = _objects.Where(t => t.IsVerified && !t.IsCompleted())
                .Where(t=>t.IsToday())
                .OrderByDescending(t => t.UpdatedDate)
                .ThenByDescending(t => t.CreatedDate)
                .ToList();
            var _views = IssueMappers.IssuesToViews(_objects);
            ViewBag.TodayIssues = _views;

            if (Request.IsAjaxRequest())
            {
                return PartialView("Issues/_IssuesToday",
                    _views.ToPagedList(pageNumber: page ?? 1, pageSize: PageSize));
            }
            return View("Index", _views);
        }

        [HttpGet]
        public async Task<ActionResult> GetExpiredIssues(int? page)
        {
            if (page == null) page = 1;
            var _objects = IssueManager.GetAllOf(User.Identity.Name);
            _objects = _objects.Where(t => t.IsVerified && t.IsExpired())
                .OrderByDescending(t=>t.UpdatedDate)
                .ThenByDescending(t=>t.CreatedDate)
                .ToList();
            var _views = IssueMappers.IssuesToViews(_objects);
            ViewBag.ExpiredIssues = _views;
            if (Request.IsAjaxRequest())
            {
                return PartialView("Issues/_IssuesExpired", 
                    _views.ToPagedList(pageNumber: page ?? 1, pageSize: PageSize));
            }
            return View("Index", _views);
        }
        [HttpGet]
        public async Task<ActionResult> GetUpcomingIssues(int? page)
        {
            if (page == null) page = 1;
            var _objects = IssueManager.GetAllOf(User.Identity.Name);
            _objects = _objects.Where(t => t.IsUpcoming(1))
                .OrderByDescending(t => t.UpdatedDate)
                .ThenByDescending(t => t.CreatedDate)
                .ToList();
            var _views = IssueMappers.IssuesToViews(_objects);
            ViewBag.UpcomingIssues = _views;
            if (Request.IsAjaxRequest())
            {
                return PartialView("Issues/_IssuesUpcoming",
                    _views.ToPagedList(pageNumber: page ?? 1, pageSize: PageSize));
            }
            return View("Index", _views);
        }
        [HttpGet]
        public async Task<ActionResult> GetToVerifyIssues(int? page)
        {
            if (page == null) page = 1;
            var _objects = IssueManager.GetAllOf(User.Identity.Name);
            _objects = _objects.Where(t => !t.IsVerified)
                .OrderByDescending(t => t.UpdatedDate)
                .ThenByDescending(t => t.CreatedDate)
                .ToList();
            var _views = IssueMappers.IssuesToViews(_objects);
            ViewBag.VerifyIssues = _views;
            if (Request.IsAjaxRequest())
            {
                return PartialView("Issues/_IssuesToVerify",
                    _views.ToPagedList(pageNumber: page ?? 1, pageSize: PageSize));
            }
            return View("Index", _views);
        }
        [HttpGet]
        public ActionResult GetQuestionsMustAnswer(string id)
        {
            var Id = Guid.Parse(id);
            var _objects = _unitOfWorkAsync.RepositoryAsync<Question>()
                .Query().Select().Where(t => t.IssueId == Id)
                .OrderByDescending(t => t.UpdatedDate).ToList();
            List<Question> _questions = new List<Question>();
            foreach (var q in _objects)
            {
                if (!QuestionManager.HasAnswers(q.Id)) _questions.Add(q);
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("Questions/_Questions", _questions);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult GetQuestionsHasAnswers(string id)
        {
            var Id = Guid.Parse(id);
            var _objects = _unitOfWorkAsync.RepositoryAsync<Question>()
                .Query().Select()
                .Where(t => t.IssueId == Id)
                .OrderByDescending(t => t.UpdatedDate).ToList();
            List<Question> _questions = new List<Question>();
            foreach(var q in _objects)
            {
                if (QuestionManager.HasAnswers(q.Id)) _questions.Add(q);
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("Questions/_Questions", _questions);
            }
            return RedirectToAction("Index");
        }
        
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> DeleteAllQuestions(Guid id)
        {
            var _questions = IssueManager.GetQuestionsBy(id);
            foreach(var _question in _questions)
            {
                QuestionManager.Delete(_question.Id);
            }
            try
            {
                await _unitOfWorkAsync.SaveChangesAsync();
                return RedirectToAction("Details", "WorkIssues");
            }
            catch(Exception ex)
            {
                return RedirectToAction("Errors");
            }
        }

        [HttpGet]
        public async Task<ActionResult> LoadIssue(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            WorkIssue workIssue = IssueManager.GetById(id.Value);
            
            if (workIssue == null)
            {
                return HttpNotFound();
            }

            var _view = Mapper.Map<WorkIssue, IssueView>(workIssue);

            if (Request.IsAjaxRequest())
            {
                return PartialView("Issues/_IssueOnly", _view);
            }
            return View(_view);
        }

        [HttpGet]
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            WorkIssue workIssue = IssueManager.GetById(id.Value);
            if(User.Identity.IsAuthenticated)
            {
                workIssue.AddViewer(User.Identity.Name);
                await IssueManager.UpdateAsync(workIssue);
            }

            if (workIssue == null)
            {
                return HttpNotFound();
            }
            workIssue = BuildContent(workIssue);
            var _view = Mapper.Map<WorkIssue, IssueView>(workIssue);
            if (Request.IsAjaxRequest())
            {
                return PartialView("Issues/_IssueItem", _view);
            }
            return View(_view);
        }

        public WorkIssue ProcessEmbedUrls(WorkIssue issue)
        {
            var _commands = issue.GetCommands();
            if (_commands != null && _commands.Count() > 0)
            {
                foreach (string command in _commands)
                {
                    var _command = new Command(command);
                    if(_command.Type== Models.CommandType.IsUrl)
                    {
                        try
                        {
                            string _id = command.Split(new string[] { "," }, StringSplitOptions.None)[1]
                                .Replace("#", string.Empty);
                            Guid id = Guid.Parse(_id);
                            var _url = UrlManager.GetById(id);
                            _url.Load();
                            if(_url.Title.IsNullOrEmptyOrWhiteSpace())
                            {
                                UrlManager.Update(_url);
                            }
                            string _htmlQuestion = RenderViewToString(ControllerContext, "Urls/_EmbedUrl", _url);
                            issue.Content = issue.Content.Replace(command, _htmlQuestion);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }                    
                }
            }
            return issue;
        }

        public WorkIssue ProcessEmbedQuestions(WorkIssue issue)
        {
            var _commands = issue.GetCommands();
            if (_commands != null && _commands.Count() > 0)
            {
                foreach (string _command in _commands)
                {
                    if (_command.CountOf(",") >= 2)
                    {
                        string _id = _command.Split(new string[] { "," }, StringSplitOptions.None)[1]
                            .Replace("#", string.Empty);
                        try
                        {
                            Guid id = Guid.Parse(_id);
                            var _question = QuestionManager.GetById(id);
                            string _htmlQuestion = RenderViewToString(ControllerContext, "Questions/_EmbedQuestion", _question);
                            issue.Content = issue.Content.Replace(_command, _htmlQuestion);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            return issue;
        }

        public WorkIssue BuildContent(WorkIssue issue)
        {
            issue = ProcessEmbedQuestions(issue);
            issue = ProcessEmbedUrls(issue);
            return issue;
        }

        [HttpGet]
        public ActionResult GetAllExpired(string userName)
        {
            var _objects = IssueManager.GetAllExpired(userName).ToList();
            var _views = IssueMappers.IssuesToViews(_objects);
            if (Request.IsAjaxRequest())
            {
                return PartialView("Issues/_Issues", _views);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult GetLastest(string userName)
        {
            var _objects = IssueManager.GetLastest(userName).ToList();
            var _views = IssueMappers.IssuesToViews(_objects);
            if (Request.IsAjaxRequest())
            {
                return PartialView("Issues/_Issues", _views);
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult GetMembers(Guid id)
        {
            var _objects = ContactManager.GetContactsOfIssue(id).ToList();
            if (Request.IsAjaxRequest())
            {
                return PartialView("Contacts/_Contacts", _objects);
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult GetContacts(Guid id)
        {
            var _objects = ContactManager.GetContactsOfIssue(id).ToList();
            if (Request.IsAjaxRequest())
            {
                return PartialView("Contacts/_List", _objects);
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult GetSubIssues(Guid id)
        {
            var _objects = IssueManager.GetSubIssues(id).ToList();
            var _views = IssueMappers.IssuesToViews(_objects);
            if (Request.IsAjaxRequest())
            {
                return PartialView("Issues/_Issues", _views);
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        [Authorize]
        public ActionResult AddQuestion(Guid issueId)
        {
            Question _question = new Question() { IssueId = issueId };

            if (Request.IsAjaxRequest())
            {
                return PartialView("Questions/_AddQuestionAjax", _question);
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        [Authorize]
        public ActionResult AddSubIssue(Guid id)
        {
            WorkIssue issue = new WorkIssue() { ParentId = id };

            if (Request.IsAjaxRequest())
            {
                return PartialView("Issues/_AjaxQuickCreateIssue", issue);
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        [Authorize]
        public ActionResult LoadInfo(Guid id)
        {
            var issue = IssueManager.GetById(id);
            var _view = Mapper.Map<WorkIssue, IssueView>(issue);
            if (Request.IsAjaxRequest())
            {
                return PartialView("Issues/_IssueInfo", _view);
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Create()
        {
            var _objects = _unitOfWorkAsync.RepositoryAsync<WorkIssue>()
                                .Query().Select()
                                .Where(m => !string.IsNullOrWhiteSpace(m.Title))
                                .OrderByDescending(t => t.CreatedDate).ToList();
            ViewBag.Issues = _objects.ToList();
            return View();
        }


        
        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(WorkIssue workIssue)
        {
            if (ModelState.IsValid)
            {
                if(!workIssue.IsValid())
                {
                    if(Request.IsAjaxRequest())
                    {
                        return PartialView("_NotifyMessage", "Issue is not valid");
                    }
                    else
                    {
                        return View("_NotifyMessage", "Issue is not valid");
                    }
                }
                workIssue.Id = Guid.NewGuid();
                workIssue.CreatedBy = User.Identity.Name;
                workIssue.PrepareToCreate();
                workIssue.Update();
                
                if(workIssue.Tags.Count > 0)
                {
                    var projects = ProjectManager.GetAllOf(User.Identity.Name).ToList();
                    var _tag = workIssue.Tags[0].ToLower();
                    var _selectProjects = projects
                        .Where(t => t.Title.ToLower().Contains(_tag))
                        .ToList();
                        
                    if (_selectProjects != null && _selectProjects.Any())
                    {
                        workIssue.ProjectId = _selectProjects.FirstOrDefault().Id;
                    }
                    else
                    {
                        var _project = ProjectManager.GetByName(_tag);
                        if(_project != null)
                        {
                            workIssue.ProjectId = _project.Id;
                        }
                    }
                }

                IssueManager.Insert(workIssue);

                if (workIssue.ProjectId.HasValue)
                {
                    var _project = ProjectManager.GetById(workIssue.ProjectId.Value);
                    if(!_project.IsOf(User.Identity.Name))
                    {
                        Contact contact = new Contact();
                        contact.CreatedBy = User.Identity.Name;
                        contact.Email = User.Identity.Name;
                        contact.UserName = User.Identity.Name;
                        contact.Projects.Add(_project);
                        workIssue.Contacts.Add(contact);
                        await ProjectManager.AddContactAsync(_project.Id, contact);
                    }
                }

                if(workIssue.HasInnerMembers())
                {
                    var _innerMembers = workIssue.GetInnerMembers();
                    foreach(string _member in _innerMembers)
                    {
                        Contact _contact = new Contact();
                        _contact.Email = _member;
                        _contact.UserName = _member;
                        _contact.CreatedBy = User.Identity.Name;
                        await ContactManager.AddContactAsync(_contact);
                    }
                }
                if(Request.IsAjaxRequest())
                {
                    var _viewObject = Mapper.Map<WorkIssue, IssueView>(workIssue);
                    return PartialView("Issues/_IssueItem", _viewObject);
                }
                return RedirectToAction("Details", "WorkIssues", new { id = workIssue.Id});
            }

            return View(workIssue);
        }
        
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> SetNextWeek(Guid id)
        {
            WorkIssue workIssue = IssueManager.GetById(id);
            if (Request.IsAjaxRequest())
            {
                return PartialView("Issues/_SetNextWeek", workIssue);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> SetWorkOnTime(Guid id)
        {
            WorkIssue workIssue = IssueManager.GetById(id);
            if (Request.IsAjaxRequest())
            {
                return PartialView("Issues/_SetTime", workIssue);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> SetWorkOnTime(Guid id, string date)
        {
            DateTime newStart = DateTime.Parse(date);
            WorkIssue workIssue = IssueManager.GetById(id);
            workIssue.UpdateTime(newStart);
            await IssueManager.UpdateAsync(workIssue);

            if (Request.IsAjaxRequest())
            {
                return PartialView("Issues/_WorkTime", "Work on " + date);
            }
            return RedirectToAction("Index");
        }

        public bool AddEvent(CalendarService service, Event item)
        {
            
            var list = service.CalendarList.List().Execute();
            var calendars = list.Items;
            if(calendars.Select(t=>t.Summary.ToLower().Contains("WorkCard.vn".ToLower())) == null)
            {
                Google.Apis.Calendar.v3.Data.Calendar newCalendar = new Google.Apis.Calendar.v3.Data.Calendar();
                newCalendar.Summary = "WorkCard.vn";
                newCalendar.Description = "Dùng để quản lý công việc với WorkCard.vn";
                newCalendar.Location = "WorkCard.vn";

                service.Calendars.Insert(newCalendar);
            }

            var myCalendar = list.Items.SingleOrDefault(c => c.Summary == "WorkCard.vn");

            if (myCalendar != null)
            {
                var newEventRequest = service.Events.Insert(item, myCalendar.Id);
                var eventResult = newEventRequest.Execute();
                return true;
            }
            return false;
        }

        public async Task<ActionResult> ToCalendar(Guid id, CancellationToken cancellationToken)
        {
            var result = await new AuthorizationCodeMvcApp(this, new AppFlowMetadata()).
                AuthorizeAsync(cancellationToken);

            if (result.Credential != null)
            {
                var service = new CalendarService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = result.Credential,
                    ApplicationName = "WorkCard.vn"
                });

                var _object = IssueManager.GetById(id);
                var _event = new Event();
                EventDateTime start = new EventDateTime();
                start.DateTime = _object.Start.Value;
                EventDateTime end = new EventDateTime();
                end.DateTime = _object.End.Value;
                _event.Summary = _object.Title;
                _event.Location = "WorkCard.vn";
                _event.Start = start;
                _event.End = end;
                _event.Description = "From WorkCard.vn";
                bool _isAdded = AddEvent(service, _event);
                
                if (Request.IsAjaxRequest())
                {
                    if (_isAdded)
                    {
                        return PartialView("_Message", "Đã thêm.");
                    }
                    else
                    {
                        return PartialView("_Message", "Lỗi. Không thêm vào Google Calendar được.");
                    }
                }
                return RedirectToAction("Index");
            }
            else
            {
                return new RedirectResult(result.RedirectUri);
            }
        }
        
        [Authorize]
        public async Task<ActionResult> ToArticle(Guid id)
        {
           
            Article article = new Article();
            var _object = IssueManager.GetById(id);
            article.Title = _object.Title;
            article.Description = _object.Description;
            article.Content = _object.Content;
            article.CreatedBy = User.Identity.Name;

            db.Articles.Add(article);
            await db.SaveChangesAsync();
            if (Request.IsAjaxRequest())
            {
                return PartialView("_Message", string.Empty);
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> AddTimeToDo(Guid id)
        {
            WorkIssue workIssue = IssueManager.GetById(id);
            if (Request.IsAjaxRequest())
            {
                return PartialView("Issues/_AddTime", workIssue);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> AddTimeToDo(Guid id, int minutes)
        {
            WorkIssue workIssue = IssueManager.GetById(id);
            workIssue.AddTimeToDo(minutes);
            await IssueManager.UpdateAsync(workIssue);

            if (Request.IsAjaxRequest())
            {
                return PartialView("Issues/_WorkTime", "Work on " + workIssue.IssueEstimation);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Notify(Guid id)
        {
            IssueManager.IsNotify = true;
            IssueManager.Notify(id);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_NotifyMessage", "Sent!");
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Verify(Guid id)
        {
            IssueManager.IsNotify = true;
            IssueManager.Verify(id);
            IssueManager.Notify(id);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_NotifyMessage", "Verified!");
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> StopEmail(Guid id)
        {
            var issue = IssueManager.GetById(id);
            if (issue != null)
            {
                issue.IsStopEmail = true;
                await IssueManager.UpdateAsync(issue);
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("_NotifyMessage", "Stop email!");
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> StartEmail(Guid id)
        {
            var issue = IssueManager.GetById(id);
            if (issue != null)
            {
                issue.IsStopEmail = false;
                await IssueManager.UpdateAsync(issue);
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("_NotifyMessage", "Stop email!");
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> MarkStatus(Guid id, IssueStatus status)
        {
            WorkIssue workIssue = IssueManager.GetById(id);
            workIssue.Status = status;
            await IssueManager.UpdateAsync(workIssue);

            if(status == IssueStatus.Done)
            {
                Message message = new Message();
                message.Title = "[Chúc mừng] - Finished " + workIssue.Title;
                message.Content = workIssue.Content;
                message.ToEmails = workIssue.GetEmails().ToList();
                message.Notify(EmailService);
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("Issues/_WorkStatus", status);
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkIssue workIssue = IssueManager.GetById(id.Value);
            if (workIssue == null)
            {
                return HttpNotFound();
            }
            
            return View(workIssue);
        }

       
        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(WorkIssue workIssue)
        {
            if (ModelState.IsValid)
            {
                workIssue.UpdatedBy = User.Identity.Name;
                try
                {
                    IssueManager.IsNotify = true;
                    await IssueManager.UpdateAsync(workIssue);
                    return RedirectToAction("Index", "Home");
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return View(workIssue);
        }

        [Authorize]
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkIssue workIssue = IssueManager.GetById(id.Value);
            if (workIssue == null)
            {
                return HttpNotFound();
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("_Delete", workIssue);
            }
            return View(workIssue);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            IssueManager.Delete(id);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_DeleteMsg", "Đã xóa");
            }
            return RedirectToAction("Index");
        }

    }
}
