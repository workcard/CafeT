using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Reflection;
using DemoApplication.Interfaces;
using DemoApplication.Attributes;

namespace DemoApplication.Helpers
{
    public static class ExtensionHelpers
    {
       
      
        public static List<T> ToList<T>(this DataTable dataTable) where T : new()
        {
            var dataList = new List<T>();
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;
            var objFieldNames = (from PropertyInfo aProp in typeof(T).GetProperties(flags) where aProp.GetCustomAttributes(typeof(DbColumnAttribute),false).Length>0
                select new
                    {
                        ((DbColumnAttribute)aProp.GetCustomAttributes(typeof(DbColumnAttribute), false)[0]).Name,
                        Type = Nullable.GetUnderlyingType(aProp.PropertyType) ?? aProp.PropertyType
                     }).ToList();
            var dataTblFieldNames = (from DataColumn aHeader in dataTable.Columns
                select new { Name = aHeader.ColumnName, Type = aHeader.DataType }).ToList();
            var commonFields = objFieldNames.Intersect(dataTblFieldNames).ToList();
            var fro = (from PropertyInfo aProp in typeof(T).GetProperties(flags)
                where aProp.GetCustomAttributes(typeof(DbColumnAttribute), false).Length > 0
                select aProp).ToList();
            foreach (var dataRow in dataTable.AsEnumerable().ToList())
            {
                var aTSource = new T();
                var i = 0;
                foreach (var aField in commonFields)
                {
                    //here
                    var pi = aTSource.GetType().GetProperty(fro[i++].Name);
                    //PropertyInfo propertyInfos = aTSource.GetType().GetProperty(objFieldNames[i++].Name);
                    pi.SetValue(aTSource, dataRow[aField.Name] == DBNull.Value ? null : dataRow[aField.Name], null);
                }
                dataList.Add(aTSource);
            }
            return dataList;
        }

        public static void ValidateEntityAndGUID<T>(this T entity) where T : IEntity
        {
            //If Entity if null throw ArgumentException
            if (entity == null)
                throw new ArgumentException(typeof(T).Name + " entity Should not be NULL when has to be delete");

            //If GUID is null or empty throw exception
            if (String.IsNullOrEmpty(((IEntity)entity).GUID))
                throw new ArgumentException(typeof(T).Name + " entity with NULL or EMPTY GUID can't be deleted");

        }

        public static string GetTableName<T>(this T entity) where T : IEntity
        {
            //Getting TableNAmeAtrribute from entity
            var tableNameAttr = entity.GetType().GetCustomAttributes(typeof(TableNameAttribute), false);
            //If not found the TableNameAttribute raise an exception
            if (tableNameAttr.Length == 0)
                throw new ArgumentException(String.Format("{0} Class has not been mapped to Database table with 'TableNameAttribute'.", entity.GetType().Name));
            //returning Table Name
            return ((TableNameAttribute)tableNameAttr[0]).Name;
        }

        //http://stackoverflow.com/a/222640
        public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }
    }
}
