using System.Collections.Generic;
using WebAPI_ASP_Net.Utils.MemoryUsage;

namespace WebAPI_ASP_Net.Utils
{
    public class PerformanceResult
    {
        public string TestName { get; set; }
        public long ExecutionTimeMs { get; set; }
        public Dictionary<string, MemoryMetricsModel> MemoriesUsage { get; set; }
    }
}