namespace WebAPI_ASP_NET_Core_Perfomance_Checker.Models
{
    public class PerformanceResult
    {
        public string CollectionName { get; set; }
        public long ExecutionTime { get; set; }

        public PerformanceResult(string collectionName, long executionTime)
        {
            CollectionName = collectionName;
            ExecutionTime = executionTime;
        }
    }
}
