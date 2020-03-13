using Microsoft.AspNetCore.Mvc;

namespace Wjire.Excel.Test.WebApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {

        [HttpGet]
        public string Get()
        {
            return "hello world";
        }
    }
}