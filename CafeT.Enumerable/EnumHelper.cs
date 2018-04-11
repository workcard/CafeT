using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CafeT.Enumerable
{
    public static class EnumHelper
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> coll)
        {
            var c = new ObservableCollection<T>();
            foreach (var e in coll)
                c.Add(e);
            return c;
        }

        public static List<T> EnumToList<T>()
        {
            Type enumType = typeof(T);

            // Can't use type constraints on value types, so have to do check like this
            if (enumType.BaseType != typeof(Enum))
                throw new ArgumentException("T must be of type System.Enum");

            Array enumValArray = Enum.GetValues(enumType);

            List<T> enumValList = new List<T>(enumValArray.Length);

            foreach (int val in enumValArray)
            {
                enumValList.Add((T)Enum.Parse(enumType, val.ToString()));
            }

            return enumValList;
        }
        
        /// <summary>
        /// https://www.codeproject.com/Tips/219870/Few-extension-methods-of-IEnumerable-in-Csharp
        /// </summary>
        /// <param name="myList"></param>
        /// <param name="itemToMatch"></param>
        /// <returns></returns>
        public static IEnumerable<string> IfMatchWith(this IEnumerable<string> myList, string itemToMatch)
        {
            foreach (var item in myList.Where(item => item == itemToMatch))
                yield return item;
        }
        public static IEnumerable<string> IfNotMatchWith(this IEnumerable<string> myList, string itemToMatch)
        {
            foreach (var item in myList.Where(item => item != itemToMatch))
                yield return item;
        }
        public static IEnumerable<string> IgnoreNullOrEmptyOrSpace(this IEnumerable<string> myList)
        {
            foreach (var item in myList.Where(item => !string.IsNullOrEmpty(item) && item != " "))
                yield return item;
        }
        public static IEnumerable<string> MakeAllUpper(this IEnumerable<string> myList)
        {
            foreach (var item in myList)
                yield return item.ToUpper();
        }
        public static IEnumerable<string> MakeAllLower(this IEnumerable<string> myList)
        {
            foreach (var item in myList)
                yield return item.ToLower();
        }
        public static IEnumerable<T> MakeAllDefault<T>(this IEnumerable<T> myList)
        {
            foreach (var item in myList)
                yield return default(T);
        }
        public static IEnumerable<string> IfMatchWithPattern(this IEnumerable<string> myList, string pattern)
        {
            foreach (var item in myList.Where(item => Regex.IsMatch(item, pattern)))
                yield return item;
        }
        public static IEnumerable<string> IfLengthEquals(this IEnumerable<string> myList, int itemLength)
        {
            foreach (var item in myList.Where(item => item.Length == itemLength))
                yield return item;
        }
        public static IEnumerable<string> IfLengthInRange(this IEnumerable<string> myList, int startOfRange, int endOfRange)
        {
            foreach (var item in myList.Where(item => item.Length >= startOfRange && item.Length <= endOfRange))
                yield return item;
        }

    }
}
