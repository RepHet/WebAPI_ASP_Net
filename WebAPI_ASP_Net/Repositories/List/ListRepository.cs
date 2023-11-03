using System.Collections.Generic;
using WebAPI_ASP_Net.Repositories.Containers.List;
using WebAPI_ASP_Net.Repositories.List;

namespace WebAPI_ASP_Net.Repositories
{
    public class ListRepository : IListRepository<int>, IListContainer<int>
    {
        private readonly IListContainer<int> _listContainer;

        public List<int> List => _listContainer.List;

        public ListRepository(IListContainer<int> container)
        {
            _listContainer = container;
        }

        public IEnumerable<int> GetAll()
        {
            return _listContainer.List;
        }

        public void Add(int item)
        {
            _listContainer.List.Add(item);
        }

        public bool Delete(int item)
        {
            return _listContainer.List.Remove(item);
        }

        public bool Update(int index, int newItem)
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
