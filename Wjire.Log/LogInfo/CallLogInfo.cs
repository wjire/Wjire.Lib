using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Wjire.Log
{
    internal class CallLogInfo : BaseLogInfo
    {
        private readonly string _method;
        private readonly object _request;
        private readonly object _response;

        public CallLogInfo(string method, object request, object response, string relativePath)
        {
            _method = method;
            _request = request;
            _response = response;
            RelativePath = relativePath ?? "Logs/CallLog";
        }


        protected override string ToContent()
        {
            StringBuilder sb = new StringBuilder(512);
            sb.Append("************************Start********************************");
            sb.Append(Environment.NewLine);
            sb.AppendFormat("Method：{0}{1}", _method, Environment.NewLine);
            sb.AppendFormat("Request:{0}{1}", _request == null ? string.Empty : JsonConvert.SerializeObject(_request), Environment.NewLine);
            sb.AppendFormat("Response:{0}{1}", _response == null ? string.Empty : JsonConvert.SerializeObject(_response), Environment.NewLine);
            sb.Append("************************End************************************");
            sb.Append(Environment.NewLine);
            return sb.ToString();
        }
    }
}
