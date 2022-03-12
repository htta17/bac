using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ReportApp
{
    public class RegisterUtil
    {        
        const string REG_PATH = "HKEY_CURRENT_USER\\MidasSoft";
        const string REG_KEY = "ConnString";
        public static object LoadRegistry(string key)
        {
            return Registry.GetValue(REG_PATH, key, default);
        }

        public static void SaveRegistry(string key, object value)
        {
            Registry.SetValue(REG_PATH, key, value);
        }

        public static string GetConnectionString()
        {
            return (string)LoadRegistry(REG_KEY);
        }

    }

    public class ConvertUtil
    {
        public static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }
    }
}
