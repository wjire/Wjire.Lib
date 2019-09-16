namespace Wjire.Common.Extension
{

    /// <summary>
    /// string 扩展方法
    /// </summary>
    public static partial class ObjectExtension
    {
        public static string If(this string str, bool condition)
        {
            return condition ? str : null;
        }
    }
}
