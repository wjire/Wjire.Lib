using System.Text;
using System.Threading.Tasks;

namespace Wjire.ASP.NET.Core
{
    public static partial class Extensions
    {
        public static IWebHostBuilder UseHttpListener(this IWebHostBuilder builder, params string[] urls)
        {
            return builder.UseServer(new HttpListenerServer(urls));
        }

        public static Task WriteAsync(this HttpResponse response, string contents)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(contents);
            return response.Body.WriteAsync(buffer, 0, buffer.Length);
        }
    }
}
