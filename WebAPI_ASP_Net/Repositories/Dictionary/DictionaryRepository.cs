using System.Collections.Generic;
using WebAPI_ASP_Net.Repositories.Containers;
using WebAPI_ASP_Net.Repositories.Containers.Dictionary;

namespace WebAPI_ASP_Net.Repositories
{
    public class DictionaryRepository : IDictionaryRepository<int, int>
    {
        private readonly Dictionary<int, int> dictionary;

        public DictionaryRepository(IDictionaryContainer<int, int> container)
        {
            dictionary = container.Dictionary;
        }
        public IEnumerable<KeyValuePair<int, int>> GetAll()
        {
            return dictionary;
        }

        public void Add(KeyValuePair<int, int> item)
        {
            dictionary.Add(item.Key, item.Value);
        }

        public bool Delete(KeyValuePair<int, int> item)
        {
            return dictionary.Remove(item.Key);
        }

        public bool Update(KeyValuePair<int, int> oldItem, KeyValuePair<int, int> newItem)
        {
            if (dictionary.TryGetValue(oldItem.Key, out int value) && value == oldItem.Value)
            {
                dictionary[oldItem.Key] = newItem.Value;
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
