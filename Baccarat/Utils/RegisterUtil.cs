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

        public const string USER_KEY = "UserName";
        public const string PWD_KEY = "Password";

        public const string TRADE_USER_KEY = "TradeUserName";
        public const string TRADE_PWD_KEY = "TradePassword";

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
