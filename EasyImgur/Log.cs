using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyImgur
{
    public static class Log
    {
        private static System.Object ThreadLock = new System.Object();

        private static string SaveLocation
        {
            get
            {
                // In non-portable mode we want to save in AppData, otherwise the local folder.
                if (!Program.InPortableMode)
                    return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\EasyImgur\\";
                else
                    return AppDomain.CurrentDomain.BaseDirectory;
            }
        }

        private static string LogFile { get { return SaveLocation + "log.log"; } }
        private static bool FirstInvocation = true;

        public static string Info( string _Message, bool _OnlyConsole = false )
        {
            return LogMessage("INFO", _Message, _OnlyConsole);
        }

        public static string Warning( string _Message, bool _OnlyConsole = false )
        {
            return LogMessage("WARNING", _Message, _OnlyConsole);
        }

        public static string Error( string _Message, bool _OnlyConsole = false )
        {
            return LogMessage("ERROR", _Message, _OnlyConsole);
        }

        private static string LogMessage( string _Prefix, string _Message, bool _OnlyConsole )
        {
            string TimeStamp = System.DateTime.Now.ToString("[yyyy/MM/dd HH:mm:ss:ffff]", System.Globalization.CultureInfo.InvariantCulture);
            string line = TimeStamp + " [" + _Prefix + "] " + _Message;
            lock (ThreadLock)
            {
                if (FirstInvocation && !_OnlyConsole)
                {
                    FirstInvocation = false;
                    try
                    {
                        System.IO.File.WriteAllText(LogFile, string.Empty);
                    }
                    catch (System.IO.FileNotFoundException ex)
                    {
                        Log.Error("Failed to save log file '" + LogFile + "': " + ex.Message, true);
                    }
                    catch (System.Exception ex)
                    {
                        Log.Error("Failed to save log file: " + ex.Message, true);
                    }
                }
                try
                {
                    if (!_OnlyConsole)
                    {
                        using (System.IO.StreamWriter w = System.IO.File.AppendText(LogFile))
                        {
                            w.WriteLine(line);
                        }
                    }
                    Console.WriteLine(line);
                }
                catch (System.Security.SecurityException ex)
                {
                    Log.Error("A security exception occurred while trying to append to the history file: " + ex.Message, true);
                }
                catch (System.Exception ex)
                {
                    Log.Error("Failed to append to the log file: " + ex.Message, true);
                }
            }
            return line;
        }
    }
}
