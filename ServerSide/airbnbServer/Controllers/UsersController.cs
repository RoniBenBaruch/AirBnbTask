using airbnbServer.BL;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace airbnbServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        // GET: api/<UserController>
        [HttpGet]
        public IEnumerable<User> Get()
        {
            User u = new User();
            return u.Read(); 
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UserController>
        [HttpPost]
        public int Post([FromBody] User u)
        {
            return u.Insert();
        }

        [HttpPost]
        [Route("LogIn")]
        public User PostLogIn([FromBody] User u)
        {
            DBservices dbs = new DBservices();
            User loggedInUser = dbs.LogIn(u.Email, u.Password);
            return loggedInUser;

        }


        // PUT api/<UserController>/5
        [HttpPut]
        public int Put([FromBody] User user)
        {
            DBservices dbs = new DBservices();
            return dbs.Update(user);
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        ////avgPriceForCityByMonth
        [HttpGet("month/{month}")]
        public List<Object> GetAvgPriceByCityAndMonth(int month)
        {
            DBservices dbs = new DBservices();

            List<Object> avgPrices = dbs.GetAverage(month);

            return avgPrices;
        }
    }
}
