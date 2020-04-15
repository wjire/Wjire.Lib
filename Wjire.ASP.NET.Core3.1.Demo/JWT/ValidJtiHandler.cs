using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Wjire.ASP.NET.Core3._1.Demo
{

    /// <summary>
    /// Jti验证者
    /// </summary>
    public class ValidJtiHandler : AuthorizationHandler<ValidJtiRequirement>
    {
        private readonly JwtBlackCollection _jwtBlackCollection;

        public ValidJtiHandler(JwtBlackCollection jwtBlackCollection)
        {
            _jwtBlackCollection = jwtBlackCollection;
        }


        /// <summary>
        /// 验证Jti
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ValidJtiRequirement requirement)
        {
            // 检查 Jti 是否存在
            string jti = context.User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
            if (string.IsNullOrWhiteSpace(jti))
            {
                context.Fail(); // 显式的声明验证失败
                return Task.CompletedTask;
            }

            // 检查 jti 是否在黑名单
            bool tokenExists = _jwtBlackCollection.IsExists(jti);
            if (tokenExists)
            {
                context.Fail();
            }
            else
            {
                context.Succeed(requirement); // 显式的声明验证成功
            }
            return Task.CompletedTask;
        }
    }
}
