using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoApplication
{
    public static class ConfigSetting
    {
        #region Constants
        /// <summary>
        /// Database connection string.
        /// </summary>
        private const string DB_CONNECTION_STRING = @"Data Source=(LocalDB)\v11.0;Initial Catalog=DemoReflection;Integrated Security=True;Pooling=False";
        #endregion
        public static string DBConnectionString
        {
            get
            {
                return DB_CONNECTION_STRING;
            }
        }
    }
}
