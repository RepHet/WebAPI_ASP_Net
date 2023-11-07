using System;
using System.Collections.Generic;
using System.Web.Http;
using WebAPI_ASP_Net.Repositories.Containers.Queue;
using WebAPI_ASP_Net.Repositories.Containers.Stack;
using WebAPI_ASP_Net.Repositories.Stack;
using WebAPI_ASP_Net.Utils;
using WebAPI_ASP_Net.Utils.MemoryUsage;
using WebAPI_ASP_Net.Utils.MetricModels;
using WebAPI_ASP_Net.Utils.Models.MetricModels;
using WebAPI_ASP_Net.Utils.Timer;

namespace WebAPI_ASP_Net.Controllers
{
    public class StackController : ApiController
    {

        const int maxElementSize = 100000 / 2;
        private readonly IStackRepository<int> _stackRepository;
        private readonly ITimer _timer;

        public StackController(IStackRepository<int> stackRepository, ITimer timer)
        {
            _stackRepository = stackRepository;
            _timer = timer;
        }

        [Route("api/stack")]
        public IHttpActionResult GetAll()
        {
            var items = _stackRepository.GetAll();
            return Ok(items);
        }

        [Route("api/stack")]
        [HttpPost]
        public IHttpActionResult Add(int item)
        {
            _stackRepository.Add(item);
            return Ok();
        }

        [Route("api/stack")]
        [HttpPut]
        public IHttpActionResult Update(int oldItem, int newItem)
        {
            bool success = _stackRepository.Update(oldItem, newItem);
            if (success)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [Route("api/stack")]
        [HttpDelete]
        public IHttpActionResult Delete(int item)
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

        [Route("api/stack/add/best")]
        [HttpGet]
        public IHttpActionResult GetBest(int maxSize = maxElementSize)
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
        [Route("api/stack/add/worst")]
        [HttpGet]
        public IHttpActionResult GetWorst(int maxSize = maxElementSize)
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