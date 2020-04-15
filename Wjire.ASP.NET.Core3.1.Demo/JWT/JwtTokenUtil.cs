using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Wjire.ASP.NET.Core3._1.Demo.JWT;

namespace Wjire.ASP.NET.Core3._1.Demo
{

    /// <summary>
    /// JwtToken工具
    /// </summary>
    public class JwtTokenUtil
    {

        private readonly IConfiguration _configuration;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

        public JwtTokenUtil(IConfiguration configuration, JwtSecurityTokenHandler jwtSecurityTokenHandler)
        {
            _configuration = configuration;
            _jwtSecurityTokenHandler = jwtSecurityTokenHandler;
        }


        /// <summary>
        /// 生成 token
        /// </summary>
        /// <param name="loginUser"></param>
        /// <param name="tokenExpired">小时</param>
        /// <returns></returns>
        public string CreateToken(LoginUser loginUser, int tokenExpired)
        {
            // push the user’s name into a claim, so we can identify the user later on.
            Claim[] claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString() ),
                new Claim(ClaimTypes.UserData, JsonConvert.SerializeObject(loginUser)),
                new Claim(JwtRegisteredClaimNames.Nbf,$"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}") ,
                new Claim (JwtRegisteredClaimNames.Exp,$"{new DateTimeOffset(DateTime.Now.AddHours(tokenExpired)).ToUnixTimeSeconds()}"),
                new Claim(ClaimTypes.Role,loginUser.IsAdmin==1?"Admin":"Common")//在这可以分配用户角色，比如管理员 、 vip会员 、 普通用户等
            };

            //sign the token using a secret key.This secret will be shared between your API and anything that needs to check that the token is legit.
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecurityKey"])); // 获取密钥
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); //凭证 ，根据密钥生成
            //.NET Core’s JwtSecurityToken class takes on the heavy lifting and actually creates the token.
            /**
             * Claims (Payload)
                Claims 部分包含了一些跟这个 token 有关的重要信息。 JWT 标准规定了一些字段，下面节选一些字段:

                iss: The issuer of the token，token 是给谁的  发送者
                aud: 接收的
                sub: The subject of the token，token 主题
                exp: Expiration Time。 token 过期时间，Unix 时间戳格式
                iat: Issued At。 token 创建时间， Unix 时间戳格式
                jti: JWT ID。针对当前 token 的唯一标识
                除了规定的字段外，可以包含其他任何 JSON 兼容的字段。
             * */
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _configuration[JwtConst.JwtIssuer],
                audience: _configuration[JwtConst.JwtAudience],
                claims: claims,
                expires: DateTime.Now.AddHours(tokenExpired),
                signingCredentials: creds
            );
            return _jwtSecurityTokenHandler.WriteToken(token);
        }



        ///// <summary>
        ///// 存储新token
        ///// </summary>
        ///// <param name="loginName"></param>
        ///// <param name="tokenInfo"></param>
        ///// <returns></returns>
        //public bool StoreNewToken(string loginName, TokenInfo tokenInfo)
        //{
        //    return RedisHelper.Set(loginName + ".token", tokenInfo, (int)tokenInfo.Expired * 60 * 60);
        //}



        ///// <summary>
        ///// 删除已存在的token,并加入黑名单.防止多个地方登录
        ///// </summary>
        ///// <param name="loginName"></param>
        ///// <returns></returns>
        //public bool CancelExistedToken(string loginName)
        //{
        //    CSRedis.CSRedisClient redis = RedisClientFactory.CreateRedisClient(RedisClientName.Advertiser);
        //    TokenInfo tokenInfo = redis.Get<TokenInfo>(loginName + ".token");
        //    if (tokenInfo == null)
        //    {
        //        return true;
        //    }
        //    JwtBlackCollection.Singleton.Value.Add(tokenInfo.Jti);
        //    long count = redis.Del(loginName + ".token");
        //    return count > 0;
        //}



        ///// <summary>
        ///// 解析token
        ///// </summary>
        ///// <param name="token"></param>
        ///// <returns></returns>
        //public LoginUserDto SerializeJwt(string token, out object jti)
        //{
        //    try
        //    {
        //        JwtSecurityTokenHandler jwtHandler = new JwtSecurityTokenHandler();
        //        JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(token);
        //        jwtToken.Payload.TryGetValue(ClaimTypes.UserData, out object loginUserDto);
        //        jwtToken.Payload.TryGetValue(JwtRegisteredClaimNames.Jti, out jti);
        //        return loginUserDto as LoginUserDto;
        //    }
        //    catch (Exception ex)
        //    {
        //        LogService.WriteLog(ex, "解析token", token);
        //        jti = null;
        //        return null;
        //    }
        //}
    }
}
