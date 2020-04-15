using Microsoft.AspNetCore.Authorization;

namespace Wjire.ASP.NET.Core3._1.Demo
{

    /// <summary>
    /// Jwt 验证
    /// </summary>
    public class BearerAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        public BearerAuthorizeAttribute() : base("Bearer") { }
    }
}