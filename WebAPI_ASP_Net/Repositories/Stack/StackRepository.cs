using System.Collections.Generic;
using System.Linq;
using WebAPI_ASP_Net.Repositories.Containers.Stack;
using WebAPI_ASP_Net.Repositories.Stack;

namespace WebAPI_ASP_Net.Repositories
{
    public class StackRepository : IStackRepository<int>
    {
        private readonly IStackContainer<int> _stackContainer;
        public Stack<int> Stack => _stackContainer.Stack;

        public StackRepository(IStackContainer<int> container)
        {
            _stackContainer = container;
        }
        public IEnumerable<int> GetAll()
        {
            return _stackContainer.Stack;
        }

        public void Add(int item)
        {
            _stackContainer.Stack.Push(item);
        }

        public bool Delete(int item)
        {
            var tempList = _stackContainer.Stack.ToList();
            bool success = tempList.Remove(item);
            if (success)
            {
                _stackContainer.Stack.Clear();
                foreach (var i in tempList)
                {
                    _stackContainer.Stack.Push(i);
                }
            }
            return success;
        }

        public int Peek()
        {
            return _stackContainer.Stack.Peek();
        }

        public bool Update(int oldItem, int newItem)
        {
            var tempList = _stackContainer.Stack.ToList();
            int index = tempList.IndexOf(oldItem);
            if (index != -1)
            {
                tempList[index] = newItem;
                _stackContainer.Stack.Clear();
                foreach (var i in tempList)
                {
                    _stackContainer.Stack.Push(i);
                }
                return true;
            }
            return false;
        }

        public bool DeleteAll()
        {
            try
            {
                _stackContainer.Stack.Clear();
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
