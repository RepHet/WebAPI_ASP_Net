using System.Collections.Generic;

namespace WebAPI_ASP_Net.Repositories.Containers.List
{
    public interface IListContainer<T>
    {
        List<T> List { get; }
    }
}
