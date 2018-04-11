using System;
using System.Text;

namespace CafeT.Text
{
    public static class ExceptionHelper
    {
        public static string Output(this Exception ex)
        {
            if (ex == null) return String.Empty;

            var res = new StringBuilder();
            res.AppendFormat("Exception of type '{0}': {1}", ex.GetType().Name, ex.Message);
            res.AppendLine();

            if (!String.IsNullOrEmpty(ex.StackTrace))
            {
                res.AppendLine(ex.StackTrace);
            }

            if (ex.InnerException != null)
            {
                res.AppendLine(ex.InnerException.Output());
            }

            return res.ToString();
        }
    }
}
