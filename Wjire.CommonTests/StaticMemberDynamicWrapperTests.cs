using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wjire.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Wjire.Common.Tests
{
    [TestClass()]
    public class StaticMemberDynamicWrapperTests
    {
        [TestMethod()]
        public void StaticMemberDynamicWrapperTest()
        {
            dynamic s = new StaticMemberDynamicWrapper(typeof(string));
            var r = s.Concat("A", "B");
            Console.WriteLine(r);
        }

        [TestMethod()]
        public void GetDynamicMemberNamesTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void TryGetMemberTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void TrySetMemberTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void TryInvokeMemberTest()
        {
            dynamic dyn = new StaticMemberDynamicWrapper(typeof(Person));
            dyn.StaticMethod();
        }
    }

    public class Person
    {
        public static void StaticMethod()
        {
            Console.WriteLine("static method");
        }

        public void Method()
        {
            Console.WriteLine("method");
        }
    }
}
