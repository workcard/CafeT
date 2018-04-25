using CafeT.GoogleManager;
using Microsoft.AspNet.Identity.Owin;
using Repository.Pattern.UnitOfWork;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Web.Managers;
using Web.Models;

namespace Web.Controllers
{
    

    public class BaseController : Controller
    {
        protected int STORIES_COUNT_TOVIEW = 5;
        protected int ISSUES_COUNT_TOVIEW = 15;
        protected int Const_DefaultGetItems = 10;
        protected int PageSize = 5;
        protected string UploadFolder = "~/Profiles/Uploads/";

        protected readonly IUnitOfWorkAsync _unitOfWorkAsync;

        protected IssuesManager IssueManager { set; get; }
        protected UrlManager UrlManager { set; get; }
        protected ContactManager ContactManager { set; get; }
        protected QuestionManager QuestionManager { set; get; }
        protected JobManager JobManager { set; get; }
        protected ProjectManager ProjectManager { set; get; }
        protected EmailService EmailService { set; get; }
        protected Translator Translator { set; get; }
        public BaseController() { }
        public BaseController(
            IUnitOfWorkAsync unitOfWorkAsync)
        {
            _unitOfWorkAsync = unitOfWorkAsync;
            IssueManager = new IssuesManager(unitOfWorkAsync);
            UrlManager = new UrlManager(unitOfWorkAsync);
            ContactManager = new ContactManager(unitOfWorkAsync);
            JobManager = new JobManager(unitOfWorkAsync);
            QuestionManager = new QuestionManager(unitOfWorkAsync);
            ProjectManager = new ProjectManager(unitOfWorkAsync);
            EmailService = new EmailService();
            Translator = new Translator();
        }

        //protected IArticleService _articleService;
        //protected ICommentService _commentService;

        //protected readonly ArticleManager _articleManager;
        //protected readonly ArticleCategoryManager _articleCategoryManager;
        //protected readonly JobManager _jobManager;
        //protected readonly CommentManager _commentManager;
        //protected readonly WorkManager _workManager;
        //protected readonly ProjectManager _projectManager;
        //protected readonly FileManager _fileManager;
        //protected readonly ImageManager _imageManager;
        //protected readonly AnswerManager _answerManager;
        //protected readonly UrlManager _urlManager;
        //protected readonly QuestionManager _questionManager;
        //protected readonly MenuManager _menuManager;
        //protected readonly Mappers.Mappers _mapper;

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
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
    }
}