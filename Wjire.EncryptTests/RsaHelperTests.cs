using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wjire.Encrypt;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Wjire.Common;

namespace Wjire.Encrypt.Tests
{
    [TestClass()]
    public class RsaHelperTests
    {
        [TestMethod()]
        public void GetKeyTest()
        {
            var path = @"C:\Users\Administrator\Desktop\";
            var keyTuple = RsaHelper.GetKey();
            FileHelper.WriteString(path + "public.txt", keyTuple.Item1);
            FileHelper.WriteString(path + "private.txt", keyTuple.Item2);
        }

        [TestMethod()]
        public void EncryptWithPublicKeyTest()
        {
            var data = 12312;
            var keyTuple = RsaHelper.GetKey();
            var publicKey = keyTuple.Item1;
            var privateKey = keyTuple.Item2;
            var encryptWithPublic = RsaHelper.EncryptWithPublicKey(data, publicKey);
            var decryptWithPrivate = RsaHelper.DecryptWithPrivateKey<int>(encryptWithPublic, privateKey);
            Debug.WriteLine(decryptWithPrivate);
            Assert.IsTrue(decryptWithPrivate == data);
        }

        [TestMethod()]
        public void EncryptWithPrivateKeyTest()
        {
            var data = 1112;
            var keyTuple = RsaHelper.GetKey();
            var publicKey = keyTuple.Item1;
            var privateKey = keyTuple.Item2;
            var encryptWithPublic = RsaHelper.EncryptWithPrivateKey(data, privateKey);
            var decryptWithPrivate = RsaHelper.DecryptWithPublicKey<int>(encryptWithPublic, publicKey);
            Debug.WriteLine(decryptWithPrivate);
            Assert.IsTrue(decryptWithPrivate == data);
        }
    }
}