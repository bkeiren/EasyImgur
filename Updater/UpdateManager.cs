using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace Updater
{
    class UpdateManager
    {
        public bool CheckForUpdates(IFeed feed)
        {
            // TODO: Replace input feed with source. Source must provide stream for feed.
            string url = "http://bryankeiren.com/easyimgur/app/updates.json";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream resStream = response.GetResponseStream();

            if (feed.Parse(resStream))
            {
                return true;
            }
            return false;
        }
    }
}
