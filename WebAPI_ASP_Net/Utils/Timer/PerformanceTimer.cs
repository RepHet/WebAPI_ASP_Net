using System;
using System.Diagnostics;

namespace WebAPI_ASP_Net.Utils.Timer
{
    public class PerformanceTimer : ITimer
    {
        private Stopwatch stopwatch = new Stopwatch();

        public void Start()
        {
            stopwatch.Start();
        }

        public void Stop()
        {
            stopwatch.Stop();
        }

        public void Reset()
        {
            stopwatch.Reset();
        }

        public TimeSpan ElapsedTime()
        {
            return stopwatch.Elapsed;
        }
    }

}