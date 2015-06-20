using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Updater
{
    class Program
    { 
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // Check online for a version file.
            UpdateManager mgr = new UpdateManager();
            mgr.CheckForUpdates(new JsonFeed());
        }
    }
}
