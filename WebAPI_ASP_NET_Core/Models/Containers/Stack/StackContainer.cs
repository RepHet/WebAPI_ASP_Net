using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI_ASP_Net.Repositories.Containers.Stack
{
    public class StackContainer<T> : IStackContainer<T>
    {
        public Stack<T> Stack { get; } = new Stack<T>();

        public bool Insert(int index, T newItem)
        {
            var tempStack = new Stack<T>();
            var count = Stack.Count;

            if (index >= 0 && index <= count)
            {
                // Витягніть елементи з початку стеку і додайте їх до тимчасового стеку
                for (int i = 0; i < index; i++)
                {
                    tempStack.Push(Stack.Pop());
                }

                // Додайте новий елемент в тимчасовий стек
                tempStack.Push(newItem);

                // Додайте інші елементи з початкового стеку назад до тимчасового стеку
                while (Stack.Count > 0)
                {
                    tempStack.Push(Stack.Pop());
                }

                // Очистіть початковий стек і скопіюйте елементи назад з тимчасового стеку
                Stack.Clear();
                foreach (var item in tempStack)
                {
                    Stack.Push(item);
                }

                return true;
            }

            return false;
        }

    }
}