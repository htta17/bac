using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Midas.Utils
{
    public class LogService
    {
        const string FOLDER_FORMAT = "Logs\\{0:yyyy-MM-dd}";
        public static void LogError(string text) 
        {
            var dateTimeNow = DateTime.Now;
            var folderName = string.Format(FOLDER_FORMAT, dateTimeNow);
            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }
            File.AppendAllText(folderName + "\\Error.log", DateTime.Now.ToString() + " " + text + Environment.NewLine);
        }

        public static void Log(string text)
        {
            var dateTimeNow = DateTime.Now;
            var folderName = string.Format(FOLDER_FORMAT, dateTimeNow);
            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }
            File.AppendAllText(folderName + "\\Full.log", DateTime.Now.ToString() + " " + text + Environment.NewLine);
        }


    }
}
