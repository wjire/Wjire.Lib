using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Wjire.Encrypt
{
    /// <summary>
    /// Des 加密解密类
    /// </summary>
    public class DesHelper
    {

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="source">明文</param>
        /// <param name="key">密钥</param>
        /// <returns>密文</returns>
        public static string Encrypt(string source, string key)
        {
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                byte[] keys = Encoding.ASCII.GetBytes(key);
                byte[] ivs = Encoding.ASCII.GetBytes(key);
                byte[] dataByteArray = Encoding.UTF8.GetBytes(source);
                des.Mode = CipherMode.CBC;
                des.Key = keys;
                des.IV = ivs;
                string encrypt;
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(dataByteArray, 0, dataByteArray.Length);
                        cs.FlushFinalBlock();
                        encrypt = Convert.ToBase64String(ms.ToArray());
                    }
                }
                return encrypt;
            }
        }


        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="key">密钥</param>
        /// <param name="source">密文</param>
        /// <returns>明文</returns>
        public static string Decrypt(string source, string key)
        {
            byte[] inputByteArray = Convert.FromBase64String(source);
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                des.Key = Encoding.ASCII.GetBytes(key);
                des.IV = Encoding.ASCII.GetBytes(key);
                MemoryStream ms = new MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                string str = Encoding.UTF8.GetString(ms.ToArray());
                ms.Close();
                return str;
            }
        }
    }
}