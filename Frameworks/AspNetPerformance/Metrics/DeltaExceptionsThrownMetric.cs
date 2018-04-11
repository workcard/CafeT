using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace AspNetPerformance.Metrics
{
    /// <summary>
    /// Performance Metric to track the count of exceptions thrown by a controller action in the last
    /// time period
    /// </summary>
    public class DeltaExceptionsThrownMetric : PerformanceMetricBase
    {
        public DeltaExceptionsThrownMetric(ActionInfo info)
            : base(info)
        {
            String categoryName = this.actionInfo.PerformaneCounterCategory;
            String instanceName = this.actionInfo.InstanceName;
            this.deltaExceptionsThrownCounter
                = this.InitializeCounter(categoryName, COUNTER_NAME, instanceName);
        }


        /// <summary>
        /// Constant defining the name of this counter
        /// </summary>
        public const String COUNTER_NAME = "Delta Exceptions Thrown";


        /// <summary>
        /// Reference to the performance counter 
        /// </summary>
        private PerformanceCounter deltaExceptionsThrownCounter;


        /// <summary>
        /// Method called by the custom action filter after the action completes
        /// </summary>
        /// <remarks>
        /// If exceptionThrown is true, then the Total Exceptions Thrown counter will be 
        /// incremented by 1
        /// </remarks>
        public override void OnActionComplete(long elapsedTicks, bool exceptionThrown)
        {
            if (exceptionThrown)
                this.deltaExceptionsThrownCounter.Increment();
        }


        /// <summary>
        /// Disposes of the Performance Counter when the metric object is disposed
        /// </summary>
        public override void Dispose()
        {
            this.deltaExceptionsThrownCounter.Dispose();
        }
    }
}
