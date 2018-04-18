using AutoMapper;
//using CafeT.GoogleServices;
using CafeT.Objects.Enums;
using CafeT.Text;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Mvc;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Owin;
using PagedList;
using Repository.Pattern.UnitOfWork;
//using SyncMyCal.Calendars;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
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
        protected int PageSize = 5;

        public WorkIssuesController(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {
        }

        #region Search
        public async Task<ActionResult> SearchBy(string keyWord)
        {
            var _objects = _unitOfWorkAsync.RepositoryAsync<WorkIssue>()
                                .Query().Select().Where(t => t.GetCommands().Count() > 0)
                                .Where(t => t.GetCommands().Contains(keyWord)).ToList();
            var _views = IssueMappers.IssuesToViews(_objects);
            if (Request.IsAjaxRequest())
            {
                return PartialView("Issues/_Issues", _views);
            }
            return View("Index", _views);
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
            _objects = _objects.Where(t => t.IsVerified && t.IsCompleted()).ToList();
            var _views = IssueMappers.IssuesToViews(_objects);


            if (Request.IsAjaxRequest())
            {
                return PartialView("Issues/_IssuesCompleted", 
                    _views.ToPagedList(pageNumber: page ?? 1, pageSize: PageSize));
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
                .ToList();
            var _views = IssueMappers.IssuesToViews(_objects);


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
            _objects = _objects.Where(t => t.IsVerified && t.IsExpired()).ToList();
            var _views = IssueMappers.IssuesToViews(_objects);

            if (Request.IsAjaxRequest())
            {
                return PartialView("Issues/_IssuesExpired", 
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
                _unitOfWorkAsync.RepositoryAsync<Question>()
                    .Delete(_question.Id);
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
                await IssueManager.UpdateAsync(id.Value, workIssue);
            }

            if (workIssue == null)
            {
                return HttpNotFound();
            }
            workIssue = BuildContent(workIssue);
            var _view = Mapper.Map<WorkIssue, IssueView>(workIssue);
            //_view.Content = BuildContent(_view.Content);
            if (Request.IsAjaxRequest())
            {
                return PartialView("Issues/_IssueItem", _view);
            }
            return View(_view);
        }
        public static string RenderViewToString(ControllerContext context, string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = context.RouteData.GetRequiredString("action");

            var viewData = new ViewDataDictionary(model);

            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(context, viewName);
                var viewContext = new ViewContext(context, viewResult.View, viewData, new TempDataDictionary(), sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }
        public string BuildContent(string html)
        {
            string[] _commands = html.ExtractAllBetween("{", "}");
            if(_commands != null && _commands.Count()>0)
            {
                foreach(string _command in _commands)
                {
                    if (_command.StartsWith("{?") && _command.EndsWith("}"))
                    {
                        if(_command.Contains(","))
                        {
                            string _questionContent = _command.Split(new string[] { "," }, StringSplitOptions.None)[1];

                            var _question = new Question()
                            {
                                Title = HttpUtility.HtmlDecode(_questionContent.GetSentences()[0]),
                                Content = _questionContent
                            };
                            string _htmlQuestion = RenderViewToString(ControllerContext, "Questions/_QuestionItem", _question);
                            html = html.Replace(_command, _htmlQuestion);
                        }
                        
                    }
                }
            }
            return html;
        }
        public WorkIssue BuildContent(WorkIssue issue)
        {
            //var _questions = IssueManager.GetQuestionsBy(issue.Id);
            var _commands = issue.GetCommands();
            if(_commands != null && _commands.Count()>0)
            {
                foreach(string _command in _commands)
                {
                    if(_command.CountOf(",")>=2)
                    {
                        string _id = _command.Split(new string[] { "," },StringSplitOptions.None)[1]
                            .Replace("#",string.Empty);
                        try
                        {
                            Guid id = Guid.Parse(_id);
                            var _question = QuestionManager.GetById(id);
                            string _htmlQuestion = RenderViewToString(ControllerContext, "Questions/_EmbedQuestion", _question);
                            issue.Content = issue.Content.Replace(_command,_htmlQuestion);
                        }
                       catch(Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
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
        [Authorize]
        public ActionResult AddQuestion(Guid issueId)
        {
            Question _question = new Question() { IssueId = issueId };

            if (Request.IsAjaxRequest())
            {
                return PartialView("Issues/_AddQuestionAjax", _question);
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
                        
                    if (_selectProjects != null && _selectProjects.Count() == 1)
                    {
                        workIssue.ProjectId = _selectProjects.FirstOrDefault().Id;
                    }
                }
                IssueManager.Insert(workIssue);
              
                if(workIssue.HasInnerMembers())
                {
                    var _innerMembers = workIssue.GetInnerMembers();
                    foreach(string _member in _innerMembers)
                    {
                        Contact _contact = new Contact();
                        _contact.Email = _member;
                        _contact.UserName = User.Identity.Name;
                        _contact.CreatedBy = User.Identity.Name;
                        await ContactManager.AddContactAsync(_contact);
                    }
                }

                if(Request.IsAjaxRequest())
                {
                    return PartialView("Issues/_IssueItem", workIssue);
                }
                return RedirectToAction("Index");
            }

            return View(workIssue);
        }
        
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> SetWorkOnTime(Guid id, string date)
        {
            DateTime newStart = DateTime.Parse(date);
            WorkIssue workIssue = IssueManager.GetById(id);
            workIssue.UpdateTime(newStart);
            await IssueManager.UpdateAsync(id, workIssue);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_WorkTime", "Work on " + date);
            }
            return RedirectToAction("Index");
        }

        public void AddEvent(CalendarService service, Event item)
        {
            //var service = new CalendarService(new BaseClientService.Initializer()
            //{
            //    HttpClientInitializer = Credential,
            //    ApplicationName = ApplicationName,
            //});
            var list = service.CalendarList.List().Execute();
            var myCalendar = list.Items.SingleOrDefault(c => c.Summary == "WorkCard.vn");

            if (myCalendar != null)
            {
                var newEventRequest = service.Events.Insert(item, myCalendar.Id);
                var eventResult = newEventRequest.Execute();
            }
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
                var _event = new Google.Apis.Calendar.v3.Data.Event();
                EventDateTime start = new EventDateTime();
                start.DateTime = _object.Start.Value;
                EventDateTime end = new EventDateTime();
                end.DateTime = _object.End.Value;
                _event.Summary = _object.Title;
                _event.Location = "WorkCard.vn";
                _event.Start = start;
                _event.End = end;
                _event.Description = "From WorkCard.vn";
                this.AddEvent(service, _event);
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_Message", string.Empty);
                }
                return RedirectToAction("Index");
            }
            else
            {
                return new RedirectResult(result.RedirectUri);
            }
        }
        //[Authorize]
        //public async Task<ActionResult> ToCalendar(Guid id)
        //{
        //    //GoogleServices google = new GoogleServices();
        //    var _object = IssueManager.GetById(id);
        //    GoogleCalendar calendar = new GoogleCalendar();
        //    bool _isConnected = calendar.ConnectCalendar();
        //    if(_isConnected)
        //    {
        //        var _event = new Google.Apis.Calendar.v3.Data.Event();
        //        EventDateTime start = new EventDateTime();
        //        start.DateTime = _object.Start.Value;
        //        EventDateTime end = new EventDateTime();
        //        end.DateTime = _object.End.Value;
        //        _event.Summary = _object.Title;
        //        _event.Location = "WorkCard.vn";
        //        _event.Start = start;
        //        _event.End = end;
        //        _event.Description = "From WorkCard.vn";
        //        calendar.AddEvent(_event);
        //    }

        //    if (Request.IsAjaxRequest())
        //    {
        //        return PartialView("_Message", string.Empty);
        //    }
        //    return RedirectToAction("Index");
        //}

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> AddTimeToDo(Guid id, int minutes)
        {
            WorkIssue workIssue = IssueManager.GetById(id);
            workIssue.AddTimeToDo(minutes);
            await IssueManager.UpdateAsync(id, workIssue);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_WorkTime", "Work on " + workIssue.IssueEstimation);
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
        public async Task<ActionResult> MarkStatus(Guid id, IssueStatus status)
        {
            WorkIssue workIssue = IssueManager.GetById(id);
            workIssue.Status = status;
            await IssueManager.UpdateAsync(id, workIssue);

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
                return PartialView("_WorkStatus", status);
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
                    await IssueManager.UpdateAsync(workIssue.Id, workIssue);
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
