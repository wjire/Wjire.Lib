using System;
using System.Linq;
using System.Net;

namespace Wjire.ASP.NET.Core
{
    public class HttpListenerServer : IServer
    {
        private readonly HttpListener _httpListener;
        private readonly string[] _urls;

        public HttpListenerServer(params string[] urls)
        {
            _httpListener = new HttpListener();
            _urls = urls.Any() ? urls : new string[] { "http://localhost:5000/" };
        }

        public void StartAsync(RequestDelegate handler)
        {
            Array.ForEach(_urls, url => _httpListener.Prefixes.Add(url));
            _httpListener.Start();
            while (true)
            {
                HttpListenerContext listenerContext = _httpListener.GetContextAsync().Result;
                HttpListenerFeature feature = new HttpListenerFeature(listenerContext);
                IFeatureCollection features = new FeatureCollection()
                    .Set<IHttpRequestFeature>(feature)
                    .Set<IHttpResponseFeature>(feature);
                HttpContext httpContext = new HttpContext(features);
                handler(httpContext);
                listenerContext.Response.Close();
            }
        }
    }
}
