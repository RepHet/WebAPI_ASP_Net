using System.Collections.Generic;
using System.Linq;
using WebAPI_ASP_Net.Repositories.Containers.Dictionary;

namespace WebAPI_ASP_Net.Repositories
{
    public class DictionaryRepository<TKey, TVal> : IDictionaryRepository<TKey, TVal>, IDictionaryContainer<TKey, TVal>
    {
        private readonly IDictionaryContainer<TKey, TVal> _dictionaryContainer;

        public Dictionary<TKey, TVal> Dictionary => _dictionaryContainer.Dictionary;

        Dictionary<TKey, TVal> IDictionaryContainer<TKey, TVal>.Dictionary => throw new System.NotImplementedException();

        public DictionaryRepository(IDictionaryContainer<TKey, TVal> container)
        {
            _dictionaryContainer = container;
        }

        public bool Update(TKey key, TVal newItem)
        {
            if (_dictionaryContainer.Dictionary.TryGetValue(key, out TVal value))
            {
                _dictionaryContainer.Dictionary[key] = newItem;
                return true;
            }
            return false;
        }

        IEnumerable<KeyValuePair<TKey, TVal>> ICollectionRepository<KeyValuePair<TKey, TVal>>.GetAll()
        {
            return _dictionaryContainer.Dictionary;
        }

        public void Add(KeyValuePair<TKey, TVal> item)
        {
            _dictionaryContainer.Dictionary.Add(item.Key, item.Value);
        }

        public bool Delete(KeyValuePair<TKey, TVal> item)
        {
            return _dictionaryContainer.Dictionary.Remove(item.Key);
        }

        public bool Update(int index, KeyValuePair<TKey, TVal> newItem)
        {
            if (index >= 0 && index < _dictionaryContainer.Dictionary.Count)
            { 
                KeyValuePair<TKey, TVal> item = _dictionaryContainer.Dictionary.ElementAt(index);
                _dictionaryContainer.Dictionary[item.Key] = newItem.Value;
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
    }
}
