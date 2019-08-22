using System;
using System.Collections.Specialized;
using System.IO;

namespace Wjire.ASP.NET.Core
{
    public interface IHttpRequestFeature
    {
        Uri Url { get; }
        NameValueCollection Headers { get; }
        Stream Body { get; }
    }
}
