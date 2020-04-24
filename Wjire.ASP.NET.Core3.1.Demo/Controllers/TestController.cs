using Microsoft.AspNetCore.Mvc;
using Wjire.ASP.NET.Core3._1.Demo.Models;
using Wjire.Common;

namespace Wjire.ASP.NET.Core3._1.Demo.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {

        [HttpGet]
        public string Values()
        {
            return "hello world";
        }


        [HttpPost]
        [ServiceFilter(typeof(OperationLogAttribute))]
        public string Person(Person person)
        {
            return person.ToJson();
        }
    }
}