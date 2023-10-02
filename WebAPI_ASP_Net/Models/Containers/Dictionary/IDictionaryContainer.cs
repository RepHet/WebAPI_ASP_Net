using System.Collections.Generic;

namespace WebAPI_ASP_Net.Repositories.Containers.Dictionary
{
    public interface IDictionaryContainer<TKey, TValue>
    {
        Dictionary<TKey, TValue> Dictionary { get; }
    }
}
