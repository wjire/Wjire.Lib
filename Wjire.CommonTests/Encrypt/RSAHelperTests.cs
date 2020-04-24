using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wjire.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Wjire.Common.Tests
{
    [TestClass()]
    public class RSAHelperTests
    {
        [TestMethod()]
        public void RSACheckContentTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void RSASignTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CreateRSAKeyTest()
        {
            var path = @"C:\Users\Administrator\Desktop\";
           var keyTuple =  RSAHelper.CreateRSAKey();
           FileHelper.WriteString(path + "public.txt", keyTuple.Item1);
           FileHelper.WriteString(path + "private.txt", keyTuple.Item2);
        }

        [TestMethod()]
        public void EncryptTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DecryptTest()
        {
            Assert.Fail();
        }
    }
}