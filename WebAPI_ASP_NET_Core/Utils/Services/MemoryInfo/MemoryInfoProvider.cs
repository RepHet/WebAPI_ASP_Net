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
    }
}
