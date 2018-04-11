using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CafeT.Enumerable
{
    //http://hintdesk.com/c-enum-of-string/
    public static class EnumString
    {
        public static string GetString(Enum enValue)
        {
            FieldInfo fiInfo = enValue.GetType().GetField(enValue.ToString());
            DescriptionAttribute[] daArray = fiInfo.GetCustomAttributes(typeof(DescriptionAttribute), false) 
                as DescriptionAttribute[];
            return daArray.Length > 0 ? daArray[0].Description : "";
        }

        public static object GetValue(string strDescription, Type tyEnum)
        {
            string[] arrNames = Enum.GetNames(tyEnum);
            foreach (string strTemp in arrNames)
            {
                if (GetString(Enum.Parse(tyEnum, strTemp) as Enum).Equals(strDescription)) return Enum.Parse(tyEnum, strTemp);
            }
            throw new Exception("No value found for this description");
        }
    }
}
