using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web;

namespace MathBot.Helpers
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
    public static class ArrayExtensions
    {
        /// <summary>
        /// Creates a copy of an object array and adds the extra elements to the created copy
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="add">Elemet to add to the created copy</param>
        /// <returns></returns>
        public static T[] CopyAddElements<T>(this T[] array, params T[] add)
        {
            for (int i = 0; i < add.Length; i++)
            {
                Array.Resize(ref array, array.Length + 1);
                array[array.Length - 1] = add[i];
            }
            return array;
        }

        public static T[] GetFromTo<T>(this T[] array, int start, int end)
        {
            List<T> _items = new List<T>();
            int length = array.Length;
            if (start < 0) return null;
            if (end < 0) return null;
            if (start > end) return null;
            if (start + end > length) return null;

            for (int i = start; i < end; i++)
            {
                _items.Add(array[i]);
            }
            return _items.ToArray();
        }
        public static T[] GetFromToEnd<T>(this T[] array, int start)
        {
            List<T> _items = new List<T>();
            int length = array.Length;
            if (start < 0) return null;
            if (start > length) return null;

            for (int i = start; i < length; i++)
            {
                _items.Add(array[i]);
            }
            return _items.ToArray();
        }
    }
}