using CafeT.Html;
using SmartTracking.Models;
using SmartTracking.Models.Objects;
using SmartTracking.ViewModels;
using System.Collections.Generic;

namespace SmartTracking.Mappers
{
    public class IssueMappers
    {
        public static IssueViewModel IssueToViewModel(Issue model)
        {
            IssueViewModel view = new IssueViewModel();
            view.IssueId = model.IssueId;
            view.IssueTitle = model.IssueTitle.HtmlToText();
            view.IssueEstimation = model.IssueEstimation;
            view.StatusName = model.StatusName;
            view.MilestoneName = model.MilestoneName;
            view.AssignedUserName = model.AssignedUserName;
            view.IssueDueDate = model.IssueDueDate;
            view.Notify = model.Notify();
            view.IsClosed = model.IsClosed;

            return view;
        }

        ////public Issue ToNextIssue(Issue issue)
        ////{
        ////    Issue _issue = new Issue();
        ////    _issue.IssueId = issue.IssueId;

        ////    bool isDaily = true;
        ////    if(isDaily)
        ////    {
        ////        _issue.IssueDueDate = issue.IssueDueDate.Value.AddDays(1);
        ////    }
        ////    return _issue;
        ////}

        public static List<IssueViewModel> IssueToViewModels(List<Issue> models)
        {
            List<IssueViewModel> views = new List<IssueViewModel>();
            foreach (var model in models)
            {
                views.Add(IssueToViewModel(model));
            }
            return views;
        }

        public static NewIssue NewIssueToViewModel(Issue model)
        {
            NewIssue view = new NewIssue();
            view.IssueTitle = model.IssueTitle.HtmlToText();
            view.IssueDescription = model.IssueDescription;
            view.ProjectId = model.ProjectId;
            view.IssueCategoryId = model.IssueCategoryId;
            view.IssueStatusId = model.IssueStatusId;
            view.IssuePriorityId = model.IssuePriorityId;
            view.IssueMilestoneId = model.IssueMilestoneId;
            view.IssueAffectedMilestoneId = model.IssueAffectedMilestoneId;
            view.IssueTypeId = model.IssueTypeId;
            view.IssueResolutionId = model.IssueResolutionId;
            view.IssueAssignedUserName = model.IssueAssignedUserName;
            view.IssueCreatorUserName = model.IssueCreatorUserName;
            view.IssueOwnerUserName = model.IssueOwnerUserName;
            view.IssueDueDate = model.IssueDueDate;
            view.IssueVisibility = model.IssueVisibility;
            view.IssueEstimation = model.IssueEstimation;
            view.IssueProgress = model.IssueProgress;
            view.IssueType = 0;

            return view;
        }
    }
}
