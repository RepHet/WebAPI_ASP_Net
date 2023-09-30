﻿using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using WebAPI_ASP_NET_Core.Models;

namespace WebAPI_ASP_Net.Controllers
{
    public class CollectionController : ControllerBase
    {
        private List<int> list = new List<int>();
        private Queue<int> queue = new Queue<int>();
        private Stack<int> stack = new Stack<int>();
        private HashSet<int> hashSet = new HashSet<int>();
        private SortedSet<int> sortedSet = new SortedSet<int>();
        private Dictionary<int, string> dictionary = new Dictionary<int, string>();
        private SortedList<int, string> sortedList = new SortedList<int, string>();

        private const int MaxSize = 1000000;

        public IActionResult MeasurePerformance()
        {
            var stopwatch = new Stopwatch();
            var results = new List<PerformanceResult>();

            // Найкращий випадок (оптимальний) для List
            stopwatch.Start();
            for (int i = 0; i < MaxSize; i++)
            {
                list.Add(i);
            }
            stopwatch.Stop();
            results.Add(new PerformanceResult("List (Best Case)", stopwatch.ElapsedMilliseconds));
            list.Clear();
            stopwatch.Reset();

            // Середній випадок (зазвичай оптимальний) для List
            for (int i = 0; i < MaxSize; i++)
            {
                list.Add(i);
            }
            stopwatch.Start();
            for (int i = 0; i < MaxSize / 2; i++)
            {
                list.Remove(i);
            }
            stopwatch.Stop();
            results.Add(new PerformanceResult("List (Average Case)", stopwatch.ElapsedMilliseconds));
            list.Clear();
            stopwatch.Reset();

            // Найгірший випадок (найменший оптимальний) для List
            for (int i = 0; i < MaxSize; i++)
            {
                list.Add(i);
            }
            stopwatch.Start();
            for (int i = 0; i < MaxSize; i++)
            {
                list.Insert(0, i);
            }
            stopwatch.Stop();
            results.Add(new PerformanceResult("List (Worst Case)", stopwatch.ElapsedMilliseconds));
            list.Clear();
            stopwatch.Reset();

            // Найкращий випадок (оптимальний) для Queue
            stopwatch.Start();
            for (int i = 0; i < MaxSize; i++)
            {
                queue.Enqueue(i);
            }
            stopwatch.Stop();
            results.Add(new PerformanceResult("Queue (Best Case)", stopwatch.ElapsedMilliseconds));
            queue.Clear();
            stopwatch.Reset();

            // Середній випадок (зазвичай оптимальний) для Queue
            for (int i = 0; i < MaxSize; i++)
            {
                queue.Enqueue(i);
            }
            stopwatch.Start();
            for (int i = 0; i < MaxSize / 2; i++)
            {
                queue.Dequeue();
            }
            stopwatch.Stop();
            results.Add(new PerformanceResult("Queue (Average Case)", stopwatch.ElapsedMilliseconds));
            queue.Clear();
            stopwatch.Reset();

            // Найгірший випадок (найменший оптимальний) для Queue
            for (int i = 0; i < MaxSize; i++)
            {
                queue.Enqueue(i);
            }
            stopwatch.Start();
            for (int i = 0; i < MaxSize / 2; i++)
            {
                queue.Enqueue(i);
            }
            stopwatch.Stop();
            results.Add(new PerformanceResult("Queue (Worst Case)", stopwatch.ElapsedMilliseconds));
            queue.Clear();
            stopwatch.Reset();

            // Найкращий випадок (оптимальний) для Stack
            stopwatch.Start();
            for (int i = 0; i < MaxSize; i++)
            {
                stack.Push(i);
            }
            stopwatch.Stop();
            results.Add(new PerformanceResult("Stack (Best Case)", stopwatch.ElapsedMilliseconds));
            stack.Clear();
            stopwatch.Reset();

            // Середній випадок (зазвичай оптимальний) для Stack
            for (int i = 0; i < MaxSize; i++)
            {
                stack.Push(i);
            }
            stopwatch.Start();
            for (int i = 0; i < MaxSize / 2; i++)
            {
                stack.Pop();
            }
            stopwatch.Stop();
            results.Add(new PerformanceResult("Stack (Average Case)", stopwatch.ElapsedMilliseconds));
            stack.Clear();
            stopwatch.Reset();

            // Найгірший випадок (найменший оптимальний) для Stack
            for (int i = 0; i < MaxSize; i++)
            {
                stack.Push(i);
            }
            stopwatch.Start();
            for (int i = 0; i < MaxSize / 2; i++)
            {
                stack.Push(i);
            }
            stopwatch.Stop();
            results.Add(new PerformanceResult("Stack (Worst Case)", stopwatch.ElapsedMilliseconds));
            stack.Clear();
            stopwatch.Reset();

            // Найкращий випадок (оптимальний) для HashSet
            stopwatch.Start();
            for (int i = 0; i < MaxSize; i++)
            {
                hashSet.Add(i);
            }
            stopwatch.Stop();
            results.Add(new PerformanceResult("HashSet (Best Case)", stopwatch.ElapsedMilliseconds));
            hashSet.Clear();
            stopwatch.Reset();

            // Середній випадок (зазвичай оптимальний) для HashSet
            for (int i = 0; i < MaxSize; i++)
            {
                hashSet.Add(i);
            }
            stopwatch.Start();
            for (int i = 0; i < MaxSize / 2; i++)
            {
                hashSet.Remove(i);
            }
            stopwatch.Stop();
            results.Add(new PerformanceResult("HashSet (Average Case)", stopwatch.ElapsedMilliseconds));
            hashSet.Clear();
            stopwatch.Reset();

            // Найгірший випадок (найменший оптимальний) для HashSet
            for (int i = 0; i < MaxSize; i++)
            {
                hashSet.Add(i);
            }
            stopwatch.Start();
            for (int i = 0; i < MaxSize / 2; i++)
            {
                hashSet.Remove(i + MaxSize);
            }
            stopwatch.Stop();
            results.Add(new PerformanceResult("HashSet (Worst Case)", stopwatch.ElapsedMilliseconds));
            hashSet.Clear();
            stopwatch.Reset();

            // Найкращий випадок (оптимальний) для SortedSet
            stopwatch.Start();
            for (int i = 0; i < MaxSize; i++)
            {
                sortedSet.Add(i);
            }
            stopwatch.Stop();
            results.Add(new PerformanceResult("SortedSet (Best Case)", stopwatch.ElapsedMilliseconds));
            sortedSet.Clear();
            stopwatch.Reset();

            // Середній випадок (зазвичай оптимальний) для SortedSet
            for (int i = 0; i < MaxSize; i++)
            {
                sortedSet.Add(i);
            }
            stopwatch.Start();
            for (int i = 0; i < MaxSize / 2; i++)
            {
                sortedSet.Remove(i);
            }
            stopwatch.Stop();
            results.Add(new PerformanceResult("SortedSet (Average Case)", stopwatch.ElapsedMilliseconds));
            sortedSet.Clear();
            stopwatch.Reset();

            // Найгірший випадок (найменший оптимальний) для SortedSet
            for (int i = 0; i < MaxSize; i++)
            {
                sortedSet.Add(i);
            }
            stopwatch.Start();
            for (int i = 0; i < MaxSize / 2; i++)
            {
                sortedSet.Remove(i + MaxSize);
            }
            stopwatch.Stop();
            results.Add(new PerformanceResult("SortedSet (Worst Case)", stopwatch.ElapsedMilliseconds));
            sortedSet.Clear();
            stopwatch.Reset();

            // Найкращий випадок (оптимальний) для Dictionary
            stopwatch.Start();
            for (int i = 0; i < MaxSize; i++)
            {
                dictionary[i] = i.ToString();
            }
            stopwatch.Stop();
            results.Add(new PerformanceResult("Dictionary (Best Case)", stopwatch.ElapsedMilliseconds));
            dictionary.Clear();
            stopwatch.Reset();

            // Середній випадок (зазвичай оптимальний) для Dictionary
            for (int i = 0; i < MaxSize; i++)
            {
                dictionary[i] = i.ToString();
            }
            stopwatch.Start();
            for (int i = 0; i < MaxSize / 2; i++)
            {
                dictionary.Remove(i);
            }
            stopwatch.Stop();
            results.Add(new PerformanceResult("Dictionary (Average Case)", stopwatch.ElapsedMilliseconds));
            dictionary.Clear();
            stopwatch.Reset();

            // Найгірший випадок (найменший оптимальний) для Dictionary
            for (int i = 0; i < MaxSize; i++)
            {
                dictionary[i] = i.ToString();
            }
            stopwatch.Start();
            for (int i = 0; i < MaxSize / 2; i++)
            {
                dictionary.Remove(i + MaxSize);
            }
            stopwatch.Stop();
            results.Add(new PerformanceResult("Dictionary (Worst Case)", stopwatch.ElapsedMilliseconds));
            dictionary.Clear();
            stopwatch.Reset();

            // Найкращий випадок (оптимальний) для SortedList
            stopwatch.Start();
            for (int i = 0; i < MaxSize; i++)
            {
                sortedList[i] = i.ToString();
            }
            stopwatch.Stop();
            results.Add(new PerformanceResult("SortedList (Best Case)", stopwatch.ElapsedMilliseconds));
            sortedList.Clear();
            stopwatch.Reset();

            // Середній випадок (зазвичай оптимальний) для SortedList
            for (int i = 0; i < MaxSize; i++)
            {
                sortedList[i] = i.ToString();
            }
            stopwatch.Start();
            for (int i = 0; i < MaxSize / 2; i++)
            {
                sortedList.Remove(i);
            }
            stopwatch.Stop();
            results.Add(new PerformanceResult("SortedList (Average Case)", stopwatch.ElapsedMilliseconds));
            sortedList.Clear();
            stopwatch.Reset();

            // Найгірший випадок (найменший оптимальний) для SortedList
            for (int i = 0; i < MaxSize; i++)
            {
                sortedList[i] = i.ToString();
            }
            stopwatch.Start();
            for (int i = 0; i < MaxSize / 2; i++)
            {
                sortedList.Remove(i + MaxSize);
            }
            stopwatch.Stop();
            results.Add(new PerformanceResult("SortedList (Worst Case)", stopwatch.ElapsedMilliseconds));
            sortedList.Clear();
            stopwatch.Reset();

            return Ok(results);
        }
    }
}