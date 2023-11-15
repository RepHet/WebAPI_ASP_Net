using Microsoft.AspNetCore.Mvc;
using WebAPI_ASP_Net.Repositories.Containers.Stack;
using WebAPI_ASP_Net.Repositories.Stack;
using WebAPI_ASP_Net.Utils;
using WebAPI_ASP_Net.Utils.MemoryUsage;
using WebAPI_ASP_Net.Utils.MetricModels;
using WebAPI_ASP_Net.Utils.Models.MetricModels;
using WebAPI_ASP_Net.Utils.Timer;

namespace WebAPI_ASP_NET_Core.Controllers
{

    [ApiController]
    public class StackController : ControllerBase
    {
        const int maxElementSize = 100000 / 2;
        private readonly IStackRepository<int> _stackRepository;
        private readonly ITimer _timer;

        public StackController(IStackRepository<int> stackRepository, ITimer timer)
        {
            _stackRepository = stackRepository;
            _timer = timer;
        }

        [HttpGet("/api/stack")]
        public ActionResult GetAll()
        {
            var items = _stackRepository.GetAll();
            return Ok(items);
        }

        [HttpPost("/api/stack")]
        public ActionResult Add(int item)
        {
            _stackRepository.Add(item);
            return Ok();
        }

        [HttpPut("/api/stack")]
        public ActionResult Update(int index, int newItem)
        {
            bool success = _stackRepository.Update(index, newItem);
            if (success)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete("/api/stack")]
        public ActionResult Delete(int item)
        {
            bool success = _stackRepository.Delete(item);
            if (success)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [Route("/api/stack/add/best")]
        [HttpGet]
        public ActionResult<PerformanceTestModel> GetBest(int maxSize = maxElementSize)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            IStackContainer<int> dataContainer = new StackContainer<int>();

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
                    dataContainer.Stack.Push(i);
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
                TestName = "Stack add (best)",
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
        [Route("/api/stack/add/worst")]
        [HttpGet]
        public ActionResult<PerformanceTestModel> GetWorst(int maxSize = maxElementSize)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            IStackContainer<int> dataContainer = new StackContainer<int>();

            var processMemorySizeBeforeTest = new MemoryInfoMetricModel
            {
                Title = "Process before Test",
                Size = MemoryInfoProvider.GetProcessMemorySize(),
                Type = EMemorySizeType.Byte.ToString(),
            };

            for (int i = 0; i < 2; i++)
            {
                dataContainer.Stack.Push(i);
            }

            _timer.Start();
            for (int i = 2; i < maxSize; i++)
            {
                dataContainer.Insert(0, i);
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
                TestName = "Stack add (worst)",
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

        [Route("api/stack/update/best")]
        [HttpGet]
        public ActionResult<PerformanceTestModel> UpdateBest(int maxSize = maxElementSize)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            IStackContainer<int> dataContainer = new StackContainer<int>();

            // Додайте елементи до стеку
            for (int i = 0; i < maxSize; i++)
            {
                dataContainer.Stack.Push(i);
            }

            var processMemorySizeBeforeTest = new MemoryInfoMetricModel
            {
                Title = "Process before test",
                Size = MemoryInfoProvider.GetProcessMemorySize(),
                Type = EMemorySizeType.Byte.ToString(),
            };

            double executionTimeMs = 0.0;

            // Оновіть елементи
            for (int i = 0; i < maxSize; i++)
            {
                try
                {
                    _timer.Start();
                    dataContainer.Update(maxSize - 1, i + 1); // Оновлення елемента
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
                TestName = "Stack update (best)",
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

        [Route("api/stack/update/worst")]
        [HttpGet]
        public ActionResult<PerformanceTestModel> UpdateWorst(int maxSize = maxElementSize)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            IStackContainer<int> dataContainer = new StackContainer<int>();

            // Додайте елементи до стеку
            for (int i = 0; i < maxSize; i++)
            {
                dataContainer.Stack.Push(i);
            }

            var processMemorySizeBeforeTest = new MemoryInfoMetricModel
            {
                Title = "Process before test",
                Size = MemoryInfoProvider.GetProcessMemorySize(),
                Type = EMemorySizeType.Byte.ToString(),
            };

            double executionTimeMs = 0.0;

            // Оновіть елементи
            for (int i = 0; i < maxSize; i++)
            {
                try
                {
                    _timer.Start();
                    dataContainer.Update(0, i + 1); // Оновлення елемента
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
                TestName = "Stack update (worst)",
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

        [Route("api/stack/remove/best")]
        [HttpGet]
        public ActionResult<PerformanceTestModel> RemoveBest(int maxSize = maxElementSize)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            IStackContainer<int> dataContainer = new StackContainer<int>();

            var processMemorySizeBeforeTest = new MemoryInfoMetricModel
            {
                Title = "Process before test",
                Size = MemoryInfoProvider.GetProcessMemorySize(),
                Type = EMemorySizeType.Byte.ToString(),
            };

            // Додайте елементи до стеку
            for (int i = 0; i < maxSize; i++)
            {
                dataContainer.Stack.Push(i);
            }

            double executionTimeMs = 0.0;

            // Видаліть елементи
            for (int i = 0; i < maxSize; i++)
            {
                try
                {
                    _timer.Start();
                    dataContainer.Stack.Pop(); // Видалення елемента
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
                TestName = "Stack remove (best)",
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
