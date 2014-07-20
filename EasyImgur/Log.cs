using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyImgur
{
    public static class Log
    {
        private static System.Object ThreadLock = new System.Object();
        private static string LogFile = "log.log";
        private static bool FirstInvocation = true;

        public static string Info( string _Message )
        {
            return LogMessage("INFO", _Message);
        }

        public static string Warning( string _Message )
        {
            return LogMessage("WARNING", _Message);
        }

        public static string Error( string _Message )
        {
            return LogMessage("ERROR", _Message);
        }

        private static string LogMessage( string _Prefix, string _Message )
        {
            string TimeStamp = System.DateTime.Now.ToString("[yyyy/MM/dd HH:mm:ss:ffff]", System.Globalization.CultureInfo.InvariantCulture);
            string line = TimeStamp + " [" + _Prefix + "] " + _Message;
            lock (ThreadLock)
            {
                if (FirstInvocation)
                {
                    FirstInvocation = false;
                    try
                    {
                        System.IO.File.WriteAllText(LogFile, string.Empty);
                    }
                    catch (System.IO.FileNotFoundException ex)
                    {
                        Log.Error("Failed to save log file '" + LogFile + "': " + ex.Message);
                    }
                    catch (System.Exception ex)
                    {
                        Log.Error("Failed to save log file: " + ex.Message);
                    }
                }
                try
                {
                    using (System.IO.StreamWriter w = System.IO.File.AppendText(LogFile))
                    {
                        w.WriteLine(line);
                    }
                    Console.WriteLine(line);
                }
                catch (System.Security.SecurityException ex)
                {
                    Log.Error("A security exception occurred while trying to append to the history file: " + ex.Message);
                }
                catch (System.Exception ex)
                {
                    Log.Error("Failed to append to the log file: " + ex.Message);
                }
            }
            return line;
        }
    }
}
