using System.Diagnostics;

namespace WebAPI_ASP_Net.Utils
{
    public class RequestsPerSecondCounter
    {
        public double MeasureRequestsPerSecond()
        {
            const int NumRequests = 10000;
            var stopwatch = new Stopwatch();

            stopwatch.Start();
            for (int i = 0; i < NumRequests; i++)
            {
                // Виконайте тут ваші запити для вимірювання кількості запитів на секунду
            }
            stopwatch.Stop();

            double elapsedSeconds = stopwatch.ElapsedMilliseconds / 1000.0;
            double requestsPerSecond = NumRequests / elapsedSeconds;
            return requestsPerSecond;
        }
    }

}