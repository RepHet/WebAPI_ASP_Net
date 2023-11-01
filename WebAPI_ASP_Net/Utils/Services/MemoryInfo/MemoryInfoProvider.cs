using System;
using System.Diagnostics;

namespace WebAPI_ASP_Net.Utils
{
    public static class MemoryInfoProvider
    {
        public static long GetGCHeapSize()
        {
            Process process = Process.GetCurrentProcess();
            return process.WorkingSet64;
        }
    }
}
