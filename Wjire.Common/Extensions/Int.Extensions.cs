using System;
using System.Collections.Generic;
using System.Text;

namespace Wjire.Common
{

	public static partial class ObjectExtensions
	{

		public static int ToInt(this object obj)
		{
			return Convert.ToInt32(obj);
		}

        public static long ToLong(this object obj)
        {
            return Convert.ToInt64(obj);
        }

        public static decimal ToDecimal(this object obj)
        {
            return Convert.ToDecimal(obj);
        }


        public static decimal ToDouble(this object obj)
        {
            return Convert.ToDouble(obj);
        }
        

        public static string If(this int data, Predicate<int> predicate, string str)
        {
            return predicate(data) ? str : null;
        }


        /// <summary>
        /// d 四舍五入,保留 n 位小数
        /// </summary>
        /// <param name="d"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static decimal RoundWithN(this decimal d, int n)
        {
            return Math.Round(d, n, MidpointRounding.AwayFromZero);
        }
    }
}
