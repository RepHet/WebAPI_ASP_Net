using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI_ASP_Net.Repositories.Containers.Queue
{
    public class QueueContainer<T> : IQueueContainer<T>
    {
        public Queue<T> Queue { get; } = new Queue<T>();
    }
}