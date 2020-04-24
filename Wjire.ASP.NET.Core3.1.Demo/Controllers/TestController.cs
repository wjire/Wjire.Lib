using Microsoft.AspNetCore.Mvc;
using Wjire.ASP.NET.Core3._1.Demo.Models;
using Wjire.Common;
using Wjire.Encrypt;

namespace Wjire.ASP.NET.Core3._1.Demo.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {

        private string privateKey = "MIICWwIBAAKBgQCUfP6Cu785zTLlQBtLGFBvRomDrE0/0AehIGjdIfLK5rG4SuK1V2vEP7A3Kbwllka1dUWDTw85oxhHKi/6ewwuOQ0IhRMZRD0slpt64LCd3Nc6IJ4C8K8G4hMLmWgocd0pkTZ8dV1d9/LMiefkeHdj267BHH9LKy5zYiJKxVxs+wIDAQABAoGADsa4Jlb75YrySKFDgLU6Xohna3n8SfWv/Kxr+FgbgBjK5gBVunP+SQ40Fpk0RuKYcLrrIEZVc90zWkmWyAHx/omlNgBz78htdCtQX2pWSqK5qgRDBjakI2a0sdM7qPEqeUu6eVuYw1nPjN4VLlZtjXdRTbRNf8G/KudQmir56ekCQQDiM3w+xcHRzYRNmwZil3pK3Pimtujgbet7kCXYy4M+ebaqs/LQoDCn4hTjUphpoPPpN9Q1o96uLuVoE0bQoNiPAkEAqAyu7DeV822G/na5ZPKyjtatAhyZYGjooBERHhhn0+gv3DH484rvKiMPwCYJES8g+UBcntGHjPbkq1/Okwbi1QJASsGPL0O2EXtlXSx9yLmdHf63YomSnxFUwMpb57Eil9QrzXCZL9+xFNq/4nzaiBY8ZfggdvXIUzKGeIXeYna1+wJACpe6EvOyBQUbZEDRkNZngO9xOlfZJyWVvDk3LgF10fqpwaR4v8k7KBnLrV34ZlflKTDwZaZpd+48Hb1NRSFC/QJADz1aBlSVFQ7mbQm1UVOECLgEC6ZG2F86Qn/flKbUcHD3B5gFjBmI0X0psSnJ1Z9mH30w/SXhWOtiPFTpgklKjA==";


        [HttpGet]
        public string Values()
        {
            return "hello world";
        }


        [HttpPost]
        [ServiceFilter(typeof(OperationLogAttribute))]
        public string Person(Person person)
        {
            person.Name = RsaHelper.DecryptWithPrivateKey2<string>(person.Name, privateKey);
            return person.ToJson();
        }
    }
}