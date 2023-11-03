using System.Collections.Generic;
using System.Linq;
using WebAPI_ASP_Net.Repositories.Containers.Queue;
using WebAPI_ASP_Net.Repositories.Queue;

namespace WebAPI_ASP_Net.Repositories
{
    public class QueueRepository : IQueueRepository<int>
    {
        private readonly IQueueContainer<int> _queueContainer;
        public Queue<int> Queue => _queueContainer.Queue;

        public QueueRepository(IQueueContainer<int> container)
        {
            _queueContainer = container;
        }
        public IEnumerable<int> GetAll()
        {
            return _queueContainer.Queue;
        }

        public void Add(int item)
        {
            _queueContainer.Queue.Enqueue(item);
        }

        public bool Delete(int item)
        {
            var tempList = _queueContainer.Queue.ToList();
            bool success = tempList.Remove(item);
            if (success)
            {
                _queueContainer.Queue.Clear();
                foreach (var i in tempList)
                {
                    _queueContainer.Queue.Enqueue(i);
                }
            }
            return success;
        }

        public int Peek()
        {
            return _queueContainer.Queue.Peek();
        }

        public bool Update(int index, int newItem)
        {
            var tempList = _queueContainer.Queue.ToList();
            if (index > -1 && index < tempList.Count)
            {
                tempList[index] = newItem;
                _queueContainer.Queue.Clear();
                foreach (var i in tempList)
                {
                    _queueContainer.Queue.Enqueue(i);
                }
                return true;
            }
            return false;
        }

        public bool DeleteAll()
        {
            try
            {
                _queueContainer.Queue.Clear();
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
