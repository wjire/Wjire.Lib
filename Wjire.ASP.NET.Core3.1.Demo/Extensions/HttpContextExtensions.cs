using System.IO;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Wjire.ASP.NET.Core3._1.Demo.JWT;
using Wjire.Common;

namespace Wjire.ASP.NET.Core3._1.Demo.Extensions
{
    public static class HttpContextExtensions
    {

        public static LoginUser GetLoginUser(this HttpContext context)
        {
            string userData = context.User.FindFirstValue(ClaimTypes.UserData);
            if (userData.IsNullOrWhiteSpace())
            {
                return null;
            }
            return userData.ToObject<LoginUser>();
        }


        public static async Task<string> GetRequestBody(this HttpContext context)
        {
            HttpRequest request = context.Request;
            string contentType = request.ContentType.ToLower();
            if (request.Method.ToLower() == "post" && (contentType == "json" || contentType == "application/json"))
            {
                request.Body.Seek(0, SeekOrigin.Begin);
                using (StreamReader reader = new StreamReader(request.Body, Encoding.UTF8))
                {
                    await reader.ReadToEndAsync();
                };
            }
            return JsonConvert.SerializeObject(request.Query);
        }


        public static string GetControllerAndAction(this ActionContext context)
        {
            string controllerName = (string)context.RouteData.Values["controller"];
            string actionName = (string)context.RouteData.Values["action"];
            return controllerName + "\\" + actionName;
        }
    }
}
