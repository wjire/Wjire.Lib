﻿namespace Wjire.ASP.NET.Core
{
    public interface IServer
    {
        void StartAsync(RequestDelegate handler);
    }
}
