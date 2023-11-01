using WebAPI_ASP_Net.Utils.MetricModels;

namespace WebAPI_ASP_Net.Utils.MemoryUsage
{
    public class HeapInfoMetricModel : IMetricModel
    {
        public long GCHeapSize { get; set; }
    }
}
