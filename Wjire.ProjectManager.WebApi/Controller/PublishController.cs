using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Wjire.Log;
using Wjire.ProjectManager.WebApi.Model;
using Wjire.ProjectManager.WebApi.Service;

namespace Wjire.ProjectManager.WebApi.Controller
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PublishController : ControllerBase
    {

        [HttpGet]
        public IEnumerable<AppInfo> GetAppInfos(int type)
        {
            try
            {
                return PublishServiceFactory.Create(type).GetAppInfos();
            }
            catch (Exception ex)
            {
                LogService.WriteExceptionAsync(ex, nameof(GetAppInfos), type);
            }

            return new List<AppInfo>();
        }



        /// <summary>
        /// 上传IIS
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage Upload()
        {
            try
            {
                Microsoft.Extensions.Primitives.StringValues appInfoJson = HttpContext.Request.Form["AppInfo"];
                if (string.IsNullOrWhiteSpace(appInfoJson))
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent("未上传程序信息")
                    };
                }
                AppInfo appInfo = JsonConvert.DeserializeObject<AppInfo>(appInfoJson);

                if (Request.Form.Files.Count == 0)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent("未上传程序文件")
                    };
                }
                using (System.IO.Stream stream = Request.Form.Files[0].OpenReadStream())
                {
                    PublishServiceFactory.Create(appInfo).PublishApp(stream);
                }

                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                LogService.WriteExceptionAsync(ex, "Upload");
                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(ex.ToString())
                };
            }
        }
    }
}