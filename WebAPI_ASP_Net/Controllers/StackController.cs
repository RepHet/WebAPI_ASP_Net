using System.Web.Http;
using WebAPI_ASP_Net.Repositories;
using WebAPI_ASP_Net.Repositories.Stack;

namespace WebAPI_ASP_Net.Controllers
{
    public class StackController : ApiController
    {
        private readonly IStackRepository<int> _stackRepository;

        public StackController(IStackRepository<int> stackRepository)
        {
            _stackRepository = stackRepository;
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
    }
}