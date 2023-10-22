using System;
using System.Collections.Generic;
using System.Web.Http;
using WebAPI_ASP_Net.Repositories.Containers.List;
using WebAPI_ASP_Net.Repositories.List;
using WebAPI_ASP_Net.Utils;
using WebAPI_ASP_Net.Utils.MemoryUsage;
using WebAPI_ASP_Net.Utils.Timer;

namespace WebAPI_ASP_Net.Controllers
{
    public class ListController : ApiController
    {
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
        public IHttpActionResult PostBest(int maxSize = 268435456/2)
        {
            IListContainer<int> listContainer = new ListContainer<int>();

            var memoryBeforeTest = new MemoryMetricsModel
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

            var memoryAfterTest = new MemoryMetricsModel
            {
                GCHeapSize = MemoryInfoProvider.GetGCHeapSize(),
            };

            Dictionary<string, MemoryMetricsModel> memoriesUsage = new Dictionary<string, MemoryMetricsModel>
            {
                { "Before test", memoryBeforeTest },
                { "After test", memoryAfterTest }
            };

            PerformanceResult performanceResult = new PerformanceResult
            {
                TestName = "List add (best)",
                ExecutionTimeMs = _timer.ElapsedTime().Milliseconds,
                MemoriesUsage = memoriesUsage,
            };

            return Ok(performanceResult);
        }
        [Route("api/list/add/worst")]
        [HttpGet]
        public IHttpActionResult PostWorst(int maxSize = 268435456/2)
        {
            IListContainer<int> listContainer = new ListContainer<int>();
            var memoryBeforeTest = new MemoryMetricsModel
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

            var memoryAfterTest = new MemoryMetricsModel
            {
                GCHeapSize = MemoryInfoProvider.GetGCHeapSize(),
            };

            Dictionary<string, MemoryMetricsModel> memoriesUsage = new Dictionary<string, MemoryMetricsModel>
            {
                { "Before test", memoryBeforeTest },
                { "After test", memoryAfterTest }
            };

            PerformanceResult performanceResult = new PerformanceResult
            {
                TestName = "List add (worst)",
                ExecutionTimeMs = _timer.ElapsedTime().Milliseconds,
                MemoriesUsage = memoriesUsage,
            };

            return Ok(performanceResult);
        }
    }
}