using System;
using System.Diagnostics;

namespace WebAPI_ASP_Net.Utils
{
    public class PerformanceTimer
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

        public TimeSpan ElapsedTime
        {
            get { return stopwatch.Elapsed; }
        }
    }

}