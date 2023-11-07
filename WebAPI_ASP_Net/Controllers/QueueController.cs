using System.Web.Http;
using WebAPI_ASP_Net.Repositories.Queue;

namespace WebAPI_ASP_Net.Controllers
{
    public class QueueController : ApiController
    {
        private readonly IQueueRepository<int> _queueRepository;

        public QueueController(IQueueRepository<int> queueRepository)
        {
            _queueRepository = queueRepository;
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
    }
}