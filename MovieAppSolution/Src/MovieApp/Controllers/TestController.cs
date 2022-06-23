using Microsoft.AspNetCore.Mvc;

namespace MovieApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "Ovo je test!";
        }
    }
}
