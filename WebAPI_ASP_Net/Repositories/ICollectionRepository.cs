using System.Collections.Generic;

namespace WebAPI_ASP_Net.Repositories
{
    public interface ICollectionRepository<T>
    {
        IEnumerable<T> GetAll();
        void Add(T item); 
        bool Delete(T item); 
        bool Update(T oldItem, T newItem); 
    }
}
