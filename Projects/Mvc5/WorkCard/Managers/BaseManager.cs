using Repository.Pattern.Repositories;
using Repository.Pattern.UnitOfWork;
using Service.Pattern;
using Web.Models;
using Web.Services;

namespace Web.Managers
{
    public class BaseManager
    {
        public readonly ApplicationDbContext dbContext;
        public readonly IsssueService issueService;
        public readonly EmailService emailService;
        public readonly JobService jobService;
        public IService<Question> QuestionService;
        public IUnitOfWorkAsync _unitOfWorkAsync;
        public IRepositoryAsync<WorkIssue> repository;

        public BaseManager(IUnitOfWorkAsync unitOfWorkAsync)
        {
            dbContext = new ApplicationDbContext();
            _unitOfWorkAsync = unitOfWorkAsync;
            issueService = new IsssueService(unitOfWorkAsync.RepositoryAsync<WorkIssue>());
            jobService = new JobService(unitOfWorkAsync.RepositoryAsync<Job>());
            emailService = new EmailService();
        }
    }
}