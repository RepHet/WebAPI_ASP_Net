using System;

namespace WebAPI_ASP_Net.Utils.Timer
{
    public interface ITimer
    {
        void Start();

        void Stop();

        void Reset();

        TimeSpan ElapsedTime();
    }
}
