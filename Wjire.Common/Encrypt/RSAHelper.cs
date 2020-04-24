using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Wjire.Common
{
    /// <summary>
    /// RSA
    /// </summary>
    public class RSAHelper
    {

        /// <summary>
        /// 检查RSA签名内容是否正确
        /// </summary>
        /// <param name="signContent">原始内容</param>
        /// <param name="sign">签名后的字符串(base64转换后的值)</param>
        /// <param name="publicKeyXml">签名私钥所对应的公钥</param>
        /// <returns>验签结果</returns>
        public static bool RSACheckContent(string signContent, string sign, string publicKeyXml)
        {
            if (string.IsNullOrWhiteSpace(signContent))
            {
                return false;
            }

            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider
            {
                PersistKeyInCsp = false
            };
            rsa.FromXmlString(publicKeyXml);

            bool bVerifyResultOriginal = rsa.VerifyData(Encoding.UTF8.GetBytes(signContent), "SHA1", Convert.FromBase64String(sign));
            return bVerifyResultOriginal;
        }

        /// <summary>
        /// 检查签名内容是否正确
        /// </summary>
        /// <param name="paramDic">参数信息</param>
        /// <param name="sign">签名后的字符串(base64转换后的值)</param>
        /// <param name="publicKeyXml">签名私钥所对应的公钥</param>
        /// <returns>验签结果</returns>
        private static bool RSACheckContent(IDictionary<string, string> paramDic, string sign, string publicKeyXml)
        {
            string param = GetSignContent(paramDic);
            return RSACheckContent(param, sign, publicKeyXml);
        }



        /// <summary>
        /// GetSignContent
        /// </summary>
        /// <param name="parameters">parameters</param>
        /// <returns>string</returns>
        private static string GetSignContent(IDictionary<string, string> parameters)
        {
            // 第一步：把字典按Key的字母顺序排序
            IDictionary<string, string> sortedParams = new SortedDictionary<string, string>(parameters);
            IEnumerator<KeyValuePair<string, string>> dem = sortedParams.GetEnumerator();

            // 第二步：把所有参数名和参数值串在一起
            StringBuilder query = new StringBuilder("");
            while (dem.MoveNext())
            {
                string key = dem.Current.Key;
                string value = dem.Current.Value;
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value) && key.ToLower() != "sign")
                {
                    query.Append(key).Append("=").Append(value).Append("&");
                }
            }
            string content = query.ToString().Substring(0, query.Length - 1);
            return content;
        }

        /// <summary>
        /// RSA签名
        /// </summary>
        /// <param name="content">参数</param>
        /// <param name="privateKeyXml">私钥</param>
        /// <returns>签名内容(base64转换后的值)</returns>
        public static string RSASign(string content, string privateKeyXml)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return null;
            }

            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(privateKeyXml);

            byte[] signatureBytes = rsa.SignData(Encoding.UTF8.GetBytes(content), "SHA1");
            return Convert.ToBase64String(signatureBytes);
        }

        /// <summary>
        /// RSA签名
        /// </summary>
        /// <param name="paramDic">参数</param>
        /// <param name="privateKeyXml">私钥</param>
        /// <returns>签名内容(base64转换后的值)</returns>
        private static string RSASign(IDictionary<string, string> paramDic, string privateKeyXml)
        {
            string param = GetSignContent(paramDic);
            return RSASign(param, privateKeyXml);
        }



        /// <summary>
        /// 生成RSA公钥、私钥
        /// </summary>
        /// <returns>参数1=公钥，参数2=私钥</returns>
        public static Tuple<string, string> CreateRSAKey()
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(1024);
            string priKey = rsa.ToXmlString(true);
            string pubKey = rsa.ToXmlString(false);
            return new Tuple<string, string>(pubKey, priKey);
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="content">加密内容</param>
        /// <param name="publicKeyXml">key</param>
        /// <returns>加密结果</returns>
        public static string Encrypt(string content, string publicKeyXml)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return string.Empty;
            }

            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(publicKeyXml);
            byte[] cipherBytes = rsa.Encrypt(Encoding.UTF8.GetBytes(content), false);
            return Convert.ToBase64String(cipherBytes);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="content">解密内容</param>
        /// <param name="privateKey">key</param>
        /// <returns>解密结果</returns>
        public static string Decrypt(string content, string privateKey)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return string.Empty;
            }

            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(privateKey);

            byte[] cipherBytes = rsa.Decrypt(Convert.FromBase64String(content), false);
            return Encoding.UTF8.GetString(cipherBytes);
        }
    }
}
