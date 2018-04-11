using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Web.Models;

namespace SmartTracking.Repositories
{
    public class ProjectRepositories
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

        public static List<Project> GetProjectById(int projectid)
        {
            List<Project> _projects = new List<Project>();
            try
            {
                BugNetConnection();
                _cmd = new SqlCommand("BugNet_Project_GetProjectById", _con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                _cmd.CommandTimeout = 1000;
                _cmd.Parameters.Add("@ProjectId", SqlDbType.Int).Value = projectid;
                _dr = _cmd.ExecuteReader();

                while (_dr.Read())
                {
                    try
                    {
                        Project _project = new Project();
                        //_project.Id = (int)_dr["ProjectId"];
                        //_project.Name = (string)_dr["ProjectName"];
                        //_project.Code = (string)_dr["ProjectCode"];
                        //_project.Description = (string)_dr["ProjectDescription"];
                        //_project.Disabled = (bool)_dr["ProjectDisabled"];
                        //_project.ManagerUserName = (string)_dr["ManagerUserName"];
                        //_project.BugNetDateCreated = (DateTime)_dr["DateCreated"];

                        _projects.Add(_project);
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
            return _projects;
        }

        public static List<Project> GetAllProject()
        {
            List<Project> _projects = new List<Project>();
            try
            {
                BugNetConnection();
                _cmd = new SqlCommand("BugNet_Project_GetAllProjects", _con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                _cmd.CommandTimeout = 1000;
                _dr = _cmd.ExecuteReader();

                while (_dr.Read())
                {
                    try
                    {
                        Project _project = new Project();
                        //_project.Id = (int)_dr["ProjectId"];
                        //_project.Name = (string)_dr["ProjectName"];
                        //_project.Code = (string)_dr["ProjectCode"];
                        //_project.Description = (string)_dr["ProjectDescription"];
                        //_project.Disabled = (bool)_dr["ProjectDisabled"];
                        //_project.ManagerUserName = (string)_dr["ManagerUserName"];
                        //_project.BugNetDateCreated = (DateTime)_dr["DateCreated"];

                        _projects.Add(_project);
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
            return _projects;
        }

        public List<Project> GetProjectsByUserName(string userName, bool activeOnly)
        {
            List<Project> _projects = new List<Project>();
            try
            {
                BugNetConnection();
                _cmd = new SqlCommand("BugNet_Project_GetProjectsByMemberUsername", _con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                _cmd.CommandTimeout = 1000;
                _cmd.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = userName;
                _cmd.Parameters.Add("@ActiveOnly", SqlDbType.Bit).Value = activeOnly;
                _dr = _cmd.ExecuteReader();

                while (_dr.Read())
                {
                    try
                    {
                        Project _project = new Project();
                        //_project.Id = (int)_dr["ProjectId"];
                        //_project.Name = (string)_dr["ProjectName"];
                        //_project.Code = (string)_dr["ProjectCode"];
                        //_project.Description = (string)_dr["ProjectDescription"];
                        //_project.Disabled = (bool)_dr["ProjectDisabled"];
                        //_project.ManagerUserName = (string)_dr["ManagerUserName"];
                        //_project.BugNetDateCreated = (DateTime)_dr["DateCreated"];

                        _projects.Add(_project);
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
            return _projects;
        }
    }
}
