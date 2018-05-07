using System;
using System.Collections.Generic;

namespace CafeT.Text
{
    public static class ExtractStringHelper
    {
        #region Extracts
        public static string GetInBetween(this  string strSource, string strBegin, string strEnd, bool includeBegin, bool includeEnd)
        {
            string[] result = { string.Empty, string.Empty };
            int iIndexOfBegin = strSource.IndexOf(strBegin);

            if (iIndexOfBegin != -1)
            {
                // include the Begin string if desired 
                if (includeBegin)
                    iIndexOfBegin -= strBegin.Length;

                strSource = strSource.Substring(iIndexOfBegin + strBegin.Length);

                int iEnd = strSource.IndexOf(strEnd);
                if (iEnd != -1)
                {
                    // include the End string if desired 
                    if (includeEnd)
                        iEnd += strEnd.Length;
                    result[0] = strSource.Substring(0, iEnd);
                    // advance beyond this segment 
                    if (iEnd + strEnd.Length < strSource.Length)
                        result[1] = strSource.Substring(iEnd + strEnd.Length);
                }
            }
            else
                // stay where we are 
                result[1] = strSource;
            return result[0];
        }

        public static string[] ExtractAllBetween(this string text, string begin, string end)
        {
            List<string> _strs = new List<string>();
            string _str = text.ExtractFirstMinBetween(begin, end);

            while (!_str.IsNullOrEmptyOrWhiteSpace())
            {
                _strs.Add(_str);
                int _index = text.IndexOf(_str);

                if (_index != -1)
                {
                    text = text.Remove(_index, _str.Length);
                }
                _str = text.ExtractFirstMinBetween(begin, end);
            }
            return _strs.ToArray();
        }

        public static string Extract(this string value, string begin_text, string end_text, int occur = 1)
        {
            if (string.IsNullOrEmpty(value) == false)
            {
                // Search Begin
                int start = -1;
                // search with number of occurs
                for (int i = 1; i <= occur; i++)
                    start = value.IndexOf(begin_text, start + 1);

                if (start < 0)
                    return value;
                start += begin_text.Length;


                // Search End
                if (string.IsNullOrEmpty(end_text))
                    return value.Substring(start);
                int end = value.IndexOf(end_text, start);
                if (end < 0)
                    return value.Substring(start);

                end -= start;

                // End Final
                return value.Substring(start, end);
            }
            else
            {
                return value;
            }

        }

        public static void ToException(this string message)
        {
            throw new Exception(message);
        }
        public static string ToEmailMessage(this string text)
        {
            return string.Empty;
        }

        

        /// <summary>
        /// Tested by: Phan Minh Tai
        /// </summary>
        /// <param name="text"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static string ExtractFirstMinBetween(this string text, string a, string b)
        {
            string _input = text;
            if (_input.GetAfter(a).Contains(b))
            {
                _input = _input.GetAfter(a);
                int _last = _input.IndexOf(b);
                return a + _input.Substring(0, _last) + b;
            }
            return string.Empty;
        }

        /// <summary>
        /// Max
        /// </summary>
        /// <param name="value"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static string ExtractMaxBetween(this string value, string a, string b)
        {
            int posA = value.IndexOf(a);
            int posB = value.LastIndexOf(b);
            if (posA == -1)
            {
                return string.Empty;
            }
            if (posB == -1)
            {
                return string.Empty;
            }
            int adjustedPosA = posA + a.Length;
            if (adjustedPosA >= posB)
            {
                return string.Empty;
            }
            return value.Substring(adjustedPosA, posB - adjustedPosA);
        }
        #endregion
    }
}
