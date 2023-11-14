using Microsoft.AspNetCore.Mvc;
using WebAPI_ASP_Net.Repositories.Containers.Queue;
using WebAPI_ASP_Net.Repositories.Queue;
using WebAPI_ASP_Net.Utils;
using WebAPI_ASP_Net.Utils.MemoryUsage;
using WebAPI_ASP_Net.Utils.MetricModels;
using WebAPI_ASP_Net.Utils.Models.MetricModels;
using WebAPI_ASP_Net.Utils.Timer;

namespace WebAPI_ASP_NET_Core.Controllers
{
    [ApiController]
    public class QueueController : ControllerBase
    {
        const int maxElementSize = 100000 / 2;
        private readonly IQueueRepository<int> _queueRepository;
        private readonly ITimer _timer;

        public QueueController(IQueueRepository<int> queueRepository, ITimer timer)
        {
            _queueRepository = queueRepository;
            _timer = timer;
        }

        [HttpGet("/api/queue")]
        public ActionResult GetAll()
        {
            var items = _queueRepository.GetAll();
            return Ok(items);
        }

        [HttpPost("/api/queue")]
        public ActionResult Add(int item)
        {
            _queueRepository.Add(item);
            return Ok();
        }

        [HttpPut("/api/queue")]
        public ActionResult Update(int oldItem, int newItem)
        {
            bool success = _queueRepository.Update(oldItem, newItem);
            if (success)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete("/api/queue")]
        public ActionResult Delete(int item)
        {
            bool success = _queueRepository.Delete(item);
            if (success)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [Route("/api/queue/add/best")]
        [HttpGet]
        public ActionResult<PerformanceTestModel> GetBest(int maxSize = maxElementSize)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            IQueueContainer<int> dataContainer = new QueueContainer<int>();

            var processMemorySizeBeforeTest = new MemoryInfoMetricModel
            {
                Title = "Process before Test",
                Size = MemoryInfoProvider.GetProcessMemorySize(),
                Type = EMemorySizeType.Byte.ToString(),
            };

            _timer.Start();
            for (int i = 0; i < maxSize; i++)
            {
                try
                {
                    dataContainer.Queue.Enqueue(i);
                }
                catch
                {
                    break;
                }
            }
            _timer.Stop();
            GC.Collect();


            var processMemorySizeAfterTest = new MemoryInfoMetricModel
            {
                Title = "Process after test",
                Size = MemoryInfoProvider.GetProcessMemorySize(),
                Type = EMemorySizeType.Byte.ToString(),
            };

            var executionTime = new ExecutionTimeMetricModel
            {
                ExecutionTimeMs = _timer.ElapsedTime().TotalMilliseconds
            };

            var GCMemorySize = new MemoryInfoMetricModel
            {
                Title = "GC",
                Size = MemoryInfoProvider.GetGCHeapSize(true),
                Type = EMemorySizeType.Byte.ToString(),
            };

            PerformanceTestModel performanceResult = new PerformanceTestModel
            {
                TestName = "Queue add (best)",
                Metrics = new Dictionary<string, IEnumerable<object>>
                {
                    {
                        "Test execution time",
                        new List<IMetricModel> {
                            executionTime
                        }
                    },
                    {
                        "Memory",
                        new List<IMetricModel>
                        {
                            GCMemorySize,
                            processMemorySizeBeforeTest,
                            processMemorySizeAfterTest
                        }
                    }
                }
            };

            return Ok(performanceResult);
        }
        [Route("/api/queue/add/worst")]
        [HttpGet]
        public ActionResult<PerformanceTestModel> GetWorst(int maxSize = maxElementSize)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            IQueueContainer<int> dataContainer = new QueueContainer<int>();

            var processMemorySizeBeforeTest = new MemoryInfoMetricModel
            {
                Title = "Process before Test",
                Size = MemoryInfoProvider.GetProcessMemorySize(),
                Type = EMemorySizeType.Byte.ToString(),
            };

            for (int i = 0; i < 2; i++)
            {
                dataContainer.Queue.Enqueue(i);
            }

            _timer.Start();
            for (int i = 2; i < maxSize; i++)
            {
                int queueSize = dataContainer.Queue.Count();
                dataContainer.Insert(1, i);
            }

            _timer.Stop();
            GC.Collect();

            var processMemorySizeAfterTest = new MemoryInfoMetricModel
            {
                Title = "Process after test",
                Size = MemoryInfoProvider.GetProcessMemorySize(),
                Type = EMemorySizeType.Byte.ToString(),
            };

            var executionTime = new ExecutionTimeMetricModel
            {
                ExecutionTimeMs = _timer.ElapsedTime().TotalMilliseconds
            };

            var GCMemorySize = new MemoryInfoMetricModel
            {
                Title = "GC",
                Size = MemoryInfoProvider.GetGCHeapSize(true),
                Type = EMemorySizeType.Byte.ToString(),
            };

