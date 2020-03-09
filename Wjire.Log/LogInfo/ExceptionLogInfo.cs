using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Wjire.Log
{
    internal class ExceptionLogInfo : BaseLogInfo
    {
        private readonly Exception _exception;
        private readonly string _remark;
        private readonly object _request;
        private readonly object _response;

        public ExceptionLogInfo(Exception exception, string remark, object request, object response, string relativePath)
        {
            _exception = exception;
            _remark = remark;
            _request = request;
            _response = response;
            RelativePath = relativePath ?? "Logs/ExceptionLog";
        }


        protected override string ToContent()
        {
            StringBuilder stringBuilder = new StringBuilder(512);
            stringBuilder.Append("************************Exception Start********************************");
            string newLine = Environment.NewLine;
            stringBuilder.Append(newLine);
            stringBuilder.AppendLine("Exception Remark：" + _remark);
            stringBuilder.AppendLine("Exception Request：" + (_request == null ? null : JsonConvert.SerializeObject(_request)));
            stringBuilder.AppendLine("Exception Response：" + (_response == null ? null : JsonConvert.SerializeObject(_response)));
            Exception innerException = _exception.InnerException;
            stringBuilder.AppendFormat("Exception Date:{0}{1}", DateTime.Now, Environment.NewLine);
            if (innerException != null)
            {
                stringBuilder.AppendFormat("Inner Exception Type:{0}{1}", innerException.GetType(), newLine);
                stringBuilder.AppendFormat("Inner Exception Message:{0}{1}", innerException.Message, newLine);
                stringBuilder.AppendFormat("Inner Exception Source:{0}{1}", innerException.Source, newLine);
                stringBuilder.AppendFormat("Inner Exception StackTrace:{0}{1}", innerException.StackTrace, newLine);
            }
            stringBuilder.AppendFormat("Exception Type:{0}{1}", _exception.GetType(), newLine);
            stringBuilder.AppendFormat("Exception Message:{0}{1}", _exception.Message, newLine);
            stringBuilder.AppendFormat("Exception Source:{0}{1}", _exception.Source, newLine);
            stringBuilder.AppendFormat("Exception StackTrace:{0}{1}", _exception.StackTrace, newLine);
            stringBuilder.Append("************************Exception End************************************");
            stringBuilder.Append(newLine);
            return stringBuilder.ToString();
        }
    }
}
