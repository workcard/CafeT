using Microsoft.AspNet.Identity.Owin;
using Web.Models;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Repository.Pattern.UnitOfWork;
using CafeT.Frameworks.Identity.Models;

namespace SmartTracking.Controllers
{
    public class BaseController : Controller
    {
        protected int PageSize = 10;
        protected string UploadFolder = "~/Profiles/Uploads/";

        protected IUnitOfWorkAsync _unitOfWorkAsync;

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
        //protected readonly IssueManager _issueManager;
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

        public BaseController(IUnitOfWorkAsync unitOfWorkAsync)
        {
            _unitOfWorkAsync = unitOfWorkAsync;
        }
    }
}