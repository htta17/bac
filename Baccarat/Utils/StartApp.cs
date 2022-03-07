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
        const string REG_KEY = "ConnString";
        public static void LoadRegistryConnectionString()
        {
            GlobalConnectionString = (string)RegisterUtil.LoadRegistry(REG_KEY);
            while (string.IsNullOrEmpty(GlobalConnectionString))
            {
                SetConnection frm = new SetConnection();
                frm.ShowDialog();
                GlobalConnectionString = (string)RegisterUtil.LoadRegistry(REG_KEY);
            }
        }

        public static void SaveConnection(string connection)
        {
            RegisterUtil.SaveRegistry(REG_KEY, connection);
        }
    }
}
