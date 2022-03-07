using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Midas.Utils
{
    public class RegisterUtil
    {
        const string REG_PATH = "HKEY_CURRENT_USER\\MidasSoft";
        public static object LoadRegistry(string key)
        {
            return Registry.GetValue(REG_PATH, key, default);
          
        }

        public static void SaveRegistry(string key, object value)
        {
            Registry.SetValue(REG_PATH, key, value);
        }
    }
}
