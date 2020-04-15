using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Wjire.Common;

namespace Wjire.ASP.NET.Core3._1.Demo.Utils
{

    /// <summary>
    /// 校验上传文件类型是否允许
    /// </summary>
    public class UploadedFileCheck
    {
        private readonly IConfiguration _configuration;

        public UploadedFileCheck(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public void ExtensionCheck(string uploadedFileName)
        {
            string[] permittedExtensions =
                _configuration.GetSection("PermittedExtensions").Get<string[]>();
            string ext = Path.GetExtension(uploadedFileName).ToLowerInvariant();
            if (ext.IsNullOrWhiteSpace() || permittedExtensions.Contains(ext) == false)
            {
                throw new CustomException("文件类型错误!");
            }
        }
    }
}
