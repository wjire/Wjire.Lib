using System.Collections.Specialized;
using System.IO;

namespace Wjire.ASP.NET.Core
{
    public interface IHttpResponseFeature
    {
        int StatusCode { get; set; }
        NameValueCollection Headers { get; }
        Stream Body { get; }
    }
}
