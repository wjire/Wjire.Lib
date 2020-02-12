using System;
using System.Collections.Generic;
using System.Text;

namespace Wjire.Common.Extension
{

	public static partial class ObjectExtension
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
    }
}
