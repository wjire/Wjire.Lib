using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Wjire.ASP.NET.Core3._1.Demo.JWT;

namespace Wjire.ASP.NET.Core3._1.Demo.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [BearerAuthorize]
    public class JwtController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly JwtTokenUtil _jwtTokenUtil;
        private readonly JwtBlackCollection _jwtBlackCollection;

        public JwtController(IConfiguration configuration, JwtTokenUtil jwtTokenUtil, JwtBlackCollection jwtBlackCollection)
        {
            _configuration = configuration;
            _jwtTokenUtil = jwtTokenUtil;
            _jwtBlackCollection = jwtBlackCollection;
        }


        [HttpPost]
        public IActionResult Login()
        {
            int tokenExpired = Convert.ToInt32(_configuration[JwtConst.TokenExpiredHours]);
            string token = _jwtTokenUtil.CreateToken(new LoginUser(), tokenExpired);
            return Ok(token);
        }


        /// <summary>
        /// 退出
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Logout()
        {
            string jti = User.FindFirst(JwtRegisteredClaimNames.Jti).Value;
            if (string.IsNullOrWhiteSpace(jti) == false)
            {
                _jwtBlackCollection.Add(jti);
            }
            return Ok();
        }
    }
}