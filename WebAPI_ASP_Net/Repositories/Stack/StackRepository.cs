using System.Collections.Generic;
using System.Linq;
using WebAPI_ASP_Net.Repositories.Containers.Stack;
using WebAPI_ASP_Net.Repositories.Stack;

namespace WebAPI_ASP_Net.Repositories
{
    public class StackRepository<T> : IStackRepository<T>, IStackContainer<T>
    {
        private readonly IStackContainer<T> _stackContainer;
        public Stack<T> Stack => _stackContainer.Stack;

        public StackRepository(IStackContainer<T> container)
        {
            _stackContainer = container;
        }
        public IEnumerable<T> GetAll()
        {
            return _stackContainer.Stack;
        }

        public void Add(T item)
        {
            _stackContainer.Stack.Push(item);
        }

        public bool Delete(T item)
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

        public T Peek()
        {
            return _stackContainer.Stack.Peek();
        }

        public bool Update(int index, T newItem)
        {
            var tempList = _stackContainer.Stack.ToList();
            if (index >= 0 && index < _stackContainer.Stack.Count)
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

        public bool Insert(int index, T newItem)
        {
            return _stackContainer.Insert(index, newItem);
        }
    }
}
