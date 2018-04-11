using Repository.Pattern.Repositories;
using Service.Pattern;
using Web.Models;

namespace Web.Services
{
    //public interface IIsssueService : IService<WorkIssue>
    //{
    //}

    public class IsssueService : Service<WorkIssue>//, IIsssueService
    {
        public IsssueService(IRepositoryAsync<WorkIssue> repository) : base(repository) {}
    }
    public class JobService : Service<Job>//, IIsssueService
    {
        public JobService(IRepositoryAsync<Job> repository) : base(repository) { }
    }
}