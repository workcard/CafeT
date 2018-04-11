using Microsoft.AspNet.Identity.Owin;
using Mvc5.CafeT.vn.Managers;
using Mvc5.CafeT.vn.Models;
using Mvc5.CafeT.vn.Services;
using Repository.Pattern.UnitOfWork;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc5.CafeT.vn.Controllers
{
    public class BaseController : Controller
    {
        protected int PageSize = 10;
        protected const int NUMBER_GET_ITEMS = 5;
        protected const int PAGE_ITEMS = 10;

        protected string UploadFolder = "~/App_Data/Uploads/";
        protected string ServerPath = "~/App_Data/Profiles/";
        protected string ImageSourcePath = "~/App_Data/Tmp/";
        protected string DefaultImagesPath = "~/App_Data/Defaults/Images/";

        protected IUnitOfWorkAsync _unitOfWorkAsync;
        protected IArticleService _articleService;
        protected ICommentService _commentService;

        protected readonly ArticleManager _articleManager;
        protected readonly ArticleCategoryManager _articleCategoryManager;
        protected readonly JobManager _jobManager;
        protected readonly CommentManager _commentManager;
        protected readonly WordManager _wordManager;
        protected readonly WorkManager _workManager;
        protected readonly ProjectManager _projectManager;
        protected readonly FileManager _fileManager;
        protected readonly ImageManager _imageManager;
        protected readonly AnswerManager _answerManager;
        protected readonly UrlManager _urlManager;
        protected readonly QuestionManager _questionManager;
        protected readonly IssueManager _issueManager;
        protected readonly Mappers.Mappers _mapper;

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

        public BaseController(IUnitOfWorkAsync unitOfWorkAsync)
        {
            _unitOfWorkAsync = unitOfWorkAsync;

            _projectManager = new ProjectManager(unitOfWorkAsync);
            _fileManager = new FileManager(unitOfWorkAsync);
            _jobManager = new JobManager(unitOfWorkAsync);
            _wordManager = new WordManager(unitOfWorkAsync);
            _workManager = new WorkManager(unitOfWorkAsync);
            _imageManager = new ImageManager(unitOfWorkAsync);
            _answerManager = new AnswerManager(unitOfWorkAsync);
            _urlManager = new UrlManager(unitOfWorkAsync);
            _questionManager = new QuestionManager(unitOfWorkAsync);
            _issueManager = new IssueManager(unitOfWorkAsync);
            _articleCategoryManager = new ArticleCategoryManager(unitOfWorkAsync);
            
            _commentService = new CommentService(unitOfWorkAsync.RepositoryAsync<CommentModel>());
            _commentManager = new CommentManager(_commentService, unitOfWorkAsync);
           

            _articleService = new ArticleService(unitOfWorkAsync.RepositoryAsync<ArticleModel>());
            _articleManager = new ArticleManager(_articleService, unitOfWorkAsync);

            _mapper = new Mappers.Mappers(_unitOfWorkAsync, _articleService);
        }

        public void LoadSettings()
        {
            var _setting = _unitOfWorkAsync.Repository<ApplicationSetting>().Query().Select()
               .FirstOrDefault();
            if(_setting != null)
            {
                ViewBag.SiteName = _setting.Name;
            }
            else
            {
                ViewBag.SiteName = "Pls configuration for Name of website";
            }
        }
    }
}