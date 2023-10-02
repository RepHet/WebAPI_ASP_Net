using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI_ASP_Net.Repositories.Containers
{
    public class StackContainer
    {
        public Stack<int> Stack { get; } = new Stack<int>();
    }
}