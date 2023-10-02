using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WebAPI_ASP_Net.Repositories.Containers;

namespace WebAPI_ASP_Net.Repositories
{
    public class QueueRepository : ICollectionRepository<int>
    {
        private readonly Queue<int> queue;
        public QueueRepository(QueueContainer container)
        {
            queue = container.Queue;
        }
        public IEnumerable<int> GetAll()
        {
            return queue;
        }

        public void Add(int item)
        {
            queue.Enqueue(item);
        }

        public bool Delete(int item)
        {
            var tempList = queue.ToList();
            bool success = tempList.Remove(item);
            if (success)
            {
                queue.Clear();
                foreach (var i in tempList)
                {
                    queue.Enqueue(i);
                }
            }
            return success;
        }

        public int Peek()
        {
            return queue.Peek();
        }

        public bool Update(int oldItem, int newItem)
        {
            var tempList = queue.ToList();
            int index = tempList.IndexOf(oldItem);
            if (index != -1)
            {
                tempList[index] = newItem;
                queue.Clear();
                foreach (var i in tempList)
                {
                    queue.Enqueue(i);
                }
                return true;
            }
            return false;
        }
    }
}
