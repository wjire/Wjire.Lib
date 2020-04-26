using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
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
        public static (string, string) GetKey(int strength = 1024)
        {
            RsaKeyPairGenerator g = new RsaKeyPairGenerator();
            g.Init(new KeyGenerationParameters(new SecureRandom(), strength));
            AsymmetricCipherKeyPair pair = g.GenerateKeyPair();

            TextWriter textPrivateWriter = new StringWriter();
            PemWriter pemPrivateWriter = new PemWriter(textPrivateWriter);
            pemPrivateWriter.WriteObject(pair.Private);
            pemPrivateWriter.Writer.Flush();
            string privateKey = textPrivateWriter.ToString();

            TextWriter textPublicWriter = new StringWriter();
            PemWriter pemPublicWriter = new PemWriter(textPublicWriter);
            pemPublicWriter.WriteObject(pair.Public);
            pemPublicWriter.Writer.Flush();
            string publicKey = textPublicWriter.ToString();

            return (publicKey, privateKey);
        }


        /// <summary>
        /// 公钥加密
        /// </summary>
        /// <param name="data">待加密数据</param>
        /// <param name="publicKey">公钥</param>
        /// <returns></returns>
        public static string EncryptWithPublicKey(string data, string publicKey)
        {
            byte[] bytesToEncrypt = Encoding.UTF8.GetBytes(data);

            Pkcs1Encoding encryptEngine = new Pkcs1Encoding(new RsaEngine());
            using (StringReader txtReader = new StringReader(publicKey))
            {
                AsymmetricKeyParameter keyParameter = (AsymmetricKeyParameter)new PemReader(txtReader).ReadObject();
                encryptEngine.Init(true, keyParameter);
            }
            string encryptedText = Convert.ToBase64String(encryptEngine.ProcessBlock(bytesToEncrypt, 0, bytesToEncrypt.Length));

            return encryptedText;
        }


        /// <summary>
        /// 公钥加密
        /// </summary>
        /// <param name="data">待加密数据</param>
        /// <param name="publicKey">公钥</param>
        /// <returns></returns>
        public static string EncryptWithPublicKey(object data, string publicKey)
        {
            byte[] bytesToEncrypt = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));

            Pkcs1Encoding encryptEngine = new Pkcs1Encoding(new RsaEngine());
            using (StringReader txtReader = new StringReader(publicKey))
            {
                AsymmetricKeyParameter keyParameter = (AsymmetricKeyParameter)new PemReader(txtReader).ReadObject();
                encryptEngine.Init(true, keyParameter);
            }
            string encryptedText = Convert.ToBase64String(encryptEngine.ProcessBlock(bytesToEncrypt, 0, bytesToEncrypt.Length));

            return encryptedText;
        }


        /// <summary>
        /// 私钥解密
        /// </summary>
        /// <param name="dataToDecrypt">待解密数据</param>
        /// <param name="privateKey">私钥</param>
        /// <returns></returns>
        public static string DecryptWithPrivateKey(string dataToDecrypt, string privateKey)
        {
            byte[] bytesToDecrypt = Convert.FromBase64String(dataToDecrypt);

            AsymmetricCipherKeyPair keyPair;
            using (StringReader stringReader = new StringReader(privateKey))
            {
                PemReader pemReader = new PemReader(stringReader);
                keyPair = (AsymmetricCipherKeyPair)pemReader.ReadObject();
            }
            AsymmetricKeyParameter keyParameter = keyPair.Private;
            IBufferedCipher cipher = CipherUtilities.GetCipher("RSA/ECB/PKCS1Padding");
            cipher.Init(false, keyParameter);
            byte[] decipheredBytes = cipher.DoFinal(bytesToDecrypt);
            string decipheredText = Encoding.UTF8.GetString(decipheredBytes);
            return decipheredText;
        }


        /// <summary>
        /// 私钥解密
        /// </summary>
        /// <param name="dataToDecrypt">待解密数据</param>
        /// <param name="privateKey">私钥</param>
        /// <returns></returns>
        public static T DecryptWithPrivateKeyAndDeserialize<T>(string dataToDecrypt, string privateKey)
        {
            byte[] bytesToDecrypt = Convert.FromBase64String(dataToDecrypt);
            AsymmetricCipherKeyPair keyPair;
            using (StringReader stringReader = new StringReader(privateKey))
            {
                PemReader pemReader = new PemReader(stringReader);
                keyPair = (AsymmetricCipherKeyPair)pemReader.ReadObject();
            }
            AsymmetricKeyParameter keyParameter = keyPair.Private;
            IBufferedCipher cipher = CipherUtilities.GetCipher("RSA/ECB/PKCS1Padding");
            cipher.Init(false, keyParameter);
            byte[] decipheredBytes = cipher.DoFinal(bytesToDecrypt);
            string decipheredText = Encoding.UTF8.GetString(decipheredBytes);
            return JsonConvert.DeserializeObject<T>(decipheredText);
        }


        /// <summary>
        /// 私钥加密
        /// </summary>
        /// <param name="data">待加密数据</param>
        /// <param name="privateKey">私钥</param>
        /// <returns></returns>
        public static string EncryptWithPrivateKey(object data, string privateKey)
        {
            byte[] bytesToEncrypt = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
            Pkcs1Encoding encryptEngine = new Pkcs1Encoding(new RsaEngine());
            using (StringReader txtReader = new StringReader(privateKey))
            {
                AsymmetricCipherKeyPair keyPair = (AsymmetricCipherKeyPair)new PemReader(txtReader).ReadObject();
                encryptEngine.Init(true, keyPair.Private);
            }
            string encryptedText = Convert.ToBase64String(encryptEngine.ProcessBlock(bytesToEncrypt, 0, bytesToEncrypt.Length));

            return encryptedText;
        }



        /// <summary>
        /// 公钥解密
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataToDecrypt">待解密数据</param>
        /// <param name="publicKey">公钥</param>
        /// <returns></returns>
        public static string DecryptWithPublicKey(string dataToDecrypt, string publicKey)
        {
            byte[] bytesToDecrypt = Convert.FromBase64String(dataToDecrypt);

            AsymmetricKeyParameter keyParameter;
            using (StringReader stringReader = new StringReader(publicKey))
            {
                PemReader pemReader = new PemReader(stringReader);
                keyParameter = (AsymmetricKeyParameter)pemReader.ReadObject();
            }
            IBufferedCipher cipher = CipherUtilities.GetCipher("RSA/ECB/PKCS1Padding");
            cipher.Init(false, keyParameter);
            byte[] decipheredBytes = cipher.DoFinal(bytesToDecrypt);
            string decipheredText = Encoding.UTF8.GetString(decipheredBytes);

            return decipheredText;
        }


        /// <summary>
        /// 公钥解密
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataToDecrypt">待解密数据</param>
        /// <param name="publicKey">公钥</param>
        /// <returns></returns>
        public static T DecryptWithPublicKeyAndDeserialize<T>(string dataToDecrypt, string publicKey)
        {
            byte[] bytesToDecrypt = Convert.FromBase64String(dataToDecrypt);

            AsymmetricKeyParameter keyParameter;
            using (StringReader stringReader = new StringReader(publicKey))
            {
                PemReader pemReader = new PemReader(stringReader);
                keyParameter = (AsymmetricKeyParameter)pemReader.ReadObject();
            }
            IBufferedCipher cipher = CipherUtilities.GetCipher("RSA/ECB/PKCS1Padding");
            cipher.Init(false, keyParameter);
            byte[] decipheredBytes = cipher.DoFinal(bytesToDecrypt);
            string decipheredText = Encoding.UTF8.GetString(decipheredBytes);

            return JsonConvert.DeserializeObject<T>(decipheredText);
        }
    }
}
