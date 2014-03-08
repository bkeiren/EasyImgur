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
            {"authorized", new MetricAuthorized()},
            {"histsize", new MetricHistorySize()},
            {"histanon", new MetricHistoryAnonymousUploads()}
        };

        public static bool GatherAndSend()
        {
            bool success = true;

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
                            metricsString += "\n\t" + c.ToString() + "\t{" + metric.Key + ": " + value.ToString() + "}";
                            ++c;
                        }
                        else
                        {
                            Log.Info("The '" + metric.Key + "' metric object returned a NULL value.");
                        }
                    }

                    Log.Info("Uploading the following metrics to the server: " + metricsString);

                    String url = "http://stats.easyimgur.bryankeiren.com/";
                    byte[] response = wc.UploadValues(url, "POST", values);
                }
                catch (System.Exception ex)
                {
                    Log.Error("Something went wrong while trying to upload statistics data. Exception: " + ex.ToString());
                    success = false;
                }

                if (success)
                {
                    Log.Info("Successfully uploaded data of " + m_StatisticsMetrics.Count.ToString() + " metrics to the server.");
                }
            }

            return success;
        }
    }
}
