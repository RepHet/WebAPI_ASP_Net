using System.Collections.Generic;

namespace WebAPI_ASP_Net.Repositories.Containers.Dictionary
{
    public class DictionaryContainer<TKey, TValue> : IDictionaryContainer<TKey, TValue>
    {
        public Dictionary<TKey, TValue> Dictionary { get; } = new Dictionary<TKey, TValue>();
    }
}