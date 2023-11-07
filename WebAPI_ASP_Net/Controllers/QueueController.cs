using System.Collections.Generic;
using System;
using System.Web.Http;
using WebAPI_ASP_Net.Repositories.Containers.List;
using WebAPI_ASP_Net.Repositories.Queue;
using WebAPI_ASP_Net.Utils.MemoryUsage;
using WebAPI_ASP_Net.Utils.MetricModels;
using WebAPI_ASP_Net.Utils.Models.MetricModels;
using WebAPI_ASP_Net.Utils;
using WebAPI_ASP_Net.Utils.Timer;
using WebAPI_ASP_Net.Repositories.Containers.Queue;
using System.Reflection;
using System.Linq;

namespace WebAPI_ASP_Net.Controllers
{
    public class QueueController : ApiController
    {
        const int maxElementSize = 100000 / 2;
        private readonly IQueueRepository<int> _queueRepository;
        private readonly ITimer _timer;

        public QueueController(IQueueRepository<int> queueRepository, ITimer timer)
        {
            _queueRepository = queueRepository;
            _timer = timer;
        }

        [Route("api/queue")]
        public IHttpActionResult GetAll()
        {
            var items = _queueRepository.GetAll();
            return Ok(items);
        }

        [Route("api/queue")]
        [HttpPost]
        public IHttpActionResult Add(int item)
        {
            _queueRepository.Add(item);
            return Ok();
        }

        [Route("api/queue")]
        [HttpPut]
        public IHttpActionResult Update(int oldItem, int newItem)
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

        [Route("api/queue")]
        [HttpDelete]
        public IHttpActionResult Delete(int item)
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
        public IHttpActionResult GetBest(int maxSize = maxElementSize)
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
                Metrics = new Dictionary<string, IEnumerable<IMetricModel>>
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
        public IHttpActionResult GetWorst(int maxSize = maxElementSize)
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
                Metrics = new Dictionary<string, IEnumerable<IMetricModel>>
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