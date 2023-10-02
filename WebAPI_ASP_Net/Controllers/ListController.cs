using System.Web.Http;
using WebAPI_ASP_Net.Repositories;

namespace WebAPI_ASP_Net.Controllers
{
    public class ListController : ApiController
    {
        private readonly ICollectionRepository<int> _listRepository;

        public ListController(ICollectionRepository<int> listRepository)
        {
            this._listRepository = listRepository;
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
    }
}