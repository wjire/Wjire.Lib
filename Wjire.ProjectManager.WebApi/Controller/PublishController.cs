using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Wjire.ProjectManager.WebApi.Model;
using Wjire.ProjectManager.WebApi.Service;

namespace Wjire.ProjectManager.WebApi.Controller
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PublishController : ControllerBase
    {
        private readonly PublishService _publishService;

        public PublishController(PublishService publishService)
        {
            _publishService = publishService;
        }


        public IEnumerable<AppInfo> GetAllApps()
        {
            return _publishService.GetAllIISAppInfo();
        }



        /// <summary>
        /// 上传
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage Upload()
        {
            try
            {
                Microsoft.Extensions.Primitives.StringValues appIdString = HttpContext.Request.Form["AppId"];
                if (string.IsNullOrWhiteSpace(appIdString))
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent("未上传程序Id")
                    };
                }
                long appId = Convert.ToInt64(appIdString);
                if (Request.Form.Files.Count == 0)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent("未上传程序文件")
                    };
                }
                using (System.IO.Stream stream = Request.Form.Files[0].OpenReadStream())
                {
                    _publishService.Publish(appId, stream);
                }

                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(ex.ToString())
                };
            }
        }
    }
}