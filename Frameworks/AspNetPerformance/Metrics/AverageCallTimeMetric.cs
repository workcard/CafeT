using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace AspNetPerformance.Metrics
{
    /// <summary>
    /// Performance Metric that updates the counters that track the average time a method took
    /// </summary>
    public class AverageCallTimeMetric : PerformanceMetricBase
    {

        public AverageCallTimeMetric(ActionInfo info)
            : base(info)
        {
            String categoryName = this.actionInfo.PerformaneCounterCategory;
            String instanceName = this.actionInfo.InstanceName;
            this.averageTimeCounter
                = this.InitializeCounter(categoryName, COUNTER_NAME, instanceName);
            this.baseCounter
                = this.InitializeCounter(categoryName, BASE_COUNTER_NAME, instanceName);
        }


        /// <summary>
        /// Constant defining the name of the average time counter
        /// </summary>
        /// <remarks>
        /// This is the counter name that will show up in perfmon
        /// </remarks>
        public const String COUNTER_NAME = "Average Time per Call";

        /// <summary>
        /// Constant defining the name of the base counter to use
        /// </summary>
        public const String BASE_COUNTER_NAME = "Average Time per Call Base";


        #region Member Variables

        private PerformanceCounter averageTimeCounter;
        private PerformanceCounter baseCounter;

        #endregion


        /// <summary>
        /// Method called by the custom action filter after the action completes
        /// </summary>
        /// <remarks>
        /// This method increments the Average Time per Call counter by the number of ticks
        /// the action took to complete and the base counter is incremented by 1 (this is
        /// done in the PerfCounterUtil.IncrementTimer() method).  
        /// </remarks>
        /// <param name="elapsedTicks">A long of the number of ticks it took to complete the action</param>
        public override void OnActionComplete(long elapsedTicks, bool exceptionThrown)
        {
            this.averageTimeCounter.IncrementBy(elapsedTicks);
            this.baseCounter.Increment();
        }

        /// <summary>
        /// Disposes of the two PerformanceCounter objects when the metric object is disposed
        /// </summary>
        public override void Dispose()
        {
            this.averageTimeCounter.Dispose();
            this.baseCounter.Dispose();
        }
    }
}
