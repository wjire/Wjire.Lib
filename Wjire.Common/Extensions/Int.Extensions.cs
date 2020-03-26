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

        public static decimal ToDecimal(this object obj)
        {
            return Convert.ToDecimal(obj);
        }


        public static decimal ToDouble(this object obj)
        {
            return Convert.ToDouble(obj);
        }


        public static decimal ToInt64(this object obj)
        {
            return Convert.ToInt64(obj);
        }


        public static string If(this int data, Predicate<int> predicate, string str)
        {
            return predicate(data) ? str : null;
        }
    }
}
