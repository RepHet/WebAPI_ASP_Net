﻿using Microsoft.AspNetCore.Mvc;
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
    [Route("api/[controller]")]
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

        [HttpGet]
        public ActionResult GetAll()
        {
            var items = _queueRepository.GetAll();
            return Ok(items);
        }

        [HttpPost]
        public ActionResult Add(int item)
        {
            _queueRepository.Add(item);
            return Ok();
        }

        [HttpPut]
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

        [HttpDelete]
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

        [Route("api/queue/add/best")]
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
        [Route("api/queue/add/worst")]
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
    }
}
