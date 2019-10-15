using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http;
using Wjire.ProjectManager.WebApi.Service;

namespace Wjire.ProjectManager.WebApi.Controller
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UploadController : ControllerBase
    {

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage Upload()
        {
            IFormFile file = Request.Form.Files[0];
            Microsoft.Extensions.Primitives.StringValues projectName = HttpContext.Request.Form["ProjectName"];
            if (string.IsNullOrWhiteSpace(projectName))
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            string dir = $@"temp\{projectName}\";
            new FileService().UnpackFiles(file.OpenReadStream(), dir);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}