using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI_ASP_Net.Repositories.Containers
{
    public class QueueContainer
    {
        public Queue<int> Queue { get; } = new Queue<int>();
    }
}