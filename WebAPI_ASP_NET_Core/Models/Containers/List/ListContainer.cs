using System.Collections.Generic;

namespace WebAPI_ASP_Net.Repositories.Containers.List
{
    public class ListContainer<T> : IListContainer<T>
    {
        public List<T> List { get; } = new List<T>();
    }
}