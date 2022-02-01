using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Midas;
using Midas.Baccarat;
using Midas.Utils;

namespace Baccarat
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            StartApp.LoadRegistryConnectionString();
            Application.Run(new BaccaratQuad2());
        }
    }
}
