using SmartTracking.Models.Objects;
using System;
using System.Collections.Generic;

namespace SmartTracking.Helpers
{
    public class IssueHelpers
    {
        private static int projectid = 22;

        public IEnumerable<SmartIssue> GetIssuesFromFile(string path)
        {
            if (path != null)
            {
                try
                {
                    List<SmartIssue> fileImport = new List<SmartIssue>();
                    var excelData = new ExcelHelpers(path);

                    var data = excelData.GetData("Issues");
                    if (data != null)
                    {
                        int index = 1;
                        string staff = "";
                        string leader = "";
                        DateTime dateNow = DateTime.Now;
                        int issueStatusId = 132;
                        int issuePriorityId = 77;
                        int issueTypeId = 48;
                        int issueCategoryId = 395;
                        int issueMilestoneId = 60;
                        int issueAffectedMilestoneId = 60;

                        foreach (var row in data)
                        {
                            if (row[0].ToString() == "Date")
                            {
                                dateNow = DateTime.Parse(row[1].ToString());
                            }
                            if (row[0].ToString() == "Staff")
                            {
                                staff = row[1].ToString();
                            }
                            if (row[0].ToString() == "Leader")
                            {
                                leader = row[1].ToString();
                            }
                            if (row[0].ToString() == index.ToString())
                            {
                                try
                                {
                                    SmartIssue item = new SmartIssue();
                                    item.IssueTitle = row[1].ToString();
                                    item.IssueDescription = row[2].ToString();
                                    item.ProjectId = projectid;
                                    item.IssueCategoryId = issueCategoryId;
                                    item.IssueStatusId = issueStatusId;
                                    item.IssuePriorityId = issuePriorityId;
                                    item.IssueMilestoneId = issueMilestoneId;
                                    item.IssueAffectedMilestoneId = issueAffectedMilestoneId;
                                    item.IssueTypeId = issueTypeId;
                                    item.IssueResolutionId = 25;
                                    item.IssueAssignedUserName = staff;
                                    item.IssueCreatorUserName = staff;
                                    item.IssueOwnerUserName = leader;
                                    item.IssueDueDate = DateTime.Parse(row[3].ToString());
                                    item.IssueVisibility = 0;
                                    item.IssueEstimation = decimal.Parse(row[4].ToString());
                                    item.IssueProgress = 0;

                                    fileImport.Add(item);

                                    index++;
                                }
                                catch
                                {

                                }
                            }
                        }
                    }
                    return fileImport;
                }
                catch
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}