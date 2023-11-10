using Microsoft.AspNetCore.Mvc;
using WebAPI_ASP_Net.Repositories.Containers.List;
using WebAPI_ASP_Net.Repositories.List;
using WebAPI_ASP_Net.Utils;
using WebAPI_ASP_Net.Utils.MemoryUsage;
using WebAPI_ASP_Net.Utils.MetricModels;
using WebAPI_ASP_Net.Utils.Models.MetricModels;
using WebAPI_ASP_Net.Utils.Timer;

namespace WebAPI_ASP_NET_Core.Controllers
{
    [ApiController]
    public class ListController : ControllerBase
    {
        const int maxElementSize = 268435456 / 2;
        private readonly IListRepository<int> _listRepository;
        private readonly ITimer _timer;

        public ListController(IListRepository<int> listRepository, ITimer timer)
        {
            _listRepository = listRepository;
            _timer = timer;
        }

        [HttpGet("/api/list")]
        public ActionResult GetAll()
        {
            var items = _listRepository.GetAll();
            return Ok(items);
        }

        [HttpPost("/api/list")]
        public ActionResult Add(int item)
        {
            _listRepository.Add(item);
            return Ok();
        }

        [HttpPut("/api/list")]
        public ActionResult Update(int oldItem, int newItem)
        {
            bool success = _listRepository.Update(oldItem, newItem);
            if (success)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete("/api/list")]
        public ActionResult Delete(int item)
        {
            bool success = _listRepository.Delete(item);
            if (success)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
        [HttpGet("/api/list/add/best")]
        public ActionResult<PerformanceTestModel> GetBest(int maxSize = maxElementSize)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            IListContainer<int> listContainer = new ListContainer<int>();

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
                    listContainer.List.Add(i);
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
                TestName = "List add (best)",
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
        [HttpGet("/api/list/add/worst")]
        public ActionResult<PerformanceTestModel> GetWorst(int maxSize = maxElementSize)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            IListContainer<int> listContainer = new ListContainer<int>();
            var processMemorySizeBeforeTest = new MemoryInfoMetricModel
            {
                Title = "Process before Test",
                Size = MemoryInfoProvider.GetProcessMemorySize(),
                Type = EMemorySizeType.Byte.ToString(),
            };

            _timer.Start();
            listContainer.List.Add(0);
            for (int i = 1; i < maxSize; i++)
            {
                try
                {
                    listContainer.List.Insert(i - 1, i);
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
                TestName = "List add (worst)",
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
