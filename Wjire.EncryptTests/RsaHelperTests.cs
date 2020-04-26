using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wjire.Common;

namespace Wjire.Encrypt.Tests
{
    [TestClass()]
    public class RsaHelperTests
    {
        [TestMethod()]
        public void GetKeyTest()
        {
            string path = @"C:\Users\Administrator\Desktop\";
            (string, string) keyTuple = RsaHelper.GetKey();
            FileHelper.WriteString(path + "public.txt", keyTuple.Item1);
            FileHelper.WriteString(path + "private.txt", keyTuple.Item2);
        }

        [TestMethod()]
        public void EncryptWithPublicKeyTest()
        {
            string data = "wjire";
            (string, string) keyTuple = RsaHelper.GetKey();
            string publicKey = keyTuple.Item1;
            string privateKey = keyTuple.Item2;

            publicKey =
                @"-----BEGIN PUBLIC KEY-----
MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCaEfoLMVNOXb9s3D/dds2Y1rQN
5NA2G7cw8SZCvbwxP9+lJxLGu1F+671cuOTJ6NTVCTnhtGtCrmrws8TDay9bEoSH
D89595LLvHjWj8hqs9YsQem/w/9zfqtiQ1z0mlMAr7cQMA/UeGnHr+n3y1tvEoY8
DyiMYVCDfbWCTyr87QIDAQAB
-----END PUBLIC KEY-----";
            privateKey = @"-----BEGIN RSA PRIVATE KEY-----
MIICXAIBAAKBgQCaEfoLMVNOXb9s3D/dds2Y1rQN5NA2G7cw8SZCvbwxP9+lJxLG
u1F+671cuOTJ6NTVCTnhtGtCrmrws8TDay9bEoSHD89595LLvHjWj8hqs9YsQem/
w/9zfqtiQ1z0mlMAr7cQMA/UeGnHr+n3y1tvEoY8DyiMYVCDfbWCTyr87QIDAQAB
AoGAGVOwhvvUy00d6rH0zbMkmRCyXeup+TsVIjaCKPaHuTuGyDS5dscpiR5iQpvN
AGQF7f9WRIQkHby5AliK6pT0Hle1gAjZrk62h8wGHnYDOH7snOtmBjtrWxQmCpWm
59u4HJglQ3eiD/ko/viho0g0qPALLxzP+2Gku/OiCb26ZQECQQDpE20CImO9iVtd
J/w5wb6P5tC3IK+jc5wOiEr60f8jKow85vaGW7hl1lSxKd5hNZ3TUdqQdN7KPvev
KSQ5wtVDAkEAqTlIOSsEB/0+oeZm2QcWi3sdogjFUg+dqOL+i81tCepI+Rr6DbDj
KAi8twFWRBDhGnseIvgLtr+75KXGxTaqDwJBAOAz4Y5GCm/Oa2anCgd9CZRfUbJ2
7L1sfle0X3v6+VSYnyIOgmIoZK8Bh6KMRfB4pQMcIAUJhy5Bd/y0tLYjZwUCQC6y
jPihozIlMzRwJS98oj8JUWsWaoUzo/kn8sBXhuB2k36ScDB5AKZaiuEhcFHGKqgp
E27o7iqXDF2TVZ+0bwcCQGJlKxUb/n3KzYhyCYDnlN4XEN5vK2yhKVwSUFwsT5T6
DLePxokQLgeswPdmD67TxcSqZWF+VEx7Cq87a4sGtc8=
-----END RSA PRIVATE KEY-----";

            string encryptWithPublic = RsaHelper.EncryptWithPublicKey(data, publicKey);
            string decryptWithPrivate = RsaHelper.DecryptWithPrivateKey(encryptWithPublic, privateKey);
            Debug.WriteLine(data);
            Debug.WriteLine(decryptWithPrivate);
            Assert.IsTrue(decryptWithPrivate == data);
        }

        [TestMethod()]
        public void EncryptWithPrivateKeyTest()
        {
            int data = 1112;
            (string, string) keyTuple = RsaHelper.GetKey();
            string publicKey = keyTuple.Item1;
            string privateKey = keyTuple.Item2;
            string encryptWithPublic = RsaHelper.EncryptWithPrivateKey(data, privateKey);
            int decryptWithPrivate = RsaHelper.DecryptWithPublicKeyAndDeserialize<int>(encryptWithPublic, publicKey);
            Debug.WriteLine(decryptWithPrivate);
            Assert.IsTrue(decryptWithPrivate == data);
        }
    }
}