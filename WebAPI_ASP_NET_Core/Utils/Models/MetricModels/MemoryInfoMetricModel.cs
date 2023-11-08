using WebAPI_ASP_Net.Utils.MetricModels;

namespace WebAPI_ASP_Net.Utils.MemoryUsage
{
    public class MemoryInfoMetricModel : IMetricModel
    {
        public string Title { get; set; }
        public long Size { get; set; }
        public string Type { get; set; }
    }
}
