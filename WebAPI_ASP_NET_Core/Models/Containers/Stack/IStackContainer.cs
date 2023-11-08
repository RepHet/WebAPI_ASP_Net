using System.Collections.Generic;

namespace WebAPI_ASP_Net.Repositories.Containers.Stack
{
    public interface IStackContainer<T>
    {
        Stack<T> Stack { get; }

        bool Insert(int index, T newItem);
    }
}
