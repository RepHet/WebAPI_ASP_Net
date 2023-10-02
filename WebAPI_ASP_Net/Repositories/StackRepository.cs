using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WebAPI_ASP_Net.Repositories.Containers;

namespace WebAPI_ASP_Net.Repositories
{
    public class StackRepository : ICollectionRepository<int>
    {
        private readonly Stack<int> stack;

        public StackRepository(StackContainer container)
        {
            stack = container.Stack;
        }
        public IEnumerable<int> GetAll()
        {
            return stack;
        }

        public void Add(int item)
        {
            stack.Push(item);
        }

        public bool Delete(int item)
        {
            var tempList = stack.ToList();
            bool success = tempList.Remove(item);
            if (success)
            {
                stack.Clear();
                foreach (var i in tempList)
                {
                    stack.Push(i);
                }
            }
            return success;
        }

        public int Peek()
        {
            return stack.Peek();
        }

        public bool Update(int oldItem, int newItem)
        {
            var tempList = stack.ToList();
            int index = tempList.IndexOf(oldItem);
            if (index != -1)
            {
                tempList[index] = newItem;
                stack.Clear();
                foreach (var i in tempList)
                {
                    stack.Push(i);
                }
                return true;
            }
            return false;
        }
    }
}
