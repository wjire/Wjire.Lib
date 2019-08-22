using System.Threading.Tasks;

namespace Wjire.ASP.NET.Core
{
    public delegate Task RequestDelegate(HttpContext context);
}
