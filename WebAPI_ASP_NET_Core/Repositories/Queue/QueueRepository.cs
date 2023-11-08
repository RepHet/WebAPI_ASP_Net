using System.Collections.Generic;
using System.Linq;
using WebAPI_ASP_Net.Repositories.Containers.Queue;
using WebAPI_ASP_Net.Repositories.Queue;

namespace WebAPI_ASP_Net.Repositories
{
    public class QueueRepository<T>: IQueueRepository<T>, IQueueContainer<T>
    {
        private readonly IQueueContainer<T> _queueContainer;
        public Queue<T> Queue => _queueContainer.Queue;

        public QueueRepository(IQueueContainer<T> container)
        {
            _queueContainer = container;
        }
        public IEnumerable<T> GetAll()
        {
            return _queueContainer.Queue;
        }

        public void Add(T item)
        {
            _queueContainer.Queue.Enqueue(item);
        }

        public bool Delete(T item)
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

        public T Peek()
        {
            return _queueContainer.Queue.Peek();
        }

        public bool Update(int index, T newItem)
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

        public bool Insert(int index, T newItem)
        {
            return _queueContainer.Insert(index, newItem);
        }
    }
}
