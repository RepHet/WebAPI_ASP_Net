using System.Collections.Generic;
using System.Linq;
using WebAPI_ASP_Net.Repositories.Containers.Queue;
using WebAPI_ASP_Net.Repositories.Queue;

namespace WebAPI_ASP_Net.Repositories
{
    public class QueueRepository : IQueueRepository<int>
    {
        private readonly Queue<int> _queue;
        public QueueRepository(IQueueContainer<int> container)
        {
            _queue = container.Queue;
        }
        public IEnumerable<int> GetAll()
        {
            return _queue;
        }

        public void Add(int item)
        {
            _queue.Enqueue(item);
        }

        public bool Delete(int item)
        {
            var tempList = _queue.ToList();
            bool success = tempList.Remove(item);
            if (success)
            {
                _queue.Clear();
                foreach (var i in tempList)
                {
                    _queue.Enqueue(i);
                }
            }
            return success;
        }

        public int Peek()
        {
            return _queue.Peek();
        }

        public bool Update(int oldItem, int newItem)
        {
            var tempList = _queue.ToList();
            int index = tempList.IndexOf(oldItem);
            if (index != -1)
            {
                tempList[index] = newItem;
                _queue.Clear();
                foreach (var i in tempList)
                {
                    _queue.Enqueue(i);
                }
                return true;
            }
            return false;
        }

        public bool DeleteAll()
        {
            throw new System.NotImplementedException();
        }
    }
}
