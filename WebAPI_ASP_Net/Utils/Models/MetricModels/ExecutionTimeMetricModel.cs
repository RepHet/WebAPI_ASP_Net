using System;

namespace WebAPI_ASP_Net.Utils.MetricModels
{
    public class ExecutionTimeMetricModel : IMetricModel
    {
        private double _executionTimeMs;

        public double ExecutionTimeMs
        {
            get => _executionTimeMs;
            set => _executionTimeMs = Math.Round(value, 6);
        }
    }
}