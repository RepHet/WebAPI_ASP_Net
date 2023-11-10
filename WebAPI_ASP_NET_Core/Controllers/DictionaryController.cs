using Microsoft.AspNetCore.Mvc;
using WebAPI_ASP_Net.Repositories;
using WebAPI_ASP_Net.Repositories.Containers.Dictionary;
using WebAPI_ASP_Net.Utils;
using WebAPI_ASP_Net.Utils.MemoryUsage;
using WebAPI_ASP_Net.Utils.MetricModels;
using WebAPI_ASP_Net.Utils.Models.MetricModels;
using WebAPI_ASP_Net.Utils.Timer;

namespace WebAPI_ASP_NET_Core.Controllers
{
    [ApiController]
    public class DictionaryController : ControllerBase
    {
        const int maxElementSize = 268435456 / 2;

        private readonly IDictionaryRepository<int, int> _dictionaryRepository;

        private readonly ITimer _timer;

        public DictionaryController(IDictionaryRepository<int, int> dictionaryRepository, ITimer timer)
        {
            _dictionaryRepository = dictionaryRepository;
            _timer = timer;
        }

        [HttpGet("/api/dictionary")]
        public ActionResult GetAll()
        {
            var items = _dictionaryRepository.GetAll();
            return Ok(items);
        }

        [HttpPost("/api/dictionary")]
        public ActionResult Add(KeyValuePair<int, int> item)
        {
            _dictionaryRepository.Add(item);
            return Ok();
        }

        [HttpPut("/api/dictionary")]
        public ActionResult Update(int key, int newItem)
        {
            bool success = _dictionaryRepository.Update(key, newItem);
            if (success)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete("/api/dictionary")]
        public ActionResult Delete(KeyValuePair<int, int> item)
        {
            bool success = _dictionaryRepository.Delete(item);
            if (success)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
        [Route("/api/dictionary/add/best")]
        [HttpGet]
        public ActionResult<PerformanceTestModel> GetBest(int maxSize = maxElementSize)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            IDictionaryContainer<int, int> dictionaryContainer = new DictionaryContainer<int, int>();

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
                    dictionaryContainer.Dictionary.Add(i, i);
                    _timer.Stop();

                    dictionaryContainer.Dictionary.Clear();

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
                TestName = "Dictionary add (best)",
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

        [Route("/api/dictionary/add/worst")]
        [HttpGet]
        public ActionResult<PerformanceTestModel> GetWorst(int maxSize = maxElementSize)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            IDictionaryContainer<int, int> dictionaryContainer = new DictionaryContainer<int, int>();

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
                    dictionaryContainer.Dictionary.Add(i, i);
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
                TestName = "Dictionary add (worst)",
                Metrics = new Dictionary<string, IEnumerable<object>>
                {
                    {
                        "Test execution time",
                        new List<ExecutionTimeMetricModel>
                        {
                            executionTime
                        }
                    },
                    {
                        "Memory",
                        new List<MemoryInfoMetricModel>
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