            PerformanceTestModel performanceResult = new PerformanceTestModel
            {
                TestName = "Queue add (worst)",
                Metrics = new Dictionary<string, IEnumerable<object>>
                {
                    {
                        "Test execution time",
                        new List<IMetricModel> {
                            executionTime
                        }
                    },
                    {
                        "Memory",
                        new List<IMetricModel>
                        {
                            GCMemorySize,
                            processMemorySizeBeforeTest,
                            processMemorySizeAfterTest
                        }
                    }
                }
            };

            return Ok(performanceResult);
        }
        //        Оновлення(Best Case) :
        //Тут найкращий варіант - оновлення елемента за його індексом, 
        //оскільки це виконується за константний час.
        //Ми використовуємо власний метод Update, який перебудовує чергу.

        [Route("api/queue/update/best")]
        [HttpGet]
        public ActionResult<PerformanceTestModel> UpdateBest(int maxSize = maxElementSize)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            IQueueContainer<int> dataContainer = new QueueContainer<int>();

            for (int i = 0; i < maxSize; i++)
            {
                dataContainer.Queue.Enqueue(i);
            }

            var processMemorySizeBeforeTest = new MemoryInfoMetricModel
            {
                Title = "Process before test",
                Size = MemoryInfoProvider.GetProcessMemorySize(),
                Type = EMemorySizeType.Byte.ToString(),
            };

            double executionTimeMs = 0.0;

            for (int i = 0; i < maxSize; i++)
            {
                try
                {
                    _timer.Start();
                    dataContainer.Update(i, i + 1); // Оновлення елемента
                    _timer.Stop();

                    executionTimeMs += _timer.ElapsedTime().TotalMilliseconds;
                    _timer.Reset();
                }
                catch
                {
                    break;
                }
            }
            GC.Collect();

            var processMemorySizeAfterTest = new MemoryInfoMetricModel
            {
                Title = "Process after test",
                Size = MemoryInfoProvider.GetProcessMemorySize(),
                Type = EMemorySizeType.Byte.ToString(),
            };

            var executionTime = new ExecutionTimeMetricModel
            {
                ExecutionTimeMs = executionTimeMs
            };

            var GCMemorySize = new MemoryInfoMetricModel
            {
                Title = "GC",
                Size = MemoryInfoProvider.GetGCHeapSize(true),
                Type = EMemorySizeType.Byte.ToString(),
            };

            PerformanceTestModel performanceResult = new PerformanceTestModel
            {
                TestName = "Queue update (best)",
                Metrics = new Dictionary<string, IEnumerable<object>>
                {
                    {
                        "Test execution time",
                        new List<IMetricModel>
                        {
                            executionTime
                        }
                    },
                    {
                        "Memory",
                        new List<IMetricModel>
                        {
                            GCMemorySize,
                            processMemorySizeBeforeTest,
                            processMemorySizeAfterTest
                        }
                    }
                }
            };

            return Ok(performanceResult);
        }
        //        Видалення(Worst Case) :
        //Найгірший варіант - видалення елементів з початку черги.
        //Оскільки при видаленні елемента з початку всі інші елементи повинні зміститися вліво,
        //то час видалення буде лінійним і залежатиме від кількості елементів у черзі.

        [Route("api/queue/remove/worst")]
        [HttpGet]
        public ActionResult<PerformanceTestModel> RemoveWorst(int maxSize = maxElementSize)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            IQueueContainer<int> dataContainer = new QueueContainer<int>();

            var processMemorySizeBeforeTest = new MemoryInfoMetricModel
            {
                Title = "Process before test",
                Size = MemoryInfoProvider.GetProcessMemorySize(),
                Type = EMemorySizeType.Byte.ToString(),
            };

            for (int i = 0; i < maxSize; i++)
            {
                dataContainer.Queue.Enqueue(i);
            }

            double executionTimeMs = 0.0;

            for (int i = 0; i < maxSize; i++)
            {
                try
                {
                    _timer.Start();
                    dataContainer.Queue.Dequeue(); // Видалення елемента
                    _timer.Stop();

                    executionTimeMs += _timer.ElapsedTime().TotalMilliseconds;
                    _timer.Reset();
                }
                catch
                {
                    break;
                }
            }
            GC.Collect();

            var processMemorySizeAfterTest = new MemoryInfoMetricModel
            {
                Title = "Process after test",
                Size = MemoryInfoProvider.GetProcessMemorySize(),
                Type = EMemorySizeType.Byte.ToString(),
            };

            var executionTime = new ExecutionTimeMetricModel
            {
                ExecutionTimeMs = executionTimeMs
            };

            var GCMemorySize = new MemoryInfoMetricModel
            {
                Title = "GC",
                Size = MemoryInfoProvider.GetGCHeapSize(true),
                Type = EMemorySizeType.Byte.ToString(),
            };

            PerformanceTestModel performanceResult = new PerformanceTestModel
            {
                TestName = "Queue remove (worst)",
                Metrics = new Dictionary<string, IEnumerable<object>>
                {
                    {
                        "Test execution time",
                        new List<IMetricModel>
                        {
                            executionTime
                        }
                    },
                    {
                        "Memory",
                        new List<IMetricModel>
                        {
                            GCMemorySize,
                            processMemorySizeBeforeTest,
                            processMemorySizeAfterTest
                        }
                    }
                }
            };

            return Ok(performanceResult);
        }
    }
}
