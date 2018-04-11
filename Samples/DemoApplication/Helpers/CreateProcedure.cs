using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using DemoApplication.Interfaces;
using DemoApplication.Attributes;

namespace DemoApplication.Helpers
{
    public class CreateProcedure
    {
        #region Public Method
        /// <summary>
        /// To Building the StoredProcedure text and storing it in database
        /// </summary>
        /// <param name="name">Table name</param>
        /// <param name="con">SqlConnection for creating StoredProcedure</param>
        /// <returns>True/False</returns>
        public static bool CreateGetAllEntityProcedure(string name, SqlConnection con)
        {
            //name of Stored Procedure 
            var storedProcedureName = "GetAll" + name;

            //Creating StoredProcedure
            #region Building StoredProcedure
            var spCreate = @"            
-- =============================================
-- Author:		Inheritance App
-- Create date: {0}
-- Description:	Getting all detail of {1}
-- =============================================
CREATE PROCEDURE {2}
AS 
BEGIN

    SELECT * from {3}
END";
            string detailsSp = String.Format(spCreate, DateTime.Now, name, storedProcedureName, name);
            #endregion

            return StoringProcedure(detailsSp, con);
        }

        /// <summary>
        /// Create Procedure for Get Entity by GUID
        /// </summary>
        /// <param name="name"></param>
        /// <param name="con"></param>
        /// <returns></returns>
        public static bool GetByGuidStoredProcedure(string name, SqlConnection con)
        {
            //Creating StoredProcedure
            #region Building StoredProcedure
            var spCreate = @"            
-- =============================================
-- Author:		Inheritance App
-- Create date: {0}
-- Description:	Get {1} by Guid
-- =============================================
CREATE PROCEDURE Get{2}ByGUID 
	@GUID varchar(50)=''
AS
BEGIN

	select * from {3} where GUID=@GUID	

END";
            string detailsSp = String.Format(spCreate, DateTime.Now, name, name, name);
            #endregion

            //Creating StoredProcedure
            return StoringProcedure(detailsSp, con);
        }
        public static bool DeleteByGuidStoredProcedure(string name, SqlConnection con)
        {
            #region Building StoredProcedure
            var spCreate = @"            
-- =============================================
-- Author:		Inheritance App
-- Create date: {0}
-- Description:	Delete {1} by Guid
-- =============================================
CREATE PROCEDURE Delete{2}ByGUID 
	@GUID varchar(50)=''
AS
BEGIN

	DELETE  FROM {3} WHERE GUID=@GUID	

END";
            string detailsSp = String.Format(spCreate, DateTime.Now, name, name, name);
            #endregion
            //Creating StoredProcedure
            return StoringProcedure(detailsSp, con);
        }


        public static bool CreateAddEntityProcedure(object entity, SqlConnection con)
        {

            string tableColumnName = "", tableColNameForValue = "", procedureParameter = "", spname = "";
            string tableName = GetTableName(entity);

            #region Building procedure
            procedureParameter = CreateParameter(entity, out tableColumnName, out tableColNameForValue);

            #region
            spname = "Add" + tableName;
            var spCreate = @"            
-- =============================================
-- Author:		Inheritance App
-- Create date: {0}
-- Description:	CREATING New Row in {1}
-- =============================================
CREATE PROCEDURE {2}
{3}
AS 
BEGIN

    INSERT INTO {4}
    ({5})
    values
    ({6})

    SELECT SCOPE_IDENTITY()
END";
            #endregion

            #endregion

            string addProcedureDetails = String.Format(spCreate, DateTime.Now,
                tableName, spname, procedureParameter, tableName, tableColumnName, tableColNameForValue);

            //if (con.State == ConnectionState.Closed)
            //    con.Open();
            //var dtCols = con.GetSchema("Columns", new[] { con.Database, null, entity.GetType().Name });

            return StoringProcedure(addProcedureDetails, con);
        }

