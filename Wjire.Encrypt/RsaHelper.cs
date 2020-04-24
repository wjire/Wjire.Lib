using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;

namespace Wjire.Encrypt
{
    public static class RsaHelper
    {

        /// <summary>
        /// 生成密钥
        /// </summary>
        /// <returns>(公,私)</returns>
        public static (string,string) GetKey(int strength = 1024)
        {
            Org.BouncyCastle.Crypto.Generators.RsaKeyPairGenerator g = new Org.BouncyCastle.Crypto.Generators.RsaKeyPairGenerator();
            g.Init(new KeyGenerationParameters(new SecureRandom(), strength));
            var pair = g.GenerateKeyPair();

            TextWriter textWriter = new StringWriter();
            PemWriter pemWriter = new PemWriter(textWriter);
            pemWriter.WriteObject(pair.Private);
            pemWriter.Writer.Flush();
            string privateKey = textWriter.ToString();

            TextWriter textPubWriter = new StringWriter();
            PemWriter pemPubWriter = new PemWriter(textPubWriter);
            pemPubWriter.WriteObject(pair.Public);
            pemPubWriter.Writer.Flush();
            string publicKey = textPubWriter.ToString();

            return (publicKey,privateKey);
        }


        /// <summary>
        /// 公钥加密
        /// </summary>
        /// <param name="data">待加密数据</param>
        /// <param name="publicKey">公钥</param>
        /// <returns></returns>
        public static string EncryptWithPublicKey(object data,string publicKey)
        {
            var bytesToEncrypt = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));

            var encryptEngine = new Pkcs1Encoding(new RsaEngine());
            using (var txtReader = new StringReader(publicKey))
            {
                var keyParameter = (AsymmetricKeyParameter)new PemReader(txtReader).ReadObject();
                encryptEngine.Init(true, keyParameter);
            }
            var encryptedText = Convert.ToBase64String(encryptEngine.ProcessBlock(bytesToEncrypt, 0, bytesToEncrypt.Length));

            return encryptedText;
        }


        /// <summary>
        /// 私钥解密
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataToDecrypt">待解密数据</param>
        /// <param name="privateKey">私钥</param>
        /// <returns></returns>
        public static T DecryptWithPrivateKey<T>(string dataToDecrypt, string privateKey)
        {
            var bytesToDecrypt = Convert.FromBase64String(dataToDecrypt);

            var stringReader = new StringReader(privateKey);
            var pemReader = new PemReader(stringReader);
            var keyPair = (AsymmetricCipherKeyPair)pemReader.ReadObject();
            var keyParameter = keyPair.Private;

            IBufferedCipher cipher = CipherUtilities.GetCipher("RSA/ECB/PKCS1Padding");
            cipher.Init(false, keyParameter);
            var decipheredBytes = cipher.DoFinal(bytesToDecrypt);
            var decipheredText = Encoding.UTF8.GetString(decipheredBytes);

            var result = JsonConvert.DeserializeObject<T>(decipheredText);
            return result;
        }



        /// <summary>
        /// 私钥解密
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataToDecrypt">待解密数据</param>
        /// <param name="privateKey">私钥</param>
        /// <returns></returns>
        public static T DecryptWithPrivateKey2<T>(string dataToDecrypt, string privateKey)
        {
            var bytesToDecrypt = Convert.FromBase64String(dataToDecrypt);
            var rsa = RSA.Create();
            rsa.ImportParameters(CreateRsaFromPrivateKey(privateKey));
            byte[] bytes = rsa.Decrypt(bytesToDecrypt, RSAEncryptionPadding.Pkcs1);
            var decipheredText = Encoding.UTF8.GetString(bytes);

            var result = JsonConvert.DeserializeObject<T>(decipheredText);
            return result;
        }



        /// <summary>
        /// 私钥加密
        /// </summary>
        /// <param name="data">待加密数据</param>
        /// <param name="privateKey">私钥</param>
        /// <returns></returns>
        public static string EncryptWithPrivateKey(object data, string privateKey)
        {
            var bytesToEncrypt = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
            var encryptEngine = new Pkcs1Encoding(new RsaEngine());
            using (var txtReader = new StringReader(privateKey))
            {
                var keyPair = (AsymmetricCipherKeyPair)new PemReader(txtReader).ReadObject();
                encryptEngine.Init(true, keyPair.Private);
            }
            var encryptedText = Convert.ToBase64String(encryptEngine.ProcessBlock(bytesToEncrypt, 0, bytesToEncrypt.Length));

            return encryptedText;
        }



        /// <summary>
        /// 公钥解密
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataToDecrypt">待解密数据</param>
        /// <param name="publicKey">公钥</param>
        /// <returns></returns>
        public static T DecryptWithPublicKey<T>(string dataToDecrypt,string publicKey)
        {
            var bytesToDecrypt = Convert.FromBase64String(dataToDecrypt);

            var stringReader = new StringReader(publicKey);
            var pemReader = new PemReader(stringReader);
            var keyParameter = (AsymmetricKeyParameter)pemReader.ReadObject();

            IBufferedCipher cipher = CipherUtilities.GetCipher("RSA/ECB/PKCS1Padding");
            cipher.Init(false, keyParameter);
            var decipheredBytes = cipher.DoFinal(bytesToDecrypt);
            var decipheredText = Encoding.UTF8.GetString(decipheredBytes);

            var result = JsonConvert.DeserializeObject<T>(decipheredText);
            return result;
        }

        private static RSAParameters CreateRsaFromPrivateKey(string privateKey)
        {
            string tmp = privateKey.Replace("\r\n", "").Replace("-----END RSA PRIVATE KEY-----", "").Replace("-----BEGIN RSA PRIVATE KEY-----", "");
            var privateKeyBits = System.Convert.FromBase64String(tmp);
            var RSAparams = new RSAParameters();

            using (var binr = new BinaryReader(new MemoryStream(privateKeyBits)))
            {
                byte bt = 0;
                ushort twobytes = 0;
                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130)
                    binr.ReadByte();
                else if (twobytes == 0x8230)
                    binr.ReadInt16();
                else
                    throw new Exception("Unexpected value read binr.ReadUInt16()");

                twobytes = binr.ReadUInt16();
                if (twobytes != 0x0102)
                    throw new Exception("Unexpected version");

                bt = binr.ReadByte();
                if (bt != 0x00)
                    throw new Exception("Unexpected value read binr.ReadByte()");

                RSAparams.Modulus = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.Exponent = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.D = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.P = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.Q = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.DP = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.DQ = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.InverseQ = binr.ReadBytes(GetIntegerSize(binr));
            }
            return RSAparams;
        }

        private static int GetIntegerSize(BinaryReader binr)
        {
            byte bt = 0;
            byte lowbyte = 0x00;
            byte highbyte = 0x00;
            int count = 0;
            bt = binr.ReadByte();
            if (bt != 0x02)
                return 0;
            bt = binr.ReadByte();

            if (bt == 0x81)
                count = binr.ReadByte();
            else
                if (bt == 0x82)
            {
                highbyte = binr.ReadByte();
                lowbyte = binr.ReadByte();
                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                count = BitConverter.ToInt32(modint, 0);
            }
            else
            {
                count = bt;
            }

            while (binr.ReadByte() == 0x00)
            {
                count -= 1;
            }
            binr.BaseStream.Seek(-1, SeekOrigin.Current);
            return count;
        }
    }
}
