using System;
using System.Collections.Generic;
using System.Text;

namespace Helper
{
    public static class DatabaseHelper
    {
        public static int GetInt(object val)
        {
            try
            {
                return Convert.ToInt32(val);
            }
            catch
            {
                return 0;
            }
        }

        public static String GetString(object val)
        {
            if (val == DBNull.Value)
            {
                return String.Empty;
            }
            else
            {
                return Convert.ToString(val);
            }
        }

        public static Boolean GetBoolean(object val)
        {
            try
            {
                return Convert.ToBoolean(val);
            }
            catch
            {
                return false;
            }
        }

        public static DateTime GetDate(object val)
        {
            try
            {
                return Convert.ToDateTime(val);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }
    }
}
