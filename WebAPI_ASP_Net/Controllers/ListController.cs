using System.Web.Http;
using WebAPI_ASP_Net.Repositories.List;
using WebAPI_ASP_Net.Utils;
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
        [Route("api/list/result/best")]
        [HttpGet]
        public IHttpActionResult GetBestResult(int maxSize)
        {
            _timer.Start();
            for (int i = 0; i < maxSize; i++)
            {
                _listRepository.Add(i);
            }
            _timer.Stop();

            MemoryUsageAnalyzer memory = new MemoryUsageAnalyzer();

            PerformanceResult performanceResult = new PerformanceResult
            {
                TestName = "List (Best)",
                ExecutionTimeMs = _timer.ElapsedTime().Milliseconds,
                MemoryUsageBytes = memory.GetMemoryUsage(),
            };
            _listRepository.DeleteAll();
            return Ok(performanceResult);
        }

        //[Route("api/list")]
        //[HttpGet]
        //public IHttpActionResult GetWorstResult(int item)
        //{
        //    for (int i = 0; i < MaxSize; i++)
        //    {
        //        list.Add(i);
        //    }
        //    stopwatch.Start();
        //    for (int i = 0; i < MaxSize; i++)
        //    {
        //        list.Insert(0, i);
        //    }
        //    stopwatch.Stop();
        //    results.Add(new PerformanceResult("List (Worst Case)", stopwatch.ElapsedMilliseconds));
        //    list.Clear();
        //    stopwatch.Reset();
        //}



        //[Route("api/list")]
        //[HttpGet]
        //public IHttpActionResult GetOptimalResult(int item)
        //{

        //    // Середній випадок (зазвичай оптимальний) для List
        //    for (int i = 0; i < MaxSize; i++)
        //    {
        //        list.Add(i);
        //    }
        //    stopwatch.Start();
        //    for (int i = 0; i < MaxSize / 2; i++)
        //    {
        //        list.Remove(i);
        //    }
        //    stopwatch.Stop();
        //    results.Add(new PerformanceResult("List (Average Case)", stopwatch.ElapsedMilliseconds));
        //    list.Clear();
        //    stopwatch.Reset();
        //}
    }
}