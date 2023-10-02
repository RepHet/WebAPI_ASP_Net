namespace WebAPI_ASP_Net.Utils
{
    public class PerformanceResult
    {
        public string TestName { get; set; }
        public long ExecutionTimeMs { get; set; }
        public long MemoryUsageBytes { get; set; }

        public PerformanceResult(string testName, long executionTimeMs)
        {
            TestName = testName;
            ExecutionTimeMs = executionTimeMs;
        }
    }
}