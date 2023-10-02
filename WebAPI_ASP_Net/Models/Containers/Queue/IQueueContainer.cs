using System.Collections.Generic;

namespace WebAPI_ASP_Net.Repositories.Containers.Queue
{
    public interface IQueueContainer<T>
    {
        Queue<T> Queue { get; }
    }
}
