using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI_ASP_Net.Repositories.Containers.Stack
{
    public class StackContainer<T> : IStackContainer<T>
    {
        public Stack<T> Stack { get; } = new Stack<T>();
    }
}