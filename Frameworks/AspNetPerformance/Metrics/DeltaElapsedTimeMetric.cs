using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;


namespace AspNetPerformance.Metrics
{
    /// <summary>
    ///  Performance metric to update the counter that tracks the number of milliseconds
    ///  that were spent in an action over the last reporting period.  The value of the 
    ///  counter is the sum of the elapsed time of all of the calls to the action in 
    ///  the given period
    /// </summary>
    public class DeltaElapsedTimeMetric : PerformanceMetricBase
    {

        public DeltaElapsedTimeMetric(ActionInfo info)
            : base(info)
        {
            String categoryName = this.actionInfo.PerformaneCounterCategory;
            String instanceName = this.actionInfo.InstanceName;
            this.deltaElapsedTimeCounter
                = this.InitializeCounter(categoryName, COUNTER_NAME, instanceName);
        }

        /// <summary>
        /// Constant defining the name of this counter
        /// </summary>
        public const String COUNTER_NAME = "Delta Elapsed Time";

        /// <summary>
        /// Reference to the performance coutner object
        /// </summary>
        private PerformanceCounter deltaElapsedTimeCounter;



        /// <summary>
        /// Method called by the custom action filter after the action completes
        /// </summary>
        /// <remarks>
        /// This method increments the "Delta Elapsed Time" counter by the number of milliseconds
        /// it took to complete the action method.  In this method, the elapsedTicks value is 
        /// converted to millisecons
        /// </remarks>
        /// <param name="elapsedTicks">A long of the ticks it took the action to complete</param>
        public override void OnActionComplete(long elapsedTicks, bool exceptionThrown)
        {
            long milliseconds = this.ConvertTicksToMilliseconds(elapsedTicks);
            this.deltaElapsedTimeCounter.IncrementBy(milliseconds);
        }


        /// <summary>
        /// Disposes of the Performance Counter when the metric object is disposed
        /// </summary>
        public override void Dispose()
        {
            this.deltaElapsedTimeCounter.Dispose();
        }
    }
}
