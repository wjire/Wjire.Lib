﻿namespace Wjire.Db.Extension
{

    /// <summary>
    /// string 扩展方法
    /// </summary>
    public static class StringExtension
    {
        public static string If(this string str, bool condition)
        {
            return condition ? str : null;
        }
    }
}
