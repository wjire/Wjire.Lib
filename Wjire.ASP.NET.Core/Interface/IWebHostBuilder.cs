using System;
using System.Collections.Generic;

namespace Wjire.ASP.NET.Core
{
    public interface IWebHostBuilder
    {
        IWebHostBuilder UseServer(IServer server);
        IWebHostBuilder Configure(Action<IApplicationBuilder> configure);
        IWebHost Build();
    }
   
}
