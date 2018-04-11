using SmartTracking.Models;
using SmartTracking.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SmartTracking.Repositories
{
    public class UserProfileRepositories
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

        public static List<ProfileUserViewModel> GetProjectById(int projectid, bool excludeReadonlyUsers)
        {
            List<ProfileUserViewModel> _userProfileBugNets = new List<ProfileUserViewModel>();
            try
            {
                BugNetConnection();
                _cmd = new SqlCommand("BugNet_User_GetUsersByProjectId", _con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                _cmd.CommandTimeout = 1000;
                _cmd.Parameters.Add("@ProjectId", SqlDbType.Int).Value = projectid;
                _cmd.Parameters.Add("@ExcludeReadonlyUsers", SqlDbType.Bit).Value = excludeReadonlyUsers;
                _dr = _cmd.ExecuteReader();

                while (_dr.Read())
                {
                    try
                    {
                        ProfileUserViewModel _userProfileBugNet = new ProfileUserViewModel();
                        _userProfileBugNet.BugNetUserId = (Guid)_dr["UserId"];
                        _userProfileBugNet.UserName = (string)_dr["UserName"];
                        _userProfileBugNet.FirstName = (string)_dr["FirstName"];
                        _userProfileBugNet.LastName = (string)_dr["LastName"];
                        _userProfileBugNet.DisplayName = (string)_dr["DisplayName"];
                        _userProfileBugNet.Email = (string)_dr["Email"];

                        _userProfileBugNets.Add(_userProfileBugNet);
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
            return _userProfileBugNets;
        }

        public static List<ProfileUserViewModel> GetUserProfileByUserName(string userName)
        {
            List<ProfileUserViewModel> _userProfileBugNets = new List<ProfileUserViewModel>();
            try
            {
                BugNetConnection();
                _cmd = new SqlCommand("BugNet_UserProfile_GetUserProfileByUserName", _con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                _cmd.CommandTimeout = 1000;
                _cmd.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = userName;
                _dr = _cmd.ExecuteReader();

                while (_dr.Read())
                {
                    try
                    {
                        ProfileUserViewModel _userProfileBugNet = new ProfileUserViewModel();
                        _userProfileBugNet.BugNetUserId = (Guid)_dr["UserId"];
                        //_userProfileBugNet.ApplicationId = (Guid)_dr["ApplicationId"];
                        _userProfileBugNet.UserName = (string)_dr["UserName"];
                        _userProfileBugNet.FirstName = (string)_dr["FirstName"];
                        _userProfileBugNet.LastName = (string)_dr["LastName"];
                        _userProfileBugNet.DisplayName = (string)_dr["DisplayName"];
                        //_userProfileBugNet.Email = (string)_dr["Email"];  Email trong stored proceduce chua khai bao
                        _userProfileBugNet.LastUpdate = (DateTime)_dr["LastUpdate"];

                        _userProfileBugNets.Add(_userProfileBugNet);
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
            return _userProfileBugNets;
        }
    }
}