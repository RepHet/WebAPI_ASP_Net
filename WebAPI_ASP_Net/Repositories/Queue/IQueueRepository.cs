using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI_ASP_Net.Repositories.Queue
{
    public interface IQueueRepository<T> : ICollectionRepository<T>
    {
        int Peek();
    }
}
