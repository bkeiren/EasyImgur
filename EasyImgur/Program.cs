using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace EasyImgur
{
    static class Program
    {
        static Dictionary<string, System.Reflection.Assembly> m_Libs = new Dictionary<string, System.Reflection.Assembly>();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Guid guid = new Guid("{75da63f2-9b76-4590-82b3-b8a108e53cf0}");
            using(SingleInstance singleInstance = new SingleInstance(guid))
            {
                if (singleInstance.IsFirstInstance)
                {
                    if(!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\EasyImgur"))
                        Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\EasyImgur");

                    singleInstance.ListenForArgumentsFromSuccessiveInstances();

                    AppDomain.CurrentDomain.AssemblyResolve += FindDLL;

                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Form1 form = new Form1(singleInstance, args);
                    Properties.Settings.Default.Reload();   // To make sure we can access the current settings.
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

        // FindDLL technique and routine obtained from http://stackoverflow.com/a/15077288 (Accessed 02-01-2014 @ 15:37).
        private static System.Reflection.Assembly FindDLL(object sender, ResolveEventArgs args)
        {
            string keyName = new System.Reflection.AssemblyName(args.Name).Name;

            // If DLL is loaded then don't load it again just return
            if (m_Libs.ContainsKey(keyName)) return m_Libs[keyName];

            String assembly_name = "EasyImgur." + keyName + ".dll";
            using (Stream stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(assembly_name))
            {
                byte[] buffer = new BinaryReader(stream).ReadBytes((int)stream.Length);
                System.Reflection.Assembly assembly = System.Reflection.Assembly.Load(buffer);
                m_Libs[keyName] = assembly;

                Log.Info("Loaded assembly: '" + assembly_name + "'.");

                return assembly;
            }
        }
    }
}
