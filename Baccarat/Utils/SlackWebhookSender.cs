using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Midas.Utils
{    
    public static class SlackWebHookSender 
    {
        //Incomming WebHook App
        const string SlackWebHookUrl = "https://hooks.slack.com/services/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";
        
        //Notify App - Not using for now
        //const string SlackWebHookUrl = "https://hooks.slack.com/services/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";
        public static bool SendMessage(string message, string channel , string userName = "MidasSoft Auto Notifier", string icon = ":interrobang:")
        {
            var body = new
            {
                text = message,
                channel,
                username = userName,
                icon_emoji = icon
            };            

            try
            {
                var request = (HttpWebRequest)WebRequest.Create(SlackWebHookUrl);
                var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(body));
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();

                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                return responseString.Equals("ok", StringComparison.OrdinalIgnoreCase);
            }
            catch 
            {                
                return false;
            }
        }
    }
}
