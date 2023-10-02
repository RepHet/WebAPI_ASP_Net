using System.Collections.Generic;
using System.Linq;
using WebAPI_ASP_Net.Repositories.Containers.Stack;
using WebAPI_ASP_Net.Repositories.Stack;

namespace WebAPI_ASP_Net.Repositories
{
    public class StackRepository : IStackRepository<int>
    {
        private readonly Stack<int> _stack;

        public StackRepository(IStackContainer<int> container)
        {
            _stack = container.Stack;
        }
        public IEnumerable<int> GetAll()
        {
            return _stack;
        }

        public void Add(int item)
        {
            _stack.Push(item);
        }

        public bool Delete(int item)
        {
            var tempList = _stack.ToList();
            bool success = tempList.Remove(item);
            if (success)
            {
                _stack.Clear();
                foreach (var i in tempList)
                {
                    _stack.Push(i);
                }
            }
            return success;
        }

        public int Peek()
        {
            return _stack.Peek();
        }

        public bool Update(int oldItem, int newItem)
        {
            var tempList = _stack.ToList();
            int index = tempList.IndexOf(oldItem);
            if (index != -1)
            {
                tempList[index] = newItem;
                _stack.Clear();
                foreach (var i in tempList)
                {
                    _stack.Push(i);
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
