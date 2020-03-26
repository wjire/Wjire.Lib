namespace Wjire.Common
{

    /// <summary>
    /// string 扩展方法
    /// </summary>
    public static partial class ObjectExtensions
    {
        public static string If(this string str, bool condition)
        {
            return condition ? str : null;
        }

        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        public static string IfNotNullAndWhiteSpace(this string source, string returnStr)
        {
            return string.IsNullOrWhiteSpace(source) ? null : returnStr;
        }
    }
}
