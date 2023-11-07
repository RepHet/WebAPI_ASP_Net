using System;
using System.Diagnostics;

namespace WebAPI_ASP_Net.Utils
{
    public static class MemoryInfoProvider
    {
        public static long GetProcessMemorySize()
        {
            Process process = Process.GetCurrentProcess();
            return process.WorkingSet64;
        }
        public static long GetGCHeapSize(bool forceFullCollection)
        {
            return GC.GetTotalMemory(forceFullCollection);
        }
        public static long GetMemorySizeWithPerfomanceCounter()
        {
            PerformanceCounter memoryCounter = new PerformanceCounter("Process", "Working Set - Private", Process.GetCurrentProcess().ProcessName);
            return memoryCounter.RawValue;
        }
    }
}
