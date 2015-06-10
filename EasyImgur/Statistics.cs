using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyImgur.StatisticsMetrics;
using System.Net;

namespace EasyImgur
{
    class Statistics
    {
        private static Dictionary<String, StatisticsMetric> m_StatisticsMetrics = new Dictionary<String, StatisticsMetric>()
        {
             {"authorized",  new MetricAuthorized()}                // Whether the user has authorized EasyImgur.
            ,{"histsize",    new MetricHistorySize()}               // The size of the history list.
            ,{"histanon",    new MetricHistoryAnonymousUploads()}   // The number of anonymous uploads in the history list.
            ,{"os",          new MetricOperatingSystem()}           // The operating system version.
            ,{"clrversion",  new MetricCLRVersion()}                // The Common Language Runtime version.
            ,{"langfull",    new MetricLanguageFull()}              // The current UI language's full English name.
            ,{"langiso",     new MetricLanguageISO()}               // The current UI language's 3-letter ISO code.
            ,{"portable",    new MetricPortableMode()}              // Whether the application is running in portable mode.
            ,{"id",          new MetricMachineID()}                 // The (hopefully) unique machine ID.
            ,{"version",     new MetricVersion()}                   // The version of the application.
        };

        public static bool GatherAndSend()
        {
            bool success = true;

            try
            {
                Statistics stats = new Statistics();

                String metricsString = String.Empty;
                using (WebClient wc = new WebClient())
                {
                    try
                    {
                        var values = new System.Collections.Specialized.NameValueCollection();
                        int c = 1;
                        foreach (KeyValuePair<String, StatisticsMetric> metric in m_StatisticsMetrics)
                        {
                            Object value = metric.Value.value;
                            if (value != null)
                            {
                                values.Add(metric.Key, value.ToString());
                                metricsString += "\r\n\t" + c.ToString() + "\t{" + metric.Key + ": " + value.ToString() + "}";
                                ++c;
                            }
                            else
                            {
                                Log.Info("The '" + metric.Key + "' metric object returned a NULL value.");
                            }
                        }

                        Log.Info("Uploading the following metrics to the server: " + metricsString);

#if !DEBUG
                        String url = "http://bryankeiren.com/easyimgur/stats.php";
                        byte[] response = wc.UploadValues(url, "POST", values);
                        //Log.Info("Response from stats server: \r\n" + Encoding.ASCII.GetString(response));
#else
                        Log.Info("Upload to server was not performed due to the application being a debug build.");
#endif
                    }
                    catch (System.Net.WebException ex)
                    {
                        HttpWebResponse response = (System.Net.HttpWebResponse)ex.Response;
                        Log.Error("Something went wrong while trying to upload statistics data.\r\n\tStatus code: " + response.StatusCode.ToString() + "\r\n\tException: " + ex.ToString());
                        success = false;
                    }

                    if (success)
                    {
                        Log.Info("Successfully uploaded data of " + m_StatisticsMetrics.Count.ToString() + " metrics to the server.");
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
