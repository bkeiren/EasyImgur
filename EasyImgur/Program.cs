using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace EasyImgur
{
    static class Program
    {
        static readonly Dictionary<string, Assembly> ResolvedAssemblyCache = new Dictionary<string, Assembly>();
        static bool _isInPortableMode = false;

        static public bool InPortableMode 
        { 
            get { return _isInPortableMode; } 
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            var guid = new Guid("{75da63f2-9b76-4590-82b3-b8a108e53cf0}");
            using (var singleInstance = new SingleInstance(guid))
            {
                if (singleInstance.IsFirstInstance)
                {
                    if(!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\EasyImgur"))
                        Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\EasyImgur");

                    singleInstance.ListenForArgumentsFromSuccessiveInstances();

                    AppDomain.CurrentDomain.AssemblyResolve += FindDll;

                    foreach (string arg in args.Where(s => s != null))
                    {
                        if (arg == "/portable")
                        {
                            MakeSettingsPortable(Properties.Settings.Default);
                            _isInPortableMode = true;
                            Log.Info("Started in portable mode.");
                        }
                    }

                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    var form = new Form1(singleInstance, args);
                    Properties.Settings.Default.Reload();   // To make sure we can access the current settings.

#if DEBUG           // We want VS to get the source of the exception instead of coming to this throw when debugging
                    Application.Run(); // Don't put the new form instance here m'kay
#else
                    try
                    {
                        Application.Run();
                    }
                    catch(Exception ex)
                    {
                        Log.Error("Fatal exception in main thread: " + ex.ToString());
                        throw; // crash and burn; I'm not sure it's safe to show a message box so just crash
                    }
#endif
                }
                else
                    singleInstance.PassArgumentsToFirstInstance(args);
            } 
        }

        // FindDLL technique and routine obtained from http://stackoverflow.com/a/15077288 (Accessed 02-01-2014 @ 15:37).
        private static Assembly FindDll(object sender, ResolveEventArgs args)
        {
            string keyName = new AssemblyName(args.Name).Name;

            // If DLL is loaded then don't load it again just return
            Assembly value;
            if (ResolvedAssemblyCache.TryGetValue(keyName, out value)) return value;

            string assemblyName = "EasyImgur." + keyName + ".dll";
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(assemblyName))
            {
                if (stream == null)
                {
                    Log.Warning("Couldn't resolve assembly: '" + assemblyName + "'");
                    return null;
                }
                using (var reader = new BinaryReader(stream))
                {
                    byte[] buffer = reader.ReadBytes((int)stream.Length);
                    Assembly assembly = Assembly.Load(buffer);
                    ResolvedAssemblyCache[keyName] = assembly;

                    Log.Info("Loaded assembly: '" + assemblyName + "'.");

                    return assembly;
                }
            }
        }

        // PortableSettingsProvider code obtained from http://stackoverflow.com/a/2579399 (Accessed 02-01-2014 @ 17:04).
        private static void MakeSettingsPortable(ApplicationSettingsBase settings)
        {
            var portableSettingsProvider = new PortableSettingsProvider(settings.GetType().Name + ".settings");
            settings.Providers.Add(portableSettingsProvider);
            foreach (SettingsProperty prop in settings.Properties)
                prop.Provider = portableSettingsProvider;
            settings.Reload();
        }
    }
}