        public static bool CreateUpdateEntityProcedure(object entity, SqlConnection con)
        {
            #region
            var storeProcedureTemplate = @"
-- =============================================
-- Author:		Inheritance App
-- Create date: {0}
-- Description:	update Existing row in {1}
-- =============================================
CREATE PROCEDURE {2}
	{3}
AS
BEGIN
		UPDATE {4}
			SET	
		{5}
		Where
		{6}
--Selecting this Row
		SELECT * FROM {7} WHERE {8}
END
";
            #endregion

            string storedProcuderName = "", procedureParams = "", setParameters = "", whereCondition = "", tableName = "";

            tableName = GetTableName(entity);

            storedProcuderName = "Update" + tableName;

            procedureParams = GetParametersForUpdate(entity, out setParameters, out whereCondition);
            string stroredProcedure = String.Format(storeProcedureTemplate, DateTime.Now, tableName, storedProcuderName,
                procedureParams, tableName, setParameters, whereCondition, tableName,whereCondition);
            return StoringProcedure(stroredProcedure, con);
        }
        #endregion

        #region Private Methods

        private static string CreateParameter(object entity, out string tableColName, out string tableValueParam)
        {
            if (!(entity is IEntity))
            {
                throw new ArgumentException("Invalid Entity to create Procedure");
            }

            var parameter = new StringBuilder();
            var TableColumnName = new StringBuilder();
            var paramForValue = new StringBuilder();
            //Getting the type of entity
            var type = entity.GetType();

            // Get the PropertyInfo object:
            var properties = type.GetProperties();

            foreach (var property in properties)
            {
                if (property.GetCustomAttributes(typeof(IdentityAttribute), false).Length > 0)
                    continue;

                var attributes = property.GetCustomAttributes(typeof(DbColumnAttribute), false);
                foreach (var attribute in attributes.Where(attribute => attribute.GetType() == typeof(DbColumnAttribute)))
                {
                    switch (property.PropertyType.FullName)
                    {
                        case "System.Int32":
                            TableColumnName.Append(String.Format("{0},", ((DbColumnAttribute)attribute).Name));
                            paramForValue.Append(String.Format("@{0},", ((DbColumnAttribute)attribute).Name));
                            parameter.AppendLine();
                            parameter.Append(String.Format("@{0} int=0,", ((DbColumnAttribute)attribute).Name));
                            break;
                        case "System.Int64":
                            TableColumnName.Append(String.Format("{0},", ((DbColumnAttribute)attribute).Name));
                            paramForValue.Append(String.Format("@{0},", ((DbColumnAttribute)attribute).Name));
                            parameter.AppendLine();
                            parameter.Append(String.Format("@{0} bigint=0,", ((DbColumnAttribute)attribute).Name));
                            break;
                        case "System.DateTime":
                            TableColumnName.Append(String.Format("{0},", ((DbColumnAttribute)attribute).Name));
                            paramForValue.Append(String.Format("@{0},", ((DbColumnAttribute)attribute).Name));
                            parameter.AppendLine();
                            parameter.Append(String.Format("@{0} datetime=1/1/0001,", ((DbColumnAttribute)attribute).Name));
                            break;
                        default:
                            TableColumnName.Append(String.Format("{0},", ((DbColumnAttribute)attribute).Name));
                            paramForValue.Append(String.Format("@{0},", ((DbColumnAttribute)attribute).Name));
                            parameter.AppendLine();
                            parameter.Append(String.Format("@{0} varchar(max)='',", ((DbColumnAttribute)attribute).Name));
                            break;

                    }
                }
            }
            GetTableName(entity);
            var allParams = parameter.ToString().Substring(0, parameter.ToString().LastIndexOf(","));
            tableColName = TableColumnName.ToString().Substring(0, TableColumnName.ToString().LastIndexOf(","));
            tableValueParam = paramForValue.ToString().Substring(0, paramForValue.ToString().LastIndexOf(","));
            return allParams;
        }

