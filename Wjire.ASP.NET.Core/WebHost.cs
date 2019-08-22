using System;
using System.Collections.Generic;
using System.Text;

namespace Wjire.ASP.NET.Core
{
    public class WebHost : IWebHost
    {
        private readonly IServer _server;
        private readonly RequestDelegate _handler;
        public WebHost(IServer server, RequestDelegate handler)
        {
            _server = server;
            _handler = handler;
        }
        public void StartAsync() => _server.StartAsync(_handler);
    }
}
