using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Runtime.Caching;
using DemoApplication.Attributes;
using DemoApplication.Helpers;
using DemoApplication.Interfaces;

namespace DemoApplication.Repository
{
    public abstract class DataRepository<T> : IDataOperation<T> where T : IEntity, new()
    {
        private readonly string _dBConString;

        protected DataRepository(string conString)
        {
            _dBConString = conString;
        }

        protected DataRepository()
            : this(ConfigSetting.DBConnectionString)
        {
        }

        public IList<T> GetEntityByParams(SqlParameter[] sqlParams, string spName)
        {
            using (var con = new SqlConnection(_dBConString))
            {
                var adpt = new SqlDataAdapter(SqlCommandInstance(spName, con));
                try
                {
                    var ds = new DataSet();
                    adpt.Fill(ds);
                    return ds.Tables[0].ToList<T>(); //Creating List of current Entity
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        #region IDataOperation<T> Members

        /// <summary>
        ///     Get Entity by its GUID
        /// </summary>
        /// <param name="guid">GUID string </param>
        /// <returns></returns>
        public T GetByGUID(string guid)
        {
            //If GUID is null/Empty, throw NullReferenceException
            if (String.IsNullOrEmpty(guid))
                throw new NullReferenceException("Guid Should not be Null or Empty");

            //Getting Table Name for
            var tableName = new T().GetTableName();

            //StoredProcedure Name
            var spName = "Get" + tableName + "ByGUID";

            using (var con = new SqlConnection(_dBConString))
            {
                var adpt = new SqlDataAdapter(SqlCommandInstance(spName, con, new[] {new SqlParameter("GUID", guid)}));
                try
                {
                    var ds = new DataSet();
                    adpt.Fill(ds);
                    var objT = ds.Tables[0].ToList<T>().SingleOrDefault();
                    return objT;
                }
                catch (SqlException sqlEx)
                {
                    if (sqlEx.Errors[0].State == 62) //If Stored Procedure Not found then creat it and execute
                    {
                        if (CreateProcedure.GetByGuidStoredProcedure(tableName, con))
                            return GetByGUID(guid);
                    }
                    throw new ArgumentException("Exception: See inner exception.", sqlEx);
                }
                catch (Exception)
                {
                    // ignored
                }
                return default(T);
            }
        }

        /// <summary>
        ///     To save entity to database.
        ///     This method by default use the 'Add+{TableAttribute value}' as storedProcedureName.
        ///     It create stored procedure when not found in database.
        ///     In case of model class of un/decorate some its properties, then delete the procedure from database, other wise it
        ///     throw exception
        /// </summary>
        /// <param name="entity">Entity to be save</param>
        /// <returns>Saved intity with Identity</returns>
        /// <example>
        ///     Student obj=new Student();
        ///     new StudentRepository().Add(obj);
        /// </example>
        public object Add(T entity)
        {
            //If Entity if null throw ArgumentException
            if (entity == null)
                throw new ArgumentException(typeof (T).Name + "object Should not be Null when Saving to database");

            var spName = "Add" + entity.GetTableName(); //StoreProcedure Name

            using (var con = new SqlConnection(_dBConString))
            {
                var cmd = new SqlCommand();
                cmd.CommandText = spName;
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    //Adding SqlParameters from Entity
                    cmd.Parameters.Clear();
                    var arr = GetAddParameter(entity).ToArray();
                    cmd.Parameters.AddRange(arr);
                    con.Open();
                    var obj=cmd.ExecuteScalar();
                    cmd.Dispose();
                    return obj;
                }
                catch (SqlException sqlEx)
                {
                    if (sqlEx.Errors[0].State == 62)
                    {
                        if (CreateProcedure.CreateAddEntityProcedure(entity, con))
                            return Add(entity);
                    }
                    throw new ArgumentException(
                        "Some Error occured at database, if error in stored procedure, delete it from DB. See inner exception for more detail exception." +
                        spName, sqlEx);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public void Delete(string guid)
        {
            //If GUID is null/Empty, throw NullReferenceException
            if (String.IsNullOrEmpty(guid))
                throw new NullReferenceException("Guid Should not be Null or Empty");

            //Getting Table Name for
            var tableName = new T().GetTableName();

            //StoredProcedure Name
            var spName = "Delete" + tableName + "ByGUID";

            using (var con = new SqlConnection(_dBConString))
            {
                var cmd = SqlCommandInstance(spName, con, new[] {new SqlParameter("GUID", guid)});

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException sqlEx)
                {
                    if (sqlEx.Errors[0].State == 62) //If Stored Procedure Not found then creat it and execute
                    {
                        if (CreateProcedure.DeleteByGuidStoredProcedure(tableName, con))
                            Delete(guid);
                    }
                    throw new ArgumentException("Exception: See inner exception.", sqlEx);
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        public T Update(T entity)
        {
            var spName = "Update" + new T().GetTableName();
            using (var con = new SqlConnection(_dBConString))
            {
                var cmd = new SqlCommand
                {
                    CommandText = spName,
                    Connection = con,
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddRange(GetUpdateParameter(entity).ToArray());
                var adpt = new SqlDataAdapter(cmd);
                var ds = new DataSet();
                try
                {
                    adpt.Fill(ds);
                    var objT = ds.Tables[0].ToList<T>();
                    return objT.SingleOrDefault();
                }
                catch (SqlException sqlEx)
                {
                    if (sqlEx.Errors[0].State == 62)
                    {
                        if (CreateProcedure.CreateUpdateEntityProcedure(entity, con))
                            return Update(entity);
                    }
                    throw new ArgumentException(
                        "Class Name and Table name must be same for this method. See inner exception", sqlEx);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        /// <summary>
        ///     To get all the Entity
        ///     By Default it create a stored procedure named GetAll+{TableName}
        /// </summary>
        /// <returns>List of Entity</returns>
        public IList<T> GetAll()
        {
            //StoredProcedure Name
            var spName = "GetAll" + new T().GetTableName();

            using (var con = new SqlConnection(_dBConString))
            {
                var adpt = new SqlDataAdapter(SqlCommandInstance(spName, con));
                try
                {
                    var ds = new DataSet();
                    adpt.Fill(ds);
                    var listT = ds.Tables[0].ToList<T>(); //Creating List of current Entity
                    return listT;
                }
                catch (SqlException sqlEx)
                {
                    //Exception is for StoredProcedure not found
                    if (sqlEx.Errors[0].State == 62)
                    {
                        //Create the StoredProcedre and run once again to get all Enitity
                        if (CreateProcedure.CreateGetAllEntityProcedure(new T().GetTableName(), con))
                            return GetAll();
                    }
                    throw;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        ///     To get List of SqlParameters
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private List<SqlParameter> GetAddParameter(object obj)
        {
            var cacheKeyName = ((T)obj).GetTableName();
            PropertyInfo[] propertyInfos= MyCache.GetMyCachedItem(cacheKeyName);

            if (propertyInfos == null)
            {
                propertyInfos = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                MyCache.AddToMyCache(cacheKeyName, propertyInfos, CacheItemPriority.Default);
            }

            var sqlParams= (from f in propertyInfos let cols = f.GetCustomAttributes(typeof (DbColumnAttribute), false) 
                            where cols.Length > 0 && f.GetCustomAttributes(typeof (IdentityAttribute), false).Length == 0 
                            let p = cols[0] 
                            select new SqlParameter(((DbColumnAttribute) p).Name, f.GetValue(obj, null))).ToList();
            return sqlParams;
        }

        private List<SqlParameter> GetUpdateParameter(object obj)
        {
            var cacheKeyName = ((T)obj).GetTableName();
            var propertyInfos = MyCache.GetMyCachedItem(cacheKeyName);
            if (propertyInfos == null)
            {
                propertyInfos = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                MyCache.AddToMyCache(cacheKeyName, propertyInfos, CacheItemPriority.Default);
            }
                
            var sqlParams= (from f in propertyInfos let cols = f.GetCustomAttributes(typeof (DbColumnAttribute), false)
                    where cols.Length > 0 
                    let p = cols[0] 
                    select new SqlParameter(((DbColumnAttribute) p).Name, f.GetValue(obj, null))).ToList();
            return sqlParams;
        }

        /// <summary>
        ///     Creating Instance of SqlCommand
        /// </summary>
        /// <param name="storedProcedureName">Name of stored procedure</param>
        /// <param name="con">SqlConnection</param>
        /// <returns></returns>
        private SqlCommand SqlCommandInstance(string storedProcedureName, SqlConnection con)
        {
            var cmd = new SqlCommand
            {
                CommandText = storedProcedureName,
                Connection = con,
                CommandType = CommandType.StoredProcedure
            };
            return cmd;
        }

        private SqlCommand SqlCommandInstance(string storedProcedureName, SqlConnection con, SqlParameter[] sqlParamArr)
        {
            var cmd = new SqlCommand
            {
                CommandText = storedProcedureName,
                Connection = con,
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddRange(sqlParamArr);
            return cmd;
        }

        #endregion
    }
}