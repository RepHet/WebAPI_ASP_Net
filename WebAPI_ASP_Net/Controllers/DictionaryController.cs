using System;
using System.Collections.Generic;
using System.Web.Http;
using WebAPI_ASP_Net.Repositories;
using WebAPI_ASP_Net.Repositories.Containers.Dictionary;
using WebAPI_ASP_Net.Utils;
using WebAPI_ASP_Net.Utils.MemoryUsage;
using WebAPI_ASP_Net.Utils.MetricModels;
using WebAPI_ASP_Net.Utils.Models.MetricModels;
using WebAPI_ASP_Net.Utils.Timer;

namespace WebAPI_ASP_Net.Controllers
{
    public class DictionaryController : ApiController
    {
        const int maxElementSize = 268435456 / 2;
        
        private readonly IDictionaryRepository<int, int> _dictionaryRepository;

        private readonly ITimer _timer;

        public DictionaryController(IDictionaryRepository<int, int> dictionaryRepository, ITimer timer)
        {
            _dictionaryRepository = dictionaryRepository;
            _timer = timer;
        }

        [Route("api/dictionary")]
        public IHttpActionResult GetAll()
        {
            var items = _dictionaryRepository.GetAll();
            return Ok(items);
        }

        [Route("api/dictionary")]
        [HttpPost]
        public IHttpActionResult Add(KeyValuePair<int, int> item)
        {
            _dictionaryRepository.Add(item);
            return Ok();
        }

        [Route("api/dictionary")]
        [HttpPut]
        public IHttpActionResult Update(int key, int newItem)
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

        [Route("api/dictionary")]
        [HttpDelete]
        public IHttpActionResult Delete(KeyValuePair<int, int> item)
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
        [Route("api/dictionary/add/best")]
        [HttpGet]
        public IHttpActionResult GetBest(int maxSize = maxElementSize)
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
                Metrics = new Dictionary<string, IEnumerable<IMetricModel>>
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

        [Route("api/dictionary/add/worst")]
        [HttpGet]
        public IHttpActionResult GetWorst(int maxSize = maxElementSize)
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
                Metrics = new Dictionary<string, IEnumerable<IMetricModel>>
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