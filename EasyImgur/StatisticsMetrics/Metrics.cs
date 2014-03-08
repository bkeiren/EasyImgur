using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyImgur.StatisticsMetrics
{
    class MetricHistorySize    : StatisticsMetric
    {
        protected override Object Gather()
        {
            return History.count;
        }
    }

    class MetricHistoryAnonymousUploads    : StatisticsMetric
    {
        protected override Object Gather()
        {
            return History.anonymousCount;
        }
    }

    class MetricAuthorized : StatisticsMetric
    {
        protected override Object Gather()
        {
            return ImgurAPI.HasBeenAuthorized();
        }
    }
}
