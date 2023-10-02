using System.Collections.Generic;
using WebAPI_ASP_Net.Repositories.Containers;

namespace WebAPI_ASP_Net.Repositories
{
    public class ListRepository : ICollectionRepository<int>
    {
        private readonly List<int> list;

        public ListRepository(ListContainer container)
        {
            list = container.List;
        }

        public IEnumerable<int> GetAll()
        {
            return list;
        }

        public void Add(int item)
        {
            list.Add(item);
        }

        public bool Delete(int item)
        {
            return list.Remove(item);
        }

        public bool Update(int oldItem, int newItem)
        {
            int index = list.IndexOf(oldItem);
            if (index != -1)
            {
                list[index] = newItem;
                return true;
            }
            return false;
        }
    }
}
