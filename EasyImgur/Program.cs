using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using EasyImgur.Properties;

namespace EasyImgur
{
    static class Program
    {
        static readonly Dictionary<string, Assembly> ResolvedAssemblyCache = new Dictionary<string, Assembly>();
        static bool isInPortableMode = false;

        public static bool InPortableMode => isInPortableMode;

        public static string RootFolder => !InPortableMode
            ? Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\EasyImgur"
            : AppDomain.CurrentDomain.BaseDirectory;

        /// <summary>
        /// Property to easily access the path of the executable, quoted for safety.
        /// </summary>
        public static string QuotedApplicationPath => "\"" + Application.ExecutablePath + "\"";

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
                    singleInstance.ListenForArgumentsFromSuccessiveInstances();
                    Entry(singleInstance, args);
                }
                else
                    singleInstance.PassArgumentsToFirstInstance(args);
            }
        }

        private static void Entry(SingleInstance singleInstance, string[] args)
        {
#if DEBUG
            try
            {
#endif
                using (var app = new EasyImgurApplication())
                {
                    AppDomain.CurrentDomain.AssemblyResolve += FindDll;
                    const string portableFlag = "portable";
                    if (args.Any(arg => arg == "/" + portableFlag) || File.Exists(portableFlag))
                    {
                        if (!File.Exists(portableFlag)) using (File.Open(portableFlag, FileMode.OpenOrCreate)) { }
                        isInPortableMode = true;
                    }

                    if (isInPortableMode)
                    {
                        MakeSettingsPortable(Settings.Default);
                        Log.Info("Started in portable mode.");
                    }
                    else
                    {
                        var folder = RootFolder;
                        if (!Directory.Exists(folder))
                            Directory.CreateDirectory(folder);
                    }

                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    var form = new Form1(singleInstance, args);
                    Settings.Default.Reload();

                    app.Initialize();
                    Application.Run();
                }
#if DEBUG
            }
            catch (Exception ex)
            {
                Log.Error("Fatal exception in main thread: " + ex.ToString());
                throw; // crash and burn; I'm not sure it's safe to show a message box so just crash
            }
#endif
        }

        // FindDLL technique and routine obtained from http://stackoverflow.com/a/15077288 (Accessed 02-01-2014 @ 15:37).
        private static Assembly FindDll(object sender, ResolveEventArgs args)
        {
            var keyName = new AssemblyName(args.Name).Name;

            // If DLL is loaded then don't load it again just return
            Assembly value;
            if (ResolvedAssemblyCache.TryGetValue(keyName, out value)) return value;

            var assemblyName = "EasyImgur." + keyName + ".dll";
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(assemblyName))
            {
                if (stream == null)
                {
                    Log.Warning("Couldn't resolve assembly: '" + assemblyName + "'");
                    return null;
                }
                using (var reader = new BinaryReader(stream))
                {
                    var buffer = reader.ReadBytes((int)stream.Length);
                    var assembly = Assembly.Load(buffer);
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
