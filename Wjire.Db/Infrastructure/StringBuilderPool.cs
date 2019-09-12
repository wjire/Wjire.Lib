using Microsoft.Extensions.ObjectPool;
using System.Text;

namespace Wjire.Db.Infrastructure
{

    /// <summary>
    /// StringBuilderPool
    /// </summary>
    public static class StringBuilderPool
    {
        private static readonly DefaultObjectPool<StringBuilder> SbPool;

        static StringBuilderPool()
        {
            StringBuilderPooledObjectPolicy sbPolicy = new StringBuilderPooledObjectPolicy();
            SbPool = new DefaultObjectPool<StringBuilder>(sbPolicy);
        }

        internal static StringBuilder Get()
        {
            return SbPool.Get();
        }

        internal static void Return(StringBuilder sb)
        {
            SbPool.Return(sb);
        }
    }
}
