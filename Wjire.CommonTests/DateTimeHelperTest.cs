using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wjire.Common;

namespace Wjire.CommonTests
{
    [TestClass()]
    public class DateTimeHelperTest
    {
        [TestMethod]
        public void Test()
        {
            DateTime d1 = DateTime.Now;
            DateTime d2 = DateTime.Now;
            DateTimeHelper.IsSameDay(d1, d2);
        }
    }
}
