using airbnbServer.BL;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace airbnbServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlatsController : ControllerBase
    {
        // GET: api/<FlatsController>
        [HttpGet]
        public IEnumerable<Flat> Get()
        {
            Flat f = new Flat();
            return f.Read();
        }

        [HttpGet("q")]
        public IActionResult GetFlatsByCityPrice(string city, double price)
        {
            Flat f = new Flat();
            List<Flat> list = f.FlatsByCityPrice(city, price);
            if (list.Count == 0)
            {
                return NotFound("sorry no such flats");
            }
            return Ok(list);
        }

        // GET api/<FlatsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<FlatsController>
        [HttpPost]
        public int Post([FromBody] Flat f)
        {
            return f.Insert();
        }

        // PUT api/<FlatsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value) //עדכון
        {
        }

        // DELETE api/<FlatsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
