using System.Collections.Generic;
using WebAPI_ASP_Net.Repositories.Containers.List;
using WebAPI_ASP_Net.Repositories.List;

namespace WebAPI_ASP_Net.Repositories
{
    public class ListRepository<T> : IListRepository<T>, IListContainer<T>
    {
        private readonly IListContainer<T> _listContainer;

        public List<T> List => _listContainer.List;

        public ListRepository(IListContainer<T> container)
        {
            _listContainer = container;
        }

        public IEnumerable<T> GetAll()
        {
            return _listContainer.List;
        }

        public void Add(T item)
        {
            _listContainer.List.Add(item);
        }

        public bool Delete(T item)
        {
            return _listContainer.List.Remove(item);
        }

        public bool Update(int index, T newItem)
        {
            if (index > -1 && index < _listContainer.List.Count)
            {
                _listContainer.List[index] = newItem;
                return true;
            }
            return false;
        }

        public bool DeleteAll()
        {
            try { 
                _listContainer.List.Clear(); 
            } catch {  
                return false; 
            }
            return true;
        }

        public int GetLength()
        {
            return _listContainer.List.Count;
        }
    }
}
