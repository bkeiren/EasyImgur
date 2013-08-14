using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace EasyImgur
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
            Form1 form = new Form1();
            Properties.Settings.Default.Reload();   // To make sure we can access the current settings.
            Application.Run();
        }
    }
}
