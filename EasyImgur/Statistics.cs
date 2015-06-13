using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using EasyImgur.StatisticsMetrics;
using System.Net;

namespace EasyImgur
{
    static class Statistics
    {
        private const string StatisticsServerUrl = "http://bryankeiren.com/easyimgur/stats.php";
        private static readonly Dictionary<String, StatisticsMetric> StatisticsMetrics = new Dictionary<String, StatisticsMetric>()
        {
            {"authorized",  new MetricAuthorized()},                // Whether the user has authorized EasyImgur.
            {"histsize",    new MetricHistorySize()},               // The size of the history list.
            {"histanon",    new MetricHistoryAnonymousUploads()},   // The number of anonymous uploads in the history list.
            {"os",          new MetricOperatingSystem()},           // The operating system version.
            {"clrversion",  new MetricCLRVersion()},                // The Common Language Runtime version.
            {"langfull",    new MetricLanguageFull()},              // The current UI language's full English name.
            {"langiso",     new MetricLanguageISO()},               // The current UI language's 3-letter ISO code.
            {"portable",    new MetricPortableMode()},              // Whether the application is running in portable mode.
            {"id",          new MetricMachineId()},                 // The (hopefully) unique machine ID.
            {"version",     new MetricVersion()},                   // The version of the application.
        };

        public static bool GatherAndSend()
        {
            bool success = true;

            try
            {
                using (var wc = new WebClient())
                {
                    try
                    {
                        int count = 1;
                        var sb = new StringBuilder();
                        var values = new NameValueCollection();

                        foreach (KeyValuePair<String, StatisticsMetric> metric in StatisticsMetrics)
                        {
                            object value = metric.Value.Value;
                            if (value != null)
                            {
                                values.Add(metric.Key, value.ToString());
                                sb.Append("\r\n\t");
                                sb.Append(count);
                                sb.Append("\t{");
                                sb.Append(metric.Key);
                                sb.Append(": ");
                                sb.Append(value);
                                sb.Append('}');
                                ++count;
                            }
                            else
                            {
                                Log.Info("The '" + metric.Key + "' metric object returned a NULL value.");
                            }
                        }

                        Log.Info("Uploading the following metrics to the server: " + sb);

#if !DEBUG
                        wc.UploadValues(StatisticsServerUrl, "POST", values);
                        //Log.Info("Response from stats server: \r\n" + Encoding.ASCII.GetString(response));
#else
                        Log.Info("Upload to server was not performed due to the application being a debug build.");
#endif
                    }
                    catch (WebException ex)
                    {
                        var response = (HttpWebResponse)ex.Response;
                        Log.Error("Something went wrong while trying to upload statistics data.\r\n\tStatus code: " + response.StatusCode + "\r\n\tException: " + ex);
                        success = false;
                    }

                    if (success)
                    {
                        Log.Info("Successfully uploaded data of " + StatisticsMetrics.Count + " metrics to the server.");
                    }
                }
            }
            catch   // Catch anything that was thrown in here and ignore it because we don't want statistics to mess with the rest of the application functionality.
            {
                // Do nothing.
            }

            return success;
        }
    }
}
