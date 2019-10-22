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

        public string Get()
        {
            return "Hello World";
        }
    }
}