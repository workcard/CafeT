using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web;

namespace TiTiBot.Models
{
    public static class Extensions
    {
        public static DbSet GetDbSetWithName(this DbContext context, string name)
        {
            var type = Assembly.GetExecutingAssembly()
                    .GetTypes()
                    .FirstOrDefault(t => t.Name == name);

            if (type != null)
            {
                DbSet catContext = context.Set(type);
                return catContext;
            }
            return null;
        }

        public static List<PropertyInfo> GetDbSetProperties(this DbContext context)
        {
            var dbSetProperties = new List<PropertyInfo>();
            var properties = context.GetType().GetProperties();

            foreach (var property in properties)
            {
                var setType = property.PropertyType;
                var isDbSet = setType.IsGenericType && (typeof(IDbSet<>).IsAssignableFrom(setType.GetGenericTypeDefinition())
                    || setType.GetInterface(typeof(IDbSet<>).FullName) != null);
                //#if EF5 || EF6
                //    var isDbSet = setType.IsGenericType && (typeof (IDbSet<>).IsAssignableFrom(setType.GetGenericTypeDefinition()) 
                //    || setType.GetInterface(typeof (IDbSet<>).FullName) != null);
                //#elif EF7
                //    var isDbSet = setType.IsGenericType && (typeof (DbSet<>).IsAssignableFrom(setType.GetGenericTypeDefinition()));
                //#endif
                if (isDbSet)
                {
                    dbSetProperties.Add(property);
                }
            }
            return dbSetProperties;
        }
    }
}