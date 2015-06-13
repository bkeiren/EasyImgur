using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyImgur.StatisticsMetrics
{
    abstract class StatisticsMetric
    {
        public object Value
        {
            get { return Gather(); }
        }

        // Should be implemented by deriving classes in order to obtain a value that 
        // is desired to be measured/gathered.
        protected abstract object Gather();
    }
}
