using SmartTracking.Helpers;
using SmartTracking.Models;
using SmartTracking.Models.Objects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace SmartTracking.Repositories
{
    public class IssueRepositories
    {
        private static readonly SqlConnection _con = new SqlConnection();
        private static SqlCommand _cmd;
        private static SqlDataReader _dr;

        public static void BugNetConnection()
        {
            try
            {
                _con.ConnectionString = ConfigurationManager.ConnectionStrings["BugNetConnection"].ConnectionString;
                _con.Open();
            }
            catch
            { }
        }

        public static List<Issue> GetIssuesOnDay(List<int> projectId, DateTime date)
        {
            List<Issue> _issues = new List<Issue>();

            foreach(int i in projectId)
            {
                List<Issue> _issuesOfProject = new List<Issue>();
                _issues.AddRange(GetIssuesOnDay(i, date));
            }
            return _issues;
        }

        public static List<Issue> GetIssuesOnDay(int projectId, DateTime date)
        {
            List<Issue> _issues = new List<Issue>();
            //_issues = 
            return _issues;
        }

        public static List<Issue> GetIssuesProjectIdFromDateToDate(DateTime fromDate, DateTime toDate)
        {
            string _fromDate = fromDate.ToShortDateString();
            string _toDate = toDate.ToShortDateString();

            List<Issue> _issues = new List<Issue>();
            try
            {
                BugNetConnection();
                _cmd = new SqlCommand("BugNet_Issue_GetIssuesByAssignedFromDateToDate", _con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                _cmd.CommandTimeout = 1000;
                _cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = _fromDate;
                _cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = _toDate;
                _dr = _cmd.ExecuteReader();

                while (_dr.Read())
                {
                    try
                    {
                        Issue _issue = new Issue();
                        _issue.IssueId = (int)_dr["IssueId"];
                        _issue.IssueTitle = (string)_dr["IssueTitle"];
                        _issue.IssueDescription = (string)_dr["IssueDescription"];
                        _issue.IssueDescription = StringHelpers.RemoveHTMLTags(_issue.IssueDescription);
                        _issue.StatusName = (string)_dr["StatusName"];
                        _issue.ProjectId = (int)_dr["ProjectId"];
                        _issue.ProjectName = (string)_dr["ProjectName"];
                        _issue.IssueDueDate = (DateTime)_dr["IssueDueDate"];
                        _issue.IssueEstimation = (decimal)_dr["IssueEstimation"];
                        _issue.IssueProgress = (int)_dr["IssueProgress"];
                        _issue.MilestoneName = (string)_dr["MilestoneName"];
                        _issue.AssignedUserName = (string)_dr["AssignedUserName"];
                        _issue.DateCreated = (DateTime)_dr["DateCreated"];
                        _issue.LastUpdate = (DateTime)_dr["LastUpdate"];
                        _issue.LastUpdateUserId = (Guid)_dr["LastUpdateUserId"];
                        _issue.IsClosed = (bool)_dr["IsClosed"];

                        _issues.Add(_issue);
                    }
                    catch
                    {

                    }
                }
                _dr.Close();
                _cmd.Dispose();
            }
            catch (Exception)
            {

            }
            return _issues;
        }


        public static List<Issue> GetIssuesEQDate(DateTime date)
        {
            List<Issue> _issues = new List<Issue>();
            try
            {
                BugNetConnection();
                _cmd = new SqlCommand("BugNet_Issue_GetIssuesByAssignedEQDate", _con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _cmd.CommandTimeout = 1000;
                _cmd.Parameters.Add("@DueDate", SqlDbType.DateTime).Value = date.ToShortDateString();
                _dr = _cmd.ExecuteReader();

                while (_dr.Read())
                {
                    try
                    {
                        Issue _issue = new Issue();
                        _issue.IssueId = (int)_dr["IssueId"];
                        _issue.IssueTitle = (string)_dr["IssueTitle"];
                        _issue.IssueDescription = (string)_dr["IssueDescription"];
                        _issue.IssueDescription = StringHelpers.RemoveHTMLTags(_issue.IssueDescription);
                        _issue.StatusName = (string)_dr["StatusName"];
                        _issue.ProjectId = (int)_dr["ProjectId"];
                        _issue.ProjectName = (string)_dr["ProjectName"];
                        _issue.IssueDueDate = (DateTime)_dr["IssueDueDate"];
                        _issue.IssueEstimation = (decimal)_dr["IssueEstimation"];
                        _issue.IssueProgress = (int)_dr["IssueProgress"];
                        _issue.MilestoneName = (string)_dr["MilestoneName"];
                        _issue.AssignedUserName = (string)_dr["AssignedUserName"];
                        _issue.DateCreated = (DateTime)_dr["DateCreated"];
                        _issue.LastUpdate = (DateTime)_dr["LastUpdate"];
                        _issue.LastUpdateUserId = (Guid)_dr["LastUpdateUserId"];
                        _issue.IsClosed = (bool)_dr["IsClosed"];

                        _issues.Add(_issue);
                    }
                    catch
                    {

                    }
                }
                _dr.Close();
                _cmd.Dispose();
            }
            catch (Exception)
            {

            }
            return _issues;
        }

        public static List<Issue> GetIssuesNotFinishedByDate(DateTime date)
        {
            List<Issue> _issues = new List<Issue>();
            try
            {
                BugNetConnection();
                _cmd = new SqlCommand("BugNet_Issue_GetIssuesByAssignedNotFinishedLTDate", _con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _cmd.CommandTimeout = 1000;
                _cmd.Parameters.Add("@DueDate", SqlDbType.DateTime).Value = date.ToShortDateString();
                _dr = _cmd.ExecuteReader();

                while (_dr.Read())
                {
                    try
                    {
                        Issue _issue = new Issue();
                        _issue.IssueId = (int)_dr["IssueId"];
                        _issue.IssueTitle = (string)_dr["IssueTitle"];
                        _issue.IssueDescription = (string)_dr["IssueDescription"];
                        _issue.IssueDescription = StringHelpers.RemoveHTMLTags(_issue.IssueDescription);
                        _issue.StatusName = (string)_dr["StatusName"];
                        _issue.ProjectId = (int)_dr["ProjectId"];
                        _issue.ProjectName = (string)_dr["ProjectName"];
                        _issue.IssueDueDate = (DateTime)_dr["IssueDueDate"];
                        _issue.IssueEstimation = (decimal)_dr["IssueEstimation"];
                        _issue.IssueProgress = (int)_dr["IssueProgress"];
                        _issue.MilestoneName = (string)_dr["MilestoneName"];
                        _issue.AssignedUserName = (string)_dr["AssignedUserName"];
                        _issue.DateCreated = (DateTime)_dr["DateCreated"];
                        _issue.LastUpdate = (DateTime)_dr["LastUpdate"];
                        _issue.LastUpdateUserId = (Guid)_dr["LastUpdateUserId"];
                        _issue.IsClosed = (bool)_dr["IsClosed"];

                        _issues.Add(_issue);
                    }
                    catch
                    {

                    }
                }
                _dr.Close();
                _cmd.Dispose();
            }
            catch (Exception)
            {

            }
            return _issues;
        }

        public static List<Issue> GetIssuesFutureByDate(DateTime date)
        {
            List<Issue> _issues = new List<Issue>();
            try
            {
                BugNetConnection();
                _cmd = new SqlCommand("BugNet_Issue_GetIssuesByAssignedGEDate", _con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _cmd.CommandTimeout = 1000;
                _cmd.Parameters.Add("@DueDate", SqlDbType.DateTime).Value = date.ToShortDateString();
                _dr = _cmd.ExecuteReader();

                while (_dr.Read())
                {
                    try
                    {
                        Issue _issue = new Issue();
                        _issue.IssueId = (int)_dr["IssueId"];
                        _issue.IssueTitle = (string)_dr["IssueTitle"];
                        _issue.IssueDescription = (string)_dr["IssueDescription"];
                        _issue.IssueDescription = StringHelpers.RemoveHTMLTags(_issue.IssueDescription);
                        _issue.StatusName = (string)_dr["StatusName"];
                        _issue.ProjectId = (int)_dr["ProjectId"];
                        _issue.ProjectName = (string)_dr["ProjectName"];
                        _issue.IssueDueDate = (DateTime)_dr["IssueDueDate"];
                        _issue.IssueEstimation = (decimal)_dr["IssueEstimation"];
                        _issue.IssueProgress = (int)_dr["IssueProgress"];
                        _issue.MilestoneName = (string)_dr["MilestoneName"];
                        _issue.AssignedUserName = (string)_dr["AssignedUserName"];
                        _issue.DateCreated = (DateTime)_dr["DateCreated"];
                        _issue.LastUpdate = (DateTime)_dr["LastUpdate"];
                        _issue.LastUpdateUserId = (Guid)_dr["LastUpdateUserId"];
                        _issue.IsClosed = (bool)_dr["IsClosed"];

                        _issues.Add(_issue);
                    }
                    catch
                    {

                    }
                }
                _dr.Close();
                _cmd.Dispose();
            }
            catch (Exception)
            {

            }
            return _issues;
        }

        public static List<Issue> GetIssuesByUser(string user)
        {
            List<Issue> _issues = new List<Issue>();
            try
            {
                BugNetConnection();
                _cmd = new SqlCommand("BugNet_Issue_GetIssuesByAssignedUser", _con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _cmd.CommandTimeout = 1000;
                _cmd.Parameters.Add("@UserName", SqlDbType.DateTime).Value = user;
                _dr = _cmd.ExecuteReader();

                while (_dr.Read())
                {
                    try
                    {
                        Issue _issue = new Issue();
                        _issue.IssueId = (int)_dr["IssueId"];
                        _issue.IssueTitle = (string)_dr["IssueTitle"];
                        _issue.IssueDescription = (string)_dr["IssueDescription"];
                        _issue.IssueDescription = StringHelpers.RemoveHTMLTags(_issue.IssueDescription);
                        _issue.StatusName = (string)_dr["StatusName"];
                        _issue.ProjectId = (int)_dr["ProjectId"];
                        _issue.ProjectName = (string)_dr["ProjectName"];
                        _issue.IssueDueDate = (DateTime)_dr["IssueDueDate"];
                        _issue.IssueEstimation = (decimal)_dr["IssueEstimation"];
                        _issue.IssueProgress = (int)_dr["IssueProgress"];
                        _issue.MilestoneName = (string)_dr["MilestoneName"];
                        _issue.AssignedUserName = (string)_dr["AssignedUserName"];
                        _issue.DateCreated = (DateTime)_dr["DateCreated"];
                        _issue.LastUpdate = (DateTime)_dr["LastUpdate"];
                        _issue.LastUpdateUserId = (Guid)_dr["LastUpdateUserId"];
                        _issue.IsClosed = (bool)_dr["IsClosed"];

                        _issues.Add(_issue);
                    }
                    catch
                    {

                    }
                }
                _dr.Close();
                _cmd.Dispose();
            }
            catch (Exception)
            {

            }
            return _issues;
        }

        public static List<Issue> GetIssuesByUserDate(string user, DateTime date)
        {
            List<Issue> _issues = new List<Issue>();
            try
            {
                BugNetConnection();
                _cmd = new SqlCommand("BugNet_Issue_GetIssuesByAssignedUserNameDate", _con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _cmd.CommandTimeout = 1000;
                _cmd.Parameters.Add("@UserName", SqlDbType.DateTime).Value = user;
                _cmd.Parameters.Add("@DueDate", SqlDbType.DateTime).Value = date.ToShortDateString();
                _dr = _cmd.ExecuteReader();

                while (_dr.Read())
                {
                    try
                    {
                        Issue _issue = new Issue();
                        _issue.IssueId = (int)_dr["IssueId"];
                        _issue.IssueTitle = (string)_dr["IssueTitle"];
                        _issue.IssueDescription = (string)_dr["IssueDescription"];
                        _issue.IssueDescription = StringHelpers.RemoveHTMLTags(_issue.IssueDescription);
                        _issue.StatusName = (string)_dr["StatusName"];
                        _issue.ProjectId = (int)_dr["ProjectId"];
                        _issue.ProjectName = (string)_dr["ProjectName"];
                        _issue.IssueDueDate = (DateTime)_dr["IssueDueDate"];
                        _issue.IssueEstimation = (decimal)_dr["IssueEstimation"];
                        _issue.IssueProgress = (int)_dr["IssueProgress"];
                        _issue.MilestoneName = (string)_dr["MilestoneName"];
                        _issue.AssignedUserName = (string)_dr["AssignedUserName"];
                        _issue.DateCreated = (DateTime)_dr["DateCreated"];
                        _issue.LastUpdate = (DateTime)_dr["LastUpdate"];
                        _issue.LastUpdateUserId = (Guid)_dr["LastUpdateUserId"];
                        _issue.IsClosed = (bool)_dr["IsClosed"];

                        _issues.Add(_issue);
                    }
                    catch
                    {

                    }
                }
                _dr.Close();
                _cmd.Dispose();
            }
            catch (Exception)
            {

            }
            return _issues;
        }

        public static Issue GetIssueDetails(int id)
        {
            Issue _issue = new Issue();
            try
            {
                BugNetConnection();
                _cmd = new SqlCommand("BugNet_Issue_GetIssueById", _con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                _cmd.CommandTimeout = 1000;
                _cmd.Parameters.Add("@IssueId", SqlDbType.Int).Value = id;
                _dr = _cmd.ExecuteReader();

                while (_dr.Read())
                {
                    try
                    {
                        _issue.IssueId = (int)_dr["IssueId"];
                        _issue.IssueTitle = (string)_dr["IssueTitle"];
                        _issue.IssueDescription = (string)_dr["IssueDescription"];
                        _issue.IssueDescription = StringHelpers.RemoveHTMLTags(_issue.IssueDescription);
                        _issue.StatusName = (string)_dr["StatusName"];
                        _issue.ProjectId = (int)_dr["ProjectId"];
                        _issue.ProjectName = (string)_dr["ProjectName"];
                        _issue.IssueDueDate = (DateTime)_dr["IssueDueDate"];
                        _issue.IssueEstimation = (decimal)_dr["IssueEstimation"];
                        _issue.IssueProgress = (int)_dr["IssueProgress"];
                        _issue.MilestoneName = (string)_dr["MilestoneName"];
                        _issue.AssignedUserName = (string)_dr["AssignedUserName"];
                        _issue.DateCreated = (DateTime)_dr["DateCreated"];
                        _issue.LastUpdate = (DateTime)_dr["LastUpdate"];
                        _issue.LastUpdateUserId = (Guid)_dr["LastUpdateUserId"];
                        _issue.IsClosed = (bool)_dr["IsClosed"];

                        try
                        {
                            _issue.IssueCategoryId = (int)_dr["IssueCategoryId"];
                            _issue.IssueStatusId = (int)_dr["IssueStatusId"];
                            _issue.IssuePriorityId = (int)_dr["IssuePriorityId"];
                            _issue.IssueMilestoneId = (int)_dr["IssueMilestoneId"];
                            _issue.IssueAffectedMilestoneId = (int)_dr["IssueAffectedMilestoneId"];
                            _issue.IssueTypeId = (int)_dr["IssueTypeId"];
                            _issue.IssueAssignedUserName = (string)_dr["AssignedUserName"];
                            _issue.IssueCreatorUserName = (string)_dr["CreatorUserName"];
                            _issue.IssueOwnerUserName = (string)_dr["OwnerUserName"];
                            _issue.IssueVisibility = (int)_dr["IssueVisibility"];
                            //_issue.IssueResolutionId = (int)_dr["IssueResolutionId"];
                        }
                        catch
                        {

                        }

                    }
                    catch
                    {

                    }
                }
                _dr.Close();
                _cmd.Dispose();
            }
            catch (Exception)
            {

            }
            return _issue;
        }

        public static string CreateNewIssue(List<SmartIssue> issues)
        {
            //STEP 1: READ ALL ROWS FROM DATAFILE
            //STEP 2: CREATE ISSUE FROM ROW

            int error = 0;
            int finished = 0;
            try
            {
                foreach (var item in issues)
                {
                    try
                    {
                        BugNetConnection();
                        _cmd = new SqlCommand("BugNet_Issue_CreateNewIssue", _con)
                        {
                            CommandType = CommandType.StoredProcedure
                        };
                        _cmd.CommandTimeout = 1000;
                        _cmd.Parameters.Add("@IssueTitle", SqlDbType.NVarChar).Value = item.IssueTitle;
                        _cmd.Parameters.Add("@IssueDescription", SqlDbType.NVarChar).Value = item.IssueDescription;
                        _cmd.Parameters.Add("@ProjectId", SqlDbType.Int).Value = item.ProjectId;
                        _cmd.Parameters.Add("@IssueCategoryId", SqlDbType.Int).Value = item.IssueCategoryId;
                        _cmd.Parameters.Add("@IssueStatusId", SqlDbType.Int).Value = item.IssueStatusId;
                        _cmd.Parameters.Add("@IssuePriorityId", SqlDbType.Int).Value = item.IssuePriorityId;
                        _cmd.Parameters.Add("@IssueMilestoneId", SqlDbType.Int).Value = item.IssueMilestoneId;
                        _cmd.Parameters.Add("@IssueAffectedMilestoneId", SqlDbType.Int).Value = item.IssueAffectedMilestoneId;
                        _cmd.Parameters.Add("@IssueTypeId", SqlDbType.Int).Value = item.IssueTypeId;
                        _cmd.Parameters.Add("@IssueResolutionId", SqlDbType.Int).Value = 25;
                        _cmd.Parameters.Add("@IssueAssignedUserName", SqlDbType.NVarChar).Value = item.IssueAssignedUserName;
                        _cmd.Parameters.Add("@IssueCreatorUserName", SqlDbType.NVarChar).Value = item.IssueCreatorUserName;
                        _cmd.Parameters.Add("@IssueOwnerUserName", SqlDbType.NVarChar).Value = item.IssueOwnerUserName;
                        _cmd.Parameters.Add("@IssueDueDate", SqlDbType.DateTime).Value = item.IssueDueDate;
                        _cmd.Parameters.Add("@IssueVisibility", SqlDbType.Decimal).Value = item.IssueVisibility;
                        _cmd.Parameters.Add("@IssueEstimation", SqlDbType.Decimal).Value = item.IssueEstimation;
                        _cmd.Parameters.Add("@IssueProgress", SqlDbType.Int).Value = item.IssueProgress;

                        _dr = _cmd.ExecuteReader();
                        _dr.Close();
                        _cmd.Dispose();
                        finished++;
                    }
                    catch
                    {
                        error++;
                    }
                }
            }
            catch (Exception)
            {
                return "error all";
            }
            return "finished: " + finished + "; error: " + error;
        }

        public static string CreateIssues(List<NewIssue> issues)
        {
            int error = 0;
            int finished = 0;
            try
            {
                foreach (var item in issues)
                {
                    try
                    {
                        BugNetConnection();
                        _cmd = new SqlCommand("BugNet_Issue_CreateNewIssue", _con)
                        {
                            CommandType = CommandType.StoredProcedure
                        };
                        _cmd.CommandTimeout = 1000;
                        _cmd.Parameters.Add("@IssueTitle", SqlDbType.NVarChar).Value = item.IssueTitle;
                        _cmd.Parameters.Add("@IssueDescription", SqlDbType.NVarChar).Value = item.IssueDescription;
                        _cmd.Parameters.Add("@ProjectId", SqlDbType.Int).Value = item.ProjectId;
                        _cmd.Parameters.Add("@IssueCategoryId", SqlDbType.Int).Value = item.IssueCategoryId;
                        _cmd.Parameters.Add("@IssueStatusId", SqlDbType.Int).Value = item.IssueStatusId;
                        _cmd.Parameters.Add("@IssuePriorityId", SqlDbType.Int).Value = item.IssuePriorityId;
                        _cmd.Parameters.Add("@IssueMilestoneId", SqlDbType.Int).Value = item.IssueMilestoneId;
                        _cmd.Parameters.Add("@IssueAffectedMilestoneId", SqlDbType.Int).Value = item.IssueAffectedMilestoneId;
                        _cmd.Parameters.Add("@IssueTypeId", SqlDbType.Int).Value = item.IssueTypeId;
                        _cmd.Parameters.Add("@IssueResolutionId", SqlDbType.Int).Value = 25;
                        _cmd.Parameters.Add("@IssueAssignedUserName", SqlDbType.NVarChar).Value = item.IssueAssignedUserName;
                        _cmd.Parameters.Add("@IssueCreatorUserName", SqlDbType.NVarChar).Value = item.IssueCreatorUserName;
                        _cmd.Parameters.Add("@IssueOwnerUserName", SqlDbType.NVarChar).Value = item.IssueOwnerUserName;
                        _cmd.Parameters.Add("@IssueDueDate", SqlDbType.DateTime).Value = item.IssueDueDate;
                        _cmd.Parameters.Add("@IssueVisibility", SqlDbType.Decimal).Value = item.IssueVisibility;
                        _cmd.Parameters.Add("@IssueEstimation", SqlDbType.Decimal).Value = item.IssueEstimation;
                        _cmd.Parameters.Add("@IssueProgress", SqlDbType.Int).Value = item.IssueProgress;

                        _dr = _cmd.ExecuteReader();
                        _dr.Close();
                        _cmd.Dispose();
                        finished++;
                    }
                    catch
                    {
                        error++;
                    }
                }
            }
            catch (Exception)
            {
                return "error all";
            }
            return "finished: " + finished + "; error: " + error;
        }
    }
}