using airbnbServer.BL;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace airbnbServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VacationsController : ControllerBase
    {
        // GET: api/<VacationsController>
        [HttpGet]
        public IEnumerable<Vacation> Get()
        {
            Vacation v = new Vacation();
            return v.Read();
        }

        //// GET: api/<VacationsController>
        [HttpGet("getByDates/{startDate}/{endDate}")]
        public IActionResult getByDates(DateTime startDate, DateTime endDate)
        {
            Vacation v = new Vacation();
            List<Vacation> filterdL = v.getByDates(startDate, endDate);
            //ולידציה שבוצעה במטלה 1 לא מוחק אותה כי אמרנו שאין צורך לבצע התאמות בקונטרולר עבור מטלה 3
            if (endDate < startDate)
            {
                return BadRequest("End date must be greater than start date");

            }
            if (filterdL.Count == 0)
            {
                return NotFound("sorry no such vications");
            }
            return Ok(filterdL);
        }


        // GET api/<VacationsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<VacationsController>
        [HttpPost]
        public IActionResult Post([FromBody] Vacation v)
        {
            //ולידציה שבוצעה במטלה 1 לא מוחק אותה כי אמרנו שאין צורך לבצע התאמות בקונטרולר עבור מטלה 3
            if (v.StartDate > v.EndDate)
            {
                return BadRequest("End date must be greater than start date");
            }
            else
            {
                return Ok(v.insert());
            }
        }

        // PUT api/<VacationsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<VacationsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
