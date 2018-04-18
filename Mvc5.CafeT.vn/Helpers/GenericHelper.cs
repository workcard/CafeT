using CafeT.Objects;
using System.Linq;

namespace Mvc5.CafeT.vn.Helpers
{
    public static class GenericHelper
    {
        public static bool HasField<T>(this T t, string field)
        {
            var _fields = t.Fields<T>();
            if (_fields != null && _fields.Count() > 0)
            {
                foreach (var _field in _fields)
                {
                    if (_field.Key == field) return true;
                }
            }
            return false;
        }
    }
}