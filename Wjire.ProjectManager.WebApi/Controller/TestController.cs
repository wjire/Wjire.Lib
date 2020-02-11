using Microsoft.AspNetCore.Mvc;

namespace Wjire.ProjectManager.WebApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {

        public string Get()
        {
            return "Hello World";
        }
    }
}