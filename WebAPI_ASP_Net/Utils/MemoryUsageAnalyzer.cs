using System.Diagnostics;

namespace WebAPI_ASP_Net.Utils
{
    public class MemoryUsageAnalyzer
    {
        public long GetMemoryUsage()
        {
            var process = Process.GetCurrentProcess();
            long memoryUsageBytes = process.WorkingSet64;
            return memoryUsageBytes;
        }
    }
}