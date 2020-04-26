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

        private readonly string privateKey = @"-----BEGIN RSA PRIVATE KEY-----
MIICXAIBAAKBgQCaEfoLMVNOXb9s3D/dds2Y1rQN5NA2G7cw8SZCvbwxP9+lJxLG
u1F+671cuOTJ6NTVCTnhtGtCrmrws8TDay9bEoSHD89595LLvHjWj8hqs9YsQem/
w/9zfqtiQ1z0mlMAr7cQMA/UeGnHr+n3y1tvEoY8DyiMYVCDfbWCTyr87QIDAQAB
AoGAGVOwhvvUy00d6rH0zbMkmRCyXeup+TsVIjaCKPaHuTuGyDS5dscpiR5iQpvN
AGQF7f9WRIQkHby5AliK6pT0Hle1gAjZrk62h8wGHnYDOH7snOtmBjtrWxQmCpWm
59u4HJglQ3eiD/ko/viho0g0qPALLxzP+2Gku/OiCb26ZQECQQDpE20CImO9iVtd
J/w5wb6P5tC3IK+jc5wOiEr60f8jKow85vaGW7hl1lSxKd5hNZ3TUdqQdN7KPvev
KSQ5wtVDAkEAqTlIOSsEB/0+oeZm2QcWi3sdogjFUg+dqOL+i81tCepI+Rr6DbDj
KAi8twFWRBDhGnseIvgLtr+75KXGxTaqDwJBAOAz4Y5GCm/Oa2anCgd9CZRfUbJ2
7L1sfle0X3v6+VSYnyIOgmIoZK8Bh6KMRfB4pQMcIAUJhy5Bd/y0tLYjZwUCQC6y
jPihozIlMzRwJS98oj8JUWsWaoUzo/kn8sBXhuB2k36ScDB5AKZaiuEhcFHGKqgp
E27o7iqXDF2TVZ+0bwcCQGJlKxUb/n3KzYhyCYDnlN4XEN5vK2yhKVwSUFwsT5T6
DLePxokQLgeswPdmD67TxcSqZWF+VEx7Cq87a4sGtc8=
-----END RSA PRIVATE KEY-----";


        [HttpGet]
        public string Values()
        {
            return "hello world";
        }


        [HttpPost]
        [ServiceFilter(typeof(OperationLogAttribute))]
        public string Person(Person person)
        {
            person.Name = RsaHelper.DecryptWithPrivateKey(person.Name, privateKey);
            return person.ToJson();
        }
    }
}