        private static string GetParametersForUpdate(object entity,out string setParameters,out string whereCondition)
        {
            if (!(entity is IEntity))
            {
                throw new ArgumentException("Invalid Entity to create Procedure");
            }
            StringBuilder procParams = new StringBuilder();
            StringBuilder procSetParams = new StringBuilder();
            StringBuilder procWhereCondition = new StringBuilder();

            //Getting the type of entity
            var type = entity.GetType();

            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                object[] attr1 = null, attr2 = null;
                attr1 = property.GetCustomAttributes(typeof(IdentityAttribute), false);
                attr2 = property.GetCustomAttributes(typeof(DbColumnAttribute), false);
                if (attr1.Length > 0 && attr2.Length > 0)
                {
                    procWhereCondition.Append(String.Format("{0}=@{1}", ((DbColumnAttribute)attr2.First()).Name, ((DbColumnAttribute)attr2.First()).Name));
                    procParams.Append(String.Format("@{0} int=0,", ((DbColumnAttribute)attr2.First()).Name, ((DbColumnAttribute)attr2.First()).Name));
                    procParams.AppendLine();
                    continue;
                }
                
                if (attr2.Length > 0)
                {
                    switch (property.PropertyType.FullName)
                    {
                        case "System.Int32":
                            procSetParams.Append(String.Format("{0}=@{1},", ((DbColumnAttribute)attr2.First()).Name, ((DbColumnAttribute)attr2.First()).Name));
                            procParams.AppendLine();
                            procParams.Append(String.Format("@{0} int=0,", ((DbColumnAttribute)attr2.First()).Name));
                            break;
                        case "System.Int64":
                            procSetParams.Append(String.Format("{0}=@{1},", ((DbColumnAttribute)attr2.First()).Name, ((DbColumnAttribute)attr2.First()).Name));
                            procParams.AppendLine();
                            procParams.Append(String.Format("@{0} bigint=0,", ((DbColumnAttribute)attr2.First()).Name));
                            break;
                        case "System.DateTime":
                            procSetParams.Append(String.Format("{0}=@{1},", ((DbColumnAttribute)attr2.First()).Name, ((DbColumnAttribute)attr2.First()).Name));
                            procParams.AppendLine();
                            procParams.Append(String.Format("@{0} datetime=1/1/0001,", ((DbColumnAttribute)attr2.First()).Name));
                            break;
                        default:
                            procSetParams.Append(String.Format("{0}=@{1},", ((DbColumnAttribute)attr2.First()).Name, ((DbColumnAttribute)attr2.First()).Name));
                            procParams.AppendLine();
                            procParams.Append(String.Format("@{0} varchar(max)='',", ((DbColumnAttribute)attr2.First()).Name));
                            break;
                    }
                }
            }
            setParameters = "";
            setParameters = procSetParams.ToString().Substring(0, procSetParams.ToString().LastIndexOf(","));
            whereCondition = procWhereCondition.ToString();

            return procParams.ToString().Substring(0, procParams.ToString().LastIndexOf(","));
        }

        private static bool StoringProcedure(string storedProcedure, SqlConnection con)
        {
            SqlCommand cmd = new SqlCommand {CommandText = storedProcedure, Connection = con};


            if (con.State == ConnectionState.Closed)
                con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        private static string GetTableName(object entity)
        {
            //Getting TableNAmeAtrribute from entity
            var tableNameAttr = entity.GetType().GetCustomAttributes(typeof(TableNameAttribute), false);

            //If not found the TableNameAttribute raise an exception
            if (tableNameAttr.Length == 0)
                throw new ArgumentException(String.Format("Class {0} has not been mapped to Database table with 'TableNameAttribute'.", entity.GetType().Name));

            //returning Table Name
            return ((TableNameAttribute)tableNameAttr[0]).Name;
        }
        #endregion


    }
}
