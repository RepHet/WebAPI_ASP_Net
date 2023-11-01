using System;
using System.Collections.Generic;
using System.Web.Http;
using WebAPI_ASP_Net.Repositories.Containers.List;
using WebAPI_ASP_Net.Repositories.List;
using WebAPI_ASP_Net.Utils;
using WebAPI_ASP_Net.Utils.MemoryUsage;
using WebAPI_ASP_Net.Utils.MetricModels;
using WebAPI_ASP_Net.Utils.Timer;

namespace WebAPI_ASP_Net.Controllers
{
    public class ListController : ApiController
    {
        const int maxElementSize = 268435456 / 2;
        private readonly IListRepository<int> _listRepository;
        private readonly ITimer _timer;

        public ListController(IListRepository<int> listRepository, ITimer timer)
        {
            _listRepository = listRepository;
            _timer = timer;
        }

        [Route("api/list")]
        public IHttpActionResult GetAll()
        {
            var items = _listRepository.GetAll();
            return Ok(items);
        }

        [Route("api/list")]
        [HttpPost]
        public IHttpActionResult Add(int item)
        {
            _listRepository.Add(item);
            return Ok();
        }

        [Route("api/list")]
        [HttpPut]
        public IHttpActionResult Update(int oldItem, int newItem)
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

        [Route("api/list")]
        [HttpDelete]
        public IHttpActionResult Delete(int item)
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
        [Route("api/list/add/best")]
        [HttpGet]
        public IHttpActionResult GetBest(int maxSize = maxElementSize)
        {
            IListContainer<int> listContainer = new ListContainer<int>();

            var heapMetricModelBeforeTest = new HeapInfoMetricModel
            {
                GCHeapSize = MemoryInfoProvider.GetGCHeapSize(),
            };

            _timer.Start();
            for (int i = 0; i < maxSize; i++)
            {
                try {
                    listContainer.List.Add(i);
                } 
                catch
                {
                    break;
                }
            }
            _timer.Stop();

            var heapMetricModelAfterTest = new HeapInfoMetricModel
            {
                GCHeapSize = MemoryInfoProvider.GetGCHeapSize(),
            };

            var executionTimeMetricModel = new ExecutionTimeMetricModel
            {
                ExecutionTimeMs = _timer.ElapsedTime().Milliseconds
            };

            IDictionary<string, IEnumerable<IMetricModel>> metrics = new Dictionary<string, IEnumerable<IMetricModel>>
            {
                {
                    "Before test",
                    new List<IMetricModel> {
                        heapMetricModelBeforeTest
                    }
                },
                {
                    "After test",
                    new List<IMetricModel> {
                        heapMetricModelAfterTest
                    }
                },
                {
                    "Test execution time",
                    new List<IMetricModel> {
                        executionTimeMetricModel
                    }
                },
            };

            PerformanceTestModel performanceResult = new PerformanceTestModel
            {
                TestName = "List add (best)",
                Metrics = metrics
            };

            return Ok(performanceResult);
        }
        [Route("api/list/add/worst")]
        [HttpGet]
        public IHttpActionResult GetWorst(int maxSize = maxElementSize)
        {
            IListContainer<int> listContainer = new ListContainer<int>();
            var heapMetricModelBeforeTest = new HeapInfoMetricModel
            {
                GCHeapSize = MemoryInfoProvider.GetGCHeapSize(),
            };

            _timer.Start();

            listContainer.List.Add(0);
            for (int i = 1; i < maxSize; i++)
            {
                try {
                    listContainer.List.Insert(i - 1, i);
                } 
                catch
                {
                    break;
                }
            }
            _timer.Stop();

            var heapMetricModelAfterTest = new HeapInfoMetricModel
            {
                GCHeapSize = MemoryInfoProvider.GetGCHeapSize(),
            };

            var executionTimeMetricModel = new ExecutionTimeMetricModel
            {
                ExecutionTimeMs = _timer.ElapsedTime().Milliseconds
            };

            IDictionary<string, IEnumerable<IMetricModel>> metrics = new Dictionary<string, IEnumerable<IMetricModel>>
            {
                {
                    "Before test",
                    new List<IMetricModel> {
                        heapMetricModelBeforeTest
                    }
                },
                {
                    "After test",
                    new List<IMetricModel> {
                        heapMetricModelAfterTest
                    }
                },
                {
                    "Test execution time",
                    new List<IMetricModel> {
                        executionTimeMetricModel
                    }
                },
            };

            PerformanceTestModel performanceResult = new PerformanceTestModel
            {
                TestName = "List add (best)",
                Metrics = metrics
            };

            return Ok(performanceResult);
        }
    }
}