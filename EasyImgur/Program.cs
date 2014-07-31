using System;
using System.Collections.Generic;
using System.IO;
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
        static void Main(string[] args)
        {
            Guid guid = new Guid("{75da63f2-9b76-4590-82b3-b8a108e53cf0}");
            using(SingleInstance singleInstance = new SingleInstance(guid))
            {
                if(singleInstance.IsFirstInstance)
                {
                    if(!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\EasyImgur"))
                        Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\EasyImgur");

                    singleInstance.ListenForArgumentsFromSuccessiveInstances();

                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Form1 form = new Form1(singleInstance, args);
                    Properties.Settings.Default.Reload();   // To make sure we can access the current settings.
                    IntPtr handle = form.Handle; // request the handle to force it to be created; see issue #8 for more detail
                    try
                    {
                        Application.Run();
                    }
                    catch(Exception ex)
                    {
                        Log.Error("Fatal exception in main thread: " + ex.ToString());
                        throw; // crash and burn; I'm not sure it's safe to show a message box so just crash
                    }
                }
                else
                    singleInstance.PassArgumentsToFirstInstance(args);
            } 
        }
    }
}
