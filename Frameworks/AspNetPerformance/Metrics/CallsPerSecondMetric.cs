using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace AspNetPerformance.Metrics
{


    /// <summary>
    /// Performance Metric to track the number of times a controller action was called per second during the
    /// last reportin interval
    /// </summary>
    public class CallsPerSecondMetric : PerformanceMetricBase
    {

        public CallsPerSecondMetric(ActionInfo info)
            : base(info)
        {
            String categoryName = this.actionInfo.PerformaneCounterCategory;
            String instanceName = this.actionInfo.InstanceName;
            this.callsPerSecondCounter
                = this.InitializeCounter(categoryName, COUNTER_NAME, instanceName);
        }


        /// <summary>
        /// Constant defining the name of this counter
        /// </summary>
        public const String COUNTER_NAME = "Calls Per Second";


        private PerformanceCounter callsPerSecondCounter;


        /// <summary>
        /// Method called by the custom action filter after the action completes
        /// </summary>
        /// <remarks>
        /// This method increments the Calls Per Second counter by 1
        /// </remarks>
        public override void OnActionComplete(long elapsedTicks, bool exceptionThrown)
        {
            this.callsPerSecondCounter.Increment();
        }


        /// <summary>
        /// Disposes of the Performance Counter when the metric object is disposed
        /// </summary>
        public override void Dispose()
        {
            this.callsPerSecondCounter.Dispose();
        }
    }

    
}
