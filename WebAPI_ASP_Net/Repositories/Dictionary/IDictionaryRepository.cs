using System.Collections.Generic;

namespace WebAPI_ASP_Net.Repositories
{
    public interface IDictionaryRepository<TKey, TValue> : ICollectionRepository<KeyValuePair<TKey, TValue>>
    {
    }
}
