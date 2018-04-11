using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;


namespace AspNetPerformance.Metrics
{

    /// <summary>
    /// Performance metric to update the counter that tracks the total number 
    /// of times an action has been called since the ASP.NET process last started
    /// </summary>
    public class TotalCallsMetric : PerformanceMetricBase
    {

        public TotalCallsMetric(ActionInfo info)
            : base(info)
        {
            String categoryName = this.actionInfo.PerformaneCounterCategory;
            String instanceName = this.actionInfo.InstanceName;
            this.totalCallsCounter 
                = this.InitializeCounter(categoryName, COUNTER_NAME, instanceName);
        }

        /// <summary>
        /// Constant defining the name of this counter
        /// </summary>
        public const String COUNTER_NAME = "Total Calls";


        /// <summary>
        /// Member variables of the counter to 
        /// </summary>
        private PerformanceCounter totalCallsCounter;


        /// <summary>
        /// Method called by the custom action filter after the action completes
        /// </summary>
        /// <remarks>
        /// This method increments the "Total Calls" counter by 1.  It does not use the
        /// elapsedTicks that is passed in
        /// </remarks>
        /// <param name="elapsedTicks">A long of the ticks it took the action to complete (not used)</param>
        public override void OnActionComplete(long elapsedTicks, bool exceptionThrown)
        {
            totalCallsCounter.Increment();
        }


        /// <summary>
        /// Disposes of the Performance Counter when the metric object is disposed
        /// </summary>
        public override void Dispose()
        {
            this.totalCallsCounter.Dispose();
        }

    }
}
