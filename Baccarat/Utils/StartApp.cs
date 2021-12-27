using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Midas.Utils
{
    public class StartApp
    {
        public static string GlobalConnectionString = "";
        const string REG_PATH = "HKEY_CURRENT_USER\\MidasSoft";
        const string REG_KEY = "ConnString";
        public static void LoadRegistryConnectionString()
        {
            GlobalConnectionString = (string)Registry.GetValue(REG_PATH, REG_KEY, string.Empty);
            while (GlobalConnectionString == string.Empty)
            {
                SetConnection frm = new SetConnection();
                frm.ShowDialog();
                GlobalConnectionString = (string)Registry.GetValue(REG_PATH, REG_KEY, string.Empty);
            }
        }

        public static void SaveConnection(string connection)
        {
            Registry.SetValue(REG_PATH, REG_KEY, connection);
        }
    }
}
