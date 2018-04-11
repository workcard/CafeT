using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace AspNetPerformance.Metrics
{
    /// <summary>
    /// Performance metric to update the counter that tracks the total amount of elapsed time
    /// spent in an action since the ASP.NET worker process was last started
    /// </summary>
    public class TotalElapsedTimeMetric : PerformanceMetricBase
    {

        /// <summary>
        /// Creates a TotalElapsedTimeMetric object
        /// </summary>
        /// <param name="info">An ActionInfo object that describes the action we are tracking performance for</param>
        public TotalElapsedTimeMetric(ActionInfo info)
            : base(info)
        {
            String categoryName = this.actionInfo.PerformaneCounterCategory;
            String instanceName = this.actionInfo.InstanceName;
            this.totalElapsedTimeCounter
                = this.InitializeCounter(categoryName, COUNTER_NAME, instanceName);
        }

        /// <summary>
        /// Constant defining the name of this counter
        /// </summary>
        public const String COUNTER_NAME = "Total Elapsed Time";


        /// <summary>
        /// Reference to the performance counter we are updating
        /// </summary>
        private PerformanceCounter totalElapsedTimeCounter;


        /// <summary>
        /// Method called by the custom action filter when the action completes
        /// </summary>
        /// <remarks>
        /// This method converts the elapsed ticks to millisecons, and then adds this
        /// value to the "Total Elapsed Time" counter
        /// </remarks>
        /// <param name="elapsedTicks">A long of the number of ticks that elapsed to complete the action</param>
        public override void OnActionComplete(long elapsedTicks, bool exceptionThrown)
        {
            long milliseconds = this.ConvertTicksToMilliseconds(elapsedTicks);
            this.totalElapsedTimeCounter.IncrementBy(milliseconds);
        }


        /// <summary>
        /// Disposes of the Performance Counter when the metric object is disposed
        /// </summary>
        public override void Dispose()
        {
            this.totalElapsedTimeCounter.Dispose();
        }

        
    }
}
