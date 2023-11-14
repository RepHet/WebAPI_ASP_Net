namespace WebAPI_ASP_Net.Repositories.Containers.Queue
{
    public class QueueContainer<T> : IQueueContainer<T>
    {
        public Queue<T> Queue { get; } = new Queue<T>();

        public bool Insert(int index, T newItem)
        {
            var tempQueue = new Queue<T>();
            var count = Queue.Count;

            if (index >= 0 && index <= count)
            {
                // Додайте елементи з початку черги до тимчасової черги
                for (int i = 0; i < index; i++)
                {
                    tempQueue.Enqueue(Queue.Dequeue());
                }

                // Додайте новий елемент до тимчасової черги
                tempQueue.Enqueue(newItem);

                // Додайте інші елементи з початкової черги назад до тимчасової черги
                while (Queue.Count > 0)
                {
                    tempQueue.Enqueue(Queue.Dequeue());
                }

                // Очистіть початкову чергу і скопіюйте елементи назад з тимчасової черги
                Queue.Clear();
                foreach (var item in tempQueue)
                {
                    Queue.Enqueue(item);
                }

                return true;
            }

            return false;
        }

        public bool Update(int index, T newItem)
        {
            var tempList = Queue.ToList();
            if (index > -1 && index < tempList.Count)
            {
                tempList[index] = newItem;
                Queue.Clear();
                foreach (var i in tempList)
                {
                    Queue.Enqueue(i);
                }
                return true;
            }
            return false;
        }

    }
}