using System.Collections.Generic;
using System.Web.Http;
using WebAPI_ASP_Net.Repositories;

namespace WebAPI_ASP_Net.Controllers
{
    public class DictionaryController : ApiController
    {
        private readonly IDictionaryRepository<int, int> _dictionaryRepository;

        public DictionaryController(IDictionaryRepository<int, int> dictionaryRepository)
        {
            _dictionaryRepository = dictionaryRepository;
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
        public IHttpActionResult Update(KeyValuePair<int, int> oldItem, KeyValuePair<int, int> newItem)
        {
            bool success = _dictionaryRepository.Update(oldItem, newItem);
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
    }
}