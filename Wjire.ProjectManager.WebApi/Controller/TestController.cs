using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Web.Administration;
using Wjire.Log;

namespace Wjire.ProjectManager.WebApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public TestController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public string Get()
        {
            return "Hello World";
        }
    }
}