using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Wjire.ASP.NET.Core3._1.Demo.Extensions
{
    public static class ServicesExtensions
    {
        public static void AddLogics(this IServiceCollection services)
        {
            Assembly ass = AppDomain.CurrentDomain.Load("SCMCC.Salary.Logic");
            foreach (Type type in ass.GetTypes().Where(w => w.Name.EndsWith("Logic")))
            {
                services.AddSingleton(type);
            }
        }
    }
}
