using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            string path = @"C:\Users\Administrator\Desktop\";
            Tuple<string, string> keyTuple = RSAHelper.CreateRSAKey();
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