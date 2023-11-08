using System.Collections.Generic;
using WebAPI_ASP_Net.Utils.MetricModels;

namespace WebAPI_ASP_Net.Utils
{
    public class PerformanceTestModel
    {
        public string TestName { get; set; }
        public IDictionary<string, IEnumerable<object>> Metrics { get; set; }
    }
}