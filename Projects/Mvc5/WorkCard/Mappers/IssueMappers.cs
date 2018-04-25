using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using Web.Models;
using Web.ModelViews;

namespace Web.Mappers
{
    //public class ProjectMapper
    //{
    //    public static ProjectSummary ToSummary(Project project)
    //    {
    //        using (var manager = new ProjectManager(unitOfWorkAsync))
    //        {

    //        }
    //    }
    //}

    public class IssueMappers
    {
        public static List<IssueView> IssuesToViews(List<WorkIssue> models)
        {
            List<IssueView> views = new List<IssueView>();
            foreach (var model in models)
            {
                var _view = Mapper.Map<WorkIssue, IssueView>(model);
                views.Add(_view);
            }
            return views.OrderBy(t=>t.CreatedDate).ToList();
        }

        //public static NewIssue NewIssueToViewModel(WorkIssue model)
        //{
        //    NewIssue view = new NewIssue();
        //    view.IssueTitle = model.IssueTitle.RemoveHtml();
        //    view.IssueDescription = model.IssueDescription;
        //    view.ProjectId = model.ProjectId;
        //    view.IssueCategoryId = model.IssueCategoryId;
        //    view.IssueStatusId = model.IssueStatusId;
        //    view.IssuePriorityId = model.IssuePriorityId;
        //    view.IssueMilestoneId = model.IssueMilestoneId;
        //    view.IssueAffectedMilestoneId = model.IssueAffectedMilestoneId;
        //    view.IssueTypeId = model.IssueTypeId;
        //    view.IssueResolutionId = model.IssueResolutionId;
        //    view.IssueAssignedUserName = model.IssueAssignedUserName;
        //    view.IssueCreatorUserName = model.IssueCreatorUserName;
        //    view.IssueOwnerUserName = model.IssueOwnerUserName;
        //    view.IssueDueDate = model.IssueDueDate;
        //    view.IssueVisibility = model.IssueVisibility;
        //    view.IssueEstimation = model.IssueEstimation;
        //    view.IssueProgress = model.IssueProgress;
        //    view.IssueType = 0;

        //    return view;
        //}
    }
}
