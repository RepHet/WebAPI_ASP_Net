using System.Collections.Generic;
using WebAPI_ASP_Net.Repositories.Containers.Dictionary;

namespace WebAPI_ASP_Net.Repositories
{
    public class DictionaryRepository : IDictionaryRepository<int, int>
    {
        private readonly IDictionaryContainer<int, int> _dictionaryContainer;

        public Dictionary<int, int> Dictionary => _dictionaryContainer.Dictionary;

        public DictionaryRepository(IDictionaryContainer<int, int> container)
        {
            _dictionaryContainer = container;
        }
        public IEnumerable<KeyValuePair<int, int>> GetAll()
        {
            return _dictionaryContainer.Dictionary;
        }

        public void Add(KeyValuePair<int, int> item)
        {
            _dictionaryContainer.Dictionary.Add(item.Key, item.Value);
        }

        public bool Delete(KeyValuePair<int, int> item)
        {
            return _dictionaryContainer.Dictionary.Remove(item.Key);
        }

        public bool Update(int key, int newItem)
        {
            if (_dictionaryContainer.Dictionary.TryGetValue(key, out int value))
            {
                _dictionaryContainer.Dictionary[key] = newItem;
                return true;
            }
            return false;
        }

        public bool DeleteAll()
        {
            try
            {
                _dictionaryContainer.Dictionary.Clear();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Update(int key, KeyValuePair<int, int> newItem)
        {
            if (_dictionaryContainer.Dictionary.TryGetValue(key, out int value))
            {
                _dictionaryContainer.Dictionary[key] = newItem.Value;
                return true;
            }
            return false;
        }
    }
}
