using System;
using System.Collections.Specialized;
using System.IO;

namespace Wjire.ASP.NET.Core
{
    public class HttpRequest
    {
        private readonly IHttpRequestFeature _feature;
        public Uri Url => _feature.Url;
        public NameValueCollection Headers => _feature.Headers;
        public Stream Body => _feature.Body;

        public HttpRequest(IFeatureCollection feature)
        {
            _feature = feature.Get<IHttpRequestFeature>();
        }
    }
}
